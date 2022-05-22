using Leopotam.EcsLite;
using UnityEngine;

sealed class PlayerMovableInputSystem : IEcsRunSystem
{
    private float moveX;
    private float moveZ;
        
    public void Run(EcsSystems systems)
    {
        SetDirection();
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<DirectionComponent>().Inc<PlayerTag>().End();
        var player = world.GetPool<DirectionComponent>();
        foreach (var i in filter)
        {
            ref var directionComponent = ref player.Get(i);
            ref var direction = ref directionComponent.Direction;
                
            direction.x = moveX;
            direction.z = moveZ;
        }
    }

    private void SetDirection()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
    }
}