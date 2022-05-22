using Sirenix.OdinInspector;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    //[CreateAssetMenu(menuName = "Settings/GamebaseUIPrefabsPaths", fileName = "GamebaseUIPrefabsPaths")]
    public class GamebaseUIPrefabsPaths : StaticScriptableObject<GamebaseUIPrefabsPaths>
    {
#if UNITY_EDITOR
        [Title("Настройки стандартный префабов UI")]
        public GamebaseUIGraphs graphs;
        public GamebaseUICanvaces canvaces;
        public GamebaseUIPanels panels;
        public GamebaseUIButtons buttons;
        public GamebaseUITexts texts;
        public GamebaseUICounters counters;
        public GamebaseUISafeAreas safeAreas;
#endif
    }
}