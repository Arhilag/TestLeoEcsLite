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
            _targetPosition = modelComponent.modelTransform.position;
            break;
        }
        
        foreach (var i in filterAI)
        {
            ref var directionComponent = ref aiUnit.Get(i);
            ref var modelComponent = ref modelUnit.Get(i);
            if (directionComponent.Direction == Vector3.zero)
            {
                directionComponent.Direction = _targetPosition - modelComponent.modelTransform.position;
            }
        }
    }

    public void Init(EcsSystems systems)
    {
        
    }
}