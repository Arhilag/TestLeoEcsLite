using System.Linq;
using UnityEngine;
// ReSharper disable CheckNamespace

/// <summary>
/// Система для централизованного управления отладкой. В настройках системы можно установить фильтры для сообщений которые можно пропускать, или которые нужно игнорировать. В коде необходимо использовать методы логгирования DebugSystem вместо Debug
/// </summary>
public static class DebugSystem
{
    private const string PREFIX_STRING = "DEBUG : ";
    private static DebugSettings Settings => DebugSettings.Instance;

    /// <summary>
    /// Включено ли логирование сообщений систем геймбейза
    /// </summary>
    public static bool EnableGamebaseMessages => Settings.enableGamebaseMessages;
    
    /// <summary>
    /// Предупреждение в консоль
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarning(object message)
    {
        Log(message, isWarning: true);
    }

    /// <summary>
    /// Ошибка в консоль
    /// </summary>
    /// <param name="message">Текст сообщения</param>
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
    
    /// <summary>
    /// Сообщение в консоль
    /// </summary>
    /// <param name="message">Текст сообщения</param>
    /// <param name="isWarning">Оповещение в консоль</param>
    public static void Log(object message, bool isWarning = false)
    {
        if (Settings == null)
        {
            Debug.LogWarning("DebugSystem : settings are null! Using standart UnityEngine.Debug");
            if (isWarning)
                Debug.LogWarning(message);
            else
                Debug.Log(message);

            return;
        }

        if (!Settings.enableConsole) return;
        if (message is string msg)
        {
            bool accept = false;

            if (Settings.enableFilter)
            {
                if (Settings.keywords.Any(keyword => keyword.enabled && !string.IsNullOrEmpty(keyword.key) && msg.Contains(keyword.key)))
                {
                    accept = true;
                }
            }
            else accept = true;

            if (Settings.enableExceptFilter)
            {
                if (Settings.consoleExceptWords.Any(exceptWord => !string.IsNullOrEmpty(exceptWord) && msg.Contains(exceptWord)))
                {
                    accept = false;
                }
            }

            if (accept)
            {
                message = string.Concat(PREFIX_STRING, message);

                if (isWarning)
                    Debug.LogWarning(message);
                else
                    Debug.Log(message);
            }
        }
        else
        {
            message = string.Concat(PREFIX_STRING, message);

            if (isWarning)
            {
                Debug.LogWarning(message);
            }
            else
                Debug.Log(message);
        }
    }
}