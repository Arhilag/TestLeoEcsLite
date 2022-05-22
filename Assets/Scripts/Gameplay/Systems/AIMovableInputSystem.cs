using Leopotam.EcsLite;
using UnityEngine;

sealed class AIMovableInputSystem : IEcsRunSystem
{
    private Vector3 _targetPosition;
        
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        
        var filter = world.Filter<ModelComponent>().Inc<PlayerTag>().End();
        var filterAI = world.Filter<ModelComponent>().Inc<AIDirectionComponent>().Inc<EnemyTag>().End();
        var modelUnit = world.GetPool<ModelComponent>();
        var aiUnit = world.GetPool<AIDirectionComponent>();
        
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