using ImprovedTimers;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This is the controller class for the player. 
    CharacterMovement movement;
    Jump jump;
    Slide slide;
    Health health;
    Hud hud;

    CountdownTimer deathTimer = new(3);
    Vector2 moveInput = Vector2.zero;

    private void Start()
    {
        movement = GetComponent<CharacterMovement>();
        jump = GetComponent<Jump>();
        slide = GetComponent<Slide>();
        health = GetComponent<Health>();
        hud = GetComponentInChildren<Hud>();

        health.onChanged += hud.SetHealthBar;
        health.onEmpty += Death;

        deathTimer.OnTimerStop += Restart;
    }

    private void OnEnable()
    {
        InputManager.inputJump += JumpKeyDown;
        InputManager.inputCrouchDown += CrouchKeyDown;
        InputManager.inputCrouchUp += CrouchKeyUp;
    }

    private void OnDisable()
    {
        InputManager.inputJump -= JumpKeyDown;
        InputManager.inputCrouchDown -= CrouchKeyDown;
        InputManager.inputCrouchUp -= CrouchKeyUp;
    }

    private void Update()
    {
        GetInput();

        movement.UpdateMovement();
        jump.UpdateJump();
        slide.UpdateSlide();
    }

    private void GetInput()
    {
        moveInput = InputManager.inputMove;

        movement.SetMoveInput(moveInput);
        slide.SetMoveInput(moveInput);
    }

    private void JumpKeyDown()
    {
        slide.JumpPressed();
        jump.JumpPressed();
    }

    private void CrouchKeyDown()
    {
        slide.CrouchPressed();
    }
    
    private void CrouchKeyUp()
    {
        slide.CrouchUp();
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
