using Leopotam.EcsLite;

sealed class HpSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld();
        var filterHp = _world.Filter<HpComponent>().End();
        var poolHp = _world.GetPool<HpComponent>();
        var poolPlayer = _world.GetPool<PlayerTag>();

        foreach (var entity in filterHp)
        {
            ref var hpComponent = ref poolHp.Get(entity);
            if (hpComponent.HP <= 0)
            {
                if (poolPlayer.Has(entity))
                {
                    var deathPlayerPool = _world.GetPool<DeathPlayerComponent>();
                    deathPlayerPool.Add(entity);
                    break;
                }
                var deathPool = _world.GetPool<DeathComponent>();
                deathPool.Add(entity);
            }
        }
    }
}