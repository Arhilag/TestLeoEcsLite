using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

sealed class EnemySpawnerSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world = null;
    readonly EcsWorldInject _defaultWorld = default;
    readonly EcsFilterInject<Inc<SpawnEnemySettingsComponent,
        TransformSpawnSettingsComponent>> _filterSpawner = default;
    readonly EcsPoolInject<SpawnEnemySettingsComponent> _poolSpawnEnemySetting = default;
    readonly EcsPoolInject<TransformSpawnSettingsComponent> _poolSpawnTransformSetting = default;
    
    readonly EcsFilterInject<Inc<PlayerTag,
        ModelComponent>> _filterPlayer = default;
    readonly EcsPoolInject<ModelComponent> _player = default;
    
    readonly EcsFilterInject<Inc<GlobalTimeComponent>> _timeFilter = default;
    readonly EcsPoolInject<GlobalTimeComponent> _timePool = default;
    
    readonly EcsFilterInject<Inc<EnemyTag,
        ModelComponent,
        SpeedComponent,
        HpComponent,
        DamageComponent,
        ExperienceCristalComponent>> _enemyFilter = default;
    readonly EcsPoolInject<ModelComponent> _enemyPool = default;
    readonly EcsPoolInject<SpeedComponent> _enemySpeedPool = default;
    readonly EcsPoolInject<HpComponent> _enemyHpPool = default;
    readonly EcsPoolInject<DamageComponent> _enemyDamagePool = default;
    readonly EcsPoolInject<ExperienceCristalComponent> _enemyExperienceCristalPool = default;
    
    private Transform _playerTransform;
    private SettingEnemySpawn _config;
    private SettingEnemySpawn[] _configs;
    private int _configNumber;
    private float _targetTime;
    private GlobalTimeComponent _globalTime;
    private float _indicationTime;

    private TransformSpawnSettingsComponent _transformSpawnConfig;
    private SpawnEnemySettingsComponent _enemySettingComponent;
    private int[] _enemyEntites;
    GameObject obj = null;

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();

        var filterSpawner = _filterSpawner.Value;
        var poolSpawnTransformSetting = _poolSpawnTransformSetting.Value;
        var poolSpawnEnemySetting = _poolSpawnEnemySetting.Value;

        var filterPlayer = _filterPlayer.Value;
        var player = _player.Value;

        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;
        
        foreach (var i in filterSpawner)
        {
            _transformSpawnConfig = poolSpawnTransformSetting.Get(i);
            _enemySettingComponent = poolSpawnEnemySetting.Get(i);
            _configs = _enemySettingComponent._settings;
            _config = _enemySettingComponent._settings[0];
            _configNumber = 0;
            _targetTime = _config.TimeLimit;
        }
        
        foreach (var i in filterPlayer)
        {
            ref var playerComponent = ref player.Get(i);

            _playerTransform = playerComponent.modelTransform;
        }
        
        foreach (var i in timeFilter)
        {
            _globalTime = timePool.Get(i);
        }
    }
    
    public void Run(EcsSystems systems)
    {
        _indicationTime += Time.deltaTime;
        if (_indicationTime >= _config.DelaySpawn)
        {
            _indicationTime = 0;
            Vector3 bulletTransform = _playerTransform.position;
            var randomSide = Random.Range(0, 4);
            switch (randomSide)
            {
                case 0:
                    bulletTransform += new Vector3(_transformSpawnConfig.Left, 0, 
                        Random.Range(_transformSpawnConfig.Down, _transformSpawnConfig.Up));
                    break;
                case 1:
                    bulletTransform += new Vector3(Random.Range(_transformSpawnConfig.Left, _transformSpawnConfig.Right),
                        0, _transformSpawnConfig.Up);
                    break;
                case 2:
                    bulletTransform += new Vector3(Random.Range(_transformSpawnConfig.Left, _transformSpawnConfig.Right),
                        0, _transformSpawnConfig.Down);
                    break;
                case 3:
                    bulletTransform += new Vector3(_transformSpawnConfig.Right, 0, 
                        Random.Range(_transformSpawnConfig.Down, _transformSpawnConfig.Up));
                    break;
            }
            obj = Object.Instantiate(_config.EnemyPrefab, bulletTransform, _config.EnemyPrefab.transform.rotation);
        }

        if (obj)
        {
            var enemyFilter = _enemyFilter.Value;
            var enemyPool = _enemyPool.Value;
            var enemySpeedPool = _enemySpeedPool.Value;
            var enemyHpPool = _enemyHpPool.Value;
            var enemyDamagePool = _enemyDamagePool.Value;
            var enemyExperienceCristalPool = _enemyExperienceCristalPool.Value;

            var filterSpawner = _filterSpawner.Value;
            var poolSpawnEnemySetting = _poolSpawnEnemySetting.Value;
            
            foreach (var i in enemyFilter)
            {
                ref var enemy = ref enemyPool.Get(i);
                if (enemy.modelTransform == obj.gameObject.transform)
                {
                    ref var enemySpeed = ref enemySpeedPool.Get(i);
                    ref var enemyHp = ref enemyHpPool.Get(i);
                    ref var enemyDamage = ref enemyDamagePool.Get(i);
                    ref var enemyExperienceCristal = ref enemyExperienceCristalPool.Get(i);
                    foreach (var j in filterSpawner)
                    {
                        ref var enemySettings = ref poolSpawnEnemySetting.Get(j);
                        enemySpeed.Speed = enemySettings._settings[_configNumber].Speed;
                        enemyHp.HP = enemySettings._settings[_configNumber].HP;
                        enemyHp.MaxHP = enemySettings._settings[_configNumber].HP;
                        enemyDamage.Damage = enemySettings._settings[_configNumber].Damage;
                        enemyExperienceCristal.ExperienceCristal = enemySettings._settings[_configNumber].ExperienceCrystal;
                        obj = null;
                    }
                }
            }
        }

        if (_targetTime <= _globalTime.GlobalTime)
        {
            _configNumber++;
            _config = _configs[_configNumber];
            _targetTime += _config.TimeLimit;
        }
    }
}