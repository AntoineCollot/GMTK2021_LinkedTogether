using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBunchAway : MonoBehaviour
{
    CharacterMovementController movementController;
    new Rigidbody2D rigidbody;
    public float forceAmount;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponentInParent<CharacterMovementController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 force = Vector2.left * forceAmount;
        if (movementController.facingDirection == Direction.Left)
            force *= -1;
        rigidbody.AddForce(force, ForceMode2D.Force);
    }
}
