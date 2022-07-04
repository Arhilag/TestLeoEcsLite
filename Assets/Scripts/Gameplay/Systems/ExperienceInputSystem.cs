using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class ExperienceInputSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<PlayerCollisionExperienceComponent,
        ExperienceCounterComponent>> _filterPlayerCollision = default;
    readonly EcsPoolInject<PlayerCollisionExperienceComponent> _poolPlayerCollision = default;
    readonly EcsPoolInject<ExperienceCounterComponent> _poolExperienceCounter = default;
    
    public void Run(EcsSystems systems)
    {
        var filterPlayerCollision = _filterPlayerCollision.Value;
        var poolPlayerCollision = _poolPlayerCollision.Value;
        var poolExperienceCounter = _poolExperienceCounter.Value;

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