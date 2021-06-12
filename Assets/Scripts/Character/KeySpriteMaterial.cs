using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpriteMaterial : MonoBehaviour
{
    public int keyPosition;
    new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        KeyBunch.Instance.onKeySwitch.AddListener(UpdateMaterial);
    }

    void UpdateMaterial()
    {
        renderer.material = KeyBunch.Instance.GetKeyAtPosition(keyPosition).Mat;
    }
}
