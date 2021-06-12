using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateToken : CharacterStateBase
{
    private bool isOn = false;

    public override bool IsOn
    {
        get
        {
            return isOn;
        }
    }

    public void SetOn(bool value)
    {
        isOn = value;
    }
}
