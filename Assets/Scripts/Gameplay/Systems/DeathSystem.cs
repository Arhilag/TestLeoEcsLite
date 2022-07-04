using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Object = UnityEngine.Object;

sealed class DeathSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    public static Action OnEnemyDead;

    readonly EcsFilterInject<Inc<DeathComponent,
        ModelComponent>> _filter = default;
    readonly EcsPoolInject<ModelComponent> _modelPool = default;
    readonly EcsPoolInject<EnemyTag> _enemyPool = default;
    
    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _filter.Value;
        var modelPool = _modelPool.Value;
        var enemyPool = _enemyPool.Value;
        
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