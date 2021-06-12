using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GroundChecker : MonoBehaviour
{
    [Tooltip("Layers with map elements only")] public LayerMask mapLayer = 256;
    [SerializeField] float groundCastRadius = 0.4f;
    [SerializeField] float groundCastDistance = 0;
    bool isGrounded;
    [Tooltip("Does the ground check refresh its grounded check itself or does it wait to be called by an other component ?"), HideInInspector] public bool autoUpdateGroundState = true;
    new Collider2D collider;

    public bool IsGrounded
    {
        get
        {
            return isGrounded && !cannotBeGroundedState.IsOn;
        }
    }

    //State
    public CharacterState cannotBeGroundedState { get; private set; }

    //Events
    [HideInInspector] public UnityEvent onGrounded = new UnityEvent();
    [HideInInspector] public UnityEvent onAirborn = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();

        cannotBeGroundedState = new CharacterState();
    }

    // Update is called once per frame
    void Update()
    {
        if (autoUpdateGroundState)
            GroundCheck();
    }

    public void GroundCheck()
    {
        //If nothing happens, we are not grounded
        bool newGroundedState = false;

        //Check if the thing is allowed to be grounded. It may not be during certain skill of after jumping
        if (!cannotBeGroundedState.IsOn)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, groundCastRadius, Vector2.down, groundCastDistance, mapLayer);
            foreach (RaycastHit2D hit in hits)
            {
                //Check if we hit ourself
                if (hit.collider == collider)
                {
                    continue;
                }
                //Otherwise it's a layer contained in the mapLayer mask, so it's the map
                else if (hit.collider != null)
                {
                    newGroundedState = true;
                }
            }
        }

        SetGrounded(newGroundedState);
    }

    public void SetGrounded(bool value)
    {
        //Check if the state changed
        if (isGrounded != value)
        {
            if (value)
                onGrounded.Invoke();
            else
                onAirborn.Invoke();
            isGrounded = value;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Ground check
        if (isGrounded)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCastDistance, groundCastRadius);
    }
#endif
}
