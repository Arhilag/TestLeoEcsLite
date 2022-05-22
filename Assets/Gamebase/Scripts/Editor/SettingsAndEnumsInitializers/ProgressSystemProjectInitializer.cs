// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class ProgressSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            DynamicEnums.Init("GameFeatureType");
            CodeGenerator.CreateSettingsInstance<ProgressSettings>("ProgressSettings");
        }

        public void OnChildProjectReset()
        {
            DynamicEnums.Reset("GameFeatureType");
            CodeGenerator.RemoveSettingsInstance("ProgressSettings");
        }
    }
}
