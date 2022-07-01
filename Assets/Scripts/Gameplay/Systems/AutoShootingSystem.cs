using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

sealed class AutoShootingSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private List<int> _queue = new List<int>();
    GameObject obj = null;

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        // _filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        // var weaponEquip = _world.GetPool<WeaponComponent>();
        // foreach (var i in _filter)
        // {
        //     ref var weaponComponent = ref weaponEquip.Get(i);
        //     foreach (var weaponConfig in weaponComponent.Weapons)
        //     {
        //         weaponConfig.IndicationDelay = 0;
        //     }
        // }
    }
    
    public void Run(EcsSystems systems)
    {
        _filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        var weaponEquip = _world.GetPool<WeaponComponent>();
        var modelEquip = _world.GetPool<ModelComponent>();
        var filterWeaponContainer = _world.Filter<WeaponContainerComponent>().End();
        var poolWeaponContainer = _world.GetPool<WeaponContainerComponent>();
        foreach (var i in _filter)
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
                            // Object.Instantiate(weaponContainer.Weapons[k].Projectile,
                            //     modelComponent.modelTransform.position, modelComponent.modelTransform.rotation);
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
                    var filterProjectile = _world.Filter<ProjectileTag>()
                        .Inc<ModelComponent>()
                        .Inc<SpeedComponent>()
                        .Inc<LifeTimeComponent>()
                        .Inc<DamageComponent>()
                        .Inc<LevelComponent>().End();
                    var poolProjectileModel = _world.GetPool<ModelComponent>();
                    var poolProjectileSpeed = _world.GetPool<SpeedComponent>();
                    var poolProjectileLifeTime = _world.GetPool<LifeTimeComponent>();
                    var poolProjectileDamage = _world.GetPool<DamageComponent>();
                    var poolProjectileLevel = _world.GetPool<LevelComponent>();
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