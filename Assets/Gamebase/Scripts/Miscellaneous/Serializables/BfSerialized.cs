using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
// ReSharper disable once CheckNamespace
// ReSharper disable EmptyConstructor
// ReSharper disable InvalidXmlDocComment
// ReSharper disable ConvertToUsingDeclaration

namespace Gamebase
{
    /// <summary>
    /// Класс для быстрого превращения любого класса в бинарный сериализуемый объект, хранящийся в persistentDataPath.
    /// Достаточно только наследовать этот класс от BfSerialized<T> и проставить классу атрибут [Serializable].
    /// </summary>
    /// <typeparam name="T">Имя наследуемого класса</typeparam>
    [Serializable]
    public abstract class BfSerialized<T> where T : new()
    {
        protected BfSerialized() {}
        
        /// <summary>
        /// Серилизовать внесенные изменения и сохранить в файл
        /// </summary>
        public void SaveToFile()
        {
            var formatter = new BinaryFormatter();
            var path = PersistentDataPath.Value + $"/{typeof(T)}.gd";
            using (var file = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(file, this);
            }
        }
        
        /// <summary>
        /// Получить содержимое серилизованного файла либо создать новый файл при его отсутствии
        /// </summary>
        /// <returns></returns>
        public static T LoadOrCreate()
        {
            var formatter = new BinaryFormatter();
            var path = PersistentDataPath.Value + $"/{typeof(T)}.gd";
            T newConfig;
            
            if (File.Exists(path))
            {
                try
                {
                    using (var file = new FileStream(path, FileMode.Open))
                    {
                        newConfig = (T) formatter.Deserialize(file);
                    }
                }
                catch
                {
                    Debug.LogError($"An error occurred while reading the {typeof(T)} file. The file may have been modified. The default values are used.");
                    newConfig = new T();
                }
            }
            else
            {
                newConfig = new T();
            }
            
            return newConfig;
        }
    }
}