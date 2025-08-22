using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Stats
    public int playerHealth = 100;
    public int playerXP = 99;
    public Vector3 playerPosition;

    // Player Data UI
    public TMP_Text playerHealth_txt;
    public TMP_Text playerXP_txt;
    public TMP_Text PlayerPosition_txt;

    public bool PlayerInputEnabled = true;

 

    private void Update()
    {
        playerPosition = this.transform.position;
        RefreshUI();

        if (PlayerInputEnabled == true)
        {
            SimpleCharacterController();
        }
    }

    private void SimpleCharacterController()
    {
        // Get horizontal and vertical input from keyboard (WASD or arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * 5.0f, Space.World); // Move the player at a speed of 5 units per second
    }

    

    public void RefreshUI()
    {
        playerHealth_txt.text = playerHealth.ToString();
        playerXP_txt.text = playerXP.ToString();
        PlayerPosition_txt.text = playerPosition.ToString(); 
    }


}