using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable AssignNullToNotNullAttribute

namespace Gamebase.Editor
{
    public static class CodeGenerator
    {
        private static string GetTemplateAndOverridenFiles(string className, out string overridenFile)
        {
            var files = Directory
                .EnumerateFiles(Application.dataPath, $"{className}_Template.txt", SearchOption.AllDirectories)
                .ToList();
            if (files.Count == 0)
            {
                Debug.LogError(
                    $"[CodeGenerator] Can`t find file |{className}_Template.txt| in dirs under |{Application.dataPath}|");
                overridenFile = null;
                return null;
            }

            var templateFile = File.ReadAllText($"{Path.GetDirectoryName(files[0])}/{className}_Template.txt");
            overridenFile = $"{Path.GetDirectoryName(files[0])}/Overridden/{className}.cs";
            return templateFile;
        }

        public static void GenerateFromTemplate(string className)
        {
            var templateFile = GetTemplateAndOverridenFiles(className, out var overridenFile);
            if (templateFile == null) return;

            if (!Directory.Exists(Path.GetDirectoryName(overridenFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(overridenFile));

            File.WriteAllText(overridenFile, templateFile);

            AssetDatabase.Refresh();

            Debug.Log($"[CodeGenerator] {className} has been created at |{Path.GetFullPath(overridenFile)}|");
        }

        public static void RemoveGeneratedClass(string className)
        {
            var templateFile = GetTemplateAndOverridenFiles(className, out var overridenFile);
            if (templateFile == null) return;

            if (File.Exists(overridenFile)) File.Delete(overridenFile);

            AssetDatabase.Refresh();

            Debug.Log($"[CodeGenerator] {className} has been removed at |{Path.GetFullPath(overridenFile)}|");
        }

        public static void CreateSettingsInstance<T>(string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            var dir = $"Assets/Resources/Settings";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var path = $"{dir}/{name}.asset";
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"[CodeGenerator] {name} has been created at |{path}|");
        }

        public static void RemoveSettingsInstance(string name)
        {
            var path = $"{Application.dataPath}/Resources/Settings/";
            var files = Directory.EnumerateFiles(path, $"{name}.asset", SearchOption.AllDirectories).ToList();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                    AssetDatabase.Refresh();
                    Debug.Log($"[CodeGenerator] {name} has been deleted at |{path}|");
                }
            }
        }
    }
}