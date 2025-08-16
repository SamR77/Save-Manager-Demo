using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector3 Position;
    public int Health;
    public int XP;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // simple TopDown movement controls WASD
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrows
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * 5.0f, Space.World); // Move the player at a speed of 5 units per second


        // inputs for saving and loading player data
        if (Keyboard.current.numpad9Key.wasPressedThisFrame)
        {
            GameManager.Instance.SaveManager.SavePlayerData();
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            GameManager.Instance.SaveManager.LoadPlayerData();
        }



    }

    #region Save and Load

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
