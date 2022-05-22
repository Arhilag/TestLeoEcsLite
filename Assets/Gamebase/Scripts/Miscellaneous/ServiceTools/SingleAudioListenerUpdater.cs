using UnityEngine;
using UnityEngine.Animations;
// ReSharper disable Unity.InefficientPropertyAccess
// ReSharper disable CheckNamespace

namespace Gamebase
{
    /// <summary>
    /// Компонент, обновляющий положение единого AudioListener при переходе между сценами 
    /// </summary>
    [RequireComponent(typeof(AudioListener)),
     RequireComponent(typeof(PositionConstraint)),
     RequireComponent(typeof(RotationConstraint))]
    public class SingleAudioListenerUpdater : MonoBehaviour
    {
        private Transform _currentCameraMainTransform;
        private PositionConstraint _positionConstraint;
        private RotationConstraint _rotationConstraint;

        private void Awake()
        {
            OnSceneChanged();
        }

        public void OnSceneChanged()
        {
            _positionConstraint ??= GetComponent<PositionConstraint>();
            _rotationConstraint ??= GetComponent<RotationConstraint>();

            if (CurrentCameraMainTransformIsNull())
            {
                ResetConstraintsAndTransform();
            }

            var newCameraMain = Camera.main;
            if (newCameraMain == null) return;
            
            var newCameraMainTransform = newCameraMain.transform;
            if (_currentCameraMainTransform == newCameraMainTransform) return;

            _currentCameraMainTransform = newCameraMainTransform;
            AddNewConstraintsSource();
        }

        private bool CurrentCameraMainTransformIsNull()
        {
            if (_currentCameraMainTransform == null)
            {
                if (_positionConstraint.sourceCount > 0)
                {
                    _currentCameraMainTransform = _positionConstraint.GetSource(0).sourceTransform;
                }
            }
            return _currentCameraMainTransform == null;
        }

        private void ResetConstraintsAndTransform()
        {
            RemoveSources();
            
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        private void RemoveSources()
        {
            if (_positionConstraint.sourceCount == 0) return;
            
            _positionConstraint.RemoveSource(0);
            _rotationConstraint.RemoveSource(0);
        }

        private void AddNewConstraintsSource()
        {
            RemoveSources();
            
            var constraintSource = new ConstraintSource
            {
                sourceTransform = _currentCameraMainTransform,
                weight = 1f
            };
            
            _positionConstraint.AddSource(constraintSource);
            _rotationConstraint.AddSource(constraintSource);
        }
    }
}