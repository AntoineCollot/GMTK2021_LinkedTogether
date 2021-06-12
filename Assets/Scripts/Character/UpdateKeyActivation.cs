using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeyActivation : MonoBehaviour
{
    public BunchBase bunch;
    public int keyPosition;

    // Start is called before the first frame update
    void Start()
    {
        bunch.onBunchUpdate.AddListener(OnBunchUpdate);
        OnBunchUpdate();
    }

    void OnBunchUpdate()
    {
        gameObject.SetActive(keyPosition < bunch.keys.Count);
    }
}
