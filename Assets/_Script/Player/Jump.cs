using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    //[Header("Components")]
    Forces forces;

    [SerializeField] PlayerStats profile;

    Vector3 horizontalVelocity = Vector3.zero;
    Vector3 verticalVelocity = Vector3.zero;
    Vector3 wallNormal = Vector3.zero;
    Vector3 prevWallNormal = Vector3.zero;
    bool isGrounded;
    bool wasGrounded;
    bool isJumpOnCooldown;
    bool isWallJumpOnCooldown;
    bool isOnWall;
    bool canWallJump; //probably redundant but may have future use
    float airTime;
    RaycastHit hit;

    public float GetAirTime() => airTime;
    public bool GetIsGrounded() => isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        forces = GetComponent<Forces>();
    }

    private void OnEnable()
    {
        InputManager.inputJump += JumpPressed;
    }

    private void OnDisable()
    {
        InputManager.inputJump -= JumpPressed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckOnWall();

        forces.AddForceConstant(horizontalVelocity);
        forces.AddForceConstant(verticalVelocity);
        horizontalVelocity = Vector3.zero;
        verticalVelocity = Vector3.zero;
    }

    void JumpPressed()
    {
        if (!isWallJumpOnCooldown && canWallJump)
        {
            WallJump();
        }

        if (airTime > profile.coyoteTime || isJumpOnCooldown)
            return;

        GroundedJump();
    }

    IEnumerator JumpCooldown()
    {
        isJumpOnCooldown = true;
        yield return new WaitForSeconds(profile.jumpCooldown);
        isJumpOnCooldown = false;
    }

    IEnumerator WallJumpCooldown()
    {
        isWallJumpOnCooldown = true;
        yield return new WaitForSeconds(profile.wallJumpCooldown);
        isWallJumpOnCooldown = false;
    }

    private void GroundedJump()
    {
        StartCoroutine(JumpCooldown());

        isGrounded = false;
        forces.AddForce(profile.jumpForce);
    }

    private void WallJump()
    {
        StartCoroutine(WallJumpCooldown());
        Debug.Log("JumpJump");
        prevWallNormal = wallNormal;
        canWallJump = false;
        isOnWall = false;

        //Add horizontal force to vertical jump force
        Vector3 bounceVec = wallNormal * profile.wallJumpForce + profile.jumpForce.force;
        Forces.Force wallJumpForce = new Forces.Force(bounceVec, profile.jumpForce.drag, profile.jumpForce.time);

        forces.AddForce(wallJumpForce);
        horizontalVelocity = wallNormal * profile.wallJumpForce;
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

    }

    void CheckGrounded()
    {
        //SET GROUNDED STATE HERE
        if (airTime > profile.jumpCooldown)
            isGrounded = Physics.CheckSphere(transform.position + profile.offset, profile.radius, profile.groundingLayers);

        if (!isGrounded && wasGrounded)
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

    private void CheckOnWall()
    {
        if (isGrounded)
        {
            isOnWall = false;
            canWallJump = false;
            return;
        }

        // check four directions rather than sphere cast to get wall normal
        Vector3[] wallCheckDirections =
        {
            transform.right,
            -transform.right,
            transform.forward,
            -transform.forward
        };

        foreach (Vector3 direction in wallCheckDirections)
        {
            if (Physics.Raycast(transform.position, direction, out hit, profile.radius + 0.25f, profile.wallLayer))
            {
                wallNormal = hit.normal;
                isOnWall = true;
                break;
            }
        }

        if (isOnWall)
        {
            if (prevWallNormal != wallNormal)
                isWallJumpOnCooldown = false;
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }
    }
}
