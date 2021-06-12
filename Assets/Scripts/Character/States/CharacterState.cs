using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : CharacterStateBase
{
    public List<CharacterStateBase> tokens = new List<CharacterStateBase>();

    /// <summary>
    /// True if any token is true
    /// </summary>
    public override bool IsOn
    {
        get
        {
            foreach (CharacterStateBase token in tokens)
            {
                if (token.IsOn)
                    return true;
            }
            return false;
        }
    }

    public void Add(CharacterStateBase token)
    {
        tokens.Add(token);
    }

    public void Remove(CharacterStateBase token)
    {
        tokens.Remove(token);
    }
}

