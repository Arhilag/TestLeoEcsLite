// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class GlobalEventsSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            DynamicEnums.Init("GlobalEventTypeEnum");
            CodeGenerator.CreateSettingsInstance<GlobalEventsSettings>("GlobalEventsSettings");
        }

        public void OnChildProjectReset()
        {
            DynamicEnums.Reset("GlobalEventTypeEnum");
            CodeGenerator.RemoveSettingsInstance("GlobalEventsSettings");
        }
    }
}
