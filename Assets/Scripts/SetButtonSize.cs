using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetButtonSize : MonoBehaviour
{
    public Canvas canvas;
    public Button leftButton; 
    public Button jumpButton;
    public Button rightButton;

    private Vector2 canvasSize;

    private void Awake() {
        canvasSize = new Vector2(canvas.GetComponent<RectTransform>().rect.width, canvas.GetComponent<RectTransform>().rect.height);

        ///leftButton 
    }
}
