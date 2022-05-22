using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabCanvaces : GamebaseAddPrefabBase
    {
        private static GamebaseUICanvaces Prefabs => GamebaseUIPrefabsPaths.Instance.canvaces; 
        
        private const string ELEMENT_PATH = "Canvaces/";

        private const string BASE_CANVAS_NAME = "Default";
        private const string HORIZONTAL_SHRINK_NAME = "Horizontal Shrink";
        private const string HORIZONTAL_EXPAND_NAME = "Horizontal Expand";
        private const string VERTICAL_SHRINK_NAME = "Vertical Shrink";
        private const string VERTICAL_EXPAND_NAME = "Vertical Expand";

        private const string BASE_CANVAS_PATH = COMMON_PATH + ELEMENT_PATH + BASE_CANVAS_NAME;
        private const string HORIZONTAL_SHRINK_PATH = COMMON_PATH + ELEMENT_PATH + HORIZONTAL_SHRINK_NAME;
        private const string HORIZONTAL_EXPAND_PATH = COMMON_PATH + ELEMENT_PATH + HORIZONTAL_EXPAND_NAME;
        private const string VERTICAL_SHRINK_PATH = COMMON_PATH + ELEMENT_PATH + VERTICAL_SHRINK_NAME;
        private const string VERTICAL_EXPAND_PATH = COMMON_PATH + ELEMENT_PATH + VERTICAL_EXPAND_NAME;
    
        [MenuItem(BASE_CANVAS_PATH, false, PRIORITY)]
        private static void CreateBaseCanvas()
        {
            InstantiatePrefab(Prefabs.baseCanvas, BASE_CANVAS_NAME);
        }
        
        [MenuItem(HORIZONTAL_SHRINK_PATH, false, PRIORITY)]
        private static void CreateHorizontalShrinkCanvas()
        {
            InstantiatePrefab(Prefabs.horizontalShrink, HORIZONTAL_SHRINK_NAME);
        }
        
        [MenuItem(HORIZONTAL_EXPAND_PATH, false, PRIORITY)]
        private static void CreateHorizontalExpandCanvas()
        {
            InstantiatePrefab(Prefabs.horizontalExpand, HORIZONTAL_EXPAND_NAME);
        }
        
        [MenuItem(VERTICAL_SHRINK_PATH, false, PRIORITY)]
        private static void CreateVerticalShrinkCanvas()
        {
            InstantiatePrefab(Prefabs.verticalShrink, VERTICAL_SHRINK_NAME);
        }
        
        [MenuItem(VERTICAL_EXPAND_PATH, false, PRIORITY)]
        private static void CreateVerticalExpandCanvas()
        {
            InstantiatePrefab(Prefabs.verticalExpand, VERTICAL_EXPAND_NAME);
        }
    }
}