using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabCounters : GamebaseAddPrefabBase
    {
        private static GamebaseUICounters Prefabs => GamebaseUIPrefabsPaths.Instance.counters; 
        
        private const string ELEMENT_PATH = "Counters/";
        
        private const string SIMPLE_NAME = "Simple Counter";
        private const string GRAPHIC_LEFT_NAME = "Icon Left";
        private const string GRAPHIC_RIGHT_NAME = "Icon Right";
        
        private const string SIMPLE_PATH = COMMON_PATH + ELEMENT_PATH + SIMPLE_NAME;
        private const string GRAPHIC_LEFT_PATH = COMMON_PATH + ELEMENT_PATH + GRAPHIC_LEFT_NAME;
        private const string GRAPHIC_RIGHT_PATH = COMMON_PATH + ELEMENT_PATH + GRAPHIC_RIGHT_NAME;
        
        [MenuItem(SIMPLE_PATH, false, PRIORITY)]
        private static void CreateSimple()
        {
            InstantiatePrefab(Prefabs.simple, SIMPLE_NAME);
        }
        
        [MenuItem(GRAPHIC_LEFT_PATH, false, PRIORITY)]
        private static void CreateGraphicLeft()
        {
            InstantiatePrefab(Prefabs.graphicLeft, GRAPHIC_LEFT_NAME);
        }
        
        [MenuItem(GRAPHIC_RIGHT_PATH, false, PRIORITY)]
        private static void CreateGraphicRight()
        {
            InstantiatePrefab(Prefabs.graphicRight, GRAPHIC_RIGHT_NAME);
        }
    }
}