using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class ExperienceSpawnSystem : IEcsRunSystem
{
    readonly EcsFilterInject<Inc<DeathComponent,
        ModelComponent,
        ExperienceCristalComponent>> _filter = default;
    readonly EcsPoolInject<ModelComponent> _modelPool = default;
    readonly EcsPoolInject<ExperienceCristalComponent> _experienceCristalPool = default;

    public void Run(EcsSystems systems)
    {
        var filter = _filter.Value;
        var modelPool = _modelPool.Value;
        var experienceCristalPool = _experienceCristalPool.Value;
        
        foreach (var i in filter)
        {
            ref var modelComponent = ref modelPool.Get(i);
            ref var experienceCristalComponent = ref experienceCristalPool.Get(i);
            Object.Instantiate(experienceCristalComponent.ExperienceCristal, 
                modelComponent.modelTransform.position, modelComponent.modelTransform.rotation);
        }
    }
}