using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArcGisMapEditor.Model.Helpers
{
    public static class ObjectCloner
    {
        /// <summary>
        /// Выполняет полное клонирование объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта копирования</typeparam>
        /// <param name="obj">Объект для копирования</param>
        /// <returns>Копия объекта</returns>
        public static T DeepClone<T>(this T obj)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "obj");
            }

            // Не сериализуем пустой объект, просто возвращаем значение по умолчанию
            if (Object.ReferenceEquals(obj, null))
            {
                return default(T);
            }

            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }         
    }
}