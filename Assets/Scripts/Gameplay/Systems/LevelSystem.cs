using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

sealed class LevelSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private PlayerConfig _playerConfig;
    private float _globalTime;
    private TextMeshProUGUI _text_time;
    public void Init(EcsSystems systems)
    {
        EcsWorld _world = systems.GetWorld ();
        
        var playerFilter = _world.Filter<PlayerTag>().Inc<PlayerParameterComponent>().End();
        var parameterpool = _world.GetPool<PlayerParameterComponent>();
        var UIFilter = _world.Filter<MainUIComponent>().End();
        var UIpool = _world.GetPool<MainUIComponent>();
        foreach (var entity in playerFilter)
        {
            ref var parameter = ref parameterpool.Get(entity);
            _playerConfig = parameter.Config;
        }

        _globalTime = 0;
        foreach (var entity in UIFilter)
        {
            ref var UI = ref UIpool.Get(entity);
            _text_time = UI.Text_time;
        }
    }
    
    public void Run(EcsSystems systems)
    {
        _globalTime += Time.deltaTime;
        var minute = (int) _globalTime / 60;
        _text_time.text = minute + ":" + ((int)_globalTime-minute*60); 
        
        if (_playerConfig.Experience > 500)
        {
        }
    }
}