using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Slide : MonoBehaviour
{
    Forces forces;
    Movement movement;

    [SerializeField] PlayerStats profile;

    Vector3 horizontalVelocity = Vector3.zero;
    bool isSliding = false;
    bool isSlideOnCooldown = false;
    bool slideDurationOver;
    float startScale;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        forces = GetComponent<Forces>();
        startScale = transform.localScale.y;
    }

    private void OnEnable()
    {
        InputManager.inputCrouchDown += CrouchPressed;
        InputManager.inputCrouchUp += CrouchUp;
        InputManager.inputJump += JumpPressed;
    }

    private void OnDisable()
    {
        InputManager.inputCrouchDown -= CrouchPressed;
        InputManager.inputCrouchUp -= CrouchUp;
        InputManager.inputJump -= JumpPressed;
    }

    // Update is called once per frame
    void Update()
    {
        if(slideDurationOver)
        {
            StopSlide();
            slideDurationOver = false;
        }

        // Only add momentum on ground but can start sliding in air
        if (movement.GetIsGrounded())
        {
            forces.AddForceConstant(horizontalVelocity);
        }
        else
        {
            forces.AddForceConstant(Vector3.zero);
        }
    }

    private void CrouchPressed()
    {
        if (!isSliding && !isSlideOnCooldown)
        {
            StartSlide();
        }
    }

    private void CrouchUp()
    {
        if (isSliding)
        {
            StopSlide();
        }
    }

    private void JumpPressed()
    {
        if (isSliding)
        {
            StopSlide();
        }
    }

    IEnumerator SlideDuration()
    {
        slideDurationOver = false;
        yield return new WaitForSeconds(profile.slideDuration);
        if (isSliding)
            slideDurationOver = true;
    }

    IEnumerator SlideCooldown()
    {
        isSlideOnCooldown = true;
        yield return new WaitForSeconds(profile.slideDuration);
        isSlideOnCooldown = false;
    }

    private void StartSlide()
    {
        Debug.Log("Sldie");
        StartCoroutine(SlideDuration());
        isSliding = true;
        movement.SetIsSliding(isSliding);
        transform.localScale = new Vector3(transform.localScale.x, profile.slideScale, transform.localScale.z);

        horizontalVelocity = Vector3.zero;
        horizontalVelocity += transform.right * InputManager.inputMove.x;
        horizontalVelocity += transform.forward * InputManager.inputMove.y;
        horizontalVelocity *= profile.slideSpeed;
    }

    private void StopSlide()
    {
        StartCoroutine(SlideCooldown());
        isSliding = false;
        movement.SetIsSliding(isSliding);
        transform.localScale = new Vector3(transform.localScale.x, startScale, transform.localScale.z);

        horizontalVelocity = Vector3.zero;
    }
}
