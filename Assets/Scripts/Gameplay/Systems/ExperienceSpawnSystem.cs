using Leopotam.EcsLite;
using UnityEngine;

sealed class ExperienceSpawnSystem : IEcsRunSystem
{
    private EcsWorld _world = null;

    public void Run(EcsSystems systems)
    {
        _world = systems.GetWorld ();
        var filter = _world.Filter<DeathComponent>()
            .Inc<ModelComponent>()
            .Inc<ExperienceCristalComponent>().End();
        var modelPool = _world.GetPool<ModelComponent>();
        var experienceCristalPool = _world.GetPool<ExperienceCristalComponent>();
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref modelPool.Get(i);
            ref var experienceCristalComponent = ref experienceCristalPool.Get(i);
            Object.Instantiate(experienceCristalComponent.ExperienceCristal, 
                modelComponent.modelTransform.position, modelComponent.modelTransform.rotation);
        }
    }
}