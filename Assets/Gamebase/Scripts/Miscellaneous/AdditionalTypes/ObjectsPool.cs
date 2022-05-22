using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

namespace Gamebase
{
    /// <summary>
    /// Пул игровых объектов
    /// </summary>
    public class ObjectsPool<T> where T : MonoBehaviour
    {
        /// <summary>
        /// Количество объектов в пуле
        /// </summary>
        public int Count => _objectsList.Count;

        /// <summary>
        /// Должен ли пул автоматически расширяться, если нет достаточного количества объектов
        /// </summary>
        public bool Expandable { get; }

        /// <summary>
        /// Объект-родитель в иерархии сцены
        /// </summary>
        public Transform ParentObject { get; }
        
        private readonly T _poolObjectPrefab;
        private readonly List<T> _objectsList;

        /// <summary>
        /// Инициализация пула объектов
        /// </summary>
        /// <param name="poolObjectPrefab">Префаб объекта для спавна</param>
        /// <param name="size">Размер пула</param>
        /// <param name="parentObject">Родительский объект</param>
        /// <param name="expandable">Расширяемый ли пул?</param>
        public ObjectsPool(T poolObjectPrefab, int size, Transform parentObject, bool expandable = false)
        {
            _objectsList = new List<T>();

            _poolObjectPrefab = poolObjectPrefab;
            Expandable = expandable;
            ParentObject = parentObject;

            for (var i = 0; i < size; i++)
            {
                Add(poolObjectPrefab);
            }
        }

        /// <summary>
        /// Индексатор, возвращает выбраный объект пула
        /// </summary>
        /// <param name="index">Индекс требуемого объекта</param>
        /// <returns></returns>
        public T this[int index]
        {
            get => _objectsList[index];
            set => _objectsList[index] = value;
        }

        private void Add(T prefab, string objectName = "")
        {
            var newObject = Object.Instantiate(prefab, ParentObject);
            if (!string.IsNullOrEmpty(objectName))
                newObject.name = $"{objectName}_{Count.ToString()}";

            newObject.gameObject.SetActive(false);
            _objectsList.Add(newObject);
        }

        /// <summary>
        /// Получить из пула объект
        /// </summary>
        /// <param name="autoActivate">Необходимо ли автоматически активировать объект</param>
        /// <returns>Возвращает объект из пула. Если нет подходящего объекта, и пул нерасширяемый - возвращает null</returns>
        public T GetObject(bool autoActivate = true)
        {
            while (true)
            {
                if (_objectsList.Count != 0)
                {
                    foreach (var candidate in _objectsList.Where(candidate => !candidate.gameObject.activeSelf))
                    {
                        if (autoActivate)
                        {
                            candidate.gameObject.SetActive(true);
                        }
                        return candidate;
                    }
                }
                
                if (!Expandable) return null;
                
                Add(_poolObjectPrefab);
            }
        }

        /// <summary>
        /// Вернуть объект в пул
        /// </summary>
        /// <param name="poolObject">Возвращаемый объект</param>
        public void ReturnToPool(T poolObject)
        {
            if (_objectsList.Contains(poolObject) && poolObject.gameObject.activeSelf)
            {
                poolObject.gameObject.SetActive(false);
            }
        }
    }
}