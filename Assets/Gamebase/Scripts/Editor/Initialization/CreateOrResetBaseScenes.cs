using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace
// ReSharper disable Unity.IncorrectMethodSignature

namespace Gamebase.Editor
{
    public class CreateOrResetBaseScenes : MonoBehaviour
    {
        private const string DIR_TEMPLATES = "Assets/Gamebase/Scenes";
        private const string DIR_NEW_SCENES = "Assets/Scenes";
        private static readonly List<EditorBuildSettingsScene> EditorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        
        #region Create
        internal static void Create(string firstSceneName, List<string> additionalScenes)
        {
            CreateScenes(firstSceneName, additionalScenes);
        }

        private static void CreateScenes(string firstSceneName, List<string> additionalScenes)
        {
            // Создание директории под сцены
            if (!Directory.Exists(DIR_NEW_SCENES))
                Directory.CreateDirectory(DIR_NEW_SCENES);

            // Создание базовых сцен
            CreateSceneSequence("Loading", "Loading", "InitializationSceneLoader");
            CreateSceneSequence("Initialization", "Initialization", "Gamebase", "GameStartPoint", "InitializationSequence");

            // Создание первой игровой сцены
            CreateSceneSequence(firstSceneName, "Template", "PlayModeController");

            // Создание дополнительных сцен
            if (additionalScenes.Count > 0)
            {
                foreach (var item in additionalScenes)
                {
                    CreateSceneSequence(item, "Template", "PlayModeController");
                }
            }

            // Сохранение сцен в Build Settings
            EditorBuildSettings.scenes = EditorBuildSettingsScenes.ToArray();

            // Переход к настройке сцен и используемых систем
            OpenScene($@"{DIR_NEW_SCENES}/Loading.unity");
            AssetDatabase.Refresh();
            SetScenesSettings(firstSceneName);
        }

        private static void CreateSceneSequence(string sceneName, string templateSceneName, params string[] gamebaseSystems)
        {
            Scene scene;
            // Если сцена с нужным именем существует
            if (CheckingExistenceScene(sceneName))
            {
                if (gamebaseSystems.Length > 0)
                {
                    // Открываем сцену
                    scene = OpenScene($@"{DIR_NEW_SCENES}/{sceneName}.unity");
                    // Проходим по всем требуемым параметрам
                    foreach (var system in gamebaseSystems)
                    {
                        // Если на сцене нет нужного компонента Gamebase
                        if (!CheckingExistenceObject(system))
                            // Добавляем компонент
                            AddAssetToScene(system);
                    }
                    // Сохраняем сцену
                    SaveScene(scene, sceneName, false);
                    // Добавляем сцену в Build Settings
                    AddToBuildSettings(sceneName);
                }
            }
            // Иначе (если сцены нет)
            else
            {
                // Создаем сцену
                scene = OpenScene($@"{DIR_TEMPLATES}/{templateSceneName}.unity");
                SaveScene(scene, sceneName);
                // Добавляем сцену в Build Settings
                AddToBuildSettings(sceneName);
            }
        }

        private static bool CheckingExistenceScene(string sceneName)
        {
            return File.Exists($@"{DIR_NEW_SCENES}/{sceneName}.unity");
        }

        private static bool CheckingExistenceObject(string objectName)
        {
            return Find(objectName) != null;
        }

        private static Scene OpenScene(string scenePath)
        {
            return EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        private static void SaveScene(Scene scene, string sceneName, bool saveAsCopy = true)
        {
            EditorSceneManager.SaveScene(scene, $@"{DIR_NEW_SCENES}/{sceneName}.unity", saveAsCopy);
        }

        private static readonly string dirAssets = "Assets/Gamebase/Prefabs/GamebaseSystems";
        private static void AddAssetToScene(string objectName)
        {
            var prefab = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath($@"{dirAssets}/{objectName}.prefab", typeof(Object)));
            prefab.name = objectName;
            if (objectName == "GameStartPoint")
            {
                Find("GameStartPoint").SetActive(false);
            }
        }

        private static void AddToBuildSettings(string sceneName)
        {
            EditorBuildSettingsScenes.Add(new EditorBuildSettingsScene($@"{DIR_NEW_SCENES}/{sceneName}.unity", true));
        }

        private static void SetScenesSettings(string firstSceneName)
        {
            var loadingSceneField = new SceneField("Loading");
            var initializationSceneField = new SceneField("Initialization");
            var firstSceneSceneField = new SceneField($@"{firstSceneName}");

            AssetDatabase.StartAssetEditing();
            ScenesSettings.Instance.LoadingScene = loadingSceneField;
            ScenesSettings.Instance.InitializationScene = initializationSceneField;
            ScenesSettings.Instance.FirstScene = firstSceneSceneField;
            AssetDatabase.StopAssetEditing();
            EditorUtility.SetDirty(ScenesSettings.Instance);
            AssetDatabase.SaveAssets();

            if (ScenesSettings.Instance.LoadingScene != loadingSceneField ||
                ScenesSettings.Instance.InitializationScene != initializationSceneField ||
                ScenesSettings.Instance.FirstScene != firstSceneSceneField)
            {
                // Этот код недостижим в нормальной ситуации, но на всякий случай пусть будет
                SetScenesSettings(firstSceneName);
            }
        }
        #endregion

        #region Reset
        private static readonly string dirScenes = "Assets/Scenes";
        internal static void Reset()
        {
            if (!Directory.Exists(dirScenes))
                return;

            var scenes = Directory.GetFiles(dirScenes, "*.unity");
            if (scenes.Length > 0)
            {
                foreach (var scenePath in scenes)
                {
                    var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                    var initializationSceneLoader = Find("InitializationSceneLoader");
                    if (initializationSceneLoader != null)
                        DestroyImmediate(initializationSceneLoader, true);

                    var gameStartPoint = Find("GameStartPoint");
                    if (gameStartPoint != null)
                        DestroyImmediate(gameStartPoint, true);

                    var initializationSequence = Find("InitializationSequence");
                    if (initializationSequence != null)
                        DestroyImmediate(initializationSequence, true);

                    var playModeController = Find("PlayModeController");
                    if (playModeController != null)
                        DestroyImmediate(playModeController, true);

                    EditorSceneManager.SaveScene(scene, scenePath);
                }
            }
        }
        #endregion

        #region Find Objects
        private static Object Find(string name, System.Type type)
        {
            Object[] objects = Resources.FindObjectsOfTypeAll(type);
            foreach (var obj in objects)
            {
                if (obj.name == name)
                {
                    return obj;
                }
            }
            return null;
        }

        private static GameObject Find(string name)
        {
            return Find(name, typeof(GameObject)) as GameObject;
        }
        #endregion
    }
}
