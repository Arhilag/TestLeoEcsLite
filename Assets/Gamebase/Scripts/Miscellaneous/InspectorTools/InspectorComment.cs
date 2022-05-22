using Sirenix.OdinInspector;
using UnityEngine;
#pragma warning disable CS4014
#pragma warning disable CS0414
// ReSharper disable NotAccessedField.Local
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Позволяет оставить комментарий на игровом объекте.
    /// </summary>
    public class InspectorComment : MonoBehaviour
    {
        [HideIf("lockEdit")]
        [TextArea(3, 10)]
        [LabelText("Комментарий к объекту:")]
        [SerializeField] private string note = "Чтобы добавить комментарий, нажмите кнопку \"Изменить комментарий\".";

        [HideIf("lockEdit")]
        [LabelText("Сохранить комментарий")]
        [Button]
        public void SaveComment() => lockEdit = true;

        [ShowIf("lockEdit")]
        [InfoBox("$note", InfoMessageType.Info)]
        [LabelText("Изменить комментарий")]
        [Button]
        public void EditComment() => lockEdit = false;

        [HideInInspector]
        [SerializeField]
        private bool lockEdit = true;
    }
}
