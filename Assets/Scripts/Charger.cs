using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Fix consistency of _ in member variables
public class Charger : MonoBehaviour {

    public KeyCode chargeKey;
    private bool keyDown = false;
    private float startTime;
    private Image bar;
    
    private float chargeTime = 1.0f;

	// Use this for initialization
	void Start () {
        bar = GetComponent<Image>();

        bar.color = Color.white;
        bar.fillAmount = 0;
	}

    public float Charge()
    {
        float returnCharge = 0; // only return actual amount if user releases key
        if (keyDown)
        {
            if (Input.GetKeyUp(chargeKey))
            {
                keyDown = false;
                returnCharge = bar.fillAmount;
                bar.fillAmount = 0;
                bar.color = Color.white;
            }

            else
            {
                float fillAmount = (Time.time - startTime) / chargeTime; 
                bar.fillAmount = fillAmount;

                if (fillAmount >= 1f)
                {
                    bar.color = Color.green;
                }

                else if (fillAmount >= 0.75f)
                {
                    bar.color = Color.yellow;
                }

                else if (fillAmount >= 0.5f)
                {
                    bar.color = Color.blue;
                }

                else if (fillAmount >= 0.25f)
                {
                    bar.color = Color.red;
                }
            }
        }

        if (Input.GetKeyDown(chargeKey))
        {
            keyDown = true;
            startTime = Time.time;
        }

        return returnCharge;
    }

    public void Reset()
    {
        bar.color = Color.white;
        bar.fillAmount = 0;
        keyDown = false;
    }
}
