using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    TextMeshProUGUI _textObject;

    private void Start()
    {
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.timeSinceLevelLoad;

        string m = Mathf.FloorToInt(t / 60).ToString().PadLeft(2, '0');
        string s = Mathf.FloorToInt(t % 60).ToString().PadLeft(2, '0');

        _textObject.text = m + ":" + s;
    }
}
