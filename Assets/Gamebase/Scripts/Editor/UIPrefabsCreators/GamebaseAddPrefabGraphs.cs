using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabGraphs : GamebaseAddPrefabBase
    {
        private static GamebaseUIGraphs Prefabs => GamebaseUIPrefabsPaths.Instance.graphs; 
        
        private const string ELEMENT_PATH = "Graphs/";

        private const string DEFAULT_NAME = "Default";

        private const string DEFAULT_PATH = COMMON_PATH + ELEMENT_PATH + DEFAULT_NAME;
        
        [MenuItem(DEFAULT_PATH, false, PRIORITY)]
        private static void CreateDefault()
        {
            InstantiatePrefab(Prefabs.defaultGraph, DEFAULT_NAME);
        }
    }
}