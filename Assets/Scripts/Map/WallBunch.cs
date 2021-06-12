﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallBunch : BunchBase
{
    public Material idleMat = null;
    public Material highlightMat = null;

    SpriteRenderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        selectedKey = 0;
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Make sure it's the player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //Set itself as the wallbunch of the player
        KeyBunch.Instance.contactWallBunch = this;

        GiveAllKeysToPlayer();

        SetHighlight(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Make sure it's the player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //Remove itself from the registered wall bunch of player
        if (KeyBunch.Instance.contactWallBunch == this)
            KeyBunch.Instance.contactWallBunch = null;

        SetHighlight(false);
    }

    public void SetHighlight(bool value)
    {
        foreach(SpriteRenderer renderer in renderers)
        {
            if (value)
                renderer.material = highlightMat;
            else
                renderer.material = idleMat;
        }
    }

    public void GiveAllKeysToPlayer()
    {
        KeyBunch.Instance.AddKeys(keys);
        keys.Clear();

        onBunchUpdate.Invoke();
    }
}
