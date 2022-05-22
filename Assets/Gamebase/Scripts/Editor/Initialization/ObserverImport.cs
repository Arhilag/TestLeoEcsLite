using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase.Editor
{
#if !GAMEBASE_INITIALIZED
    [InitializeOnLoad]
    public class ObserverImport : MonoBehaviour
    {
        static ObserverImport()
        {
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
        }

        private static void OnImportPackageCompleted(string packagename)
        {
            if (GamebaseInitialization.Instance == null)
            {
                GamebaseInitialization.ShowWindow();
                AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;
            }
        }
    }
#endif
}
