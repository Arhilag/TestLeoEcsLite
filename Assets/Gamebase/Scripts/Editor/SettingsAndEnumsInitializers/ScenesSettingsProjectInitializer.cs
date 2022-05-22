// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class ScenesSettingsProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            CodeGenerator.CreateSettingsInstance<ScenesSettings>("ScenesSettings");
        }

        public void OnChildProjectReset()
        {
            CodeGenerator.RemoveSettingsInstance("ScenesSettings");
        }
    }
}
