using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public static class CreateBaseArchitecture
    {
        private static readonly List<string> Names = new List<string>()
        {
            "Gameplay",
            "Graphics",
            "UIControllers",
            "UIElements",
            "Services",
            "Utilities",
            "Editor"
        };
        private static readonly List<string> PreExistingAssembly = new List<string>();

        internal static void Create()
        {
            //Создает в папке скриптов основу для архитектуры (папки разных модулей с Assembly Definition в каждой)

            var assetsDir = Application.dataPath;
            var scriptsPath = $@"{assetsDir}/Scripts";
            if (!Directory.Exists(scriptsPath))
            {
                Directory.CreateDirectory(scriptsPath);
            }

            DirectoryInfo info = new DirectoryInfo(Application.dataPath);
            var gamebaseEditorAssembly = info.GetFiles("Gamebase.Editor.asmdef", SearchOption.AllDirectories);

            if (gamebaseEditorAssembly.Length > 0)
            {
                var template = File.ReadAllText(gamebaseEditorAssembly[0].FullName
                    .Replace("Gamebase.Editor.asmdef", $@"AssemblyTemplates/AssemblyTemplate.json"));
                foreach (var name in Names)
                {
                    CreateModuleCatalogue(scriptsPath, name, template);
                }
            }

            AssetDatabase.Refresh();

            CreateAssemblyReferences(scriptsPath);

            AssetDatabase.Refresh();

            Debug.Log($"[GAMEBASE_INITIALIZER] The basic architecture has been created.");
        }

        private static void CreateModuleCatalogue(string scriptsPath, string name, string template)
        {
            //Создаем папку
            var dir = $@"{scriptsPath}/{name}";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //Создаем файл Assembly Definition
            var replacedTemplate = template.Replace("{NAME}", name);
            var assemblyFile = $@"{dir}/{name}.asmdef";
            if (!File.Exists(assemblyFile))
            {
                File.WriteAllText(assemblyFile, replacedTemplate);
            }
            else
            {
                // Записываем, что файл уже существовал ранее
                PreExistingAssembly.Add(assemblyFile);
            }
        }

        /// <summary>
        /// Прописывание параметров в Assembly Definition
        /// </summary>
        private static void CreateAssemblyReferences(string scriptsPath)
        {
            var gamebaseScriptsDir = $@"Assets/Gamebase/Scripts";
            var pluginsDir = $@"Assets/Gamebase/3dParty";

            var referencesPrefix = "\"references\": [";
            var includePlatformsPrefix = "\"includePlatforms\": [";

            var oneReferencePrefix = "\"GUID:";
            var oneReferenceSuffix = "\",";

            // перебираем все файлы Assembly по очереди
            foreach (var name in Names)
            {
                // получаем полный адрес до файла, с которым будем работать
                var dir = $@"{scriptsPath}/{name}";
                var assemblyFilePath = $@"{dir}/{name}.asmdef";

                // пропускаем шаг цикла, если файл уже существовал ранее
                if (PreExistingAssembly.Contains(assemblyFilePath))
                {
                    break;
                }

                // получаем текст текущего Assembly
                var assemblyFile = File.ReadAllText(assemblyFilePath);

                // получаем имена Reference, которые нужно будет добавить
                var referencesTempalate = $@"{gamebaseScriptsDir}/Editor/AssemblyTemplates/{name}References.txt";
                if (!File.Exists(referencesTempalate))
                {
                    File.WriteAllText(referencesTempalate, "");
                }

                var referencesNames = File.ReadAllText(referencesTempalate).Split(',');

                var referencesGUIDs = new StringBuilder();
                referencesGUIDs.Append(referencesPrefix);

                // подготавливаем общие для всех ссылки на библиотеки
                var assemblyGamebase = $@"{gamebaseScriptsDir}/Gamebase.asmdef";
                var assemblyGamebaseTools = $@"{gamebaseScriptsDir}/Tools/Gamebase.Tools.asmdef";
                var assemblyZenject = $@"{pluginsDir}/Zenject/zenject.asmdef";
                var assemblyUniTask = $@"{pluginsDir}/UniTask/Runtime/UniTask.asmdef";

                referencesGUIDs
                    .Append(oneReferencePrefix).Append(AssetDatabase.AssetPathToGUID(assemblyGamebase)).Append(oneReferenceSuffix)
                    .Append(oneReferencePrefix).Append(AssetDatabase.AssetPathToGUID(assemblyGamebaseTools)).Append(oneReferenceSuffix)
                    .Append(oneReferencePrefix).Append(AssetDatabase.AssetPathToGUID(assemblyZenject)).Append(oneReferenceSuffix)
                    .Append(oneReferencePrefix).Append(AssetDatabase.AssetPathToGUID(assemblyUniTask)).Append(oneReferenceSuffix);

                // подготавливаем индивидуальные ссылки
                if (!referencesNames[0].Equals(""))
                {
                    foreach (var item in referencesNames)
                    {
                        // получаем полный адрес до требуемой библиотеки
                        var tempDir = $@"Assets/Scripts/{item}";
                        var tempAssemblyFilePath = $@"{tempDir}/{item}.asmdef";
                        // добавляем индивидуальную ссылку
                        referencesGUIDs.Append(oneReferencePrefix)
                            .Append(AssetDatabase.AssetPathToGUID(tempAssemblyFilePath))
                            .Append(oneReferenceSuffix);
                    }
                }

                // удаляем последнюю запятую
                referencesGUIDs.Remove(referencesGUIDs.Length - 1, 1);

                // подготавливаем поддерживаемые платформы
                var includePlatforms = new StringBuilder();
                includePlatforms.Append(includePlatformsPrefix);
                if (name.Equals("Editor"))
                {
                    includePlatforms.Append("\"Editor\"");
                }

                // заменяем текст на заготовки в файле, с которым идет работа
                var replacedTemplate = assemblyFile.Replace(referencesPrefix, referencesGUIDs.ToString())
                    .Replace(includePlatformsPrefix, includePlatforms.ToString());

                // прописываем результат в файл Assembly
                File.WriteAllText(assemblyFilePath, replacedTemplate);
            }

            Debug.Log($"[GAMEBASE_INITIALIZER] Assembly references are configured.");
        }
    }
}