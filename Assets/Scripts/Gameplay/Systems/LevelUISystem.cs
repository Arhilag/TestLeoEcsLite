using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class LevelUISystem : IEcsRunSystem, IEcsInitSystem
{
    private int _lvlNumber;
    public static Action OnLevelUp;
    
    readonly EcsFilterInject<Inc<PlayerTag,
        ExperienceCounterComponent,
        PlayerExperienceComponent>> _playerFilter = default;
    readonly EcsPoolInject<ExperienceCounterComponent> _experienceCounterPool = default;
    readonly EcsPoolInject<PlayerExperienceComponent> _experiencePlayerPool = default;
    
    readonly EcsFilterInject<Inc<TextComponent,
    UILevelComponent>> _uiTextFilter = default;
    readonly EcsPoolInject<TextComponent> _uiTextPool = default;
    
    readonly EcsFilterInject<Inc<SliderComponent,
        LevelSliderComponent>> _sliderFilter = default;
    readonly EcsPoolInject<SliderComponent> _sliderPool = default;
    
    public void Init(EcsSystems systems)
    {
        _lvlNumber = 0;
    }
    
    public void Run(EcsSystems systems)
    {
        var playerFilter = _playerFilter.Value;
        var experienceCounterPool = _experienceCounterPool.Value;
        var experiencePlayerPool = _experiencePlayerPool.Value;
        
        var uiTextFilter = _uiTextFilter.Value;
        var uiTextPool = _uiTextPool.Value;

        var sliderFilter = _sliderFilter.Value;
        var sliderPool = _sliderPool.Value;
        
        foreach (var i in playerFilter)
        {
            ref var experienceCounter = ref experienceCounterPool.Get(i);
            ref var experiencePlayer = ref experiencePlayerPool.Get(i);
            
            foreach (var entity in uiTextFilter)
            {
                ref var mainUi = ref uiTextPool.Get(entity);
                mainUi.Text.text = _lvlNumber + "";
                
                foreach (var j in sliderFilter)
                {
                    ref var slider = ref sliderPool.Get(j);
                    slider.Slider.value = experienceCounter.Experience / experiencePlayer.ExperienceToUp[_lvlNumber];
                }

                if (_lvlNumber < experiencePlayer.ExperienceToUp.Length-1)
                {
                    if (experienceCounter.Experience > experiencePlayer.ExperienceToUp[_lvlNumber])
                    {
                        _lvlNumber++;
                        experienceCounter.Experience = 0;
                        OnLevelUp?.Invoke();
                    }
                }
            }
        }
    }
}