using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public delegate void ClickEvent(string message);

    private RawImage buttonImage;
    public ClickEvent clickEvent;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setClickevent(ClickEvent click) {
        
    }

    public void hover(bool isHovered) {
        if (isHovered) {
            buttonImage.color = new Color(1, 1, 1, 0.8f);
        } else {
            buttonImage.color = new Color(1, 1, 1, 1);
        }
    }

    public void click() {
        clickEvent("test");
    }
}
