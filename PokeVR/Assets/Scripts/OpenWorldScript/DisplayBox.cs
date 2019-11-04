using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBox : MonoBehaviour {

    public GameObject Box;
    public float timer;
    public float delay = 2;
    public bool isPressed;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(isPressed)
            timer += Time.deltaTime;

        if (timer > delay)
        {
            if (!Box.activeSelf)
                Box.SetActive(true);
            else
                Box.SetActive(false);

            isPressed = false;
            timer = 0;
        }
    }

    public void CheckDisplayDisplay()
    {
        Debug.Log("asdasvvccvccvcvcv");
        /* if (!Box.activeSelf)
             isPressed = true;

         else
             Box.SetActive(false);*/
        isPressed = true;
    }

    public void Released()
    {
        Debug.Log("asd");
        timer = 0;
        isPressed = false;
    }
}
