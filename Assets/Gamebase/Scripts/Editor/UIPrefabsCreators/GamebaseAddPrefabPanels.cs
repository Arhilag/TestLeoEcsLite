using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabPanels : GamebaseAddPrefabBase
    {
        private static GamebaseUIPanels Prefabs => GamebaseUIPrefabsPaths.Instance.panels; 
        
        private const string ELEMENT_PATH = "Panels/";

        private const string BASE_PANEL_NAME = "Base Panel";
        private const string DIALOG_NAME = "Dialog Panel (with YES and NO buttons)";
        private const string INFORMATION_NAME = "Information Panel (with OK button)";
        private const string VICTORY_NAME = "Victory Panel";
        private const string DEFEAT_NAME = "Defeat Panel";

        private const string BASE_PANEL_PATH = COMMON_PATH + ELEMENT_PATH + BASE_PANEL_NAME;
        private const string DIALOG_PATH = COMMON_PATH + ELEMENT_PATH + DIALOG_NAME;
        private const string INFORMATION_PATH = COMMON_PATH + ELEMENT_PATH + INFORMATION_NAME;
        private const string VICTORY_PATH = COMMON_PATH + ELEMENT_PATH + VICTORY_NAME;
        private const string DEFEAT_PATH = COMMON_PATH + ELEMENT_PATH + DEFEAT_NAME;
        
        [MenuItem(BASE_PANEL_PATH, false, PRIORITY)]
        private static void CreateBase()
        {
            InstantiatePrefab(Prefabs.basePanel, BASE_PANEL_NAME);
        }
        
        [MenuItem(DIALOG_PATH, false, PRIORITY)]
        private static void CreateDialog()
        {
            InstantiatePrefab(Prefabs.dialog, DIALOG_NAME);
        }
        
        [MenuItem(INFORMATION_PATH, false, PRIORITY)]
        private static void CreateInformation()
        {
            InstantiatePrefab(Prefabs.information, INFORMATION_NAME);
        }
        
        [MenuItem(VICTORY_PATH, false, PRIORITY)]
        private static void CreateVictory()
        {
            InstantiatePrefab(Prefabs.victory, VICTORY_NAME);
        }
        
        [MenuItem(DEFEAT_PATH, false, PRIORITY)]
        private static void CreateDefeat()
        {
            InstantiatePrefab(Prefabs.defeat, DEFEAT_NAME);
        }
    }
}