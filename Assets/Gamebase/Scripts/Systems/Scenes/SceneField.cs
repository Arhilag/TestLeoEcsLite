using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif
// ReSharper disable CheckNamespace
// ReSharper disable NotAccessedField.Local
// ReSharper disable MemberInitializerValueIgnored
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace Gamebase
{
    [Serializable]
    public class SceneField
    {
        [SerializeField] private Object sceneAsset;
        [SerializeField] private string sceneName = "";

        public string SceneName => sceneName;

        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }

        public SceneField(string sceneName)
        {
            this.sceneName = sceneName;
            #if UNITY_EDITOR
            sceneAsset = GetSceneObject(this.sceneName);
            #endif
        }

        #if UNITY_EDITOR
        private SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneObjectName) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }
            Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
            return null;
        }
        #endif
    }
}