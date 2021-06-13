using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class MapBuilder : MonoBehaviour
{
#if UNITY_EDITOR
    public int cellSize = 1;
    const int MAP_LAYER = 8;
    public LayerMask mapMask = 1 << MAP_LAYER;

    public Transform cellParent;
    public WallBunch wallBunchPrefab;
    public Transform wallBunchParent;
    public Door[] doorPrefabs;
    public Transform doorParent;

    Camera cam;

    [Header("Blocks")]
    public List<Sprite> blockLibrary = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        //EditorApplication.update += MapEditorUpdate;
    }

    private void OnDisable()
    {
        //EditorApplication.update -= MapEditorUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        MapEditorUpdate();
    }
#endif

    void MapEditorUpdate()
    {
        //Create
        if (Input.GetMouseButtonDown(0))
        {
            WanderBlock(1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            WanderBlock(-1);
        }
        //Delete
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Delete))
        {
            GameObject obj = GetObjectAtMousePos();
            if (obj != null)
                DestroyCell(obj);
        }

        //Rotate
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCell(-1);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCell(1);
        }
        //Inverse
        if (Input.GetKeyDown(KeyCode.R))
        {
            InverseCell();
        }

        //Prefabs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateDoor(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateDoor(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateDoor(2);
        }
        //Prefabs
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CreateWallBunch();
        }
    }

    void RotateCell(int dir)
    {
        SpriteRenderer renderer = GetRendererAtMousePos();
        if (renderer == null)
            return;

        if (dir > 0)
            renderer.transform.Rotate(Vector3.forward * -90);
        else
            renderer.transform.Rotate(Vector3.forward * 90);
    }

    void InverseCell()
    {
        GameObject obj = GetObjectAtMousePos();
        if (obj == null)
            return;

        Vector3 scale = obj.transform.localScale;
        scale.x *= -1;
        obj.transform.localScale = scale;
    }

    void WanderBlock(int dir)
    {
        SpriteRenderer renderer = GetRendererAtMousePos();

        //No object yet, create one
        if (renderer == null)
        {
            renderer = CreateSpriteCell(GetMouseCoords());
        }

        //Find the ID
        int id = GetSpiteID(renderer.sprite);

        if (dir > 0)
        {
            id++;
        }
        if (dir < 0)
        {
            if (id == -1)
                id = blockLibrary.Count;
            id--;
        }

        //If we reached the end, del block
        if (id >= blockLibrary.Count || id < 0)
        {
            DestroyCell(renderer.gameObject);
        }
        else
        {
            renderer.sprite = blockLibrary[id];
        }
    }

    int GetSpiteID(Sprite sprite)
    {
        if (sprite == null)
            return -1;

        if (blockLibrary.Contains(sprite))
            return blockLibrary.IndexOf(sprite);

        return -1;
    }

    void DestroyCell(GameObject go)
    {
        Destroy(go);
    }

    SpriteRenderer CreateSpriteCell(Vector2Int coords)
    {
        GameObject newCell = new GameObject("Cell_" + coords.ToString(), typeof(SpriteRenderer), typeof(BoxCollider2D));
        newCell.transform.SetParent(cellParent, false);
        newCell.transform.position = new Vector3(coords.x, coords.y);
        newCell.layer = MAP_LAYER;
        newCell.GetComponent<BoxCollider2D>().size = Vector2.one * cellSize;

        SpriteRenderer renderer = newCell.GetComponent<SpriteRenderer>();
        renderer.sortingLayerName = "Map Back";
        renderer.sortingOrder = 5;
        return renderer;
    }

    void CreateDoor(int type)
    {
        Vector2Int coords = GetMouseCoords();
        Instantiate(doorPrefabs[type], new Vector3(coords.x, coords.y), Quaternion.identity, doorParent);
    }

    void CreateWallBunch()
    {
        Vector2Int coords = GetMouseCoords();
        Instantiate(wallBunchPrefab, new Vector3(coords.x, coords.y), Quaternion.identity, wallBunchParent);
    }

    GameObject GetObjectAtMousePos()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(GetMouseCoords());
        if (hitCollider == null)
            return null;

        return hitCollider.gameObject;
    }

    SpriteRenderer GetRendererAtMousePos()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(GetMouseCoords(), mapMask);
        if (hitCollider == null)
            return null;

        SpriteRenderer renderer = hitCollider.GetComponent<SpriteRenderer>();
        return renderer;
    }

    Vector2Int CoordsFromPos(Vector2 position)
    {
        Vector2Int coords = new Vector2Int();
        coords.x = Mathf.RoundToInt(position.x / cellSize) * cellSize;
        coords.y = Mathf.RoundToInt(position.y / cellSize) * cellSize;
        return coords;
    }

    Vector2Int GetMouseCoords()
    {
        if (cam == null)
            cam = Camera.main;
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        return CoordsFromPos(mouseWorldPos);
    }

    [ContextMenu("Activate Keys")]
    public void ActivateAllKeys()
    {
        int count = 0;
        UpdateKeyActivation[] updateKeyActivations  = GetComponentsInChildren<UpdateKeyActivation>(true);
        foreach(UpdateKeyActivation obj in updateKeyActivations)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                count++;
                obj.gameObject.SetActive(true);
            }
        }
        print(gameObject.name+" : Activated " + count + " key objects");
    }


    private void OnDrawGizmos()
    {
        Color col = new Color(1, 1, 1, 0.05f);

        Vector2Int coords = GetMouseCoords();
        Vector3 pos = new Vector3(coords.x, coords.y, 0);
        Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
    }
#endif
}
