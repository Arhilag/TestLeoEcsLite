using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

sealed class DamageInputSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;
    private GameObject _enemyCollision;
    private GameObject _playerCollision;
    private GameObject _projectileCollision;
    private GameObject _sender;
    private float _damage;
    
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filterEnter = world.Filter<OnTriggerEnterEvent>().End();
        var poolEnter = world.GetPool<OnTriggerEnterEvent>();
        
        var projectileEnter = world.Filter<ProjectileTag>().Inc<ModelComponent>()
            .Inc<WeaponComponent>().End();
        var enemyEnter = world.Filter<EnemyTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<ModelComponent>()
            .Inc<ParameterComponent>().End();
        
        var unitPool = world.GetPool<ModelComponent>();
        var weaponPool = world.GetPool<WeaponComponent>();
        var paramPool = world.GetPool<ParameterComponent>();
        
        foreach (var entity in filterEnter)
        {
            ref var eventData = ref poolEnter.Get(entity);
            _sender = eventData.senderGameObject;
            foreach (var entityProj in projectileEnter)
            {
                ref var projectile = ref unitPool.Get(entityProj);
                ref var projectileConfig = ref weaponPool.Get(entityProj);
                _damage = projectileConfig.Weapons[0].Damage;
                _projectileCollision = projectile.modelTransform.gameObject;
                if (_projectileCollision == _sender)
                {
                    _enemyCollision = eventData.collider.gameObject;
                    break;
                }
            }
            foreach (var entityEnemy in enemyEnter)
            {
                ref var enemy = ref unitPool.Get(entityEnemy);
                if (enemy.modelTransform.gameObject == _sender)
                {
                    _playerCollision = eventData.collider.gameObject;
                    break;
                }
            }
            poolEnter.Del(entity);
        }
        

        foreach (var entity in enemyEnter)
        {
            ref var enemy = ref unitPool.Get(entity);
            ref var enemyParam = ref paramPool.Get(entity);
            if (enemy.modelTransform.gameObject == _enemyCollision && enemy.modelTransform.gameObject)
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
                }
                _projectileCollision.SetActive(false);
                _enemyCollision = null;
                break;
            }
        }
        
        
        foreach (var entity in playerFilter)
        {
            ref var player = ref unitPool.Get(entity);
            if (player.modelTransform.gameObject == _playerCollision && player.modelTransform.gameObject)
            {
                Debug.Log("damage to player");
                _playerCollision = null;
                break;
            }
        }
    }
}