using LeoEcsPhysics;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class CollisionSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    private GameObject _enemyCollisionIn;
    private GameObject _enemyCollisionOut;
    private GameObject _playerCollision;
    private GameObject _projectileCollision;
    private GameObject _sender;
    private GameObject _collision;
    private Slider _hpBar;
    private TextMeshProUGUI _textCountKills;
    private int _countKill;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld();
        var filterEnter = _world.Filter<OnTriggerEnterEvent>().End();
        var poolEnter = _world.GetPool<OnTriggerEnterEvent>();
        
        var projectileFilter = _world.Filter<ProjectileTag>()
            .Inc<ModelComponent>()
            .Inc<DamageComponent>()
            .Inc<LevelComponent>().End();
        var enemyFilter = _world.Filter<EnemyTag>()
            .Inc<ModelComponent>()
            .Inc<HpComponent>()
            .Inc<DamageComponent>()
            .Inc<ExperienceCristalComponent>().End();
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<ModelComponent>()
            .Inc<HpComponent>()
            .Inc<ExperienceCounterComponent>().End();
        var experienceFilter = _world.Filter<ExperienceComponent>()
            .Inc<ModelComponent>().End();
        
        var unitPool = _world.GetPool<ModelComponent>();
        var damagePool = _world.GetPool<DamageComponent>();
        var experiencePool = _world.GetPool<ExperienceComponent>();

        foreach (var entity in filterEnter)
        {
            ref var eventData = ref poolEnter.Get(entity);
            _sender = eventData.senderGameObject;
            _collision = eventData.collider.gameObject;
            foreach (var entityProj in projectileFilter)
            {
                ref var projectile = ref unitPool.Get(entityProj);
                ref var damage = ref damagePool.Get(entityProj);
                _projectileCollision = projectile.modelTransform.gameObject;
                if (_projectileCollision == _sender)
                {
                    foreach (var enemy in enemyFilter)
                    {
                        ref var enemyCollision = ref unitPool.Get(enemy);
                        if (enemyCollision.modelTransform.gameObject == _collision)
                        {
                            var collisionPool = _world.GetPool<ProjectileCollisionEnemyComponent>();
                            if (!collisionPool.Has(enemy))
                            {
                                collisionPool.Add(enemy);
                                ref var collisionEnemyComponent = ref collisionPool.Get(enemy);
                                collisionEnemyComponent.Damage = damage.Damage;
                            }
                            else
                            {
                                ref var collisionEnemyComponent = ref collisionPool.Get(enemy);
                                collisionEnemyComponent.Damage += damage.Damage;
                            }
                        }
                    }
                    break;
                }
            }
            foreach (var entityEnemy in enemyFilter)
            {
                ref var enemy = ref unitPool.Get(entityEnemy);
                ref var damage = ref damagePool.Get(entityEnemy);
                if (enemy.modelTransform.gameObject == _sender)
                {
                    foreach (var player in playerFilter)
                    {
                        ref var playerCollision = ref unitPool.Get(player);
                        if (playerCollision.modelTransform.gameObject == _collision)
                        {
                            var collisionPool = _world.GetPool<EnemyCollisionPlayerComponent>();
                            if (!collisionPool.Has(player))
                            {
                                collisionPool.Add(player);
                                ref var collisionPlayerComponent = ref collisionPool.Get(player);
                                collisionPlayerComponent.Damage = damage.Damage;
                            }
                            else
                            {
                                ref var collisionPlayerComponent = ref collisionPool.Get(player);
                                collisionPlayerComponent.Damage += damage.Damage;
                            }
                        }
                    }
                    break;
                }
            }
            foreach (var entityExperience in experienceFilter)
            {
                ref var experience = ref unitPool.Get(entityExperience);
                ref var experienceConfig = ref experiencePool.Get(entityExperience);
                if (experience.modelTransform.gameObject == _sender)
                {
                    foreach (var player in playerFilter)
                    {
                        ref var playerCollision = ref unitPool.Get(player);
                        if (playerCollision.modelTransform.gameObject == _collision)
                        {
                            var collisionPool = _world.GetPool<PlayerCollisionExperienceComponent>();
                            if (!collisionPool.Has(player))
                            {
                                collisionPool.Add(player);
                                ref var collisionEnemyComponent = ref collisionPool.Get(player);
                                collisionEnemyComponent.Experience = experienceConfig.CristalConfig.Experience;
                            }
                            else
                            {
                                ref var collisionEnemyComponent = ref collisionPool.Get(player);
                                collisionEnemyComponent.Experience += experienceConfig.CristalConfig.Experience;
                            }
                            var deathPool = _world.GetPool<DeathComponent>();
                            deathPool.Add(entityExperience);
                        }
                    }
                    break;
                }
            }
            poolEnter.Del(entity);
        }
    }
}