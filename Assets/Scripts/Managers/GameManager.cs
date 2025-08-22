using System.Collections;
using UnityEngine;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{
    public GameData currentGameData;

    private PlayerController player;
    private SettingsManager settingsManager;

    public GameObject saveLoadIndicator;

    public TMP_Text saveLoadIndicator_text;

    public Stopwatch saveLoadStopwatch;

    public int simulatedPrepOperationTime = 1500; // 1.5 seconds
    public int simulatedSaveLoadOperationTime = 3000; // 5 seconds
        


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

    private void OnEnable()
    {

        SaveManager.PreparingToSave += OnPreparingToSave;
        SaveManager.Saving += OnSaving;
        SaveManager.FinishedSaving += OnFinishedSaving;


        SaveManager.PreparingToLoad += OnPreparingToLoad;
        SaveManager.Loading += OnLoading;
        SaveManager.FinishedLoading += OnFinishedLoading;
    }

    void OnDestroy()
    {
        SaveManager.PreparingToSave -= OnPreparingToSave;

        SaveManager.PreparingToLoad -= OnPreparingToLoad;
        SaveManager.Loading -= OnLoading;
        SaveManager.FinishedLoading -= OnFinishedLoading;
    }

    #region Save Game

    private void SaveGame()
    { 
        saveLoadStopwatch.Begin();
        SaveManager.SaveGame(this);
    }

    private void OnPreparingToSave()
    {
        UnityEngine.Debug.Log("OnPreparingToSave, time: " + saveLoadStopwatch.GetMilliseconds() );

        saveLoadIndicator_text.text = "Saving...";

        Time.timeScale = 0f; // Pause Game
        player.PlayerInputEnabled = false;
        saveLoadIndicator.SetActive(true);
    }

    public string PrepareGameData()
    {
        currentGameData = new GameData
        {
            PlayerHealth = player.playerHealth,
            PlayerXP = player.playerXP,
            PlayerPosition = player.transform.position
        };

        return JsonUtility.ToJson(currentGameData);
    }

  

private void OnSaving()
    {
        UnityEngine.Debug.Log("OnSaving, time: " + saveLoadStopwatch.GetMilliseconds());
    }

    private void OnFinishedSaving()
    {
        Time.timeScale = 1f; // Resume Game 
        player.PlayerInputEnabled = true;
        saveLoadIndicator.SetActive(false);

        UnityEngine.Debug.Log("OnFinishedSaving, time: " + saveLoadStopwatch.GetRawElapsedTime());
    }


    #endregion

    #region Load Game

    public void LoadMostRecentSave()
    {
        // This will internally call ApplyGameData() and then trigger OnDataLoaded
        saveLoadStopwatch.Begin();

        SaveManager.LoadGame();
    }

    private void OnPreparingToLoad()
    {
        UnityEngine.Debug.Log("OnPreparingToLoad, time: " + saveLoadStopwatch.GetMilliseconds());

        saveLoadIndicator_text.text = "Loading...";

        Time.timeScale = 0f; // Pause Game
        player.PlayerInputEnabled = false;
        saveLoadIndicator.SetActive(true);
    }

    private void OnLoading(string jsonGameData, Action OnComplete)
    {
        UnityEngine.Debug.Log("OnLoading, time: " + saveLoadStopwatch.GetMilliseconds());

        if (!string.IsNullOrEmpty(jsonGameData))
        {
            currentGameData = JsonUtility.FromJson<GameData>(jsonGameData);
            ApplyGameData(currentGameData);
        }
        else
        {
            UnityEngine.Debug.LogWarning("No game data found to load.");
        }
        OnComplete?.Invoke();
    }

    private void ApplyGameData(GameData data)
    {
            // Apply loaded values to the player
            player.playerHealth = data.PlayerHealth;
            player.playerXP = data.PlayerXP;
            player.transform.position = data.PlayerPosition;

            // Refresh UI
            player.RefreshUI();      
    }

    private void OnFinishedLoading()
    {

        Time.timeScale = 1f; // Resume Game 
        player.PlayerInputEnabled = true;
        saveLoadIndicator.SetActive(false);

        UnityEngine.Debug.Log("OnFinishedLoading, time: " + saveLoadStopwatch.GetMilliseconds());

    }

    #endregion


    void Start()
    {
        SaveManager.Init();
        // reference PlayerController
        player = GetComponentInChildren<PlayerController>();
        settingsManager = GetComponentInChildren<SettingsManager>();
    }












    

    public void OpenSavedGameFileLocation()
    {
        string folderPath = SaveManager.BASE_SAVE_FOLDER + "/Saves/";

        Application.OpenURL("file://" + folderPath);

    }

    [Serializable]
    public class GameData
    {
        public int PlayerHealth;
        public int PlayerXP;
        public Vector3 PlayerPosition;
    }


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



            UnityEngine.Debug.Log("Settings loaded successfully.");            
        }
    }

    public void OpenSavedSettingsFileLocation()
    {
        string folderPath = SaveManager.BASE_SAVE_FOLDER;

        Application.OpenURL("file://" + folderPath);
    }


    #region Helper Methods

    
    public void SimulateOperation(float seconds, Action onComplete)
    {
        StartCoroutine(SimulateOperationCorpoutine(seconds, onComplete));
    }

    public IEnumerator SimulateOperationCorpoutine(float seconds, Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime; // Works even when Time.timeScale = 0
            yield return null;
        }

        onComplete?.Invoke();
    }
    
    #endregion

    private class SettingsData
    { 
        public int MasterVolume = 50;
        public int MusicVolume = 50;
        public int SFXVolume = 50;
        public int VoiceoverVolume = 50;

        // Possible add logic to validate that data is within acceptable range [0-100]
    }

   

}
