using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class AIMovableInputSystem : IEcsRunSystem
{
    private Vector3 _targetPosition;
        
    readonly EcsFilterInject<Inc<ModelComponent,
        PlayerTag>> _filter = default;
    readonly EcsPoolInject<ModelComponent> _modelUnit = default;
    
    readonly EcsFilterInject<Inc<ModelComponent,
        DirectionComponent,
        EnemyTag>> _filterAI = default;
    readonly EcsPoolInject<DirectionComponent> _aiUnit = default;
    
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = _filter.Value;
        var filterAI = _filterAI.Value;
        var modelUnit = _modelUnit.Value;
        var aiUnit = _aiUnit.Value;
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref modelUnit.Get(i);
            _targetPosition = modelComponent.modelTransform.position;
        }
        
        foreach (var i in filterAI)
        {
            ref var directionComponent = ref aiUnit.Get(i);
            ref var modelComponent = ref modelUnit.Get(i);
            ref var direction = ref directionComponent.Direction;
            direction = _targetPosition - modelComponent.modelTransform.position;
        }
    }
}