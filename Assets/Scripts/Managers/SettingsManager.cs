using UnityEngine;
using UnityEngine.InputSystem;

// Manages Settings Data (settings screen) and handles saving/loading of settings data via SaveManager.
public class SettingsManager : MonoBehaviour
{
    // Volume settings with range limits for UI sliders or inspector
    [Range(0, 100)] public int masterVolume = 50;
    [Range(0, 100)] public int musicVolume = 50;
    [Range(0, 100)] public int sfxVolume = 50;
    [Range(0, 100)] public int voiceoverVolume = 50;


    // Subscribes to SaveManager events when the script starts.
    void Start()
    {
        SaveManager.OnSaveSettings += HandleSave;
        SaveManager.OnLoadSettings += HandleLoad;
    }

    // Unsubscribes from SaveManager events when the script is destroyed.
    void OnDestroy()
    {
        SaveManager.OnSaveSettings -= HandleSave;
        SaveManager.OnLoadSettings -= HandleLoad;
    }


    // Checks for keyboard input to trigger save/load operations.
    private void Update()
    {
        // Press F7 to save settings
        if (Keyboard.current.f7Key.wasPressedThisFrame)
        {
            SaveManager.SaveSettings();
        }

        // Press F11 to load settings
        if (Keyboard.current.f11Key.wasPressedThisFrame)
        {
            SaveManager.LoadSettings();
        }
    }


    // Called when SaveManager wants to save settings.
    // Converts volume values to strings and stores them.
    private void HandleSave()
    {
        // Create an instance of GameData with the current player stats
        SettingsData settingsData = new SettingsData(masterVolume, musicVolume, sfxVolume, voiceoverVolume);

        // Convert the GameData object to a JSON string and save it
        string json = JsonUtility.ToJson(settingsData);
        SaveManager.SetString("GameData", json);
    }

    // Called when SaveManager wants to load settings.
    // Retrieves saved strings and parses them into integers.
    private void HandleLoad()
    {
        // Retrieve the saved JSON string
        string json = SaveManager.GetString("SettingsData", "");

        // Check if the data exists before trying to load it
        if (!string.IsNullOrEmpty(json))
        {
            // Deserialize the JSON string back into a GameData object
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);

            // Update the player stats and position
            masterVolume = settingsData.MasterVolume;
            musicVolume = settingsData.MusicVolume;
            sfxVolume = settingsData.SFXVolume;
            voiceoverVolume = settingsData.VoiceoverVolume;       
        }
    }
}

[System.Serializable]
public class SettingsData
{
    public int MasterVolume;
    public int MusicVolume;
    public int SFXVolume;
    public int VoiceoverVolume;


    // Constructor or methods to populate data
    public SettingsData(int masterVolume, int musicVolume, int sfxVolume, int voiceoverVolume)
    {
        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        SFXVolume = sfxVolume;
        VoiceoverVolume = voiceoverVolume;
    }
}