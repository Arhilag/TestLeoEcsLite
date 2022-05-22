using System;
using UnityEngine;
using Zenject;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable InconsistentNaming

namespace Gamebase
{
    /// <summary>
    /// Система прогресса предоставляет интерфейс для работы с уровнем игрового аватара и опытом, а также с разблокируемыми фичами игры, которые должны открываться только по достижении игровым аватаром определенного уровня.
    /// </summary>
    public class ProgressSystem
    {
        [Inject]
        public ProgressSystem(GlobalEventsSystem globalEventsSystem, ResourcesSystem resourcesSystem)
        {
            _globalEventsSystem = globalEventsSystem;
            _resourcesSystem = resourcesSystem;
        }
        
        private static ProgressSettings Settings => ProgressSettings.Instance;
        private readonly GlobalEventsSystem _globalEventsSystem;
        private readonly ResourcesSystem _resourcesSystem;

        /// <summary>
        /// Событие изменения уровня
        /// </summary>
        public Action<int> OnProgressLevelChanged;
        public int CurrentProgressLevel
        {
            get => _resourcesSystem.Int.Get(ResourceType.ProgressLevel);
            private set
            {
                var oldVal = CurrentProgressLevel;
                if (oldVal != value)
                {
                    _resourcesSystem.Int.Set(ResourceType.ProgressLevel, value);
                    OnProgressLevelChanged?.Invoke(CurrentProgressLevel);
                    _globalEventsSystem.Invoke(GlobalEventType.ProgressLevelChanged);
                }
            }
        }

        /// <summary>
        /// Событие изменения количества опыта
        /// </summary>
        public Action<int> OnXPChanged;
        public int CurrentXP
        {
            get
            {
                NoUseXPErrorCheck();
                return _resourcesSystem.Int.Get(ResourceType.Xp);
            }
            private set
            {
                NoUseXPErrorCheck();
                var oldVal = CurrentXP;
                if (oldVal != value)
                {
                    _resourcesSystem.Int.Set(ResourceType.Xp, value);
                    OnXPChanged?.Invoke(CurrentXP);
                    _globalEventsSystem.Invoke(GlobalEventType.XPChanged);
                    if (Settings.OpenLevelsWithXP)
                        CheckForNewLevel();
                }
            }
        }

        private void NoUseXPErrorCheck()
        {
            if (!Settings.UseXP)
                Debug.LogError(
                    "[ProgressSystem] - В настройках системы прогресса отключено использование XP, но произошел запрос на использование его или связанных с ним настроек! Включите использование XP, либо не используйте его в коде!");
        }

        private ProgressLevelSetting NextLvlSetting
        {
            get
            {
                NoUseXPErrorCheck();
                return Settings.levelSettings.Count > CurrentProgressLevel
                    ? Settings.levelSettings[CurrentProgressLevel]
                    : null;
            }
        }

        /// <summary>
        /// Добавить опыт
        /// </summary>
        /// <param name="count">Добавляемое количество очков опыта</param>
        public void AddXP(int count)
        {
            CurrentXP += count;
        }

        /// <summary>
        /// Открыть следующий уровень
        /// </summary>
        public void OpenNextLevel()
        {
            CurrentProgressLevel++;
        }

        private void CheckForNewLevel()
        {
            NoUseXPErrorCheck();
            while (NextLvlSetting != null && CurrentXP >= NextLvlSetting.XpRequired)
            {
                if (Settings.ClearXPAfterEachLevel)
                    CurrentXP -= NextLvlSetting.XpRequired;
                OpenNextLevel();
            }
        }

        /// <summary>
        /// Проверка доступности игровой фичи
        /// </summary>
        /// <param name="type">Тип игровой фичи</param>
        /// <returns>True, если доступна интересующая игровая фича с текущим количеством открытых уровней</returns>
        public bool FeatureIsUnlocked(GameFeatureType type)
        {
            var featureSetting = Settings.GetFeatureSetting(type);
            if (featureSetting == null)
                throw new Exception($"Не найдено фичи типа {type} в настройках {Settings.name}!");

            return CurrentProgressLevel >= featureSetting.ProgressLevelRequired;
        }

        /// <summary>
        /// Разблокировать все уровни
        /// </summary>
        public void UnlockAllLevels()
        {
            CurrentProgressLevel = 999;
        }

        /// <summary>
        /// Обнулить прогресс открытых уровней и набранных очков опыта, если его использование указано в настройках
        /// </summary>
        public void ResetProgress()
        {
            CurrentProgressLevel = 0;
            if (Settings.UseXP)
                CurrentXP = 0;
        }
    }
}