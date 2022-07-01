using LeoEcsPhysics;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class DamageInputSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
    }

    public void Run(EcsSystems systems)
    {
        var filterEnemyCollision = _world.Filter<ProjectileCollisionEnemyComponent>()
            .Inc<HpComponent>().End();
        var poolEnemyCollision = _world.GetPool<ProjectileCollisionEnemyComponent>();
        var poolHp = _world.GetPool<HpComponent>();
        
        var filterPlayerCollision = _world.Filter<EnemyCollisionPlayerComponent>().End();
        var poolPlayerCollision = _world.GetPool<EnemyCollisionPlayerComponent>();

        foreach (var entity in filterEnemyCollision)
        {
            Debug.Log("damage to enemy");
            ref var enemyCollision = ref poolEnemyCollision.Get(entity);
            ref var enemyHp = ref poolHp.Get(entity);
            enemyHp.HP -= enemyCollision.Damage;
            poolEnemyCollision.Del(entity);
        }
        
        foreach (var entity in filterPlayerCollision)
        {
            Debug.Log("damage to player");
            ref var playerCollision = ref poolPlayerCollision.Get(entity);
            ref var playerHp = ref poolHp.Get(entity);
            playerHp.HP -= playerCollision.Damage;
            poolPlayerCollision.Del(entity);
        }
    }
}