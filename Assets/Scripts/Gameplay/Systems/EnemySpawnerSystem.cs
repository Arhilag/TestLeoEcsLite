using System;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

sealed class EnemySpawnerSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld world = null;
    private Transform _playerTransform;

    public void Run(EcsSystems systems)
    {
    }
    
    public void Init(EcsSystems systems)
    {
        world = systems.GetWorld ();
        var filter = world.Filter<SpawnerComponent>().End();
        var spawner = world.GetPool<SpawnerComponent>();
        var filterPlayer = world.Filter<ModelComponent>().Inc<PlayerTag>().End();
        var player = world.GetPool<ModelComponent>();
        foreach (var i in filterPlayer)
        {
            ref var playerComponent = ref player.Get(i);

            _playerTransform = playerComponent.modelTransform;
        }
        foreach (var i in filter)
        {
            ref var spawnerComponent = ref spawner.Get(i);

            SpawnEnemy(spawnerComponent.SpawnerConfig);
        }
    }
    
    private async void SpawnEnemy(SpawnerConfig config)
    {
        while (true)
        {
            Vector3 bulletTransform = _playerTransform.position;
            var randomSide = Random.Range(0, 4);
            switch (randomSide)
            {
                case 0:
                    bulletTransform += new Vector3(config.Left, 0, 
                        Random.Range(config.Down, config.Up));
                    break;
                case 1:
                    bulletTransform += new Vector3(Random.Range(config.Left, config.Right),
                        0, config.Up);
                    break;
                case 2:
                    bulletTransform += new Vector3(Random.Range(config.Left, config.Right),
                        0, config.Down);
                    break;
                case 3:
                    bulletTransform += new Vector3(config.Right, 0, 
                        Random.Range(config.Down, config.Up));
                    break;
            }
            Object.Instantiate(config._prefab, bulletTransform, config._prefab.transform.rotation);
            // var filter = world.Filter<EnemyTag>().Inc<ParameterComponent>().End();
            // var parameter = world.GetPool<ParameterComponent>();
            //
            // foreach (var i in filter)
            // {
            //     var parameterComponent = parameter.Get(i);
            //     if (parameterComponent.HP == 0)
            //     {
            //         parameterComponent.HP = parameterComponent.Config.HP;
            //     }
            // }
            await UniTask.Delay(TimeSpan.FromSeconds(config.Delay), ignoreTimeScale: false);
        }
    }
}