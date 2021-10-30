using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeUpBlink : MonoBehaviour
{
    public Image eyelid;

    private Color cl;
    private bool closing = true;
    private int phase = 255;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (phase > 0)
        {
            var tempColor = eyelid.color;
            if (closing)
            {
                tempColor.a = tempColor.a + 0.01f;
            }
            else
            {
                tempColor.a = tempColor.a - 0.01f;
            }

            eyelid.color = tempColor;
            phase--;
        }
        else
        {
            closing = !closing;
            phase = 255;
        }
    }
}
