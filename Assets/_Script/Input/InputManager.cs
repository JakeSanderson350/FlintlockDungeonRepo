using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput controls;
    PlayerInput.DefaultActions defaultControls;
    PlayerInput.MenuActions menuControls;

    public static Vector2 inputMove;
    public static Vector2 inputDeltaPointer;
    public static Action inputJump;

    public static Action inputMenu;
    public static Action inputStart;

    private void Awake()
    {
        //setup controls
        controls = new PlayerInput();
        defaultControls = controls.Default;
        menuControls = controls.Menu;

        defaultControls.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        defaultControls.Look.performed += ctx => inputDeltaPointer = ctx.ReadValue<Vector2>();
        defaultControls.Jump.started += ctx => inputJump?.Invoke();

        menuControls.Menu.started += ctx => inputMenu?.Invoke();
        menuControls.Start.started += ctx => inputStart?.Invoke();
    }

    private void OnEnable() //restarts controls when needed
    {
        controls.Enable();
    }

    private void OnDisable() //prevents controls running in editor
    {
        controls.Disable();   
    }

    /*void EnableControls(InputManager.Profile profile)
    {
        
    }*/
}
