using Leopotam.EcsLite;
using UnityEngine;

sealed class MovementSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<DirectionComponent>()
            .Inc<ModelComponent>()
            .Inc<SpeedComponent>().End();
        var directionC = _world.GetPool<DirectionComponent>();
        var model = _world.GetPool<ModelComponent>();
        var speedPool = _world.GetPool<SpeedComponent>();
        
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