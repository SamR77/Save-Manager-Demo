using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Static class responsible for saving and loading game data and settings
public static class SaveManager
{
    // Internal dictionary to store key-value pairs of data to be saved
    private static Dictionary<string, string> data = new Dictionary<string, string>();

    // Events triggered when saving or loading game data
    public static event System.Action OnSaveGameData;
    public static event System.Action OnLoadGameData;

    // Events triggered when saving or loading settings
    public static event System.Action OnSaveSettings;
    public static event System.Action OnLoadSettings;

    // Saves current game data to a file
    public static void SaveGameData()
    {        
        data.Clear(); // Clear any existing data before saving new values
        
        OnSaveGameData?.Invoke(); // Notify subscribed components to populate the data dictionary
        
        string json = JsonUtility.ToJson(new SaveData(data)); // Convert the dictionary to JSON format using a helper class
        
        File.WriteAllText(Application.persistentDataPath + "/SaveXX.save", json); // Write the JSON string to a file in the persistent data path
        
        Debug.Log("Game Saved!"); // Log confirmation to the Unity console
    }

    // Loads game data from a previously saved file
    public static void LoadGameData()
    {
        
        string path = Application.persistentDataPath + "/SaveXX.save"; // Construct the full path to the save file

        // Check if the file exists before attempting to read
        if (File.Exists(path))
        {            
            string json = File.ReadAllText(path); // Read the JSON string from the file
            
            data = JsonUtility.FromJson<SaveData>(json).ToDictionary(); // Deserialize the JSON back into the dictionary
                        
            OnLoadGameData?.Invoke(); // Notify subscribed components to read from the data dictionary
            
            Debug.Log("Game Loaded!"); // Log confirmation to the Unity console
        }
    }

    // Saves current settings to a file
    public static void SaveSettings()
    {        
        data.Clear(); // Clear any existing data before saving new values
       
        OnSaveSettings?.Invoke();  // Notify subscribed components to populate the data dictionary with settings
       
        string json = JsonUtility.ToJson(new SaveData(data), true );  // Convert the dictionary to JSON format using a helper class
        
        File.WriteAllText(Application.persistentDataPath + "/Settings.cfg", json); // Write the JSON string to a settings file
        
        Debug.Log("Settings Saved!"); // Log confirmation to the Unity console
    }

    // Loads settings from a previously saved file
    public static void LoadSettings()
    {        
        string path = Application.persistentDataPath + "/Settings.cfg"; // Construct the full path to the settings file

        if (File.Exists(path)) // Check if the file exists before attempting to read
        {            
            string json = File.ReadAllText(path); // Read the JSON string from the file
            
            data = JsonUtility.FromJson<SaveData>(json).ToDictionary(); // Deserialize the JSON back into the dictionary
            
            OnLoadSettings?.Invoke(); // Notify subscribed components to read from the data dictionary
            
            Debug.Log("Settings Loaded!"); // Log confirmation to the Unity console
        }
    }

    // Stores a string value in the data dictionary under the specified key
    public static void SetString(string key, string value) => data[key] = value;

    // Retrieves a string value from the data dictionary; returns defaultValue if key is not found
    public static string GetString(string key, string defaultValue = "") =>
        data.ContainsKey(key) ? data[key] : defaultValue;
}

// Serializable helper class used to convert the dictionary to and from JSON
[System.Serializable]
public class SaveData
{
    // Lists to store keys and values separately for serialization
    public List<string> keys = new List<string>();
    public List<string> values = new List<string>();

    // Default constructor required for deserialization
    public SaveData() { }

    // Constructor that populates the lists from a dictionary
    public SaveData(Dictionary<string, string> dict)
    {
        foreach (var kvp in dict)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    // Converts the serialized lists back into a dictionary
    public Dictionary<string, string> ToDictionary()
    {
        var result = new Dictionary<string, string>();
        for (int i = 0; i < keys.Count; i++)
            result[keys[i]] = values[i];
        return result;
    }
}

