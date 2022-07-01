using System;
using Leopotam.EcsLite;
using Object = UnityEngine.Object;

sealed class DeathSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    public static Action OnEnemyDead;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<DeathComponent>()
            .Inc<ModelComponent>().End();
        var modelPool = _world.GetPool<ModelComponent>();
        var enemyPool = _world.GetPool<EnemyTag>();
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref modelPool.Get(i);
            var transform = modelComponent.modelTransform;
            if (enemyPool.Has(i))
            {
                OnEnemyDead?.Invoke();
            }
            _world.DelEntity(i);
            Object.Destroy(transform.gameObject);
        }
    }
}