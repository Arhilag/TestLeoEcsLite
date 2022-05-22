using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    public class OpenLevelScene : MonoBehaviour
    {
        [MenuItem("Gamebase/Scenes/Load Scene 1 #&1")]
        private static void LoadScene1() { LoadScene(0); }

        [MenuItem("Gamebase/Scenes/Load Scene 2 #&2")]
        private static void LoadScene2() { LoadScene(1); }

        [MenuItem("Gamebase/Scenes/Load Scene 3 #&3")]
        private static void LoadScene3() { LoadScene(2); }

        [MenuItem("Gamebase/Scenes/Load Scene 4 #&4")]
        private static void LoadScene4() { LoadScene(3); }

        [MenuItem("Gamebase/Scenes/Load Scene 5 #&5")]
        private static void LoadScene5() { LoadScene(4); }

        [MenuItem("Gamebase/Scenes/Load Scene 6 #&6")]
        private static void LoadScene6() { LoadScene(5); }

        [MenuItem("Gamebase/Scenes/Load Scene 7 #&7")]
        private static void LoadScene7() { LoadScene(6); }

        [MenuItem("Gamebase/Scenes/Load Scene 8 #&8")]
        private static void LoadScene8() { LoadScene(7); }

        [MenuItem("Gamebase/Scenes/Load Scene 9 #&9")]
        private static void LoadScene9() { LoadScene(8); }

        [MenuItem("Gamebase/Scenes/Load Scene 10 #&0")]
        private static void LoadScene10() { LoadScene(9); }

        private static void LoadScene(int buildIndex)
        {
            if (SceneManager.sceneCountInBuildSettings < buildIndex + 1) return;
            
            var sceneByBuildIndex = EditorBuildSettings.scenes[buildIndex];
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(sceneByBuildIndex.path);
        }
    }
}