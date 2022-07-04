using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class HpBarSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<PlayerTag,
        HpComponent>> _timeFilter = default;
    readonly EcsPoolInject<HpComponent> _timePool = default;
    
    readonly EcsFilterInject<Inc<SliderComponent,
    HPBarSliderComponent>> _uiFilter = default;
    readonly EcsPoolInject<SliderComponent> _uiPool = default;

    public void Run(EcsSystems systems)
    {
        var playerFilter = _timeFilter.Value;
        var playerHpPool = _timePool.Value;

        var uiFilter = _uiFilter.Value;
        var uiPool = _uiPool.Value;
        
        foreach (var entity in playerFilter)
        {
            ref var playerHp = ref playerHpPool.Get(entity);
            foreach (var i in uiFilter)
            {
                ref var uiHp = ref uiPool.Get(i);
                uiHp.Slider.value = playerHp.HP / playerHp.MaxHP;
            }
        }
    }
}