using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    /// <summary>
    /// Класс, включающий/выключающий режим Debug по кнопке.
    /// </summary>
    public class DebugMode : MonoBehaviour
    {
        private static List<BuildTargetGroup> BuildTargetGroups =>
            GamebaseBuildTargetGroups.GetAllBuildTargetGroups().ToList();

        private const string DEFINE = "DEBUG_GAMEBASE";

#if GAMEBASE_INITIALIZED
        [MenuItem("Gamebase/Debug Mode On")]
#endif
        internal static void ModeOn()
        {
            // Заносим в дефайны флаг
            foreach (var item in BuildTargetGroups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(item);
                var splitDefines =
                    new List<string>(defines.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                if (!splitDefines.Contains(DEFINE))
                {
                    splitDefines.Add(DEFINE);
                }

                defines = string.Join(";", splitDefines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(item, defines);
            }

            PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
            
            Debug.Log($"[{DEFINE} MODE] - On");
        }
        
        
#if GAMEBASE_INITIALIZED
        [MenuItem("Gamebase/Debug Mode On", validate = true)]
#endif
        internal static bool ModeOnValidate()
        {
#if !DEBUG_GAMEBASE
            return true;
#else
            return false;
#endif
        }

#if GAMEBASE_INITIALIZED
        [MenuItem("Gamebase/Debug Mode Off")]
#endif
        internal static void ModeOff()
        {
            foreach (var item in BuildTargetGroups)
            {
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(item);
                var splitDefines =
                    new List<string>(defines.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                if (splitDefines.Contains(DEFINE))
                {
                    splitDefines.Remove(DEFINE);
                }

                defines = string.Join(";", splitDefines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(item, defines);
            }

            PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
            PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
            PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
            
            Debug.Log($"[{DEFINE} MODE] - Off");
        }
        
#if GAMEBASE_INITIALIZED
        [MenuItem("Gamebase/Debug Mode Off", validate = true)]
#endif
        internal static bool ModeOffValidate()
        {
#if DEBUG_GAMEBASE
            return true;
#else
            return false;
#endif
        }
    }
}
