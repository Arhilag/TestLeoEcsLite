using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class TimeUISystem : IEcsRunSystem
{
    private EcsWorld _world = null;
    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld();
        var timeFilter = _world.Filter<GlobalTimeComponent>().End();
        var timePool = _world.GetPool<GlobalTimeComponent>();

        var uiFilter = _world.Filter<MainUIComponent>().Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var uiPool = _world.GetPool<MainUIComponent>();
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            foreach (var entity in uiFilter)
            {
                ref var uiPoolComponent = ref uiPool.Get(entity);
                var minute = (int) globalTime.GlobalTime / 60;
                uiPoolComponent.Text_time.text = minute + ":" + ((int)globalTime.GlobalTime-minute*60);
            }
        }
    }
}