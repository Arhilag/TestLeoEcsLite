using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
    // [CreateAssetMenu(fileName = "DynamicEnumsCreatorTemplates", menuName = "Settings/DynamicEnumsCreatorTemplates", order = 0)]
    public class DynamicEnumsCreatorTemplates : StaticScriptableObject<DynamicEnumsCreatorTemplates>
    {
#if UNITY_EDITOR
        public TextAsset templateFile;
        public TextAsset defaultFile;
        public TextAsset overriddenFile;
        public TextAsset scriptableObjectFile;
#endif
    }
}