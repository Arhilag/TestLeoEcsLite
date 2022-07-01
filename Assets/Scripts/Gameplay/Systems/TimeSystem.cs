using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class TimeSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    // private GlobalTimeComponent _globalTime;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        
        var timeFilter = _world.Filter<GlobalTimeComponent>().End();
        var timePool = _world.GetPool<GlobalTimeComponent>();
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            globalTime.GlobalTime = 0;
        }
    }
    
    public void Run(EcsSystems systems)
    {
        var timeFilter = _world.Filter<GlobalTimeComponent>().End();
        var timePool = _world.GetPool<GlobalTimeComponent>();
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            globalTime.GlobalTime += Time.deltaTime;
        }
    }
}