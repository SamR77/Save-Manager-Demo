using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static event Action PreparingToSave;
    public static event Action Saving;
    public static event Action FinishedSaving;

   

    public static event Action PreparingToLoad;
    public static event Action<string, Action> Loading; // Add callback parameter
    public static event Action FinishedLoading;

    // Event for applying loaded data
    public static event Action<string> OnDataLoaded;


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

    #region Save Game

    public static void SaveGame(GameManager gameManager)
    {
        // Step 1: Signal the start of the save process
        PreparingToSave?.Invoke();

        // Step 2: Call a method on GameManager to package up the Data into Json string
        string gameDataString = gameManager.PrepareGameData();

        // Step 3: Verify that the gameData is not null or empty
        if (string.IsNullOrEmpty(gameDataString))
        {
            Debug.LogError("No game data to save.");
        }

        Saving?.Invoke();
        // Step 4: Save the Game Data to a file
        string saveFolder = BASE_SAVE_FOLDER + "/Saves/";
        int saveSlots = 3;

        // Step 5: Create a new save file with a timestamp (add milliseconds to avoid collisions)
        string fileName = "save_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ".save";
        string fullPath = Path.Combine(saveFolder, fileName);
        File.WriteAllText(fullPath, gameDataString);

        // Step 6: Sort all save files by creationTime        
        DirectoryInfo directoryInfo = new DirectoryInfo(saveFolder);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.save");        
        Array.Sort(saveFiles, (a, b) => b.CreationTime.CompareTo(a.CreationTime));

        // Step 7: Cleanup, if there are more save files than save slots, delete the oldest one        
        for (int i = saveSlots; i < saveFiles.Length; i++)
        {
            saveFiles[i].Delete();
        }
        FinishedSaving?.Invoke();
    }
    #endregion

    #region Load Game

    public static void LoadGame()
    {
        // Notify any listeners that the game is preparing to load
        PreparingToLoad?.Invoke();

        // Convert the saved game data into a JSON string
        string json = ConvertSaveToJson();

        // Check if the JSON string is valid (i.e., not null or empty)
        if (!string.IsNullOrEmpty(json))
        {
            // Trigger the Loading event, passing the JSON data and a callback to apply the game data
            Loading?.Invoke(json, OnGameDataApplied); // Pass callback
        }
        else
        {
            // Log a warning if no save data was found
            Debug.LogWarning("No save file found to load.");
        }
    }

    private static string ConvertSaveToJson()
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

    private static void OnGameDataApplied()
    {
        FinishedLoading?.Invoke();
    }

    #endregion

    #region Save Settings
    public static void SaveSettings(string saveSettingsString)
    {
        try
        {
            //PreparingToSave?.Invoke();
            string saveFolder = BASE_SAVE_FOLDER;

            string fileName = "Settings.cfg";
            string fullPath = Path.Combine(saveFolder, fileName);

            File.WriteAllText(fullPath, saveSettingsString);
        }

        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    #endregion


    #region Load Settings   
    public static string LoadSettingsData()
    {
        PreparingToLoad?.Invoke();
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

    #endregion






}