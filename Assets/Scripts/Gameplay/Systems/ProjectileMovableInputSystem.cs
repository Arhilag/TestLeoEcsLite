using Leopotam.EcsLite;
using UnityEngine;

class ProjectileMovableInputSystem : IEcsRunSystem, IEcsInitSystem
{
    private Vector3 _targetPosition;
        
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        
        var filter = world.Filter<ModelComponent>().Inc<EnemyTag>().End();
        var filterAI = world.Filter<ModelComponent>().Inc<DirectionComponent>().Inc<ProjectileTag>().End();
        var modelUnit = world.GetPool<ModelComponent>();
        var aiUnit = world.GetPool<DirectionComponent>();
        
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
            if (_targetPosition == null)
            {
                directionComponent.Direction = Vector3.left;
            }
            if (directionComponent.Direction == Vector3.zero)
            {
                directionComponent.Direction = _targetPosition - modelComponent.modelTransform.position;
                directionComponent.Angle = new Vector3(0, 0.5f, 0);
            }
        }
    }

    public void Init(EcsSystems systems)
    {
        
    }
}