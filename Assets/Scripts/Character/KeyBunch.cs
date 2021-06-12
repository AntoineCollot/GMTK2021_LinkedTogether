using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyBunch : MonoBehaviour
{
    public List<Key> keys = new List<Key>();
    int selectedKey = 0;

    public UnityEvent onKeySwitch = new UnityEvent();

    PlayerInputs inputs;
    Animator anim;
    CharacterMovementController movementController;

    public static KeyBunch Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponentInParent<CharacterMovementController>();
        anim = movementController.GetComponentInChildren<Animator>();
        inputs = GetComponentInParent<PlayerInputs>();
        inputs.onSwitchUpButton.AddListener(OnSwitchUpButton);
        inputs.onSwitchDownButton.AddListener(OnSwitchDownButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSwitchUpButton()
    {
        Switch(1);
    }

    void OnSwitchDownButton()
    {
        Switch(-1);
    }

    void Switch(int dir)
    {
        //Allow switch when grounded
        if (!movementController.isGrounded)
            return;

        //Make sure there are enough keys
        if (keys.Count < 2)
            return;

        //Change id
        selectedKey += dir;

        //Loop ids
        if (selectedKey < 0)
            selectedKey = keys.Count - 1;
        selectedKey %= keys.Count;

        SelectKey(keys[selectedKey]);
        onKeySwitch.Invoke();

        anim.SetTrigger("Switch");
    }

    public void SelectKey(Key key)
    {
        movementController.moveSpeed = key.MoveSpeed;
        movementController.jumpHeight = key.JumpHeight;
    }

    public Key GetKeyAtPosition(int keyPosition)
    {
        int id = selectedKey + keyPosition;
        id %= keys.Count;

        return keys[id];
    }
}
