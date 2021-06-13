using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int extentRight;
    public float cameraSize = 4;
    public Vector2 Position { get => transform.position; }
    public float MaxBound { get=> Position.x + extentRight; }
}
