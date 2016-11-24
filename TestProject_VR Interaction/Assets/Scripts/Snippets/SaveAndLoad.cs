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
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.comboland"); //you can call it anything you want
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
}