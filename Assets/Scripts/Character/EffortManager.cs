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
    public Transform bodyTransform;
    float refRotation;
    float currentAngle;
    public float rotationSmooth = 0.1f;
    public float maxEffortAngle = 25;
    public int maxEffortKey = 5;

    CharacterMovementController movementController;

    public bool IsDoingEffort
    {
        get {
            return (KeyBunch.Instance.KeyCountInBunch >= effortKeyCount) && Mathf.Abs(PlayerInputs.Instance.movementInputs.x)>0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<CharacterMovementController>();

        KeyBunch.Instance.onBunchUpdated.AddListener(UpdateValues);
        UpdateValues();
    }

    private void Update()
    {
        RotateBasedOnEffort();
    }

    void UpdateValues()
    {
        Key key = KeyBunch.Instance.CurrentKey;
        movementController.moveSpeed = Mathf.Max(key.MoveSpeed - KeyBunch.Instance.KeyCountInBunch * movementSpeedLossPerKey, minMovementSpeed);
        movementController.jumpHeight = Mathf.Max(key.JumpHeight - KeyBunch.Instance.KeyCountInBunch * jumpHeightLossPerKey, minJumpHeight);
    }

    void RotateBasedOnEffort()
    {
        float effortAmount = Mathf.Abs(PlayerInputs.Instance.movementInputs.x) * KeyBunch.Instance.KeyCountInBunch / maxEffortKey;
        float effortAngle = Mathf.Lerp(0, maxEffortAngle, effortAmount) * Mathf.Sign(PlayerInputs.Instance.movementInputs.x);
        currentAngle = Mathf.SmoothDamp(currentAngle, effortAngle, ref refRotation, rotationSmooth);
        transform.localEulerAngles = new Vector3(0, 0, -currentAngle);
    }
}
