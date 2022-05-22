using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Zenject;
// ReSharper disable CheckNamespace
#pragma warning disable CS4014

namespace Gamebase
{
    /// <summary>
    /// Система наград позволяет массово начислять ресурсы через подготовленные структуры.
    /// </summary>
    public class RewardsSystem
    {
        [Inject]
        public RewardsSystem(ResourcesSystem resourcesSystem)
        {
            _resourcesSystem = resourcesSystem;
        }
        
        private RewardsSystemSettings Settings => RewardsSystemSettings.Instance;
        private readonly ResourcesSystem _resourcesSystem;
        
        /// <summary>
        /// Выдать награду по списку структур RewardStruct
        /// </summary>
        /// <param name="rewards">Список структур RewardStruct</param>
        /// <param name="onComplete">Метод, который необходимо выполнить по завершению выдачи (или визуализации, если она предусмотрена)</param>
        /// <param name="visualization">Подготовленная визуализация, реализующая IRewardVisualization.</param>
        public void GiveReward(List<RewardStruct> rewards, Action onComplete = null, IRewardVisualization visualization = null)
        {
            GiveRewardSequence(rewards, onComplete, visualization);
        }

        /// <summary>
        /// Выдать награду по одной подготовленной структуре RewardStruct
        /// </summary>
        /// <param name="rewardStruct">Структура RewardStruct</param>
        /// <param name="onComplete">Метод, который необходимо выполнить по завершению выдачи (или визуализации, если она предусмотрена)</param>
        /// <param name="visualization">Подготовленная визуализация, реализующая IRewardVisualization.</param>
        public void GiveReward(RewardStruct rewardStruct, Action onComplete = null, IRewardVisualization visualization = null)
        {
            GiveRewardSequence(new List<RewardStruct> { rewardStruct }, onComplete, visualization);
        }

        /// <summary>
        /// Выдать награду по подготовленной через ScriptableObject конфигурации
        /// </summary>
        /// <param name="rewardConfiguration">Готовая конфигурация RewardConfiguration</param>
        /// <param name="onComplete">Метод, который необходимо выполнить по завершению выдачи (или визуализации, если она предусмотрена)</param>
        /// <param name="visualization">Подготовленная визуализация, реализующая IRewardVisualization.</param>
        public void GiveReward(RewardConfiguration rewardConfiguration, Action onComplete = null, IRewardVisualization visualization = null)
        {
            GiveRewardSequence(rewardConfiguration.rewardStructures, onComplete, visualization);
        }

        private async UniTask GiveRewardSequence(List<RewardStruct> rewards, Action onComplete = null, IRewardVisualization visualization = null)
        {
            if (rewards.Count > 0)
            {
                AddResources(rewards);
                if (visualization != null)
                    await visualization.Invoke();
                onComplete?.Invoke();
                Log("The reward was successfully awarded.");
            }
            else
                DebugSystem.LogError("[RewardsSystem] - The reward structure does not contain any elements.");

            await UniTask.Yield();
        }

        private void AddResources(List<RewardStruct> rewards)
        {
            foreach (var reward in rewards)
            {
                var resourceType = Settings.GetResourceType(reward.type);
                var playerPrefsResourceType = _resourcesSystem.GetPlayerPrefsResourceType(resourceType);
                switch (playerPrefsResourceType)
                {
                    case PlayerPrefsResourceType.Int:
                        _resourcesSystem.Int.Add(resourceType, (int) reward.value);
                        break;
                    case PlayerPrefsResourceType.Float:
                        _resourcesSystem.Float.Add(resourceType, reward.value);
                        break;
                    default:
                        Log($"The specified resource {resourceType} is not an Int or Float type resource");
                        break;
                }
            }
        }
        
        private void Log(string message)
        {
            if (!DebugSystem.EnableGamebaseMessages) return;
            DebugSystem.Log($"[RewardsSystem] - {message}");
        }
    }
}
