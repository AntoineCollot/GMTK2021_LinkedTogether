using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyType : MonoBehaviour
{
    public enum KeyMaterial { Iron, CastIron, Charged, Heart }
    [System.Serializable] public struct KeyMaterialPropreties
    {
        public float moveSpeed;
        public float jumpHeight;
        public bool isHeavy;

        [Header("Colors")]
        public Material mat;

        [Header("Sprites")]
        public Sprite sprite1;
        public Sprite sprite2;
        public Sprite sprite3;
    }

    [System.Serializable]
    public struct KeyMaterialPropretiesEditor
    {
        public KeyMaterial material;
        public KeyMaterialPropreties properties;
    }

    [SerializeField] List<KeyMaterialPropretiesEditor> materialList = new List<KeyMaterialPropretiesEditor>();
    public static Dictionary<KeyMaterial, KeyMaterialPropreties> materials;

    private void Awake()
    {
        materials = new Dictionary<KeyMaterial, KeyMaterialPropreties>();
        foreach(KeyMaterialPropretiesEditor m in materialList)
        {
            materials.Add(m.material, m.properties);
        }
    }
}
