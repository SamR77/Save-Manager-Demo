using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // make this a singleton
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


    public SettingsManager SettingsManager; // reference to the SettingsManager script
    public PlayerController Player;
    public SaveManager SaveManager;

    //declare int with a range of 0 to 100 for each volume setting




    // Update is called once per frame
    void Update()
    {

    }
}
