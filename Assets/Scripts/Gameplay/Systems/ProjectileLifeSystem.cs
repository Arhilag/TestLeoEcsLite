using Leopotam.EcsLite;
using UnityEngine;

sealed class ProjectileLifeSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;

    public void Run(EcsSystems systems)
    {
        var filter = _world.Filter<LifeTimeComponent>()
            .Inc<ProjectileTag>().End();
        var weaponEquip = _world.GetPool<LifeTimeComponent>();
        foreach (var i in filter)
        {
            ref var lifeTimeComponent = ref weaponEquip.Get(i);
            lifeTimeComponent.LifeTime -= Time.deltaTime;
            if (lifeTimeComponent.LifeTime <= 0)
            {
                var deathPool = _world.GetPool<DeathComponent>();
                deathPool.Add(i);
            }
        }
    }

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld();
    }

}