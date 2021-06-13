using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public CharacterMovementController movementController;
    public AudioSource footStepSource;
    public AudioSource sfxSource;

    //0 Jump;
    //1 pick up key
    //2 switch key
    //3 Land;
    //4 Door open;
    //5 Lever
    public AudioClip[] clips;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(movementController.isGrounded && Mathf.Abs(PlayerInputs.Instance.movementInputs.x)>0.3f)
        {
            footStepSource.volume = 0.1f;
        }
        else
        {
            footStepSource.volume = 0;
        }
    }

    public static void PlaySound(int id)
    {
        if(Instance!=null)
            Instance.sfxSource.PlayOneShot(Instance.clips[id]);
    }
}
