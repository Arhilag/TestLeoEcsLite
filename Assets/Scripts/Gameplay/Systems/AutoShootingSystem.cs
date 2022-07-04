using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;

sealed class AutoShootingSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    
    readonly EcsWorldInject _defaultWorld = default;
    
    readonly EcsFilterInject<Inc<PlayerTag,
        WeaponComponent,
        ModelComponent>> _filter = default;
    readonly EcsPoolInject<WeaponComponent> _weaponEquip = default;
    readonly EcsPoolInject<ModelComponent> _modelEquip = default;
    
    readonly EcsFilterInject<Inc<WeaponContainerComponent>> _filterWeaponContainer = default;
    readonly EcsPoolInject<WeaponContainerComponent> _poolWeaponContainer = default;
    
    readonly EcsFilterInject<Inc<ProjectileTag,
        ModelComponent,
        SpeedComponent,
        LifeTimeComponent,
        DamageComponent,
        LevelComponent>> _filterProjectile = default;
    readonly EcsPoolInject<ModelComponent> _poolProjectileModel = default;
    readonly EcsPoolInject<SpeedComponent> _poolProjectileSpeed = default;
    readonly EcsPoolInject<LifeTimeComponent> _poolProjectileLifeTime = default;
    readonly EcsPoolInject<DamageComponent> _poolProjectileDamage = default;
    readonly EcsPoolInject<LevelComponent> _poolProjectileLevel = default;
    
    private List<int> _queue = new List<int>();
    GameObject obj = null;

    public void Init(EcsSystems systems)
    {
        _world = _defaultWorld.Value;
    }
    
    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var weaponEquip = _weaponEquip.Value;
        var modelEquip = _modelEquip.Value;
        var filterWeaponContainer = _filterWeaponContainer.Value;
        var poolWeaponContainer = _poolWeaponContainer.Value;
        foreach (var i in filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            ref var modelComponent = ref modelEquip.Get(i);

            foreach (var j in filterWeaponContainer)
            {
                ref var weaponContainer = ref poolWeaponContainer.Get(j);
                for (int k = 0; k < weaponComponent.LevelSettings.Length; k++)
                {
                    if (weaponComponent.LevelSettings[k].Level > 0)
                    {
                        weaponContainer.Weapons[k].CountingTime += Time.deltaTime;
                        if (weaponContainer.Weapons[k].CountingTime >= weaponContainer.Weapons[k].DelayShoot)
                        {
                            weaponContainer.Weapons[k].CountingTime = 0;
                            _queue.Add(k);
                        }
                    }
                }

                if (_queue.Count > 0 && !obj)
                {
                    obj = Object.Instantiate(weaponContainer.Weapons[_queue[0]].Projectile, 
                        modelComponent.modelTransform.position, modelComponent.modelTransform.rotation);
                }

                if (obj)
                {
                    var filterProjectile = _filterProjectile.Value;
                    var poolProjectileModel = _poolProjectileModel.Value;
                    var poolProjectileSpeed = _poolProjectileSpeed.Value;
                    var poolProjectileLifeTime = _poolProjectileLifeTime.Value;
                    var poolProjectileDamage = _poolProjectileDamage.Value;
                    var poolProjectileLevel = _poolProjectileLevel.Value;
                    foreach (var l in filterProjectile)
                    {
                        ref var modelProjectileComponent = ref poolProjectileModel.Get(l);
                        if (modelProjectileComponent.modelTransform == obj.transform)
                        {
                            ref var speedProjectileComponent = ref poolProjectileSpeed.Get(l);
                            ref var lifeTimeProjectileComponent = ref poolProjectileLifeTime.Get(l);
                            ref var damageProjectileComponent = ref poolProjectileDamage.Get(l);
                            ref var levelProjectileComponent = ref poolProjectileLevel.Get(l);
                            speedProjectileComponent.Speed = weaponContainer.Weapons[_queue[0]].Speed;
                            lifeTimeProjectileComponent.LifeTime = weaponContainer.Weapons[_queue[0]].LifeTime;
                            damageProjectileComponent.Damage = weaponContainer.Weapons[_queue[0]].Damage;
                            levelProjectileComponent.Level = weaponComponent.LevelSettings[_queue[0]].Level;
                            obj = null;
                            _queue.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }
}