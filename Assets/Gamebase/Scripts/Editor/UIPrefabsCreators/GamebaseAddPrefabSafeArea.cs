using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabSafeArea : GamebaseAddPrefabBase
    {
        private static GamebaseUISafeAreas Prefabs => GamebaseUIPrefabsPaths.Instance.safeAreas;
        
        private const string ELEMENT_PATH = "Safe Area/";
        
        private const string PORTRAIT_NAME = "Portrait";
        private const string LANDSCAPE_NAME = "Landspace";

        private const string PORTRAIT_PATH = COMMON_PATH + ELEMENT_PATH + PORTRAIT_NAME;
        private const string LANDSCAPE_PATH = COMMON_PATH + ELEMENT_PATH + LANDSCAPE_NAME;
        
        [MenuItem(PORTRAIT_PATH, false, PRIORITY)]
        private static void CreatePortrait()
        {
            InstantiatePrefab(Prefabs.portrait, PORTRAIT_NAME);
        }
        
        [MenuItem(LANDSCAPE_PATH, false, PRIORITY)]
        private static void CreateLandscape()
        {
            InstantiatePrefab(Prefabs.landspace, LANDSCAPE_NAME);
        }
    }
}