using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

sealed class AutoShootingSystem : IEcsRunSystem, IEcsInitSystem
{
    private GameObject _projectile;
    private WeaponConfig[] _weapons;
    public void Run(EcsSystems systems)
    {
        // EcsWorld world = systems.GetWorld ();
        // var filter = world.Filter<WeaponComponent>().Inc<ParameterComponent>().Inc<PlayerTag>().End();
        // var weaponEquip = world.GetPool<WeaponComponent>();
        // // var parameterUnit = world.GetPool<ParameterComponent>();
        //
        // foreach (var i in filter)
        // {
        //     ref var weaponComponent = ref weaponEquip.Get(i);
        //     // ref var parameterComponent = ref parameterUnit.Get(i);
        //     // if (_weapons.Length != parameterComponent.Config.WeaponCount)
        //     // {
        //     //     //start
        //     //     _weapons = weaponComponent.Weapons;
        //     // }
        // }
        // var gameObject = Object.Instantiate(_projectile);
    }

    public void Init(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld ();
        var filter = world.Filter<WeaponComponent>().Inc<ModelComponent>().Inc<PlayerTag>().End();
        var weaponEquip = world.GetPool<WeaponComponent>();
        var modelEquip = world.GetPool<ModelComponent>();
        foreach (var i in filter)
        {
            ref var weaponComponent = ref weaponEquip.Get(i);
            ref var modelComponent = ref modelEquip.Get(i);
            var coutWeapons = weaponComponent.Weapons.Length;
            var transform = modelComponent.modelTransform;
            for (int j = 0; j < coutWeapons; j++)
            { 
                var projectile = weaponComponent.Weapons[j].Projectile;
                var delay = weaponComponent.Weapons[j].Delay;
                SpawnProjectile(projectile, delay, transform);
            }
        }
    }

    private async void SpawnProjectile(GameObject projectile, float delay, Transform parent)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: false);
            var bullet = Object.Instantiate(projectile);
            bullet.transform.position = parent.position;
            //set direction
            //set speed
            //take damage and destroy projectile
        }
    }
}