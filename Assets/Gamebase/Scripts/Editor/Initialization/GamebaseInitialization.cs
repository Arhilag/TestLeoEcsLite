using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS4014
// ReSharper disable ParameterHidesMember
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable UnusedMember.Local
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Gamebase.Editor
{
    public class GamebaseInitialization : OdinEditorWindow
    {
        #region Переменные
        
        private bool scenesSettings => numberPage == Page.ScenesSettings;
        private bool debugModeSettings => numberPage == Page.DebugMode;
        private bool done => numberPage == Page.Done;
        private bool scenesNameIsCorrect => ValidateFirstSceneName(firstSceneName) && ValidateAdditionalScenes(additionalScenes);
        
        private Page numberPage = 0;
        private enum Page
        {
            ScenesSettings = 0,
            DebugMode = 1,
            Done = 2
        }
        
        #endregion
        
        #region Instance

        private static GamebaseInitialization _instance;

        public static GamebaseInitialization Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Window;
                if (_instance != null) return _instance;
                _instance = GetWindow<GamebaseInitialization>(true, "Инициализация Gamebase");
                return _instance;
            }
        }

        private static GamebaseInitialization Window
        {
            get
            {
                var windows = Resources.FindObjectsOfTypeAll<GamebaseInitialization>();
                return windows.Length > 0 ? windows[0] : null;
            }
        }

        #endregion

        #region Показ окна инициализации
        
#if !GAMEBASE_INITIALIZED
        [MenuItem("Gamebase/Initialize Gamebase Window")]
#endif
        public static void ShowWindow()
        {
            GetWindow<GamebaseInitialization>(true, "Инициализация Gamebase");
        }

        #endregion

        #region Страница настроек сцен
        
        [ShowIfGroup("scenesSettings")]

        [BoxGroup("scenesSettings/ScenesSettings", showLabel: false)]
        [InfoBox("При инициализации Gamebase будет создана базовая архитектура проекта.", InfoMessageType.None)]
        [InfoBox("Будут созданы стандартные сцены: Loading, на которой можно установить арт загрузки, и Initialization" +
            ", которая загружает необходимые модули Gamebase и запускает следующую сцену.", InfoMessageType.None)]
        [InfoBox("Придумайте название первой сцены игры, которая будет загружаться после Initialization:", InfoMessageType.None)]
        [ValidateInput("ValidateFirstSceneName", "Имя должно содержать только латинские символы и цифры!", InfoMessageType.Error)]
        [LabelText("Имя первой сцены:")]
        [SerializeField]
        private string firstSceneName = "MainMenu";

        private bool ValidateFirstSceneName(string firstSceneName)
        {
            var pattern = @"^[a-zA-Z0-9]+$";
            return Regex.IsMatch(firstSceneName, pattern);
        }

        [Space]

        [BoxGroup("scenesSettings/ScenesSettings")]
        [InfoBox("Если требуется создать дополнительные сцены, рекомендуется сделать это прямо сейчас" +
            "В последствии игру в редакторе можно будет запускать с любой сцены.", InfoMessageType.None)]
        [ValidateInput("ValidateAdditionalScenes", "Имена должны содержать только латинские символы и цифры!", InfoMessageType.Error)]
        [LabelText("Дополнительные сцены:")]
        [SerializeField]
        private List<string> additionalScenes = new List<string>() { "CoreGameplay" };

        private bool ValidateAdditionalScenes(List<string> additionalScenes)
        {
            var result = true;
            var pattern = @"^[a-zA-Z0-9]+$";
            if (additionalScenes.Count > 0)
            {
                foreach (var item in additionalScenes)
                {
                    if (!Regex.IsMatch(item, pattern))
                        result = false;
                }
            }

            return result;
        }

        [BoxGroup("scenesSettings/ScenesSettings")]
        [EnableIf("scenesNameIsCorrect")]
        [Button("Далее", ButtonSizes.Large)]
        public void ShowDebugModeSettings()
        {
            numberPage++;
        }
        
        #endregion
        
        #region Страница включения режима Debug по умолчанию

        [ShowIfGroup("debugModeSettings")]

        [BoxGroup("debugModeSettings/DebugModeSettings", showLabel: false)]
        [InfoBox("Debug Mode прописывает в ProjectSettings флаг DEBUG_GAMEBASE и включает все режимы логирования. В дальнейшем Вы можете изменить режим в меню Gamebase.")]
        [SerializeField]
        [Tooltip("Включение Debug Mode после инициализации Gamebase")]
        private bool debugMode = true;

        [BoxGroup("debugModeSettings/DebugModeSettings")]
        [Button("Назад", ButtonSizes.Large)]
        public void ShowScenesSettings()
        {
            numberPage--;
        }
        
        #endregion

        #region Инициализация
        
        [InfoBox("Не закрывайте данное окно до появления соответствующей надписи во избежание ошибок при инициализации!", InfoMessageType.Warning)]
        [BoxGroup("debugModeSettings/DebugModeSettings")]
        [Button("Начать инициализацию", ButtonSizes.Large)]
        public void StartInitialize()
        {
            ChildProjectInitializer.InitializeGamebase();
            CreateBaseArchitecture.Create();
            CreateOrResetBaseScenes.Create(firstSceneName, additionalScenes);
            if (debugMode)
                DebugMode.ModeOn();
            else
                DebugMode.ModeOff();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            numberPage++;
        }
        
        #endregion

        #region Страница окончания инициализации
        
        [ShowIfGroup("done")]
        [BoxGroup("done/Done", showLabel: false)]
        [InfoBox("Инициализация успешно завершена!")]
        [Button("Приступить к работе", ButtonSizes.Large)]
        public void CloseWindow()
        {
            Close();
        }
        
        #endregion
    }
}
