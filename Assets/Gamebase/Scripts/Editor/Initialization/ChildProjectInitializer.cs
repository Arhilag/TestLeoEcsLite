using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Doozy.Engine.Settings;
using UnityEditor;
using UnityEngine;
// ReSharper disable UnusedMember.Local
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    /// <summary>
    /// Компонент, отвечающий за начальную инициализацию базы на дочеренем проекте. Пересоздает файлы локальных настроек и перечислений
    /// </summary>
    public static class ChildProjectInitializer
    {
        private static List<BuildTargetGroup> BuildTargetGroups  =>
            GamebaseBuildTargetGroups.GetAllBuildTargetGroups().ToList();

        private const string GAMEBASE_DEFINE = "GAMEBASE_INITIALIZED";
        private const string DUI_TEXTMESHPRO_DEFINE = "dUI_TextMeshPro";

        internal static void InitializeGamebase()
        {
            InvokeOnAllChildInitializers((e) => e.OnChildProjectInit());
            MoveSettings(
                $"Assets/Gamebase/Settings/GamebaseUIPrefabsPaths.asset",
                $"Assets/Resources/Settings/GamebaseUIPrefabsPaths.asset");
            MoveSettings(
                $"Assets/Gamebase/Settings/DynamicEnumsCreatorTemplates.asset",
                $"Assets/Resources/Settings/DynamicEnumsCreatorTemplates.asset");
            MoveSettings(
                $"Assets/Gamebase/Settings/ProjectContext.prefab",
                $"Assets/Resources/ProjectContext.prefab");

            //Заносим в дефайны флаг, что проинициализировали базу. После этого дефолтные перечисления будут исключены из сборки
            foreach (var item in BuildTargetGroups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(item);
                var splitDefines =
                    new List<string>(defines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                if (!splitDefines.Contains(GAMEBASE_DEFINE))
                    splitDefines.Add(GAMEBASE_DEFINE);
                if (!splitDefines.Contains(DUI_TEXTMESHPRO_DEFINE))
                {
                    DoozySettings.Instance.UseTextMeshPro = true;
                    EditorUtility.SetDirty(DoozySettings.Instance);
                    AssetDatabase.Refresh();
                    splitDefines.Add(DUI_TEXTMESHPRO_DEFINE);
                }
                defines = string.Join(";", splitDefines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(item, defines);
            }

            Debug.Log($"[GAMEBASE_INITIALIZER] Gamebase init ok");
        }

        private static void MoveSettings(string fromPath, string toPath)
        {
            if (!File.Exists(fromPath)) return;
            AssetDatabase.MoveAsset(fromPath, toPath);
            AssetDatabase.SaveAssets();
        }

#if GAMEBASE_INITIALIZED && GAMEBASE_DEVELOPMENT
        [MenuItem("Gamebase/Reset Gamebase")]
#endif
        private static void ResetGamebase()
        {
            InvokeOnAllChildInitializers((e) => e.OnChildProjectReset());

            foreach (var item in BuildTargetGroups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(item);
                var splitDefines =
                    new List<string>(defines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                if (splitDefines.Contains(GAMEBASE_DEFINE))
                    splitDefines.Remove(GAMEBASE_DEFINE);
                defines = string.Join(";", splitDefines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(item, defines);
            }
            
            MoveSettings(
                $"Assets/Resources/Settings/GamebaseUIPrefabsPaths.asset",
                $"Assets/Gamebase/Settings/GamebaseUIPrefabsPaths.asset");
            MoveSettings(
                $"Assets/Resources/Settings/DynamicEnumsCreatorTemplates.asset",
                $"Assets/Gamebase/Settings/DynamicEnumsCreatorTemplates.asset");
            MoveSettings(
                $"Assets/Resources/ProjectContext.prefab",
                $"Assets/Gamebase/Settings/ProjectContext.prefab");

            CreateOrResetBaseScenes.Reset();
            DebugMode.ModeOff();
            Debug.Log($"[GAMEBASE_INITIALIZER] Gamebase local configs disabled ok");
        }

        private static void InvokeOnAllChildInitializers(Action<IChildProjectInitializer> action)
        {
            var initializerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
            var initializers = initializerTypes.Where(t => typeof(IChildProjectInitializer).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract).ToList();

            foreach (var item in initializers)
            {
                var res = (IChildProjectInitializer)Activator.CreateInstance(item, null);
                action.Invoke(res);
            }
        }
    }
}
