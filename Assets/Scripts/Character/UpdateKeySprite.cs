using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeySprite : MonoBehaviour
{
    public int keyPosition;
    new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        KeyBunch.Instance.onKeySwitch.AddListener(OnKeySwitch);
    }

    void OnKeySwitch()
    {
        UpdateMaterial();
        UpdateSprite();
    }

    void UpdateMaterial()
    {
        renderer.material = KeyBunch.Instance.GetKeyAtPosition(keyPosition).Mat;
    }

    void UpdateSprite()
    {
        renderer.sprite = KeyBunch.Instance.GetKeyAtPosition(keyPosition).Sprite;
    }
}
