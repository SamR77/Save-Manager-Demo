using UnityEngine;
using System.IO;

public class SaveManager
{

    private static SaveData saveData = new SaveData();




    [System.Serializable]
    public struct SaveData
    {
        public SettingsData settingsData;
        public PlayerData playerData;
    }

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