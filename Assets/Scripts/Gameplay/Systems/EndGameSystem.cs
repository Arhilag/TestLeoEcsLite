using Leopotam.EcsLite;
using UnityEngine;

sealed class EndGameSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld();
        var filter = _world.Filter<DeathPlayerComponent>().End();
        var deathPlayerPool = _world.GetPool<DeathPlayerComponent>();
        
        var filterUI = _world.Filter<UIComponent>().End();
        var poolUI = _world.GetPool<UIComponent>();

        foreach (var entity in filter)
        {
            Time.timeScale = 0;
            foreach (var entityUI in filterUI)
            {
                ref var uiPool = ref poolUI.Get(entityUI);
                uiPool._view_Lose.Show();
                deathPlayerPool.Del(entity);
            }
        }
    }
}