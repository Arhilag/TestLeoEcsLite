using System;
using System.IO;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable InvalidXmlDocComment
// ReSharper disable EmptyConstructor

namespace Gamebase
{
    /// <summary>
    /// Класс для быстрого превращения любого класса в JSON-сериализуемый объект, хранящийся в persistentDataPath.
    /// Достаточно только наследовать этот класс от JSONSerialized<T>
    /// </summary>
    /// <typeparam name="T">Имя наследуемого класса</typeparam>
    [Serializable]
    public abstract class JsonSerialized<T> where T : new()
    {
        protected JsonSerialized() {}

        /// <summary>
        /// Серилизовать внесенные изменения и сохранить в файл
        /// </summary>
        public void SaveToFile()
        {
            File.WriteAllText(PersistentDataPath.Value + $"/{typeof(T)}.json", JsonUtility.ToJson(this, true));
        }

        /// <summary>
        /// Получить содержимое серилизованного файла либо создать новый файл при его отсутствии
        /// </summary>
        /// <returns></returns>
        public static T LoadOrCreate()
        {
            //Ищем файл прогресса
            string path = PersistentDataPath.Value + $"/{typeof(T)}.json";
            string jsonString = null;

            if (File.Exists(path)) //Если он есть - читаем
                jsonString = File.ReadAllText(path);

            if (jsonString != null)
            {
                T config = JsonUtility.FromJson<T>(jsonString);
                return config;
            }
            else
            {
                T newConfig = new T();
                return newConfig;
            }
        }
    }
}