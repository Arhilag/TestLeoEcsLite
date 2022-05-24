using Leopotam.EcsLite;

sealed class LevelSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    private UnitConfig _playerConfig;
    public void Init(EcsSystems systems)
    {
        EcsWorld _world = systems.GetWorld ();
        
        var playerFilter = _world.Filter<PlayerTag>().Inc<ParameterComponent>().End();
        var pool = _world.GetPool<ParameterComponent>();
        foreach (var entity in playerFilter)
        {
            ref var parameter = ref pool.Get(entity);
            _playerConfig = parameter.Config;
        }
    }
    
    public void Run(EcsSystems systems)
    {
        if (_playerConfig.Experience > 500)
        {
        }
    }
}