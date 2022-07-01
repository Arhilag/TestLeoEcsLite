using Leopotam.EcsLite;

sealed class HpBarSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world = null;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
    }

    public void Run(EcsSystems systems)
    {
        var playerFilter = _world.Filter<PlayerTag>()
            .Inc<HpComponent>().End();
        var playerHpPool = _world.GetPool<HpComponent>();
        
        var uiFilter = _world.Filter<MainUIComponent>().End();
        var uiPool = _world.GetPool<MainUIComponent>();
        
        foreach (var entity in playerFilter)
        {
            ref var playerHp = ref playerHpPool.Get(entity);
            foreach (var i in uiFilter)
            {
                ref var uiHp = ref uiPool.Get(i);
                uiHp.HPbar.value = playerHp.HP / playerHp.MaxHP;
            }
        }
    }
}