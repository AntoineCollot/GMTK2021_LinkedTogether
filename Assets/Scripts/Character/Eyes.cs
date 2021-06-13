using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public Sprite[] sprites;

    [Header("Blink")]
    public float minBlinkinterval = 2;
    public float maxBlinkinterval = 10;
    public int blinkAnimFramerate = 10;
    float nextBlindTime;
    int animId = 0;
    bool wasDoingEffort = false;

    [Header("Position")]
    public Vector2 position1;
    public Vector2 position2;
    public Vector2 position3;
    public Vector2 positionHeart;

    new SpriteRenderer renderer;
    EffortManager effort;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        effort = GetComponentInParent<EffortManager>();

        KeyBunch.Instance.onKeySwitch.AddListener(UpdatePosition);
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (effort.IsDoingEffort)
        {
            StopAllCoroutines();
            //effort is sprite 3
            renderer.sprite = sprites[3];
            wasDoingEffort = true;
        }
        else
        {
            if (wasDoingEffort)
            {
                animId = 0;
                wasDoingEffort = false;
            }
            if (Time.time > nextBlindTime)
            {
                nextBlindTime = Time.time + Random.Range(minBlinkinterval, maxBlinkinterval);

                StopAllCoroutines();
                StartCoroutine(Blink());
            }
            renderer.sprite = sprites[animId];
        }
    }

    IEnumerator Blink()
    {
        for (int i = 1; i < 3; i++)
        {
            animId = i;
            yield return new WaitForSeconds(1.0f / blinkAnimFramerate);
        }
        for (int i = 2; i >= 0; i--)
        {
            animId = i;
            yield return new WaitForSeconds(1.0f / blinkAnimFramerate);
        }
    }

    void UpdatePosition()
    {
        if(KeyBunch.Instance.CurrentKey.material ==KeyType.KeyMaterial.Heart)
        {
            transform.localPosition = positionHeart;
            return;
        }

        switch (KeyBunch.Instance.CurrentKeyLength)
        {
            case 1:
            default:
                transform.localPosition = position1;
                break;
            case 2:
                transform.localPosition = position2;
                break;
            case 3:
                transform.localPosition = position3;
                break;
        }
    }
}
