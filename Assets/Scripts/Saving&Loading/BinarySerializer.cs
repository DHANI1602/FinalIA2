using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySerializer
{
    public static void SaveBinary<T>(T objToSerialize, string absolutePath)
    {
        var binaryFormatter = new BinaryFormatter();
        var file = File.Create(absolutePath);
        binaryFormatter.Serialize(file, objToSerialize);

        file.Close();
    }

    public static T LoadBinary<T>(string absolutePath)
    {
        if (!File.Exists(absolutePath))
        {
            return default(T);
        }
        var binaryFormatter = new BinaryFormatter();
        var file = File.Open(absolutePath, FileMode.Open);
        var deserializedObj = (T)binaryFormatter.Deserialize(file);
        file.Close();
        return deserializedObj;
    }
}
