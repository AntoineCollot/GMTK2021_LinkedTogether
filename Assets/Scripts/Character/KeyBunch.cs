using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyBunch : BunchBase
{
    [Header("Player Bunch")]

    [HideInInspector] public WallBunch contactWallBunch = null;

    public UnityEvent onKeySwitch = new UnityEvent();

    PlayerInputs inputs;
    Animator anim;
    CharacterMovementController movementController;

    public static KeyBunch Instance;

    public int CurrentKeyLength { get => keys[selectedKey].length; }
    public Key CurrentKey { get => keys[selectedKey]; }
    public int KeyCountInBunch { get => keys.Count - 1; }

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
        //Allow switch when grounded
        if (!movementController.isGrounded)
            return;

        //Make sure there are enough keys
        if (keys.Count < 2)
            return;

        //If in contact with a wallBunch, leave the ex-selected key there
        if (contactWallBunch != null && contactWallBunch.keys.Count < WallBunch.MAX_KEYS)
        {
            //Add the key to the wallbunch
            contactWallBunch.AddKey(keys[selectedKey]);

            keys.RemoveAt(selectedKey);

            Switch(-1,true);
        }
    }

    void Switch(int dir, bool forceSwitch = false)
    {
        //Allow switch when grounded
        if (!movementController.isGrounded)
            return;

        //Make sure there are enough keys
        if (!forceSwitch && keys.Count < 2)
            return;

        //Change id
        selectedKey += dir;

        //Loop ids
        if (selectedKey < 0)
            selectedKey = keys.Count - 1;
        selectedKey %= keys.Count;

        onKeySwitch.Invoke();
        onBunchUpdated.Invoke();

        anim.SetTrigger("Switch");
    }
}
