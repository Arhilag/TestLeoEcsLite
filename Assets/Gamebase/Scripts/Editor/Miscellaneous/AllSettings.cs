using Sirenix.OdinInspector.Editor;
#if GAMEBASE_INITIALIZED
using UnityEditor;
#endif
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class AllSettings : OdinMenuEditorWindow
    {
#if GAMEBASE_INITIALIZED
    [MenuItem("Gamebase/Settings")]
#endif
        public static void ShowWindow()
        {
            GetWindow<AllSettings>("Настройки Gamebase").Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree
            {
                {"События", GlobalEventsSettings.Instance},
                {"Ресурсы", ResourcesSystemSettings.Instance},
                {"Прогресс", ProgressSettings.Instance},
                {"Звуки", SoundSettings.Instance},
                {"Награды", RewardsSystemSettings.Instance},
                {"Сцены", ScenesSettings.Instance},
                {"Логирование", DebugSettings.Instance}
            };

            return tree;
        }
    }
}