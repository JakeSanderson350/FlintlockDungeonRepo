using UnityEngine;

public class Player : MonoBehaviour
{
    //This is the controller class for the player. 
    Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }


}
