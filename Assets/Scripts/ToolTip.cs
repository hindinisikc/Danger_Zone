using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public Sprite toolTipImage;

    private void OnMouseEnter()
    {
        ToolTipManager._instance.SetAndShowToolTip(toolTipImage);
    }

    private void OnMouseExit()
    {
        ToolTipManager._instance.HideToolTip();
    }
}
