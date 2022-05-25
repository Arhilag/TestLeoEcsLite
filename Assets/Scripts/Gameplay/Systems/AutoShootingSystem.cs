using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

sealed class AutoShootingSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        _filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        var weaponEquip = _world.GetPool<WeaponComponent>();
        foreach (var i in _filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            foreach (var weaponConfig in weaponComponent.Weapons)
            {
                weaponConfig.IndicationDelay = 0;
            }
        }
    }
    
    public void Run(EcsSystems systems)
    {
        var weaponEquip = _world.GetPool<WeaponComponent>();
        var modelEquip = _world.GetPool<ModelComponent>();
        foreach (var i in _filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            ref var modelComponent = ref modelEquip.Get(i);
            var transform = modelComponent.modelTransform;
            foreach (var weaponConfig in weaponComponent.Weapons)
            {
                if (weaponConfig.Level > 0)
                {
                    weaponConfig.IndicationDelay += Time.deltaTime;
                    if (weaponConfig.IndicationDelay >= weaponConfig.Delay)
                    {
                        weaponConfig.IndicationDelay = 0;
                        weaponConfig.IndicationTime = 0;
                        Object.Instantiate(weaponConfig.Projectile, transform.position, transform.rotation);
                    }
                }
            }
        }
    }
}