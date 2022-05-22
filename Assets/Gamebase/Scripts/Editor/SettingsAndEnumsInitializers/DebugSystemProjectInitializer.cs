// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class DebugSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            CodeGenerator.CreateSettingsInstance<DebugSettings>("DebugSettings");
        }

        public void OnChildProjectReset()
        {
            CodeGenerator.RemoveSettingsInstance("DebugSettings");
        }
    }
}
