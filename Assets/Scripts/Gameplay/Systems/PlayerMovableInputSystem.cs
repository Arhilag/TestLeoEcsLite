using Leopotam.EcsLite;
using UnityEngine;

sealed class PlayerMovableInputSystem : IEcsRunSystem
{
    private float _moveX;
    private float _moveZ;
        
    public void Run(EcsSystems systems)
    {
        SetDirection();
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<DirectionComponent>()
            .Inc<PlayerTag>().End();
        var player = world.GetPool<DirectionComponent>();
        foreach (var i in filter)
        {
            ref var directionComponent = ref player.Get(i);
            ref var direction = ref directionComponent.Direction;
                
            direction.x = _moveX;
            direction.z = _moveZ;
        }
    }

    private void SetDirection()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveZ = Input.GetAxis("Vertical");
    }
}