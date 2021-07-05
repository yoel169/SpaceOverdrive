using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataSaver 
{

    public static void SavePlayer (PlayerData player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/slotSave" +player.GetSlot().ToString() + ".pso";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, player);
        stream.Close();
    }

    public static PlayerData LoadPlayer(int slot)
    {
        string path = Application.persistentDataPath + "/slotSave" + slot.ToString() + "Save.pso";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save File not found in " + path);
            return null;
        }
    }
}
