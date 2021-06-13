using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallBunch : BunchBase
{
    public Material idleMat = null;
    public Material highlightMat = null;
    public const int MAX_KEYS = 5;

    public SpriteRenderer[] bunchRenderers;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        selectedKey = 0;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Make sure it's the player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //Set itself as the wallbunch of the player
        KeyBunch.Instance.contactWallBunch = this;

        GiveAllKeysToPlayer();

        SetHighlight(true);
    }

    protected void OnTriggerExit2D(Collider2D collision)
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
        foreach(SpriteRenderer renderer in bunchRenderers)
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

        onBunchUpdated.Invoke();

        AudioManager.PlaySound(2);
    }
}
