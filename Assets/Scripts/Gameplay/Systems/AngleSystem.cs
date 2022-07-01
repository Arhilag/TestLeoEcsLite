using Leopotam.EcsLite;
using UnityEngine;

sealed class AngleSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<AngleComponent>()
            .Inc<ModelComponent>().End();
        var anglePool = _world.GetPool<AngleComponent>();
        var modelPool = _world.GetPool<ModelComponent>();
        
        foreach (var i in filter)
        {
            ref var angleComponent = ref anglePool.Get(i);
            ref var modelComponent = ref modelPool.Get(i);

            modelComponent.modelTransform.Rotate(angleComponent.Angle);
        }
    }
}