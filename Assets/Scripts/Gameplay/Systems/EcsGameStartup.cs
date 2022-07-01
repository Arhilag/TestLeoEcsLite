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
            .Add(new TimeSystem())
            .Add(new LevelSystem())
            .Add(new EnemySpawnerSystem())
            .Add(new AutoShootingSystem())
            .Add(new PlayerMovableInputSystem())
            .Add(new AIMovableInputSystem())
            .Add(new ProjectileMovableInputSystem())
            .Add(new MovementSystem())
            .Add(new AngleSystem())
            .Add(new CollisionSystem())
            .Add(new DamageInputSystem())
            .Add(new HpSystem())
            .Add(new ExperienceSpawnSystem())
            .Add(new ExperienceInputSystem())
            .Add(new ProjectileLifeSystem())
            .Add(new EndGameSystem())
            .Add(new DeathSystem())
            .Add(new HpBarSystem())
            .Add(new KillCountingSystem())
            .Add(new LevelUISystem())
            .Add(new TimeUISystem());
    }
    
    private void AddOneFrames()
    {
    }
}
