using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

sealed class TriggerInputSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private GameObject _enemyCollision;
    private GameObject _playerCollision;
    private GameObject _projectileCollision;
    private GameObject _experienceCollision;
    private GameObject _sender;
    private float _damage;
    private float _experience;
    
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        
        var projectileFilter = world.Filter<ModelComponent>()
            .Inc<WeaponComponent>().Inc<OnTriggerEnterEvent>().End();
        var enemyFilter = world.Filter<EnemyTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().Inc<OnTriggerEnterEvent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().Inc<OnTriggerEnterEvent>().End();
        var experienceFilter = world.Filter<ExperienceComponent>().Inc<ModelComponent>()
            .Inc<OnTriggerEnterEvent>().End();
        
        var unitPool = world.GetPool<ModelComponent>();
        var weaponPool = world.GetPool<WeaponComponent>();
        var paramPool = world.GetPool<ParameterComponent>();
        var experiencePool = world.GetPool<ExperienceComponent>();
        var poolEnter = world.GetPool<OnTriggerEnterEvent>();
        
        var filterUI = world.Filter<UIComponent>().End();
        var poolUI = world.GetPool<UIComponent>();
        //enemy
        foreach (var entityProj in projectileFilter)
        {
            Debug.Log("2");
            ref var eventData = ref poolEnter.Get(entityProj);
            _projectileCollision = eventData.senderGameObject;
            _enemyCollision = eventData.collider.gameObject;
            ref var projectileConfig = ref weaponPool.Get(entityProj);
            _damage = projectileConfig.Weapons[0].Damage;
            foreach (var entity in enemyFilter)
            {
                ref var enemy = ref unitPool.Get(entity);
                ref var enemyParam = ref paramPool.Get(entity);
                if (enemy.modelTransform.gameObject == _enemyCollision)
                {
                    Debug.Log("damage to enemy");
                    if (enemyParam.HP == 0)
                    {
                        enemyParam.HP = enemyParam.Config.HP;
                    }
                    enemyParam.HP -= _damage;
                    if (enemyParam.HP <= 0)
                    {
                        _enemyCollision.SetActive(false);
                        Object.Instantiate(enemyParam.Config.ExperienceCrystal, _enemyCollision.transform.position,
                            _enemyCollision.transform.rotation);
                    }
                    _projectileCollision.SetActive(false);
                    _enemyCollision = null;
                
                    break;
                }
            }
        }
        //player
        foreach (var entityEnemy in enemyFilter)
        {
            ref var eventData = ref poolEnter.Get(entityEnemy);
            ref var enemy = ref unitPool.Get(entityEnemy);
            ref var enemyConfig = ref paramPool.Get(entityEnemy);
            _damage = enemyConfig.Config.Damage;
            if (enemy.modelTransform.gameObject == _sender)
            {
                _playerCollision = eventData.collider.gameObject;
                break;
            }
        }
        //exp
        foreach (var entityExperience in experienceFilter)
        {
            ref var eventData = ref poolEnter.Get(entityExperience);
            ref var experience = ref unitPool.Get(entityExperience);
            ref var experienceConfig = ref experiencePool.Get(entityExperience);
            _experience = experienceConfig.ExperienceConfig.Experience;
            if (experience.modelTransform.gameObject == _sender)
            {
                _playerCollision = eventData.collider.gameObject;
                _experienceCollision = _sender;
                break;
            }
        }
        
        // foreach (var entity in filterEnter)
        // {
        //     ref var eventData = ref poolEnter.Get(entity);
        //     _sender = eventData.senderGameObject;
        //     foreach (var entityProj in projectileFilter)
        //     {
        //         ref var projectile = ref unitPool.Get(entityProj);
        //         ref var projectileConfig = ref weaponPool.Get(entityProj);
        //         _damage = projectileConfig.Weapons[0].Damage;
        //         _projectileCollision = projectile.modelTransform.gameObject;
        //         if (_projectileCollision == _sender)
        //         {
        //             _enemyCollision = eventData.collider.gameObject;
        //             break;
        //         }
        //     }
        //     foreach (var entityEnemy in enemyFilter)
        //     {
        //         ref var enemy = ref unitPool.Get(entityEnemy);
        //         ref var enemyConfig = ref paramPool.Get(entityEnemy);
        //         _damage = enemyConfig.Config.Damage;
        //         if (enemy.modelTransform.gameObject == _sender)
        //         {
        //             _playerCollision = eventData.collider.gameObject;
        //             break;
        //         }
        //     }
        //     foreach (var entityExperience in experienceFilter)
        //     {
        //         ref var experience = ref unitPool.Get(entityExperience);
        //         ref var experienceConfig = ref experiencePool.Get(entityExperience);
        //         _experience = experienceConfig.ExperienceConfig.Experience;
        //         if (experience.modelTransform.gameObject == _sender)
        //         {
        //             _playerCollision = eventData.collider.gameObject;
        //             _experienceCollision = _sender;
        //             break;
        //         }
        //     }
        //     poolEnter.Del(entity);
        // }
        

        foreach (var entity in playerFilter)
        {
            ref var player = ref unitPool.Get(entity);
            ref var playerConfig = ref paramPool.Get(entity);
            if (player.modelTransform.gameObject == _playerCollision)
            {
                Debug.Log("damage to player");
                if (playerConfig.HP == 0)
                {
                    playerConfig.HP = playerConfig.Config.HP;
                }
                playerConfig.HP -= _damage;
                if (playerConfig.HP <= 0)
                {
                    Time.timeScale = 0;
                    foreach (var entityUI in filterUI)
                    {
                        ref var UIpool = ref poolUI.Get(entityUI);
                        UIpool._view_Lose.Show();
                    }
                }
                _playerCollision = null;
                break;
            }

            if(player.modelTransform.gameObject == _playerCollision && _experience > 0)
            {
                Debug.Log("damage to player");
                _experienceCollision.SetActive(false);
                playerConfig.Config.Experience += _experience;
                _playerCollision = null;
                break;
            }
        }
    }
}