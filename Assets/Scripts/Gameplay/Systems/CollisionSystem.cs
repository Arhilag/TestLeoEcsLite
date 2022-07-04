using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class CollisionSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<OnTriggerEnterEvent>> _filterEnter = default;
    readonly EcsPoolInject<OnTriggerEnterEvent> _poolEnter = default;
    
    readonly EcsFilterInject<Inc<ProjectileTag,
        ModelComponent,
        DamageComponent,
        LevelComponent>> _projectileFilter = default;
    readonly EcsFilterInject<Inc<EnemyTag,
        ModelComponent,
        HpComponent,
        DamageComponent,
        ExperienceCristalComponent>> _enemyFilter = default;
    readonly EcsFilterInject<Inc<PlayerTag,
        ModelComponent,
        HpComponent,
        ExperienceCounterComponent>> _playerFilter = default;
    readonly EcsFilterInject<Inc<ExperienceComponent,
        ModelComponent>> _experienceFilter = default;
    readonly EcsPoolInject<ModelComponent> _unitPool = default;
    readonly EcsPoolInject<DamageComponent> _damagePool = default;
    readonly EcsPoolInject<ExperienceComponent> _experiencePool = default;
    
    readonly EcsPoolInject<ProjectileCollisionEnemyComponent> _collisionProjectilePool = default;
    readonly EcsPoolInject<EnemyCollisionPlayerComponent> _collisionEnemyPool = default;
    readonly EcsPoolInject<PlayerCollisionExperienceComponent> _collisionPlayerPool = default;
    
    readonly EcsPoolInject<DeathComponent> _deathPool = default;
    
    private GameObject _projectileCollision;
    private GameObject _sender;
    private GameObject _collision;

    public void Run(EcsSystems systems)
    {
        var filterEnter = _filterEnter.Value;
        var poolEnter = _poolEnter.Value;

        var projectileFilter = _projectileFilter.Value;
        var enemyFilter = _enemyFilter.Value;
        var playerFilter = _playerFilter.Value;
        var experienceFilter = _experienceFilter.Value;

        var unitPool = _unitPool.Value;
        var damagePool = _damagePool.Value;
        var experiencePool = _experiencePool.Value;

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
                            var collisionPool = _collisionProjectilePool.Value;
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
                            var collisionPool = _collisionEnemyPool.Value;
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
                            var collisionPool = _collisionPlayerPool.Value;
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
                            var deathPool = _deathPool.Value;
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