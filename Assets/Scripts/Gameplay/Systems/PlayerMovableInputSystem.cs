using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class PlayerMovableInputSystem : IEcsRunSystem
{
    private float _moveX;
    private float _moveZ;
        
    readonly EcsFilterInject<Inc<DirectionComponent, 
        PlayerTag>> _filter = default;
    readonly EcsPoolInject<DirectionComponent> _player = default;
    
    public void Run(EcsSystems systems)
    {
        SetDirection();
        var filter = _filter.Value;
        var player = _player.Value;
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