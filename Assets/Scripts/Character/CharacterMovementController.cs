using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Direction { Top, Right, Bottom, Left, None }
public class CharacterMovementController : MonoBehaviour
{
    protected CharacterStateController charState;
    PlayerInputs inputs;

    [Header("Movements")]
    public float moveSpeed = 1;
    [SerializeField, Tooltip("How much smooth to apply on the ground")] protected float groundMovementSmooth = 0.05f;
    [SerializeField, Tooltip("How much smooth to apply in the air (increase for more inertia)")] protected float airMovementSmooth = 0.3f;
    protected new Rigidbody2D rigidbody;
    protected Vector2 refVelocity;
    protected CharacterStateToken isMovingToken;

    [Header("Jump")]
    [Tooltip("How high the jump can go")] public float jumpHeight = 3;
    [Tooltip("How much time it takes to reach the apex of a normal jump")] public float timeToJumpApex = 0.4f;
    [Tooltip("How much the gravity should increase when releasing the jump button early")] public float shortHopGravityMultiplier = 1.5f;
    [Tooltip("How much the gravity should increase when the character is going down")] public float fallingGravityMultiplier = 2f;
    public int airJumpCount = 0;
    protected int airJumpUsed;
    protected float defaultGravity;
    protected float currentGravity;

    //Facing
    [HideInInspector] public Direction facingDirection;

    //Grounded
    GroundChecker groundChecker;
    protected CharacterStateToken freezeGroundedStateAfterJump = new CharacterStateToken();

    //Anims
    protected Animator anim;

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    public Vector2 Forward
    {
        get
        {
            switch (facingDirection)
            {
                case Direction.Left:
                    return Vector2.left;
                case Direction.Right:
                default:
                    return Vector2.right;
            }
        }
    }


    public bool isGrounded
    {
        get
        {
            return groundChecker.IsGrounded;
        }
    }

    // Start is called before the first frame update
    protected void Awake()
    {
        charState = GetComponent<CharacterStateController>();
        groundChecker = GetComponent<GroundChecker>();
        inputs = GetComponent<PlayerInputs>();

        isMovingToken = new CharacterStateToken();

        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected void Start()
    {
        inputs.onJumpButton.AddListener(Jump);
        charState.isMovingState.Add(isMovingToken);

        //Link the groundChecker cannot be grounded state to the characterController ones
        //So that setting the state on the characterController affect the groundChecker
        groundChecker.cannotBeGroundedState.Add(charState.cannotBeGroundedState);
        groundChecker.cannotBeGroundedState.Add(freezeGroundedStateAfterJump);
    }

    // Update is called once per frame
    protected void Update()
    {
        groundChecker.GroundCheck();
        if (groundChecker.IsGrounded)
        {
            //Reset the jump count
            airJumpUsed = 0;
        }

        UpdateGravityValue();
        UpdateFacing();
        Move();

        if (charState.hitStunState.IsOn)
        {
            rigidbody.simulated = false;
            anim.speed = 0;
        }
        else
        {
            rigidbody.simulated = true;
            anim.speed = 1;
        }

        anim.SetBool("IsGrounded", groundChecker.IsGrounded);
    }

    /// <summary>
    /// Update the direction the character should be looking in
    /// </summary>
    public void UpdateFacingDirection()
    {
        if (inputs.movementInputs.x > 0.1f)
        {
            facingDirection = Direction.Right;
        }
        else if (inputs.movementInputs.x < -0.1f)
        {
            facingDirection = Direction.Left;
        }
    }

    /// <summary>
    /// Called in update
    /// </summary>
    protected virtual void UpdateFacing()
    {
        //Get the current facing direction base don the directional inputs if allowed and grounded (do not turn in air with simple directional inputs)
        if (!charState.freezeDirectionalInputsState.IsOn && groundChecker.IsGrounded)
        {
            UpdateFacingDirection();
        }

        //Apply the direction
        switch (facingDirection)
        {
            case Direction.Right:
                anim.transform.localScale = new Vector3(1, 1, 1);
                break;
            case Direction.Left:
                anim.transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }

    public virtual void Move()
    {
        if (charState.freezeDirectionalInputsState.IsOn)
            return;

        Vector2 targetVelocity = rigidbody.velocity;
        targetVelocity.x = moveSpeed * inputs.movementInputs.x;

        //Default the move token as false
        isMovingToken.SetOn(false);

        switch (groundChecker.IsGrounded)
        {
            case true:
                rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, groundMovementSmooth);

                //Acitvate the move token if the velocity or input is great enough
                if (Mathf.Abs(rigidbody.velocity.x) > 0.1f || Mathf.Abs(inputs.movementInputs.x) > 0.1f)
                    isMovingToken.SetOn(true);
                break;
            case false:
                rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, airMovementSmooth);
                break;
        }

        anim.SetFloat("InputSpeed", Mathf.Abs(inputs.movementInputs.x));
    }

    protected void UpdateGravityValue()
    {
        //The default gravity is based on the jump height
        //jumpHeight = (gravity * TimeToJumpApex²) / 2
        defaultGravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        currentGravity = defaultGravity;

        //If we are going up
        if (rigidbody.velocity.y > 0.01f)
        {
            //If the jump input is not pressed while going up (we released early into the jump)
            if (!inputs.jumpButton)
            {
                currentGravity *= shortHopGravityMultiplier;
            }
        }
        //If we are falling (going down)
        else if (rigidbody.velocity.y < -0.01f && !groundChecker.IsGrounded)
        {
            currentGravity *= fallingGravityMultiplier;
        }

        //play with the rigidbody gravity multiplier to result in the right gravity
        if (charState.noGravityState.IsOn)
        {
            rigidbody.gravityScale = 0;
        }
        else
        {
            try
            {
            rigidbody.gravityScale = currentGravity / -Physics2D.gravity.y;
            }
            catch(System.Exception e)
            {
                print("Error");
            }
        }

    }

    public void Jump()
    {
        //Check if we can perform the input
        if (charState.freezeJumpInputsState.IsOn)
            return;

        //Check if a jump is available.
        if (!groundChecker.IsGrounded && airJumpUsed >= airJumpCount)
            return;

        //Count the jump
        //Do it before doing the actualy jump cause after that we don't know if we were grounded or not anymore
        if (!isGrounded)
        {
            airJumpUsed++;
            UpdateFacingDirection();
        }

        ForceJump();
    }

    public void ForceJump()
    {
        AudioManager.PlaySound(1);

        anim.SetTrigger("Jump");

        //Don't allow to be grounded for a few frames after jumping
        freezeGroundedStateAfterJump.SetOn(true);
        Invoke("AllowBeingGrounded", 0.05f);

        //finalVelocity = initialVelocity + acceleration * time
        //Solving for the apex, so at timeToJumpApex where final velocity = 0
        float jumpVelocity = defaultGravity * timeToJumpApex;

        //Apply the jump velocity
        Vector2 velocity = rigidbody.velocity;
        velocity.y = jumpVelocity;
        rigidbody.velocity = velocity;
    }

    public void JumpAtHeight(float height, bool playAnim)
    {
        if (playAnim)
            anim.SetTrigger("Jump");

        //Don't allow to be grounded for a few frames after jumping
        freezeGroundedStateAfterJump.SetOn(true);
        Invoke("AllowBeingGrounded", 0.05f);

        //jumpHeight = (gravity * TimeToJumpApex²) / 2
        //Solve for time to apex which is the thing we want to adjust
        float targeTimeToJumpApex = Mathf.Sqrt((2 * height) / currentGravity);

        //finalVelocity = initialVelocity + acceleration * time
        //Solving for the apex, so at timeToJumpApex where final velocity = 0
        float jumpVelocity = currentGravity * targeTimeToJumpApex;

        //Apply the jump velocity
        Vector2 velocity = rigidbody.velocity;
        velocity.y = jumpVelocity;
        rigidbody.velocity = velocity;
    }

    protected void AllowBeingGrounded()
    {
        freezeGroundedStateAfterJump.SetOn(false);
    }
}