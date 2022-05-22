using System;
using System.Collections;
using System.Collections.Generic;
using LeoEcsPhysics;
using UnityEngine;
using Leopotam.EcsLite;
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
        
        AddInjections();
        AddOneFrames();
        AddSystems();
        
        _systems.Init();
    }

    void Update()
    {
        _systems.Run();
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
    }

    private void AddSystems()
    {
        _systems
            .Add(new PlayerMovableInputSystem())
            .Add(new MovementSystem())
            .Add(new AIMovableInputSystem())
            .Add(new AIMovementSystem())
            .Add(new DamageInputSystem());
    }
    
    private void AddOneFrames()
    {
    }
}
