using Leopotam.EcsLite;

sealed class KillCountingSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsWorld _world = null;
    private int _countKill;
    
    public void Init(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        _countKill = 0;
        DeathSystem.OnEnemyDead += NewEnemyDead;
    }

    public void Destroy(EcsSystems systems)
    {
        DeathSystem.OnEnemyDead -= NewEnemyDead;
    }

    public void Run(EcsSystems systems)
    {
        var uiFilter = _world.Filter<MainUIComponent>().End();
        var uiPool = _world.GetPool<MainUIComponent>();
        
        foreach (var entity in uiFilter)
        {
            ref var uiComponent = ref uiPool.Get(entity);
            uiComponent.Text_countKill.text = _countKill +"";
        }
    }

    private void NewEnemyDead()
    {
        _countKill++;
    }
}