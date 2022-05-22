// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class ResourcesSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            DynamicEnums.Init("ResourceTypeEnum");
            CodeGenerator.CreateSettingsInstance<ResourcesSystemSettings>("ResourcesSystemSettings");
        }

        public void OnChildProjectReset()
        {
            DynamicEnums.Reset("ResourceTypeEnum");
            CodeGenerator.RemoveSettingsInstance("ResourcesSystemSettings");
        }
    }
}