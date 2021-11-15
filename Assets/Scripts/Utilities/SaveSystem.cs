using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utilities
{
    public static class SaveSystem
    {
        public static void Save<T>(string fileName,T data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + fileName;
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream,data);
            stream.Close();
        }

        public static bool Load<T>(string fileName,out T data) where T:new()
        {
            string path = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                data = (T)formatter.Deserialize(stream);
                stream.Close();
                return true;
            }
            data = new T();
            Save(fileName,data);
            return false;
        }
    }
}
