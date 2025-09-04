using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform playerBody;
    [SerializeField] PlayerPrefs prefs;
    [SerializeField] bool enableMouse = false;

    float xRotation;
    float yRotation;
    Vector2 deltaPointer;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //preset rotation 
        xRotation = 0;
        //yRotation = playerBody.rotation.y;
    }
    private void Update()
    {
        deltaPointer = InputManager.inputDeltaPointer;

        CheckMouseState();
        if (enableMouse) return;

        yRotation += Twist();

        xRotation -= Tilt();
        LockCameraTilt();

        //rotate camera up-down
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //rotate body left-right
        playerBody.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    float Twist()
    {
        float mouseX = deltaPointer.x * prefs.sentitivityX * (prefs.invertX ? -1 : 1) * Time.deltaTime * 90f;
        return mouseX;
    }

    float Tilt()
    {
        float mouseY = deltaPointer.y * prefs.sentitivityY * (prefs.invertY ? -1 : 1) * Time.deltaTime * 90f;
        return mouseY;
    }

    void CheckMouseState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enableMouse = !enableMouse;
            Cursor.lockState = enableMouse ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = enableMouse;
        }
    }

    void LockCameraTilt()
    {
        xRotation = xRotation % 360;

        if (xRotation < -270f) xRotation += 360f;
        if(xRotation > 270f) xRotation -= 360f;

        xRotation = Mathf.Clamp(xRotation, -89.9f, 89.9f);
    }

    public void SetSensitivityX(float val)
    {
        prefs.sentitivityX = val;
    }

    public void SetSensitivityY(float val)
    {
        prefs.sentitivityY = val;
    }

    public void SetInvertY(bool val)
    {
        prefs.invertY = val;
    }
}


