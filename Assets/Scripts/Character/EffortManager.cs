using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffortManager : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeedLossPerKey = 0.5f;
    public float minMovementSpeed = 1f;

    [Header("Jump")]
    public float jumpHeightLossPerKey = 0.5f;
    public float minJumpHeight = 1.1f;

    [Header("Effort")]
    public int effortKeyCount = 3;

    CharacterMovementController movementController;

    public bool IsDoingEffort
    {
        get {
            return (KeyBunch.Instance.KeyCountInBunch - 1 > effortKeyCount) && Mathf.Abs(PlayerInputs.Instance.movementInputs.x)>0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<CharacterMovementController>();

        KeyBunch.Instance.onBunchUpdated.AddListener(UpdateValues);
        UpdateValues();
    }

    void UpdateValues()
    {
        Key key = KeyBunch.Instance.CurrentKey;
        movementController.moveSpeed = Mathf.Max(key.MoveSpeed - KeyBunch.Instance.KeyCountInBunch * movementSpeedLossPerKey, minMovementSpeed);
        movementController.jumpHeight = Mathf.Max(key.JumpHeight - KeyBunch.Instance.KeyCountInBunch * jumpHeightLossPerKey, minJumpHeight);
    }
}
