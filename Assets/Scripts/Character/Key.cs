using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "ScriptableObjects/Key", order = 1)]
public class Key : ScriptableObject
{
    public KeyType.KeyMaterial material;
    public int length = 1;

    public float MoveSpeed { get => KeyType.materials[material].moveSpeed; }
    public float JumpHeight { get => KeyType.materials[material].jumpHeight; }
    public bool IsHeavy { get => KeyType.materials[material].isHeavy; }
    public Material Mat { get => KeyType.materials[material].mat; }
    public Sprite Sprite
    {
        get
        {
            switch (length)
            {
                case 1:
                    return Sprite1;
                case 2:
                default:
                    return Sprite2;
                case 3:
                    return Sprite3;
            }
        }
    }
    Sprite Sprite1 { get => KeyType.materials[material].sprite1; }
    Sprite Sprite2 { get => KeyType.materials[material].sprite2; }
    Sprite Sprite3 { get => KeyType.materials[material].sprite3; }
}