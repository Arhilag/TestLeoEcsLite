// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class RewardsSystemProjectInitializer : IChildProjectInitializer
    {
        public void OnChildProjectInit()
        {
            DynamicEnums.Init("RewardTypeEnum");
            CodeGenerator.CreateSettingsInstance<RewardsSystemSettings>("RewardsSystemSettings");
        }

        public void OnChildProjectReset()
        {
            DynamicEnums.Reset("RewardTypeEnum");
            CodeGenerator.RemoveSettingsInstance("RewardsSystemSettings");
        }
    }
}
