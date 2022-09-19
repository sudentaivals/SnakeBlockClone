using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData<T>(T data, string fileAdditionalpath)
    {
        var binaryFormetter = new BinaryFormatter();
        var path = Application.persistentDataPath + fileAdditionalpath;
        FileStream fs = new FileStream(path, FileMode.Create);

        binaryFormetter.Serialize(fs, data);

        fs.Close();
    }

    public static T GetData<T>(string fileAdditionalpath) where T: class, new()
    {
        if (!typeof(T).IsSerializable) return null;
        var path = Application.persistentDataPath + fileAdditionalpath;
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            var levelData = binaryFormatter.Deserialize(fs) as T;
            fs.Close();
            return levelData;
        }
        else
        {
            SaveData(new T(), fileAdditionalpath);
            return GetData<T>(fileAdditionalpath);
        }

    }

    public static void ClearSaveData()
    {
        var path = Application.persistentDataPath;
        DirectoryInfo info = new DirectoryInfo(path);
        foreach (FileInfo file in info.GetFiles())
        {
            file.Delete();
        }
    }

}
