using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour
{
    [HideInInspector] public Vector2 movementInputs;

    //Jump
    [HideInInspector] public bool jumpButtonDown;
    [HideInInspector] public bool jumpButton;
    public UnityEvent onJumpButton = new UnityEvent();

    //Switch
    [HideInInspector] public bool switchUpButtonDown;
    [HideInInspector] public bool switchDownButtonDown;
    public UnityEvent onSwitchUpButton = new UnityEvent();
    public UnityEvent onSwitchDownButton = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementInputs.x = Input.GetAxisRaw("Horizontal");
        //movementInputs.y= Input.GetAxisRaw("Vertical");

        jumpButtonDown = Input.GetKeyDown(KeyCode.Space);
        jumpButton = Input.GetKey(KeyCode.Space);

        switchUpButtonDown = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W);
        switchDownButtonDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

        if (jumpButtonDown)
            onJumpButton.Invoke();

        if (switchUpButtonDown)
            onSwitchUpButton.Invoke();

        if (switchDownButtonDown)
            onSwitchDownButton.Invoke();
    }
}
