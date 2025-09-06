using UnityEngine;

public class Player : MonoBehaviour
{
    //This is the controller class for the player. 
    CharacterMovement movement;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
    }


}
