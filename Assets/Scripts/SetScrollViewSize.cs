using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScrollViewSize : MonoBehaviour
{
    public GameObject scrollview, component;
    public static SetScrollViewSize instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }


        float propo = ((float)1080 / (float)2400) / ((float)Screen.width / (float)Screen.height);
        RectTransform rt1 = (RectTransform)component.transform;
        rt1.sizeDelta = new Vector2(0, 1950 * propo);
        RectTransform rt2 = (RectTransform)scrollview.transform;
        rt2.sizeDelta = new Vector2(0, 1800 * propo);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
