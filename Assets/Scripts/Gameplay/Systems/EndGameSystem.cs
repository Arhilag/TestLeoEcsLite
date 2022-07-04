using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class EndGameSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<DeathPlayerComponent>> _filter = default;
    readonly EcsPoolInject<DeathPlayerComponent> _deathPlayerPool = default;
    
    readonly EcsFilterInject<Inc<ViewComponent,
    UILoseComponent>> _filterViewUI = default;
    readonly EcsPoolInject<ViewComponent> _poolViewUI = default;

    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var deathPlayerPool = _deathPlayerPool.Value;
        
        var filterViewUI = _filterViewUI.Value;
        var poolViewUI = _poolViewUI.Value;

        foreach (var entity in filter)
        {
            Time.timeScale = 0;
            foreach (var entityUI in filterViewUI)
            {
                ref var uiPool = ref poolViewUI.Get(entityUI);
                uiPool.View.Show();
                deathPlayerPool.Del(entity);
            }
        }
    }
}