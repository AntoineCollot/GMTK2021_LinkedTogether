using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeySprite : MonoBehaviour
{
    public BunchBase bunch;
    public int keyPosition;
    new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        bunch.onBunchUpdate.AddListener(OnBunchUpdate);
        OnBunchUpdate();
    }

    void OnBunchUpdate()
    {
        UpdateMaterial();
        UpdateSprite();
    }

    void UpdateMaterial()
    {
        Key key = bunch.GetKeyAtPosition(keyPosition);
        if(key!=null)
            renderer.material = key.Mat;
    }

    void UpdateSprite()
    {
        Key key = bunch.GetKeyAtPosition(keyPosition);
        if (key != null)
            renderer.sprite = key.Sprite;
    }
}
