using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager _instance;

    public Image imageComponent;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Input.mousePosition;
    }

    public void SetAndShowToolTip(Sprite tooltipImage)
    {
        gameObject.SetActive(true);
        imageComponent.sprite = tooltipImage;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        imageComponent.sprite = null;
    }
}
