using Leopotam.EcsLite;
using UnityEngine;

class InitSystem : IEcsInitSystem
{
    private EcsWorld world = null;
    private TransformSpawnerConfig _transformConfig;
    private SpawnerConfig[] _enemyConfigs;
    
    public void Init(EcsSystems systems)
    {
        Time.timeScale = 1;
        world = systems.GetWorld ();
        var filterConfig = world.Filter<SpawnerConfigComponent>()
            .Inc<TransformSpawnerConfigComponent>()
            .Inc<TimeLimitLevelConfigComponent>()
            .Inc<EnemySpeedConfigComponent>()
            .Inc<PlayerConfigComponent>()
            .Inc<WeaponConfigComponent>()
            .Inc<PlayerExperienceConfigComponent>().End();
        var poolEnemyConfig = world.GetPool<SpawnerConfigComponent>();
        var poolTransformConfig = world.GetPool<TransformSpawnerConfigComponent>();
        var poolLimitLevelConfig = world.GetPool<TimeLimitLevelConfigComponent>();
        var poolEnemySpeedConfig = world.GetPool<EnemySpeedConfigComponent>();
        var poolPlayerSpeedConfig = world.GetPool<PlayerConfigComponent>();
        var poolWeaponConfig = world.GetPool<WeaponConfigComponent>();
        var poolPlayerExperienceConfig = world.GetPool<PlayerExperienceConfigComponent>();
        
        var filterSpawner = world.Filter<SpawnEnemySettingsComponent>()
            .Inc<TransformSpawnSettingsComponent>().End();
        var poolSpawnEnemySetting = world.GetPool<SpawnEnemySettingsComponent>();
        var poolSpawnTransformSetting = world.GetPool<TransformSpawnSettingsComponent>();
        
        var timeFilter = world.Filter<GlobalTimeComponent>().End();
        var timePool = world.GetPool<GlobalTimeComponent>();
        
        var playerFilter = world.Filter<SpeedComponent>()
            .Inc<PlayerTag>()
            .Inc<HpComponent>()
            .Inc<WeaponComponent>()
            .Inc<PlayerExperienceComponent>().End();
        var playerSpeedPool = world.GetPool<SpeedComponent>();
        var playerHpPool = world.GetPool<HpComponent>();
        var playerWeaponPool = world.GetPool<WeaponComponent>();
        var playerExperiencePool = world.GetPool<PlayerExperienceComponent>();
        
        var weaponContainerFilter = world.Filter<WeaponContainerComponent>().End();
        var weaponContainerPool = world.GetPool<WeaponContainerComponent>();

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
                // poolEnemyConfig.Del(i);
                // poolTransformConfig.Del(i);
                
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
            }
        }
    }
}