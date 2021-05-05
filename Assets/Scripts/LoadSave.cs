using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class LoadSave
{
    public static List<LevelData> savedLevels = new List<LevelData>();

    public static void Save()
    {
        savedLevels.Add(LevelData.openLevel);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/levels.ld");
        bf.Serialize(file, LoadSave.savedLevels);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/levels.ld"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/levels.ld", FileMode.Open);
            LoadSave.savedLevels = (List<LevelData>)bf.Deserialize(file);
            file.Close();
            Debug.Log(Application.persistentDataPath);
        }
    }

}
