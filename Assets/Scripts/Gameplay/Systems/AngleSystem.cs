using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class AngleSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<AngleComponent,
        ModelComponent>> _filter = default;
    readonly EcsPoolInject<AngleComponent> _anglePool = default;
    readonly EcsPoolInject<ModelComponent> _modelPool = default;
    
    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var anglePool = _anglePool.Value;
        var modelPool = _modelPool.Value;
        
        foreach (var i in filter)
        {
            ref var angleComponent = ref anglePool.Get(i);
            ref var modelComponent = ref modelPool.Get(i);

            modelComponent.modelTransform.Rotate(angleComponent.Angle);
        }
    }
}