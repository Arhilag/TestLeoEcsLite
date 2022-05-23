using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

sealed class AutoShootingSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private GameObject _projectile;
    private int _weaponsCount;
    public void Run(EcsSystems systems)
    {
        var filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        var weaponEquip = _world.GetPool<WeaponComponent>();
        var modelEquip = _world.GetPool<ModelComponent>();
        foreach (var i in filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            if (weaponComponent.Weapons.Length > _weaponsCount)
            {
                ref var modelComponent = ref modelEquip.Get(i);
                var transform = modelComponent.modelTransform;
                SpawnWeapons(_weaponsCount, weaponComponent, transform);
            }
        }
    }

    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        var weaponEquip = _world.GetPool<WeaponComponent>();
        var modelEquip = _world.GetPool<ModelComponent>();
        foreach (var i in filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            ref var modelComponent = ref modelEquip.Get(i);
            var transform = modelComponent.modelTransform;
            SpawnWeapons(0, weaponComponent, transform);
        }
    }

    private void SpawnWeapons(int countSpawnWeapon, WeaponComponent weapons, Transform transform)
    {
        _weaponsCount = weapons.Weapons.Length;
        for (int j = countSpawnWeapon; j < weapons.Weapons.Length; j++)
        { 
            var weaponConfig = weapons.Weapons[j];
            SpawnProjectile(weaponConfig, transform);
        }
    }

    private async void SpawnProjectile(WeaponConfig weaponConfig, Transform parent)
    {
        while (true)
        {
            var bullet = Object.Instantiate(weaponConfig.Projectile);
            bullet.transform.position = parent.position;
            await UniTask.Delay(TimeSpan.FromSeconds(weaponConfig.Delay), ignoreTimeScale: false);
            
            LifeProjectile(weaponConfig, bullet);
            //take damage and destroy projectile
        }
    }

    private async void LifeProjectile(WeaponConfig weaponConfig, GameObject projectile)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(weaponConfig.LifeTime), ignoreTimeScale: false);
        
        // Object.Destroy(projectile);
        projectile.SetActive(false);
    }
}