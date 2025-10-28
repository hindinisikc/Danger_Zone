using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarouselScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.2f;

    void Update()
    {
        // Get the mouse's horizontal position
        float mouseX = Input.GetAxis("Mouse X");

        // Scroll horizontally based on the mouse's X movement
        if (Mathf.Abs(mouseX) > 0.1f)
        {
            float scrollValue = scrollRect.horizontalNormalizedPosition - mouseX * scrollSpeed * Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollValue);
        }
    }
}
