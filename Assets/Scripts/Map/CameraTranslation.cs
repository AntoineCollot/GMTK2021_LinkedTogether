using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTranslation : MonoBehaviour
{
    public static CameraTranslation Instance;
    Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Translate(Vector2 target, float size, float time)
    {
        StopAllCoroutines();
        StartCoroutine(TranslateAnim(target,size, time));
    }

    IEnumerator TranslateAnim(Vector2 target,float size, float time)
    {
        Vector2 originalPosition = transform.position;
        float originalSize = cam.orthographicSize;

        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / time;

            Vector3 newPos = Curves.QuadEaseInOut(originalPosition, target, Mathf.Clamp01(t));
            newPos.z = -10;
            transform.position = newPos;
            cam.orthographicSize = Curves.QuadEaseInOut(originalSize, size, Mathf.Clamp01(t));

            yield return null;
        }
    }
}
