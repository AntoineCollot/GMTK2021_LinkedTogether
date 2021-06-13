using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLevel : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        LevelManager.Instance.onLevelChanged.AddListener(UpdateText);    
    }

    void UpdateText(int levelId)
    {
        text.text = $"Level : {levelId + 1}";
    }
}
