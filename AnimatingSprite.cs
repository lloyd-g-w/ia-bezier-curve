using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Threading;

public class AnimatingSprite : MonoBehaviour
{

    [SerializeField] private Slider tSlider;
    [SerializeField] private Text tText;
    [SerializeField] private GameObject circle;
    [SerializeField] private Toggle animateToggle;
    public Bezier_Splines_Viz Bez;
    private int p = 0;
    private bool forward = true;
    private float rate = Navigation.rate;






    // Start is called before the first frame updatef3
    void Start()
    {
        tSlider.maxValue = Navigation.SplineCount * 100;
        StartCoroutine(animateObj());
    }

    // Update is called once per frame


    IEnumerator animateObj()
    {
        while (true)
        {
            if (animateToggle.isOn)
            {
                if (forward == true)
                {
                    if (tSlider.value == tSlider.maxValue)
                    {
                        forward = false;
                        tSlider.value -= 1;
                    }
                    else tSlider.value += 1;
                }
                else if (tSlider.value == tSlider.minValue)
                {
                    forward = true;
                    tSlider.value -= 1;
                }
                else tSlider.value -= 1;
            }
            yield return new WaitForSeconds(rate);
        }
        
    }
    


    void Update()
    {
        rate = Navigation.rate;
        var t = tSlider.value/100;
        var tToText = t.ToString();
        if (tToText.Contains(".") == false){
            tToText += ".";
        }
        if (tToText.Length < 4)
        {
            for (int i = 0; i <= 4 - tToText.Length; i++)
            {
                tToText += "0";
            }
            tText.text = "t = " + tToText;
        }
        else tText.text = "t = " + tToText;

        if (p != 0)
        {
            circle.transform.position = Bez.Bezierpts[(int)tSlider.value];
        }
        p += 1;
        

    }
}
