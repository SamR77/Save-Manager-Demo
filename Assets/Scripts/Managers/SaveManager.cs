using System;
using System.IO;
using UnityEngine;


public class SaveManager
{
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


    
}