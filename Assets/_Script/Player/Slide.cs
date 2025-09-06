using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Slide : MonoBehaviour
{
    Forces forces;
    CharacterMovement movement;

    [SerializeField] PlayerStats profile;

    Vector2 moveInput = Vector2.zero;
    Vector3 horizontalVelocity = Vector3.zero;
    bool isSliding;
    bool isSlideOnCooldown;
    bool slideDurationOver;
    float startScale;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<CharacterMovement>();
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
        moveInput = InputManager.inputMove;

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
        CrouchPlayerDown();

        if (moveInput != Vector2.zero && !isSliding && !isSlideOnCooldown)
        {
            StartSlide();
        }
    }

    private void CrouchUp()
    {
        CrouchPlayerUp();

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
    private void CrouchPlayerDown()
    {
        transform.localScale = new Vector3(transform.localScale.x, profile.slideScale, transform.localScale.z);
        movement.SetIsCrouching(true);
    }

    private void CrouchPlayerUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, startScale, transform.localScale.z);
        movement.SetIsCrouching(false);
    }

    private void StartSlide()
    {
        StartCoroutine(SlideDuration());
        isSliding = true;
        movement.SetIsSliding(isSliding);

        horizontalVelocity = Vector3.zero;
        horizontalVelocity += transform.right * moveInput.x;
        horizontalVelocity += transform.forward * moveInput.y;
        horizontalVelocity *= profile.slideSpeed;
    }

    private void StopSlide()
    {
        StartCoroutine(SlideCooldown());
        isSliding = false;
        movement.SetIsSliding(isSliding);

        horizontalVelocity = Vector3.zero;
    }
}
