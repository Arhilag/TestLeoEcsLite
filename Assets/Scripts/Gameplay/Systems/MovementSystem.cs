using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class MovementSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<DirectionComponent,
        ModelComponent,
        SpeedComponent>> _filter = default;
    readonly EcsPoolInject<DirectionComponent> _directionC = default;
    readonly EcsPoolInject<ModelComponent> _model = default;
    readonly EcsPoolInject<SpeedComponent> _speedPool = default;
    
    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var directionC = _directionC.Value;
        var model = _model.Value;
        var speedPool = _speedPool.Value;
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref model.Get(i);
            ref var directionComponent = ref directionC.Get(i);
            ref var speedComponent = ref speedPool.Get(i);

             var direction = directionComponent.Direction.normalized;
            ref var transform = ref modelComponent.modelTransform;

            var speed = speedComponent.Speed;

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}