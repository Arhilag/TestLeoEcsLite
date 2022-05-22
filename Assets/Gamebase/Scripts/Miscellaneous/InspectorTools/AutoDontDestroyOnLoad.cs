using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Компонент, применяющий к объекту, на котором он расположен, свойство DontDestroyOnLoad
    /// </summary>
    public class AutoDontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}