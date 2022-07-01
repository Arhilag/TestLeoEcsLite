﻿using Leopotam.EcsLite;
using UnityEngine;

class ProjectileMovableInputSystem : IEcsRunSystem
{
    private Vector3 _targetPosition;
        
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        
        var filter = world.Filter<ModelComponent>()
            .Inc<EnemyTag>().End();
        var filterAI = world.Filter<ModelComponent>()
            .Inc<DirectionComponent>()
            .Inc<ProjectileTag>()
            .Inc<AngleComponent>().End();
        var modelUnit = world.GetPool<ModelComponent>();
        var aiUnit = world.GetPool<DirectionComponent>();
        var angle = world.GetPool<AngleComponent>();
        
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