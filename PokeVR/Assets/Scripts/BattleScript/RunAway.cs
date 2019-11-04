using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunAway : MonoBehaviour {
    public float timerStart;
    private bool isPressed = false;
    
	// Update is called once per frame
	void Update () {
        if (isPressed)
        {
            timerStart += Time.deltaTime;
        }
        if (timerStart > 2f)
            SceneManager.LoadScene("OpenWorldScene");
	}

    public void Press()
    {
        isPressed = true;
    }

    public void UnClick()
    {
        isPressed = false;
        timerStart = 0;
    }
}
