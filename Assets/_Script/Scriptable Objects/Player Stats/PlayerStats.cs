using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new player profile", menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Stats")]
    public float speed = 10f;
    public float airSpeed = 2f;
    public float airDrag = 20f;
    public float terminalGravity = 30f;
    public Forces.Force jumpForce;

    [Header("Grounding")]
    public LayerMask groundingLayers;
    public LayerMask wallLayer;
    public Vector3 offset = Vector3.zero;
    public float radius = 0.4f;
    public float coyoteTime = 0.2f;
    public float jumpCooldown = 0.25f;
    public float wallJumpCooldown = 0.20f;
    public float wallJumpForce = 10.0f;

    [Header("Sliding")]
    public float crouchSpeed = 10.0f;
    public float slideSpeed = 35.0f;
    public float slideDuration = 2.0f;
    public float slideScale = 0.5f;
    public float slideCooldown = 0.75f;
}
