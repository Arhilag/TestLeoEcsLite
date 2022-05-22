using Leopotam.EcsLite;
using UnityEngine;

sealed class DamageInputSystem : IEcsRunSystem
{
    
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<DirectionComponent>().Inc<PlayerTag>().End();
        var player = world.GetPool<DirectionComponent>();
        foreach (var i in filter)
        {
            ref var directionComponent = ref player.Get(i);
            ref var direction = ref directionComponent.Direction;
        }
    }
}