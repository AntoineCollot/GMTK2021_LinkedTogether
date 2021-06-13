using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeyColliderSize : MonoBehaviour
{
    new CapsuleCollider2D collider;
    const float COLLIDER_WIDTH = 0.4f;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        KeyBunch.Instance.onKeySwitch.AddListener(UpdateColliderSize);
        UpdateColliderSize();
    }

    void UpdateColliderSize()
    {
        if(KeyBunch.Instance.CurrentKey.material==KeyType.KeyMaterial.Heart)
        {
            collider.offset = new Vector2(0, -0.225f);
            collider.size = new Vector2(COLLIDER_WIDTH, 0.55f);
            return;
        }

        switch (KeyBunch.Instance.CurrentKey.length)
        {
            case 1:
                collider.offset = new Vector2(0, -0.15f);
                collider.size = new Vector2(COLLIDER_WIDTH, 0.7f);
                break;
            case 2:
                collider.offset = new Vector2(0, -0.08f);
                collider.size = new Vector2(COLLIDER_WIDTH, 0.84f);
                break;
            case 3:
            default:
                collider.offset = new Vector2(0, -0.05f);
                collider.size = new Vector2(COLLIDER_WIDTH, 0.9f);
                break;
        }
    }
}
