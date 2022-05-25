using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

sealed class EnemySpawnerSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world = null;
    private Transform _playerTransform;
    private SpawnerConfig _config;
    private SpawnerConfig[] _configs;
    private int _configNumber;
    private LevelConfig _levelConfig;
    private float _targetTime;

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();

        var filter = _world.Filter<SpawnerComponent>().End();
        var spawner = _world.GetPool<SpawnerComponent>();
        
        var filterPlayer = _world.Filter<ModelComponent>().Inc<PlayerTag>().End();
        var player = _world.GetPool<ModelComponent>();
        
        var levelFilter = _world.Filter<LevelSettingComponent>().End();
        var levelPool = _world.GetPool<LevelSettingComponent>();
        
        foreach (var i in filterPlayer)
        {
            ref var playerComponent = ref player.Get(i);

            _playerTransform = playerComponent.modelTransform;
        }
        foreach (var i in filter)
        {
            ref var spawnerComponent = ref spawner.Get(i);
            _configs = spawnerComponent.SpawnerConfig;
            _config = spawnerComponent.SpawnerConfig[0];
            _configNumber = 0;
            _targetTime = _config.LevelTime;
            _config.IndicationTime = 0;
        }
        foreach (var i in levelFilter)
        {
            ref var levelComponent = ref levelPool.Get(i);

            _levelConfig = levelComponent.Setting;
        }
    }
    
    public void Run(EcsSystems systems)
    {
        _config.IndicationTime += Time.deltaTime;
        if (_config.IndicationTime >= _config.Delay)
        {
            _config.IndicationTime = 0;
            Vector3 bulletTransform = _playerTransform.position;
            var randomSide = Random.Range(0, 4);
            switch (randomSide)
            {
                case 0:
                    bulletTransform += new Vector3(_config.Left, 0, 
                        Random.Range(_config.Down, _config.Up));
                    break;
                case 1:
                    bulletTransform += new Vector3(Random.Range(_config.Left, _config.Right),
                        0, _config.Up);
                    break;
                case 2:
                    bulletTransform += new Vector3(Random.Range(_config.Left, _config.Right),
                        0, _config.Down);
                    break;
                case 3:
                    bulletTransform += new Vector3(_config.Right, 0, 
                        Random.Range(_config.Down, _config.Up));
                    break;
            }
            Object.Instantiate(_config._prefab, bulletTransform, _config._prefab.transform.rotation);
        }

        if (_targetTime >= _levelConfig.GlobalTime)
        {
            _configNumber++;
            _config = _configs[_configNumber];
            _targetTime += _config.LevelTime;
        }
    }
}