using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

sealed class DamageInputSystem : IEcsRunSystem
{
    private GameObject _enemyCollision;
    private GameObject _playerCollision;
    private GameObject _projectileCollision;
    private GameObject _sender;
    public void Run(EcsSystems systems)
    {
        var filterEnter = systems.GetWorld().Filter<OnTriggerEnterEvent>().End();
        var poolEnter = systems.GetWorld().GetPool<OnTriggerEnterEvent>();
        var projectileEnter = systems.GetWorld().Filter<ProjectileTag>().Inc<ModelComponent>().End();
        var enemyFilter = systems.GetWorld().Filter<EnemyTag>().Inc<ModelComponent>().End();
        var unitPool = systems.GetWorld().GetPool<ModelComponent>();
        
        var enemyEnter = systems.GetWorld().Filter<EnemyTag>().Inc<ModelComponent>().End();
        var playerFilter = systems.GetWorld().Filter<PlayerTag>().Inc<ModelComponent>().End();
        
        foreach (var entity in filterEnter)
        {
            ref var eventData = ref poolEnter.Get(entity);
            _sender = eventData.senderGameObject;
            foreach (var entityProj in projectileEnter)
            {
                ref var projectile = ref unitPool.Get(entityProj);
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

        foreach (var entity in enemyFilter)
        {
            ref var enemy = ref unitPool.Get(entity);
            if (enemy.modelTransform.gameObject == _enemyCollision && enemy.modelTransform.gameObject)
            {
                Debug.Log("damage to enemy");
                _projectileCollision.SetActive(false);
                _enemyCollision.SetActive(false);
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