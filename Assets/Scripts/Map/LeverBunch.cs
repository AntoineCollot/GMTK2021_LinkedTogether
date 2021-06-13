using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverBunch : WallBunch
{
    public float slideTime = 0.75f;
    public enum State { Off, On }
    State currentState;

    [Header("Lever")]
    public Transform slidingLever;
    public Vector2 leverPositionOff;
    public Vector2 leverPositionOn;

    [Header("Obstacle")]
    Vector3 originalObstaclePos;
    public Transform slidingObstacle;
    public Vector2 obstaclePositionOff;
    public Vector2 obstaclePositionOn;

    public bool HasHeavyKey
    {
        get
        {
            return keys.Any(k => k.IsHeavy);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        onBunchUpdated.AddListener(LookForSlide);
        if (slidingObstacle != null)
            originalObstaclePos = slidingObstacle.position;

        LookForSlide();
    }

    void LookForSlide()
    {
        if (HasHeavyKey && currentState != State.On)
            StartCoroutine(Slide(State.On));
        else if (!HasHeavyKey && currentState != State.Off)
            StartCoroutine(Slide(State.Off));
    }

    IEnumerator Slide(State state)
    {
        float t = 0;

        AudioManager.PlaySound(5);
        currentState = state;
        //Collision
        //slidingObstacle.GetComponent<Collider2D>().enabled = state == State.Off;

        while (t < 1)
        {
            t += Time.deltaTime / slideTime;
            float effectiveT = Mathf.Clamp01(t);
            if (state == State.Off)
                effectiveT = 1 - effectiveT;

            slidingLever.localPosition = Curves.QuadEaseInOut(leverPositionOff, leverPositionOn, effectiveT);
            if (slidingObstacle != null)
                slidingObstacle.position = originalObstaclePos + (Vector3)Curves.QuadEaseInOut(obstaclePositionOff, obstaclePositionOn, effectiveT);

            yield return null;
        }

    }
}
