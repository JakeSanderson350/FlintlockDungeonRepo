using ImprovedTimers;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This is the controller class for the player. 
    CharacterMovement movement;
    Health health;
    Hud hud;

    CountdownTimer deathTimer = new(3);

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        health = GetComponent<Health>();
        hud = GetComponentInChildren<Hud>();

        health.onChanged += hud.SetHealthBar;
        health.onEmpty += Death;

        deathTimer.OnTimerStop += Restart;
    }
    private void OnDestroy()
    {
        deathTimer.Dispose();
    }
    void Death()
    {
        EventManager.PlayerDied();
        deathTimer.Start();
    }

    void Restart()
    {
        GameManager.inst.ReloadLevel();
    }

    
}
