using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

class InitSystem : IEcsInitSystem
{
    readonly EcsWorldInject _defaultWorld = default;
    
    readonly EcsFilterInject<Inc<SpawnerConfigComponent, 
        TransformSpawnerConfigComponent, 
        TimeLimitLevelConfigComponent,
        EnemySpeedConfigComponent,
        PlayerConfigComponent,
        WeaponConfigComponent,
        PlayerExperienceConfigComponent>> _filterConfig = default;
    readonly EcsPoolInject<SpawnerConfigComponent> _poolEnemyConfig = default;
    readonly EcsPoolInject<TransformSpawnerConfigComponent> _poolTransformConfig = default;
    readonly EcsPoolInject<TimeLimitLevelConfigComponent> _poolLimitLevelConfig = default;
    readonly EcsPoolInject<EnemySpeedConfigComponent> _poolEnemySpeedConfig = default;
    readonly EcsPoolInject<PlayerConfigComponent> _poolPlayerSpeedConfig = default;
    readonly EcsPoolInject<WeaponConfigComponent> _poolWeaponConfig = default;
    readonly EcsPoolInject<PlayerExperienceConfigComponent> _poolPlayerExperienceConfig = default;
    
    readonly EcsFilterInject<Inc<SpawnEnemySettingsComponent, 
        TransformSpawnSettingsComponent>> _filterSpawner = default;
    readonly EcsPoolInject<SpawnEnemySettingsComponent> _poolSpawnEnemySetting = default;
    readonly EcsPoolInject<TransformSpawnSettingsComponent> _poolSpawnTransformSetting = default;
    
    readonly EcsFilterInject<Inc<GlobalTimeComponent>> _timeFilter = default;
    readonly EcsPoolInject<GlobalTimeComponent> _timePool = default;
    
    readonly EcsFilterInject<Inc<SpeedComponent, 
        PlayerTag,
        HpComponent, 
        WeaponComponent,
        PlayerExperienceComponent>> _playerFilter = default;
    readonly EcsPoolInject<SpeedComponent> _playerSpeedPool = default;
    readonly EcsPoolInject<HpComponent> _playerHpPool = default;
    readonly EcsPoolInject<WeaponComponent> _playerWeaponPool = default;
    readonly EcsPoolInject<PlayerExperienceComponent> _playerExperiencePool = default;
    
    readonly EcsFilterInject<Inc<WeaponContainerComponent>> _weaponContainerFilter = default;
    readonly EcsPoolInject<WeaponContainerComponent> _weaponContainerPool = default;
    
    private TransformSpawnerConfig _transformConfig;
    private SpawnerConfig[] _enemyConfigs;
    
    public void Init(EcsSystems systems)
    {
        Time.timeScale = 1;
        var world = _defaultWorld.Value;
        var filterConfig = _filterConfig.Value;
        var poolEnemyConfig = _poolEnemyConfig.Value;
        var poolTransformConfig = _poolTransformConfig.Value;
        var poolLimitLevelConfig = _poolLimitLevelConfig.Value;
        var poolEnemySpeedConfig = _poolEnemySpeedConfig.Value;
        var poolPlayerSpeedConfig = _poolPlayerSpeedConfig.Value;
        var poolWeaponConfig = _poolWeaponConfig.Value;
        var poolPlayerExperienceConfig = _poolPlayerExperienceConfig.Value;

        var filterSpawner = _filterSpawner.Value;
        var poolSpawnEnemySetting = _poolSpawnEnemySetting.Value;
        var poolSpawnTransformSetting = _poolSpawnTransformSetting.Value;

        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;

        var playerFilter = _playerFilter.Value;
        var playerSpeedPool = _playerSpeedPool.Value;
        var playerHpPool = _playerHpPool.Value;
        var playerWeaponPool = _playerWeaponPool.Value;
        var playerExperiencePool = _playerExperiencePool.Value;

        var weaponContainerFilter = _weaponContainerFilter.Value;
        var weaponContainerPool = _weaponContainerPool.Value;

        foreach (var i in filterSpawner)
        {
            ref var enemySettingComponent = ref poolSpawnEnemySetting.Get(i);
            ref var transformSettingComponent = ref poolSpawnTransformSetting.Get(i);
            
            foreach (var k in filterConfig)
            {
                ref var configComponent = ref poolEnemyConfig.Get(k);
                ref var configTransformComponent = ref poolTransformConfig.Get(k);
                ref var configLimitLevelComponent = ref poolLimitLevelConfig.Get(k);
                ref var configEnemySpeedComponent = ref poolEnemySpeedConfig.Get(k);
                ref var configPlayerSpeedComponent = ref poolPlayerSpeedConfig.Get(k);
                ref var configWeaponComponent = ref poolWeaponConfig.Get(k);
                ref var configPlayerExperienceComponent = ref poolPlayerExperienceConfig.Get(k);
            
                _enemyConfigs = configComponent.SpawnerConfig;
                _transformConfig = configTransformComponent.TransformSpawnerConfig;
            
                transformSettingComponent.Down = _transformConfig.Down;
                transformSettingComponent.Left = _transformConfig.Left;
                transformSettingComponent.Right = _transformConfig.Right;
                transformSettingComponent.Up = _transformConfig.Up;
            
                enemySettingComponent._settings = new SettingEnemySpawn[_enemyConfigs.Length];
                var enemySetting = enemySettingComponent._settings;
            
                for (int j = 0; j < enemySetting.Length; j++)
                {
                    enemySetting[j].EnemyPrefab = _enemyConfigs[j]._prefab;
                    enemySetting[j].DelaySpawn = _enemyConfigs[j].Delay;
                    enemySetting[j].TimeLimit = _enemyConfigs[j].TimeLimit;
                    enemySetting[j].Speed = configEnemySpeedComponent.SpeedConfig[j].Speed;
                    enemySetting[j].Damage = configEnemySpeedComponent.SpeedConfig[j].Damage;
                    enemySetting[j].HP = configEnemySpeedComponent.SpeedConfig[j].HP;
                    enemySetting[j].ExperienceCrystal = configEnemySpeedComponent.SpeedConfig[j].ExperienceCrystal;
                }
                
                foreach (var l in timeFilter)
                {
                    ref var _globalTime = ref timePool.Get(l);
                    _globalTime.TimeLimit = configLimitLevelComponent.TimeLimitConfig.TimeLimitLevel;
                }
                
                foreach (var d in playerFilter)
                {
                    ref var playerSpeed = ref playerSpeedPool.Get(d);
                    ref var playerHp = ref playerHpPool.Get(d);
                    ref var playerWeapon = ref playerWeaponPool.Get(d);
                    ref var playerExperience = ref playerExperiencePool.Get(d);
                    playerSpeed.Speed = configPlayerSpeedComponent.PlayerConfig.Speed;
                    playerHp.HP = configPlayerSpeedComponent.PlayerConfig.HP;
                    playerHp.MaxHP = configPlayerSpeedComponent.PlayerConfig.HP;
                    for (var i1 = 0; i1 < playerWeapon.LevelSettings.Length; i1++)
                    {
                        playerWeapon.LevelSettings[i1].Level = configWeaponComponent.WeaponConfigs[i1].Level;
                    }

                    playerExperience.ExperienceToUp = 
                        new int[configPlayerExperienceComponent.ExperienceSetting.ExperienceToUp.Length];
                    for (int j = 0; j < configPlayerExperienceComponent.ExperienceSetting.ExperienceToUp.Length; j++)
                    {
                        playerExperience.ExperienceToUp[j] =
                            configPlayerExperienceComponent.ExperienceSetting.ExperienceToUp[j];
                    }
                }

                foreach (var m in weaponContainerFilter)
                {
                    ref var weaponContainer = ref weaponContainerPool.Get(m);
                    weaponContainer.Weapons = new WeaponInstance[configWeaponComponent.WeaponConfigs.Length];
                    for (var j = 0; j < configWeaponComponent.WeaponConfigs.Length; j++)
                    {
                        weaponContainer.Weapons[j].Projectile = configWeaponComponent.WeaponConfigs[j].Projectile;
                        weaponContainer.Weapons[j].Damage = configWeaponComponent.WeaponConfigs[j].Damage;
                        weaponContainer.Weapons[j].Speed = configWeaponComponent.WeaponConfigs[j].Speed;
                        weaponContainer.Weapons[j].DelayShoot = configWeaponComponent.WeaponConfigs[j].Delay;
                        weaponContainer.Weapons[j].LifeTime = configWeaponComponent.WeaponConfigs[j].LifeTime;
                    }
                }
                
                world.DelEntity(k);
            }
        }
    }
}