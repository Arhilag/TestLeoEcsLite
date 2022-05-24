using System;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

sealed class EnemySpawnerSystem : IEcsInitSystem
{
    private EcsWorld world = null;
    private Transform _playerTransform;
    private SpawnerConfig _config;
    private SpawnerConfig[] _configs;
    private int _configNumber;
    
    
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
            _configs = spawnerComponent.SpawnerConfig;
            _config = spawnerComponent.SpawnerConfig[0];
            _configNumber = 0;
            SpawnEnemy();
            Timer();
        }
    }
    
    private async void SpawnEnemy()
    {
        while (true)
        {
            Vector3 bulletTransform = _playerTransform.position;
            var randomSide = Random.Range(0, 4);
            switch (randomSide)
            {
                case 0:
                    bulletTransform += new Vector3(_config.Left, 0, 
                        Random.Range(_config.Down, _config.Up));
                    break;
                case 1:
                    bulletTransform += new Vector3(Random.Range(_config.Left, _config.Right),
                        0, _config.Up);
                    break;
                case 2:
                    bulletTransform += new Vector3(Random.Range(_config.Left, _config.Right),
                        0, _config.Down);
                    break;
                case 3:
                    bulletTransform += new Vector3(_config.Right, 0, 
                        Random.Range(_config.Down, _config.Up));
                    break;
            }
            Object.Instantiate(_config._prefab, bulletTransform, _config._prefab.transform.rotation);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.Delay), ignoreTimeScale: false);
        }
    }

    private async void Timer()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_config.TotalTime), ignoreTimeScale: false);
        if (_configNumber < _configs.Length)
        {
            _configNumber++;
            _config = _configs[_configNumber];
            Timer();
        }
    }
}