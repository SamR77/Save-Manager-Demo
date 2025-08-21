//using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private SettingsManager settingsManager;

    // Singleton pattern
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }



    void Start()
    {
        SaveManager.Init();

        // reference PlayerController
        player = GetComponentInChildren<PlayerController>();
        settingsManager = GetComponentInChildren<SettingsManager>();
    }

    void OnDestroy()
    {
        
    }




    #region Game Save Handling

    // Called when SaveManager triggers OnSaveGameData event
    public void SaveGame()
    {
        // Save

        // store all data in a temporary SaveGameData object
        SaveGameData saveGameData = new SaveGameData
        {
            PlayerHealth = player.playerHealth,
            PlayerXP = player.playerXP,
            PlayerPosition = player.transform.position
        };

        // Convert SaveGameData to string to prep for Json
        string saveGameString = JsonUtility.ToJson(saveGameData);

        // Call the Save method in SaveSystem and pass 
        SaveManager.SaveGame(saveGameString);
    }

    public void LoadGame()
    {
        Debug.Log("Load Game Called");

        string loadGameString = SaveManager.LoadGameData();

        if (loadGameString != null)
        {
            // Load save data into a temporart SaveGameData Object
            SaveGameData saveObject = JsonUtility.FromJson<SaveGameData>(loadGameString);


            // TODO: consider creating a method in player Controller to populate these values, and just pass them through.
            player.playerHealth = saveObject.PlayerHealth;
            player.playerXP = saveObject.PlayerXP;

            // Move Player to Position
            player.transform.position = saveObject.PlayerPosition;

            // Update the debug UI to show the new values
            player.RefreshUI();

        }
        else
        {
            Debug.LogWarning("No save data found");
        }
    }


    public void OpenSavedGameFileLocation()
    {
        string folderPath = SaveManager.BASE_SAVE_FOLDER + "/Saves/";

        Application.OpenURL("file://" + folderPath);

    }

    private class SaveGameData
    {
        public int PlayerHealth;
        public int PlayerXP;
        public Vector3 PlayerPosition;
    }

    #endregion

    #region Settings Save Handling

    public void SaveSettings()
    {
        // store all data in a SaveGameData object
        SettingsData settingsData = new SettingsData
        {
            MasterVolume = settingsManager.masterVolume,
            MusicVolume = settingsManager.musicVolume,
            SFXVolume = settingsManager.sfxVolume,
            VoiceoverVolume = settingsManager.voiceoverVolume
        };

        // Convert SettingsData to string to prep for Json
        string saveSettingsString = JsonUtility.ToJson(settingsData);

        // Call the Save method in SaveSystem and pass 
        SaveManager.SaveSettings(saveSettingsString);
    }

    public void LoadSettings()
    {
        // Retrieve the settings data from the SaveManager
        string loadSettingsString = SaveManager.LoadSettingsData();

        // Check if the loaded string is not empty
        if (!string.IsNullOrEmpty(loadSettingsString))
        {            
            // Deserialize the JSON string back into a SettingsData object
            SettingsData loadedSettings = JsonUtility.FromJson<SettingsData>(loadSettingsString);

            // Apply the loaded settings to the SettingsManager
            settingsManager.masterVolume_slider.value = loadedSettings.MasterVolume;
            settingsManager.musicVolume_slider.value = loadedSettings.MusicVolume;
            settingsManager.sfxVolume_slider.value = loadedSettings.SFXVolume;
            settingsManager.voiceoverVolume_slider.value = loadedSettings.VoiceoverVolume;



            Debug.Log("Settings loaded successfully.");            
        }
    }

    public void OpenSavedSettingsFileLocation()
    {
        string folderPath = SaveManager.BASE_SAVE_FOLDER;

        Application.OpenURL("file://" + folderPath);
    }

    private class SettingsData
    { 
        public int MasterVolume;
        public int MusicVolume;
        public int SFXVolume;
        public int VoiceoverVolume;
    }

    #endregion


}
