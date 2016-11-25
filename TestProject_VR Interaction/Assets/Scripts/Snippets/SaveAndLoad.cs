using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{

    public static List<GameFile> savedGames = new List<GameFile>();

    //it's static so we can call it from anywhere
    public static void Save()
    {
        savedGames.Add(GameFile.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.comboland");
        bf.Serialize(file, savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedData.comboland"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.comboland", FileMode.Open);
            savedGames = (List<GameFile>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static bool FileExists()
    {
        return File.Exists(Application.persistentDataPath + "/savedData.comboland");
    }
}