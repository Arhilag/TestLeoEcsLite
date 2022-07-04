using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

class ProjectileMovableInputSystem : IEcsRunSystem
{
    private Vector3 _targetPosition;
        
    readonly EcsFilterInject<Inc<ModelComponent,
        EnemyTag>> _filter = default;
    readonly EcsFilterInject<Inc<ModelComponent,
        DirectionComponent,
        ProjectileTag,
        AngleComponent>> _filterAI = default;
    readonly EcsPoolInject<ModelComponent> _modelUnit = default;
    readonly EcsPoolInject<DirectionComponent> _aiUnit = default;
    readonly EcsPoolInject<AngleComponent> _angle = default;
    
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();

        var filter = _filter.Value;
        var filterAI = _filterAI.Value;
        var modelUnit = _modelUnit.Value;
        var aiUnit = _aiUnit.Value;
        var angle = _angle.Value;
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref modelUnit.Get(i);
            if (modelComponent.modelTransform.gameObject.activeSelf == false)
            {
                continue;
            }
            _targetPosition = modelComponent.modelTransform.position;
            break;
        }
        
        foreach (var i in filterAI)
        {
            ref var directionComponent = ref aiUnit.Get(i);
            ref var modelComponent = ref modelUnit.Get(i);
            ref var angleComponent = ref angle.Get(i);
            if (_targetPosition == null)
            {
                directionComponent.Direction = Vector3.left;
            }
            if (directionComponent.Direction == Vector3.zero)
            {
                directionComponent.Direction = _targetPosition - modelComponent.modelTransform.position;
                angleComponent.Angle = new Vector3(0, 0.5f, 0);
            }
        }
    }
}