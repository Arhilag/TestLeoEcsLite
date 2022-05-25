using Leopotam.EcsLite;
using UnityEngine;

sealed class MovementSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<DirectionComponent>().Inc<ModelComponent>().Inc<MovableComponent>()
            .Inc<PlayerParameterComponent>().End();
        var directionC = _world.GetPool<DirectionComponent>();
        var model = _world.GetPool<ModelComponent>();
        var movable = _world.GetPool<MovableComponent>();
        var parameter = _world.GetPool<PlayerParameterComponent>();
        
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