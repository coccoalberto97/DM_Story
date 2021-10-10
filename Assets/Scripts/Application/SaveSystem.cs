using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static bool SaveData()
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string directory = Application.persistentDataPath + "/data";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = directory + "/Profile.dat";
            FileStream fileStream = new FileStream(path, FileMode.Create);

            SaveData data = new SaveData();
            data.currentHp = Player.instance.getHealth();
            data.maxHp = Player.instance.maxHealth;
            data.currentX = Player.instance.transform.position.x;
            data.currentY = Player.instance.transform.position.y;
            data.currentWeapon = 0;

            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
        return false;
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/data/Profile.dat";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            SaveData data = binaryFormatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();
            return data;
        }

        Debug.LogError("no data");
        return null;
    }
}
