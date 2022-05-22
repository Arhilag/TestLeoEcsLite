using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    public class GamebaseSystemsInstaller : MonoInstaller
    {
        [Title("Запускаемые системы Gamebase")]
        
        [SerializeField] private bool globalEventsSystem = true;
        [SerializeField] private bool resourcesSystem = true;
        [SerializeField, ShowIf("resourcesSystem")] private bool rewardsSystem = true;
        [SerializeField] private bool soundSystem = true;
        [SerializeField, ShowIf("globalEventsSystem"), ShowIf("resourcesSystem")] private bool progressSystem = true;

        [Title("Источники для системы звуков")]
        [SerializeField] private AudioSources audioSources;
        
        public override void InstallBindings()
        {
            if (globalEventsSystem)
            {
                Container.BindInterfacesAndSelfTo<GlobalEventsSystem>().AsSingle().NonLazy();
            }

            if (resourcesSystem)
            {
                Container.BindInterfacesAndSelfTo<ResourcesSystem>().AsSingle().NonLazy();
            }

            if (progressSystem && resourcesSystem && globalEventsSystem)
            {
                Container.BindInterfacesAndSelfTo<ProgressSystem>().AsSingle().NonLazy();
            }

            if (rewardsSystem && resourcesSystem)
            {
                Container.BindInterfacesAndSelfTo<RewardsSystem>().AsSingle().NonLazy();
            }

            if (soundSystem)
            {
                Container.BindInterfacesAndSelfTo<SoundSystem>().AsSingle().WithArguments(audioSources);
            }
        }
    }
}