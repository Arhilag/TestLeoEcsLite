using UnityEditor;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

namespace Gamebase.Editor
{
    public class GamebaseAddPrefabButtons : GamebaseAddPrefabBase
    {
        private static GamebaseUIButtons Prefabs => GamebaseUIPrefabsPaths.Instance.buttons;
        
        private const string ELEMENT_PATH = "Buttons/";
        
        private const string CIRCLE_NAME = "Circle Small";
        private const string ROUNDED_NAME = "Rounded Small";
        private const string CIRCLE_ICON_TEXT_NAME = "Circle Icon Text";
        private const string ROUNDED_ICON_TEXT_NAME = "Rounded Icon Text";
        private const string CIRCLE_TEXT_NAME = "Circle Text";
        private const string ROUNDED_TEXT_NAME = "Rounded Text";
        
        private const string OK_NAME = "Ok";
        private const string YES_NAME = "Yes";
        private const string NO_NAME = "No";
        private const string CLOSE_NAME = "Close";
        private const string SETTINGS_NAME = "Settings";

        private const string CIRCLE_PATH = COMMON_PATH + ELEMENT_PATH + CIRCLE_NAME;
        private const string ROUNDED_PATH = COMMON_PATH + ELEMENT_PATH + ROUNDED_NAME;
        private const string CIRCLE_ICON_TEXT_PATH = COMMON_PATH + ELEMENT_PATH + CIRCLE_ICON_TEXT_NAME;
        private const string ROUNDED_ICON_TEXT_PATH = COMMON_PATH + ELEMENT_PATH + ROUNDED_ICON_TEXT_NAME;
        private const string CIRCLE_TEXT_PATH = COMMON_PATH + ELEMENT_PATH + CIRCLE_TEXT_NAME;
        private const string ROUNDED_TEXT_PATH = COMMON_PATH + ELEMENT_PATH + ROUNDED_TEXT_NAME;

        private const string OK_PATH = COMMON_PATH + ELEMENT_PATH + OK_NAME;
        private const string YES_PATH = COMMON_PATH + ELEMENT_PATH + YES_NAME;
        private const string NO_PATH = COMMON_PATH + ELEMENT_PATH + NO_NAME;
        private const string CLOSE_PATH = COMMON_PATH + ELEMENT_PATH + CLOSE_NAME;
        private const string SETTINGS_PATH = COMMON_PATH + ELEMENT_PATH + SETTINGS_NAME;
        
        [MenuItem(CIRCLE_PATH, false, PRIORITY)]
        private static void CreateCircle()
        {
            InstantiatePrefab(Prefabs.circleIcon, CIRCLE_NAME);
        }
        
        [MenuItem(ROUNDED_PATH, false, PRIORITY)]
        private static void CreateRounded()
        {
            InstantiatePrefab(Prefabs.roundedIcon, ROUNDED_NAME);
        }
        
        [MenuItem(CIRCLE_ICON_TEXT_PATH, false, PRIORITY)]
        private static void CreateCircleIconText()
        {
            InstantiatePrefab(Prefabs.circleIconText, CIRCLE_ICON_TEXT_NAME);
        }
        
        [MenuItem(ROUNDED_ICON_TEXT_PATH, false, PRIORITY)]
        private static void CreateRoundedIconText()
        {
            InstantiatePrefab(Prefabs.roundedIconText, ROUNDED_ICON_TEXT_NAME);
        }
        
        [MenuItem(CIRCLE_TEXT_PATH, false, PRIORITY)]
        private static void CreateCircleText()
        {
            InstantiatePrefab(Prefabs.circleText, CIRCLE_TEXT_NAME);
        }
        
        [MenuItem(ROUNDED_TEXT_PATH, false, PRIORITY)]
        private static void CreateRoundedText()
        {
            InstantiatePrefab(Prefabs.roundedText, ROUNDED_TEXT_NAME);
        }
        
        [MenuItem(OK_PATH, false, PRIORITY)]
        private static void CreateOk()
        {
            InstantiatePrefab(Prefabs.ok, OK_NAME);
        }
        
        [MenuItem(YES_PATH, false, PRIORITY)]
        private static void CreateYes()
        {
            InstantiatePrefab(Prefabs.yes, YES_NAME);
        }
        
        [MenuItem(NO_PATH, false, PRIORITY)]
        private static void CreateNo()
        {
            InstantiatePrefab(Prefabs.no, NO_NAME);
        }
        
        [MenuItem(CLOSE_PATH, false, PRIORITY)]
        private static void CreateClose()
        {
            InstantiatePrefab(Prefabs.close, CLOSE_NAME);
        }
        
        [MenuItem(SETTINGS_PATH, false, PRIORITY)]
        private static void CreateSettings()
        {
            InstantiatePrefab(Prefabs.settings, SETTINGS_NAME);
        }
    }
}