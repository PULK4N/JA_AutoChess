using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Debug = UnityEngine.Debug;

public class SceneDataWriter
{

    /// <summary>
    /// Loads a class from a flat file as a binary object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static T Load<T>(string filename) where T : class
    {
        if (!File.Exists(filename))
            return default(T);

        try
        {
            using (Stream stream = File.OpenRead(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("SceneDataWriter : Error with file " + e.Message);
            return default(T);
        }
    }

    public static void Save<T>(string filename, T data) where T : class
    {
        using (Stream stream = File.OpenWrite(filename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
    }

}
