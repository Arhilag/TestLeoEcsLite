using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class ProjectileLifeSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<LifeTimeComponent,
        ProjectileTag>> _filter = default;
    readonly EcsPoolInject<LifeTimeComponent> _weaponEquip = default;
    readonly EcsPoolInject<DeathComponent> _deathPool = default;
    
    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var weaponEquip = _weaponEquip.Value;
        foreach (var i in filter)
        {
            ref var lifeTimeComponent = ref weaponEquip.Get(i);
            lifeTimeComponent.LifeTime -= Time.deltaTime;
            if (lifeTimeComponent.LifeTime <= 0)
            {
                var deathPool = _deathPool.Value;
                deathPool.Add(i);
            }
        }
    }
}