using Leopotam.EcsLite;
using UnityEngine;

sealed class ProjectileMovementSystem : IEcsRunSystem
{
    private readonly EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<DirectionComponent>().Inc<ModelComponent>()
            .Inc<WeaponComponent>().Inc<ProjectileTag>().End();
        var directionC = world.GetPool<DirectionComponent>();
        var model = world.GetPool<ModelComponent>();
        var parameter = world.GetPool<WeaponComponent>();
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref model.Get(i);
            ref var directionComponent = ref directionC.Get(i);
            ref var parameterComponent = ref parameter.Get(i);

            var direction = directionComponent.Direction.normalized;
            var transform = modelComponent.modelTransform;

            var speed = parameterComponent.Weapons[0].Speed;

            transform.position += direction * speed * Time.deltaTime;
        }
    }
}