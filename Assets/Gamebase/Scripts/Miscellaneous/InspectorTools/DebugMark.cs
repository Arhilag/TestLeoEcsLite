using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Класс, отключающий объект, на котором находится, если режим Debug в Gamebase выключен.
    /// </summary>
    [DefaultExecutionOrder(-100000)]
    public class DebugMark : MonoBehaviour
    {
#if !DEBUG_GAMEBASE
        private void Awake()
        {
            gameObject.SetActive(false);
        }
#endif
    }
}
