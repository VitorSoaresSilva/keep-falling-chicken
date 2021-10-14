using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private const string file = "/player2.pd";
    public static void SaveGame() // modificar
    {
        PlayerData playerData = GameManager.Instance.GetPlayerData();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + file;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream,playerData);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + file;
        PlayerData data;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }

        Debug.LogError("Save file not found");
        data = new PlayerData();
        PowerUpsManager.Instance.PowerUpsInit(data.powerUpLevels);
        SaveGame();
        return data;
    }

    public static void __DeleteSave()
    {
        PlayerData playerData = new PlayerData();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + file;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream,playerData);
        stream.Close();
        PowerUpsManager.Instance.PowerUpsInit(playerData.powerUpLevels);
    }

    // public static void SavePowerUps(PowerUp[] powerUps)
    // {
    //     BinaryFormatter formatter = new BinaryFormatter();
    //     string path = Application.persistentDataPath + "/power_ups.data";
    //     FileStream stream = new FileStream(path, FileMode.Create);
    //     // formatter 
    // }
    
    
    
    //TODO: create an save to config infos and something like 'hasAlreadyWatchedCutscene'
}
