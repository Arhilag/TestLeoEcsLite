using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabBase
    {
        protected const int PRIORITY = -10;
        protected const string COMMON_PATH = "GameObject/Gamebase/";
        private const string SETTINGS_PATH = "/Assets/Resources/Settings/GamebaseUIPrefabsPaths";

        protected static void InstantiatePrefab(GameObject prefab, string name)
        {
            if (prefab == null)
            {
                Debug.LogError($"[GamebaseAddPrefab] - В SO {SETTINGS_PATH} не указана ссылка на префаб {name}.");
                return;
            }

            var element = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            var parent = GetParent();
            if (parent != null)
            {
                element.transform.SetParent(parent);
            }
            element.transform.SetAsLastSibling();
            element.transform.localPosition = Vector3.zero;
            element.transform.localScale = Vector3.one;

            Selection.activeGameObject = element;
            EditorUtility.SetDirty(element);
            Undo.RegisterCreatedObjectUndo(element, $"Create {name}");
        }

        private static Transform GetParent()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length == 1)
            {
                return selectedObjects[0].transform;
            }
            
            var prefab = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefab != null)
            {
                return prefab.prefabContentsRoot.transform;
            }

            return null;
        }
    }
}