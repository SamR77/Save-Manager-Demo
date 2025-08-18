using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework.Internal;

public class PlayerController : MonoBehaviour
{


    

    void Update()
    {
        

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


    }



    
}