using Leopotam.EcsLite;
using UnityEngine;

sealed class ProjectileLifeSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;

    public void Run(EcsSystems systems)
    {
        var filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<ProjectileTag>().End();
        var weaponEquip = _world.GetPool<WeaponComponent>();
        var modelEquip = _world.GetPool<ModelComponent>();
        foreach (var i in filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            ref var modelComponent = ref modelEquip.Get(i);
            var transform = modelComponent.modelTransform;
            var weaponConfig = weaponComponent.Weapons[0];
            weaponConfig.IndicationTime += Time.deltaTime;
            if (weaponConfig.IndicationTime >= weaponConfig.LifeTime)
            {
                weaponConfig.IndicationTime = 0;
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld();
    }

}