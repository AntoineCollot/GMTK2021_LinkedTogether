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

        //If in contact with a wallBunch, leave the ex-selected key there
        if (contactWallBunch != null && contactWallBunch.keys.Count<WallBunch.MAX_KEYS)
        {
            //Add the key to the wallbunch
            contactWallBunch.AddKey(keys[selectedKey]);

            keys.RemoveAt(selectedKey);

            //Only change id if going back, since otherwise the id is now the one of the next key
            if (dir < 0 && keys.Count>1)
                selectedKey--;
        }
        else
        {
            //Change id
            selectedKey += dir;
        }

        //Loop ids
        if (selectedKey < 0)
            selectedKey = keys.Count - 1;
        selectedKey %= keys.Count;

        onKeySwitch.Invoke();
        onBunchUpdated.Invoke();

        anim.SetTrigger("Switch");
    }
}
