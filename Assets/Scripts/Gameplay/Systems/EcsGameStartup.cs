using LeoEcsPhysics;
using UnityEngine;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Voody.UniLeo.Lite;

public class EcsGameStartup : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;
    
    void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        EcsPhysicsEvents.ecsWorld = _world;
        
        _systems.ConvertScene();
        
        AddOneFrames();
        AddSystems();
        
        AddInjections();
        _systems.Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        if(_systems == null)
            return;
        
        EcsPhysicsEvents.ecsWorld = null;
        _systems.Destroy();
        _systems = null;
        _world.Destroy();
        _world = null;
    }

    private void AddInjections()
    {
        _systems.Inject();
    }

    private void AddSystems()
    {
        _systems
            .Add(new InitSystem())
            .Add(new LevelSystem())
            .Add(new PlayerMovableInputSystem())
            .Add(new MovementSystem())
            .Add(new AIMovableInputSystem())
            .Add(new AIMovementSystem())
            .Add(new ProjectileMovableInputSystem())
            .Add(new ProjectileMovementSystem())
            .Add(new ProjectileLifeSystem())
            .Add(new DamageInputSystem())
            .Add(new AutoShootingSystem())
            .Add(new EnemySpawnerSystem());
    }
    
    private void AddOneFrames()
    {
    }
}
