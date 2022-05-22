using Leopotam.EcsLite;
using UnityEngine;

sealed class MovementSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<DirectionComponent>().Inc<ModelComponent>().Inc<MovableComponent>()
            .Inc<ParameterComponent>().End();
        var directionC = world.GetPool<DirectionComponent>();
        var model = world.GetPool<ModelComponent>();
        var movable = world.GetPool<MovableComponent>();
        var parameter = world.GetPool<ParameterComponent>();
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref model.Get(i);
            ref var movableComponent = ref movable.Get(i);
            ref var directionComponent = ref directionC.Get(i);
            ref var parameterComponent = ref parameter.Get(i);

            ref var direction = ref directionComponent.Direction;
            ref var transform = ref modelComponent.modelTransform;

            ref var characterController = ref movableComponent.characterController;
            var speed = parameterComponent.Config.Speed;
                
            var rawDirection = (transform.right * direction.x) + (transform.forward * direction.z);

            characterController.Move(rawDirection * speed * Time.deltaTime);
        }
    }
}