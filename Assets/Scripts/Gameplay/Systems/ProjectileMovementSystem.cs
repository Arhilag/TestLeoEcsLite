using Leopotam.EcsLite;
using UnityEngine;

sealed class ProjectileMovementSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<DirectionComponent>().Inc<ModelComponent>()
            .Inc<WeaponComponent>().Inc<ProjectileTag>().End();
        var directionC = _world.GetPool<DirectionComponent>();
        var model = _world.GetPool<ModelComponent>();
        var parameter = _world.GetPool<WeaponComponent>();
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref model.Get(i);
            ref var directionComponent = ref directionC.Get(i);
            ref var parameterComponent = ref parameter.Get(i);

            var direction = directionComponent.Direction.normalized;
            var transform = modelComponent.modelTransform;
            modelComponent.modelTransform.Rotate(directionComponent.Angle);
            var speed = parameterComponent.Weapons[0].Speed;

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}