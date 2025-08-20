<<<<<<< Updated upstream
=======
using System;
using System.Collections.Generic;
using System.IO;
>>>>>>> Stashed changes
using UnityEngine;
using System.IO;

public class SaveManager
{
<<<<<<< Updated upstream
=======
    // The base directory for all saves
    public static readonly string BASE_SAVE_FOLDER = Application.persistentDataPath;

    


    public static void Init()
    {
        // Check if the base does not exist, if its not there we will create one
        if (!Directory.Exists(BASE_SAVE_FOLDER))
        {
            Directory.CreateDirectory(BASE_SAVE_FOLDER);
        }

        // Check if Saves folder exists
        if (!Directory.Exists(BASE_SAVE_FOLDER + "/Saves/"))
        {
            Directory.CreateDirectory(BASE_SAVE_FOLDER + "/Saves/");
        }
    }

    public static void SaveGame(string saveGameString)
    {
        string saveFolder = BASE_SAVE_FOLDER + "/Saves/";
        int saveSlots = 3;

        // Create a new save file with a timestamp (add milliseconds to avoid collisions)
        string fileName = "save_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".save";
        string fullPath = Path.Combine(saveFolder, fileName);
        File.WriteAllText(fullPath, saveGameString);

        // Get all save files again after writing
        DirectoryInfo directoryInfo = new DirectoryInfo(saveFolder);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.save");

        // Sort by creation time descending
        Array.Sort(saveFiles, (a, b) => b.CreationTime.CompareTo(a.CreationTime));

        // Delete oldest if count exceeds limit
        for (int i = saveSlots; i < saveFiles.Length; i++)
        {
            saveFiles[i].Delete();
        }

        Debug.Log("Total saves: " + saveFiles.Length);
    }

    public static string LoadGameData()
    {
        string gameSaveFolder = BASE_SAVE_FOLDER + "/Saves/";

        DirectoryInfo directoryInfo = new DirectoryInfo(gameSaveFolder);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.save");

        // Load the most recent file if it exists
        if (saveFiles.Length > 0)
        {
            // Sort saves by creation time descending
            Array.Sort(saveFiles, (a, b) => b.CreationTime.CompareTo(a.CreationTime));
            string mostRecentSaveFile = File.ReadAllText(saveFiles[0].FullName);
            return mostRecentSaveFile;
        }
        return null;
    }


    public static void SaveSettings(string saveSettingsString)
    {
        string saveFolder = BASE_SAVE_FOLDER;

        string fileName = "Settings.cfg";
        string fullPath = Path.Combine(saveFolder, fileName);

        File.WriteAllText(fullPath, saveSettingsString);
    }

    public static string LoadSettingsData()
    {
        string saveFolder = BASE_SAVE_FOLDER;
        string fullPath = Path.Combine(saveFolder, "Settings.cfg");

        if (File.Exists(fullPath))
        {
            return File.ReadAllText(fullPath);
        }
        else
        {
            Debug.LogError("No Setting file found");
            return string.Empty;
        } 
    }












    // Internal dictionary to store key-value pairs of data to be saved
    private static Dictionary<string, string> data = new Dictionary<string, string>();
>>>>>>> Stashed changes

    private static SaveData saveData = new SaveData();



<<<<<<< Updated upstream

    [System.Serializable]
    public struct SaveData
    {
        public SettingsData settingsData;
        public PlayerData playerData;
    }
=======
>>>>>>> Stashed changes

    // Note: files will be save to C:\Users\[user]\AppData\LocalLow\DEMOTEST\SaveDataTutorial

    #region Save and Load Settings Data

    private string SettingsSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath + "/Settings" + ".cfg");
    }

    public void SaveSettingsData()
    {
        Debug.Log("Saving settings data to " + SettingsSaveFilePath());

        GameManager.Instance.SettingsManager.Save(ref saveData.settingsData);

        string json = JsonUtility.ToJson(saveData.settingsData, true);
        File.WriteAllText(SettingsSaveFilePath(), json );
    }

    public void LoadSettingsData()
    {
        if (File.Exists(SettingsSaveFilePath()))
        {
            string content = File.ReadAllText(SettingsSaveFilePath());
            saveData.settingsData = JsonUtility.FromJson<SettingsData>(content);
            GameManager.Instance.SettingsManager.Load(saveData.settingsData);
        }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }
    }
    #endregion


    
    #region Save and Load Player Data

    private string PlayerSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath + "/SaveGame" + ".save");
    }

    public void SavePlayerData()
    {
        Debug.Log("Saving player data to " + PlayerSaveFilePath());

        GameManager.Instance.Player.Save(ref saveData.playerData);

        string json = JsonUtility.ToJson(saveData.playerData, true);
        File.WriteAllText(PlayerSaveFilePath(), json);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(PlayerSaveFilePath()))
        {
            string content = File.ReadAllText(PlayerSaveFilePath());
            saveData.playerData = JsonUtility.FromJson<PlayerData>(content);
            GameManager.Instance.Player.Load(saveData.playerData);
        }
        else
        {
            Debug.LogError("player file not found.");
            return;
        }
    }
    #endregion
    








}