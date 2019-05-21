using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class UIBehavior : MonoBehaviour
{

    private RawImage UI_Top;
    private RawImage UI_Right;
    private RawImage UI_Left;
    private RawImage UI_Life;
    private FirstPersonController player;
    private float pos = 1.0f;
    private int status = 0;

    // Start is called before the first frame update
    void Start()
    {
        UI_Top = GameObject.Find("UI_Top").GetComponent<RawImage>();
        UI_Right = GameObject.Find("UI_Right").GetComponent<RawImage>();
        UI_Left = GameObject.Find("UI_Left").GetComponent<RawImage>();
        UI_Life = GameObject.Find("UI_LifeFull").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {

        if (status != 0){
            pos += status * Time.deltaTime * 2;
            if (pos >= 1|| pos<=0) {
                pos = (int)pos;
                status = 0;
            }
            UI_Top.transform.localPosition = new Vector3(0, 250 * pos + 1, 0);
            UI_Right.transform.localPosition = new Vector3(100 * pos + 1, -250 * pos , 0);
            UI_Left.transform.localPosition = new Vector3(-100 * pos - 1, -250 * pos , 0);
        }


        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (status == 0) {
                if (pos == 0f) {
                    status = 1;
                } else {
                    status = -1;
                }
            }
        }


    }

    public void updateHealth(float percent) {

        if (percent >= 1) percent = 1;
        if (percent <= 0) percent = 0;

        percent *= 0.546f;
        percent += 0.227f;

        Rect newUV = UI_Life.uvRect;
        newUV.width = percent;
        UI_Life.uvRect = newUV;
        UI_Life.rectTransform.localPosition = new Vector3(-960+960*percent,0,0);
        UI_Life.rectTransform.sizeDelta = new Vector2(1920*percent,1080);
    }
}
