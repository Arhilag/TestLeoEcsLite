using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class TimeSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    readonly EcsWorldInject _defaultWorld = default;
    readonly EcsFilterInject<Inc<GlobalTimeComponent>> _timeFilter = default;
    readonly EcsPoolInject<GlobalTimeComponent> _timePool = default;

    
    public void Init(EcsSystems systems)
    {
        _world = _defaultWorld.Value;

        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            globalTime.GlobalTime = 0;
        }
    }
    
    public void Run(EcsSystems systems)
    {
        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            globalTime.GlobalTime += Time.deltaTime;
        }
    }
}