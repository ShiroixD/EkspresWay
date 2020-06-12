using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public const string pathFile = "/EkspresWayData.file";
    public static void SaveGameData(int stage, float speedLimit, float timeLimit, float spawnTimeDelay, float obstaclePercentage, int obstacleGap, int comboTimeBonusLimit)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + pathFile;
        if (File.Exists(path))
            File.Delete(path);
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData(stage, speedLimit, timeLimit, spawnTimeDelay, obstaclePercentage, obstacleGap, comboTimeBonusLimit);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + pathFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path + ". Creating new file.");
            return null;
        }
    
    }
}
