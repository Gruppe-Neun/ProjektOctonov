using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public delegate void ClickEvent(int information);

    public bool clickAble = true;
    public ClickEvent clickEvent;
    public int clickEventParam = 0;

    protected RawImage buttonImage;


    private void Awake() {
        buttonImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setClickevent(ClickEvent click) {
        
    }

    public void hover(bool isHovered) {
        if (isHovered && clickAble) {
            buttonImage.color = new Color(1, 1, 1, 0.8f);
        } else {
            buttonImage.color = new Color(1, 1, 1, 1);
        }
    }

    public virtual void click() {
        if (clickAble) {
            clickEvent(clickEventParam);
        }
    }

    public virtual void mirror() {
        transform.localScale = new Vector3(-1, 1, 1);
    }
}
