using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class DamageInputSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<ProjectileCollisionEnemyComponent,
        HpComponent>> _filterEnemyCollision = default;
    readonly EcsPoolInject<ProjectileCollisionEnemyComponent> _poolEnemyCollision = default;
    readonly EcsPoolInject<HpComponent> _poolHp = default;
    
    readonly EcsFilterInject<Inc<EnemyCollisionPlayerComponent>> _filterPlayerCollision = default;
    readonly EcsPoolInject<EnemyCollisionPlayerComponent> _poolPlayerCollision = default;
    
    public void Run(EcsSystems systems)
    {
        var filterEnemyCollision = _filterEnemyCollision.Value;
        var poolEnemyCollision = _poolEnemyCollision.Value;
        var poolHp = _poolHp.Value;

        var filterPlayerCollision = _filterPlayerCollision.Value;
        var poolPlayerCollision = _poolPlayerCollision.Value;

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