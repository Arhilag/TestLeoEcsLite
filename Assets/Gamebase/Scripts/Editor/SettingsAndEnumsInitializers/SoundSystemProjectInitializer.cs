// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class SoundSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            DynamicEnums.Init("SoundEnum");
            DynamicEnums.Init("MusicEnum");
            CodeGenerator.CreateSettingsInstance<SoundSettings>("SoundSettings");
        }

        public void OnChildProjectReset()
        {
            DynamicEnums.Reset("SoundEnum");
            DynamicEnums.Reset("MusicEnum");
            CodeGenerator.RemoveSettingsInstance("SoundSettings");
        }
    }
}
