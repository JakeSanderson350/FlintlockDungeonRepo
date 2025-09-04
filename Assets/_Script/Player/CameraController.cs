using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    [SerializeField] private Transform playerDirection;
    [SerializeField] private Transform playerCamPos;

    private float xRot, yRot;
    private float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        RotateCam();
        MoveCamera();
    }

    private void GetInput()
    {
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;
    }

    private void RotateCam()
    {
        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0.0f);
        playerDirection.rotation = Quaternion.Euler(xRot, yRot, 0.0f);
    }

    private void MoveCamera()
    {
        transform.position = playerCamPos.position;
    }
}
