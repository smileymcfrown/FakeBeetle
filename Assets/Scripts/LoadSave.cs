using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public static class LoadSave
{
    //List of LevelData class(es? instances?)
    public static List<LevelData> savedLevels = new List<LevelData>();


    // Loading and Saving Level Data to streamingAssetsPath/levels.ld so it will appear in the build folder
    // Saved levels are loaded into LevelData.savedLevels list to be accessed by the game.

    public static void Load()
    {
        // Check for the level file
        if (File.Exists(Application.streamingAssetsPath + "/levels.ld"))
        {
            // Get a binary formatter, open a file, deserialize from binary,
            // pipe it into a List<LevelData>, close it up, bing bang bosh!
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.streamingAssetsPath + "/levels.ld", FileMode.Open);
            LoadSave.savedLevels = (List<LevelData>)bf.Deserialize(file);
            file.Close();

            //To confirm file location
            Debug.Log(Application.streamingAssetsPath);
        }
    }

    public static void Save()
    {
        // Force saved levels into the right places.. Main game levels are set to specific names in a specific order.
        // All other level names are added to the list unless the name already exists in which case it replaces itself.
        // None of that is of any use if I can't get the Custom Game level loading lists working.

        if (LevelData.openLevel.levelName.Contains("razil"))
            if(savedLevels.Count == 0) { savedLevels.Add(LevelData.openLevel); }
            else { savedLevels.RemoveAt(0); savedLevels.Insert(0,LevelData.openLevel); }

        else if (LevelData.openLevel.levelName.Contains("rance"))
            if (savedLevels.Count < 1) { savedLevels.Add(LevelData.openLevel); }
            else { savedLevels.RemoveAt(1); savedLevels.Insert(1, LevelData.openLevel); }

        else if (LevelData.openLevel.levelName.Contains("gypt"))
            if (savedLevels.Count < 2) { savedLevels.Add(LevelData.openLevel); }
            else { savedLevels.RemoveAt(2); savedLevels.Insert(2, LevelData.openLevel); }

        /* Attempted to find out if new level is already in the list and replace it.
         * Could not get searching the list for a matching variable within the class within the list
         * using:
         * if (savedLevels.Any(f => f.levelName == LevelData.openLevel.levelName) != null)
         *
         * The following may work, but I've left it out until I have written code to show the contents of
         * LevelData.savedLevel to test if it is working... or not.
        */

        // else if (savedLevels.FindIndex(x => x.LevelName() == LevelData.openLevel.levelName) != -1)
        // {
        //     int i = savedLevels.FindIndex(x => x.LevelName() == LevelData.openLevel.levelName);
        //     savedLevels.RemoveAt(i); 
        //    savedLevels.Insert(i, LevelData.openLevel);
        // }
        
        else { savedLevels.Add(LevelData.openLevel); }

        // Get a binary formatter, open a file, serialize LevelData into binary and pipe it in, close it up, bing bang bosh!
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.streamingAssetsPath + "/levels.ld");
        bf.Serialize(file, LoadSave.savedLevels);
        file.Close();

        //To confirm file location
        Debug.Log(Application.streamingAssetsPath);
    }
}

