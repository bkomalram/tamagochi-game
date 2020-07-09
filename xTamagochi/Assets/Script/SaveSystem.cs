using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    public static void SavedPlayer(Tamagochi player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Save.ck";
        FileStream stream = new FileStream(path, FileMode.Create);

        TamagochiData data = new TamagochiData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static TamagochiData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Save.ck";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TamagochiData data = formatter.Deserialize(stream) as TamagochiData;            
            stream.Close();

            return data;

        } else
        {
            Debug.LogError("Save File not found");
            return null;
        }
    }
}
