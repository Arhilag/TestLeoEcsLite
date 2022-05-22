using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

sealed class DamageInputSystem : IEcsRunSystem
{
    private GameObject _enemyCollision;
    private GameObject _playerCollision;
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
            foreach (var entit in projectileEnter)
            {
                ref var projectile = ref unitPool.Get(entit);
                if (projectile.modelTransform.gameObject == _sender)
                {
                    _enemyCollision = eventData.collider.gameObject;
                    break;
                }
            }
            foreach (var entit in enemyEnter)
            {
                ref var enemy = ref unitPool.Get(entit);
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