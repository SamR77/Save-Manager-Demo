using NUnit.Framework.Internal;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
<<<<<<< Updated upstream
    public Vector3 Position;
    public int Health;
    public int XP;


=======
    // Player Stats
    public int playerHealth = 100;
    public int playerXP = 99;
    public Vector3 playerPosition;

    // Player Data UI
    public TMP_Text playerHealth_txt;
    public TMP_Text playerXP_txt;
    public TMP_Text PlayerPosition_txt;



    private void Awake()
    {
        
    }
>>>>>>> Stashed changes

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
<<<<<<< Updated upstream
        
    }

    // Update is called once per frame
    void Update()
    {
        // simple TopDown movement controls WASD
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrows
=======
        playerPosition = this.transform.position;

        RefreshUI();


        #region SimplePlayerController
        // Get horizontal and vertical input from keyboard (WASD or arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on input
>>>>>>> Stashed changes
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * 5.0f, Space.World); // Move the player at a speed of 5 units per second


<<<<<<< Updated upstream
        // inputs for saving and loading player data
        if (Keyboard.current.numpad9Key.wasPressedThisFrame)
        {
            GameManager.Instance.SaveManager.SavePlayerData();
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            GameManager.Instance.SaveManager.LoadPlayerData();
        }



=======
        #endregion
>>>>>>> Stashed changes
    }

    #region Save and Load

<<<<<<< Updated upstream
    public void Save(ref PlayerData data)
    {
        data.Position = transform.position; // Save the player's position
        data.Health = Health; // Save the player's health
        data.XP = XP; // Save the player's experience points

    }

    public void Load(PlayerData data)
    {
        Position = data.Position; // Load the player's position
        Health = data.Health; // Load the player's health
        XP = data.XP; // Load the player's experience points
    }
    #endregion

}
=======
    public void RefreshUI()
    {
        playerHealth_txt.text = playerHealth.ToString();
        playerXP_txt.text = playerXP.ToString();
        PlayerPosition_txt.text = playerPosition.ToString(); 
    }


}

    

    



>>>>>>> Stashed changes
