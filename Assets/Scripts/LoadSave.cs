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
        if (LevelData.openLevel.levelName.Contains("razil"))
            if(savedLevels.Count < 0)
            { savedLevels.Add(LevelData.openLevel); }
        else
        { savedLevels.Insert(0,LevelData.openLevel); }
        else if (LevelData.openLevel.levelName.Contains("rance"))
            if (savedLevels.Count < 1)
            { savedLevels.Add(LevelData.openLevel); }
            else
            { savedLevels.Insert(1, LevelData.openLevel); }
        else if (LevelData.openLevel.levelName.Contains("gypt"))
            if (savedLevels.Count < 2)
            { savedLevels.Add(LevelData.openLevel); }
            else
            { savedLevels.Insert(2, LevelData.openLevel); }
        else { savedLevels.Add(LevelData.openLevel); }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.streamingAssetsPath + "/levels.ld");
        bf.Serialize(file, LoadSave.savedLevels);
        file.Close();
        Debug.Log(Application.streamingAssetsPath);
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/levels.ld"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.streamingAssetsPath + "/levels.ld", FileMode.Open);
            LoadSave.savedLevels = (List<LevelData>)bf.Deserialize(file);
            file.Close();
            Debug.Log(Application.streamingAssetsPath);
        }
    }

}

