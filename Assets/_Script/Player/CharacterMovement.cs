using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //[Header("Components")]
    CharacterController characterController;
    Forces forces;

    [SerializeField] PlayerStats profile;
    [SerializeField] List<AudioClip> footstepSounds;

    Vector2 moveInput = Vector2.zero;
    Vector3 horizontalVelocity = Vector3.zero;
    Vector3 verticalVelocity = Vector3.zero;
    bool isGrounded;
    bool wasGrounded;
    bool isCrouching;
    bool isSliding;
    float airTime;
    float footstepTimer = 0.0f; // Sry this is very jank - jake:(

    public float GetAirTime() => airTime;
    public bool GetIsGrounded() => isGrounded;
    public void SetIsSliding(bool _isSliding) => isSliding = _isSliding;
    public void SetIsCrouching(bool _isCrouching) => isCrouching = _isCrouching;
    public void SetMoveInput(Vector2 _moveInput) => moveInput = _moveInput;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        forces = GetComponent<Forces>();
    }

    private void OnDrawGizmos()
    {
        if(profile == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + profile.offset, profile.radius);
    }

    public void UpdateMovement()
    {
        //check Groudning. Do this first before airtime gets cutdown
        CheckGrounded();

        if (isSliding && isGrounded)
        {
            horizontalVelocity = Vector3.zero;
        }
        else if (isGrounded)
        {
            horizontalVelocity = Vector3.zero;
            horizontalVelocity += transform.right * moveInput.x;
            horizontalVelocity += transform.forward * moveInput.y;
            horizontalVelocity *= isCrouching ? profile.crouchSpeed : profile.speed;
        }
        else
        {
            Vector3 airVelocity = horizontalVelocity - (horizontalVelocity * profile.airDrag * Time.deltaTime);
            airVelocity += transform.right * moveInput.x * profile.airSpeed;
            airVelocity += transform.forward * moveInput.y * profile.airSpeed;
            horizontalVelocity = Vector3.ClampMagnitude(airVelocity, profile.speed);
        }

        verticalVelocity = characterController.velocity.y > -0.001f ? Physics.gravity : Vector3.ClampMagnitude(verticalVelocity + Physics.gravity * Time.deltaTime, profile.terminalGravity);

        //move
        //characterController.Move(horizontalVelocity * Time.deltaTime);
        forces.AddForceConstant(horizontalVelocity);
        forces.AddForceConstant(verticalVelocity);
    }

    void AirbornTrigger()
    {
        //AIRBORN STATE IN HERE
    }

    void LandingTrigger()
    {
        //LANDING STATE IN HERE
        airTime = 0;
    }

    void AirbornUpdate()
    {
        //AIRBORN TICK STUFF HERE
        airTime += Time.deltaTime;

    }

    void GroundedUpdate()
    {
        //GROUNDED TICK STUFF HERE
        HandleFootstep();
    }

    void CheckGrounded()
    {
        //SET GROUNDED STATE HERE
        //if(airTime > profile.jumpCooldown)
            isGrounded = Physics.CheckSphere(transform.position + profile.offset, profile.radius, profile.groundingLayers);

        if(!isGrounded && wasGrounded)
        {
            AirbornTrigger();
            wasGrounded = false;
            return;
        }

        if (isGrounded && !wasGrounded)
        {
            LandingTrigger();
            wasGrounded = true;
            return;
        }

        if (isGrounded)
            GroundedUpdate();
        else
            AirbornUpdate();
    }

    private void HandleFootstep()
    {
        if(isGrounded && !isSliding && moveInput != Vector2.zero)
        {
            footstepTimer += Time.deltaTime;

            float interval = isCrouching ? 0.6f : 0.3f; //sry james sry james sry james sry james sry jame sry sry sry

            if (footstepTimer >= interval)
            {
                AudioPlayer.PlaySFX(footstepSounds, transform);
                footstepTimer = 0.0f;
            }
        }
        else
        {
            footstepTimer = 0.0f;
        }
    }
}