using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class fps : MonoBehaviour
{
    public TMP_Text fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;
    private float _timer;

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > _timer) {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }
}
