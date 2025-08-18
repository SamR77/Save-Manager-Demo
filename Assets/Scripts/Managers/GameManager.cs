
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;



public class GameManager : MonoBehaviour
{
    private PlayerController player;


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

    // Player Stats
    public int playerHealth = 100;
    public int playerXP = 99;
    public Vector3 playerPosition;

    void Start()
    {
        // Subscribe to SaveManager events when the object is initialized
        SaveManager.OnSaveGameData += HandleSave;
        SaveManager.OnLoadGameData += HandleLoad;


        // reference PlayerController
        player = GetComponentInChildren<PlayerController>();

    }

    void OnDestroy()
    {
        // Unsubscribe from SaveManager events when the object is destroyed
        SaveManager.OnSaveGameData -= HandleSave;
        SaveManager.OnLoadGameData -= HandleLoad;
    }

    void Update()
    {
        playerPosition = player.transform.position;

        #region SimplePlayerController
        // Get horizontal and vertical input from keyboard (WASD or arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the player in world space using the movement vector
        // Multiplied by deltaTime for frame-rate independence and speed factor of 5
        transform.Translate(movement * Time.deltaTime * 5.0f, Space.World);

        #endregion

        // Check if F5 key was pressed this frame to trigger save
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            SaveManager.SaveGameData(); // Triggers OnSaveGameData event
        }

        // Check if F10 key was pressed this frame to trigger load
        if (Keyboard.current.f10Key.wasPressedThisFrame)
        {
            SaveManager.LoadGameData(); // Triggers OnLoadGameData event
        }
    }



    public void OpenFileLocation()
    { 
        string folderPath = Path.GetDirectoryName(Application.persistentDataPath);
        Process.Start("explorer.exe", folderPath);
    }

    // Called when SaveManager triggers OnSaveGameData event
    private void HandleSave()
    {
        // Create an instance of GameData with the current player stats
        GameData gameData = new GameData(playerHealth, playerXP, this.transform.position);

        // Convert the GameData object to a JSON string and save it
        string json = JsonUtility.ToJson(gameData);
        SaveManager.SetString("GameData", json);
    }

    private void HandleLoad()
    {
        // Retrieve the saved JSON string
        string json = SaveManager.GetString("GameData", "");

        // Check if the data exists before trying to load it
        if (!string.IsNullOrEmpty(json))
        {
            // Deserialize the JSON string back into a GameData object
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // Update the player stats and position
            playerHealth = gameData.PlayerHealth;
            playerXP = gameData.PlayerXP;
            this.transform.position = gameData.PlayerPosition;
        }
    }
}

[System.Serializable]
public class GameData
{
    public int PlayerHealth;
    public int PlayerXP;
    public Vector3 PlayerPosition;

    // Constructor or methods to populate data
    public GameData(int health, int xp, Vector3 position)
    {
        PlayerHealth = health;
        PlayerXP = xp;
        PlayerPosition = position;
    }
}