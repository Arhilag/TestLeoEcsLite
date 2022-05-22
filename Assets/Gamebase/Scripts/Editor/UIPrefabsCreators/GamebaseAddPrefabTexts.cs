using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabTexts : GamebaseAddPrefabBase
    {
        private static GamebaseUITexts Prefabs => GamebaseUIPrefabsPaths.Instance.texts; 
        
        private const string ELEMENT_PATH = "Texts/";

        private const string SIMPLE_NAME = "Simple";
        private const string TITLE_NAME = "Title";
        private const string SMALL_NAME = "Small";
        
        private const string SIMPLE_PATH = COMMON_PATH + ELEMENT_PATH + SIMPLE_NAME;
        private const string TITLE_PATH = COMMON_PATH + ELEMENT_PATH + TITLE_NAME;
        private const string SMALL_PATH = COMMON_PATH + ELEMENT_PATH + SMALL_NAME;
        
        [MenuItem(SIMPLE_PATH, false, PRIORITY)]
        private static void CreateSimple()
        {
            InstantiatePrefab(Prefabs.simple, SIMPLE_NAME);
        }
        
        [MenuItem(TITLE_PATH, false, PRIORITY)]
        private static void CreateTitle()
        {
            InstantiatePrefab(Prefabs.title, TITLE_NAME);
        }
        
        [MenuItem(SMALL_PATH, false, PRIORITY)]
        private static void CreateSmall()
        {
            InstantiatePrefab(Prefabs.small, SMALL_NAME);
        }
    }
}