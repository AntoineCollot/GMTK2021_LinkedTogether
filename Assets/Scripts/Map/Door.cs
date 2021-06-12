using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int requieredLength = 1;
    Animator anim;
    int animOpenable;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animOpenable = Animator.StringToHash("IsOpenable");
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBunch.Instance != null)
            anim.SetBool(animOpenable, KeyBunch.Instance.HasKeyOfLength(requieredLength));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Make sure it's the player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //Do not open if not the right length
        if (KeyBunch.Instance.CurrentKeyLength != requieredLength)
        {
            anim.SetTrigger("Wrong");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Make sure it's the player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        //Make sure its grounded
        if (!collision.gameObject.GetComponent<CharacterMovementController>().isGrounded)
            return;

        //Do not open if not the right length
        if (KeyBunch.Instance.CurrentKeyLength != requieredLength)
        {
            return;
        }

        anim.SetTrigger("Open");
        GetComponent<Collider2D>().enabled = false;
    }
}
