using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJBT_BodyController : MonoBehaviour
{
    [SerializeField] CharacterController body;
    [SerializeField] float playerSpeed;
    [SerializeField] float maximumJumpLength = 1f;
    //[Tooltip("Jump strength should be higher than gravity")]
    [SerializeField] float jumpStrength = 19f;
    [SerializeField] float jumpFalloff = 10f;
    float jumpTimer;
    [SerializeField] float gravityStrength = 9.8f;
    [SerializeField] Vector3 groundCheckOffset = new Vector3(0f, -1f, 0f);
    [Tooltip("How long after the player isn't touching the floor, should they still be able to jump")]
    [SerializeField] float jumpLeniency = 0.5f;
    float groundedTimer;
    bool jumping;
    bool grounded;
    [Tooltip("What should the player register as ground (they can only jump whilst grounded)")]
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckRadius = 0.3f;

    private void Start()
    {
        jumpTimer = 0f;
    }

    private void Update()
    {
        MoveBody();
    }

    void MoveBody()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime;
        float moveZ = Input.GetAxisRaw("Vertical") * playerSpeed * Time.deltaTime;

        float moveY = -gravityStrength * Time.deltaTime;//This gets over-written if the player is jumping

        if (!jumping && Physics.CheckSphere(body.transform.position + groundCheckOffset, groundCheckRadius, groundLayerMask))
        {
            grounded = true;
            groundedTimer = jumpLeniency;//if the player leaves a platform, they should have a short amount of time to jump. basically so they can time their jump perfectly even if they do the jump a bit late
        }
        else
        {
            groundedTimer -= Time.deltaTime;//the grounded timer is how long the player can be off the ground and still be allowed to jump. (if they fall off a platform and then jump. This is set to 0 if the player actually jumps tho)
            if (groundedTimer <= 0f)
                grounded = false;
        }

        if (Input.GetButtonDown("Jump") && !jumping && grounded)
        {
            //do a check to see if the player is on the ground before letting the player jump
            jumping = true;
            jumpTimer = maximumJumpLength;
            groundedTimer = 0f;//there was a bug where the player can jump numerous times whilst the ground timer ticks down. Now it sets to 0 when the player jumps. The ground timer only exists to give leniency to the jumping window, not to give multiple jumps
            Debug.Log("Boing");
        }
        //if (jumping || jumpTimer > 0f)
        if (jumpTimer > 0f)
        {
            //jump feels floaty, going to try adjusting the speed over time
            jumpTimer -= Time.deltaTime;

            float jumpPower = Mathf.Lerp(jumpStrength / jumpFalloff, jumpStrength, jumpTimer / maximumJumpLength);//remember this lerp moves backwards cus it will start at 1


            if (jumping)
            {
                if (Input.GetButton("Jump"))
                {
                    //if the player is jumping, and holding the button down. give them more distance on their jump as long as the jump timer lasts
                    moveY += jumpPower * Time.deltaTime;
                    //whilst jumping, i want to reduce teh strength of gravity for maybe the start of the jump
                }
                else 
                {
                    jumping = false;
                    moveY += jumpPower * Time.deltaTime * 0.5f;
                }

            }
            else
            {
                jumpTimer -= Time.deltaTime;//double the speed at which the player falls to the ground
                moveY += jumpPower * Time.deltaTime * 0.5f;//
            }
            
            if (jumpTimer <= 0f)
            {
                jumping = false;
            }
        }

        Vector3 whereToMove = moveX * this.transform.right + moveY * this.transform.up + moveZ * this.transform.forward;
        body.Move(whereToMove);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(body.transform.position + groundCheckOffset, groundCheckRadius);
    }
}
