using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStateController : MonoBehaviour
{
    //States
    public CharacterState freezeDirectionalInputsState;
    public CharacterState freezeJumpInputsState;
    public CharacterState noGravityState;
    public CharacterState noFrictionState;
    public CharacterState isMovingState;
    public CharacterState hitStunState;
    public CharacterState cannotBeGroundedState;

    //Components
    public CharacterMovementController movementController { get; private set; }
    public new Rigidbody2D rigidbody { get; private set; }
    public Animator anim { get; private set; }

    //Events
    [HideInInspector] public UnityEvent onInterrupt = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        freezeDirectionalInputsState = new CharacterState();
        freezeJumpInputsState = new CharacterState();
        noGravityState = new CharacterState();
        noFrictionState = new CharacterState();
        isMovingState = new CharacterState();
        hitStunState = new CharacterState();
        cannotBeGroundedState = new CharacterState();

        //Freeze every input during hitstun
        freezeDirectionalInputsState.Add(hitStunState);
        freezeJumpInputsState.Add(hitStunState);

        movementController = GetComponent<CharacterMovementController>();
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    /// <summary>
    /// Stun the character controller for a given time
    /// </summary>
    /// <param name="time"></param>
    public void StunForTime(float time)
    {
        StartCoroutine(StunForTimeC(time));
    }

    IEnumerator StunForTimeC(float time)
    {
        CharacterStateToken freezeToken = new CharacterStateToken();
        hitStunState.Add(freezeToken);
        freezeToken.SetOn(true);

        yield return new WaitForSeconds(time);

        hitStunState.Remove(freezeToken);
    }

    public Vector2 ApplyFacingDirection(Vector2 v)
    {
        if (movementController.facingDirection == Direction.Left)
            v.x *= -1;

        return v;
    }

    public Vector2 RelativeToWorldPos(Vector2 relativePos)
    {
        return (Vector2)transform.position + ApplyFacingDirection(relativePos);
    }
}
