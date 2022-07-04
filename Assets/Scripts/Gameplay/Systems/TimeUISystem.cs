using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class TimeUISystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<GlobalTimeComponent>> _timeFilter = default;
    readonly EcsPoolInject<GlobalTimeComponent> _timePool = default;

    readonly EcsFilterInject<Inc<TextComponent,
    UITimeComponent>> _uiTextFilter = default;
    readonly EcsPoolInject<TextComponent> _uitextPool = default;
    
    public void Run(EcsSystems systems)
    {
        var timeFilter = _timeFilter.Value;
        var timePool = _timePool.Value;
        
        var uiTextFilter = _uiTextFilter.Value;
        var uiTextPool = _uitextPool.Value;
        
        foreach (var i in timeFilter)
        {
            ref var globalTime = ref timePool.Get(i);
            foreach (var j in uiTextFilter)
            {
                var minute = (int) globalTime.GlobalTime / 60;
                ref var uiTextComponent = ref uiTextPool.Get(j);
                uiTextComponent.Text.text = minute + ":" + ((int)globalTime.GlobalTime-minute*60);
            }
        }
    }
}