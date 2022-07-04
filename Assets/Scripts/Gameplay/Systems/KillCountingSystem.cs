using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class KillCountingSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    readonly EcsFilterInject<Inc<KillCounterComponent>> _killCounterFilter = default;
    readonly EcsPoolInject<KillCounterComponent> _killCounterPool = default;
    
    readonly EcsFilterInject<Inc<TextComponent,
    UIKillsComponent>> _uiTextFilter = default;
    readonly EcsPoolInject<TextComponent> _uiTextPool = default;
    
    public void Init(EcsSystems systems)
    {
        foreach (var entity in _killCounterFilter.Value)
        {
            ref var killCounterComponent = ref _killCounterPool.Value.Get(entity);
            killCounterComponent.countKill = 0;
        }
        DeathSystem.OnEnemyDead += NewEnemyDead;
    }

    public void Destroy(EcsSystems systems)
    {
        DeathSystem.OnEnemyDead -= NewEnemyDead;
    }

    public void Run(EcsSystems systems)
    {
        var uiTextFilter = _uiTextFilter.Value;
        var killCounterPool = _killCounterPool.Value;
        
        var killCounterFilter = _killCounterFilter.Value;
        var uiTextPool = _uiTextPool.Value;
        
        foreach (var entity in uiTextFilter)
        {
            ref var uiComponent = ref uiTextPool.Get(entity);
            foreach (var i in killCounterFilter)
            {
                ref var killCounterComponent = ref killCounterPool.Get(i);
                uiComponent.Text.text = killCounterComponent.countKill +"";
            }
        }
    }

    private void NewEnemyDead()
    {
        foreach (var entity in _killCounterFilter.Value)
        {
            ref var killCounterComponent = ref _killCounterPool.Value.Get(entity);
            killCounterComponent.countKill++;
        }
    }
}