using System;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed class LevelUISystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private int _lvlNumber;
    public static Action OnLevelUp;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        _lvlNumber = 0;
    }
    
    public void Run(EcsSystems systems)
    {
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<ExperienceCounterComponent>()
            .Inc<PlayerExperienceComponent>().End();
        var experienceCounterPool = _world.GetPool<ExperienceCounterComponent>();
        var experiencePlayerPool = _world.GetPool<PlayerExperienceComponent>();
        
        var uiFilter = _world.Filter<MainUIComponent>()
            .Inc<UIComponent>()
            .Inc<LevelButtonComponent>().End();
        var mainUiPool = _world.GetPool<MainUIComponent>();
        
        foreach (var i in playerFilter)
        {
            ref var experienceCounter = ref experienceCounterPool.Get(i);
            ref var experiencePlayer = ref experiencePlayerPool.Get(i);
            
            foreach (var entity in uiFilter)
            {
                ref var mainUi = ref mainUiPool.Get(entity);
                mainUi.Text_level.text = _lvlNumber + "";
                mainUi.Levelbar.value = experienceCounter.Experience / experiencePlayer.ExperienceToUp[_lvlNumber];

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