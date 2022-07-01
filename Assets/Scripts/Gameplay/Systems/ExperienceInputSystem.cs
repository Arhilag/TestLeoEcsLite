using LeoEcsPhysics;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class ExperienceInputSystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    
    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld();
        var filterPlayerCollision = _world.Filter<PlayerCollisionExperienceComponent>()
            .Inc<ExperienceCounterComponent>().End();
        var poolPlayerCollision = _world.GetPool<PlayerCollisionExperienceComponent>();
        var poolExperienceCounter = _world.GetPool<ExperienceCounterComponent>();

        foreach (var entity in filterPlayerCollision)
        {
            Debug.Log("EXP to player");
            ref var playerCollision = ref poolPlayerCollision.Get(entity);
            ref var experienceCounter = ref poolExperienceCounter.Get(entity);
            experienceCounter.Experience += playerCollision.Experience;
            poolPlayerCollision.Del(entity);
        }
    }
}