using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

sealed class HpSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<HpComponent>> _filterHp = default;
    readonly EcsPoolInject<HpComponent> _poolHp = default;
    readonly EcsPoolInject<PlayerTag> _poolPlayer = default;
    readonly EcsPoolInject<DeathPlayerComponent> _deathPlayerPool = default;
    readonly EcsPoolInject<DeathComponent> _deathPool = default;
    
    public void Run(EcsSystems systems)
    {
        var filterHp = _filterHp.Value;
        var poolHp = _poolHp.Value;
        var poolPlayer = _poolPlayer.Value;

        foreach (var entity in filterHp)
        {
            ref var hpComponent = ref poolHp.Get(entity);
            if (hpComponent.HP <= 0)
            {
                if (poolPlayer.Has(entity))
                {
                    var deathPlayerPool = _deathPlayerPool.Value;
                    deathPlayerPool.Add(entity);
                    break;
                }

                var deathPool = _deathPool.Value;
                deathPool.Add(entity);
            }
        }
    }
}