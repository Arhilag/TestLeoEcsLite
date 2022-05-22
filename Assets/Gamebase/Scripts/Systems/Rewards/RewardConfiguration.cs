using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Конфигурация награды.
    /// </summary>
#if GAMEBASE_INITIALIZED
    [CreateAssetMenu(menuName = "Settings/Reward Configuration")]
#endif
    public class RewardConfiguration : ScriptableObject
    {
        [Title("Конфигурация награды.")]

        [Tooltip("Список наград для выдачи.")]
        public List<RewardStruct> rewardStructures;
    }
}
