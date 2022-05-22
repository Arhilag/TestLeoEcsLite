using Zenject;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Gamebase
{
    public class GamebaseSystems : Singleton<GamebaseSystems>
    {
        [Inject] public GlobalEventsSystem GlobalEventsSystem { get; private set; }
        [Inject] public ProgressSystem ProgressSystem { get; private set; }
        [Inject] public ResourcesSystem ResourcesSystem { get; private set; }
        [Inject] public RewardsSystem RewardsSystem { get; private set; }
        [Inject] public SoundSystem SoundSystem { get; private set; }
    }
}