using Zenject;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    public class ProgressSystemResourcesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProgressLevel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Xp>().AsSingle().NonLazy();
        }
    }
}