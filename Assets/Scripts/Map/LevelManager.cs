using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public float camTranslationTime = 2;
    Transform player;
    int currentLevelId = -1;
    int instantiatedLevelId = -1;

    public class LevelEvent : UnityEvent<int> { }
    public LevelEvent onLevelChanged = new LevelEvent();

    public static LevelManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentLevelId!= GetLevelThePlayerIsIn())
            ChangeLevel();
    }

    //bool CheckIfPlayerChangedLevel()
    //{
    //    float minBound = Mathf.Infinity;
    //    if (currentLevelId > 0)
    //        minBound = levels[currentLevelId - 1].MaxBound;

    //    float maxBound = Mathf.Infinity;
    //    if(currentLevelId>=0 && currentLevelId < levels.Count)
    //        maxBound = levels[currentLevelId].MaxBound;

    //    return player.position.x < minBound || player.position.x > maxBound;
    //}

    int GetLevelThePlayerIsIn()
    {
        int level = 0;
        //Find which level the player is in
        for (int i = levels.Count-1; i >0; i--)
        {
            if (player.position.x > levels[i-1].MaxBound)
            {
                level = i;
                break;
            }
        }

        return level;
    }

    void ChangeLevel()
    {
        currentLevelId = GetLevelThePlayerIsIn();
        int levelsToInstantiate = Mathf.Min(currentLevelId +1, levels.Count-1);

        //Instantiate levels
        for (int i = instantiatedLevelId+1; i <= levelsToInstantiate; i++)
        {
            Instantiate(levels[i], transform);
        }
        instantiatedLevelId = Mathf.Max(instantiatedLevelId, levelsToInstantiate);

        //Activate levels
        int minActiveLevelId = Mathf.Max(0, currentLevelId - 2);
        int maxActiveLevelId = Mathf.Min(levels.Count - 1, currentLevelId + 2);

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].gameObject.SetActive(i >= minActiveLevelId && i <= maxActiveLevelId);
        }

        //Camera
        CameraTranslation.Instance.Translate(levels[currentLevelId].Position, levels[currentLevelId].cameraSize, camTranslationTime);
        onLevelChanged.Invoke(currentLevelId);
    }
}
