using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
// ReSharper disable CheckNamespace
// ReSharper disable AssignNullToNotNullAttribute

namespace Gamebase
{
#if UNITY_EDITOR
    /// <summary>
    /// Система для кодогенерации при изменениях в инспекторе, позволяет геймдизайнерам не касаться кода,
    /// когда требуется добавить новый вариант какого-либо перечисления.
    /// </summary>
    public static class DynamicEnums
    {
        private static string GetTemplateAndOverridenFiles(string className, out string overridenFile)
        {
            var files = Directory
                .EnumerateFiles(Application.dataPath, $"{className}_Default.cs", SearchOption.AllDirectories).ToList();
            if (files.Count == 0)
            {
                Debug.LogError(
                    $"[DynamicEnums] Can`t find file |{className}_Default.cs| in dirs under |{Application.dataPath}|");
                overridenFile = null;
                return null;
            }

            var templateFile = File.ReadAllText($"{Path.GetDirectoryName(files[0])}/{className}_Template.txt");
            overridenFile = $"{Path.GetDirectoryName(files[0])}/Overridden/{className}_Overridden.cs";

            return templateFile;
        }

        /// <summary>
        /// Обновить содержимое Enum
        /// </summary>
        /// <param name="className">Имя класса</param>
        /// <param name="valuesNames">Новый список элементов</param>
        public static void ReplaceAndSave(string className, List<string> valuesNames)
        {
            var templateFile = GetTemplateAndOverridenFiles(className, out var overridenFile);
            if (templateFile == null) return;

            var values = valuesNames.Where(e => !string.IsNullOrEmpty(e)).ToList();

            if (values.Count > 0)
            {
                foreach (var val in values)
                {
                    if (templateFile.Contains(val)) continue;
                    templateFile = templateFile.Replace("@1", $"{val}={val.GetHashCode()},\n@1");
                }
            }

            templateFile = templateFile.Replace("@1", "").Replace(",\n\n}", "\n}");

            if (!Directory.Exists(Path.GetDirectoryName(overridenFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(overridenFile));
            File.WriteAllText(overridenFile, templateFile);

            AssetDatabase.Refresh();

            Debug.Log($"[DynamicEnums] Enum({className}) was written to file |{Path.GetFullPath(overridenFile)}|");
        }

        /// <summary>
        /// Инициализировать динамический Enum
        /// </summary>
        /// <param name="className">Имя класса</param>
        public static void Init(string className)
        {
            var templateFile = GetTemplateAndOverridenFiles(className, out var overridenFile);
            if (templateFile == null) return;

            templateFile = templateFile.Replace("@1", "").Replace(",\n\n}", "\n}");

            if (!Directory.Exists(Path.GetDirectoryName(overridenFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(overridenFile));
            File.WriteAllText(overridenFile, templateFile);

            AssetDatabase.Refresh();

            Debug.Log($"[DynamicEnums] Init({className}) - |{Path.GetFullPath(overridenFile)}|");
        }

        /// <summary>
        /// Отключить динамический Enum
        /// </summary>
        /// <param name="className">Имя класса</param>
        public static void Reset(string className)
        {
            var templateFile = GetTemplateAndOverridenFiles(className, out var overridenFile);
            if (templateFile == null) return;

            if (File.Exists(overridenFile)) File.Delete(overridenFile);

            AssetDatabase.Refresh();

            Debug.Log($"[DynamicEnums] Reset({className}) - |{Path.GetFullPath(overridenFile)}|");
        }
    }
#endif
}