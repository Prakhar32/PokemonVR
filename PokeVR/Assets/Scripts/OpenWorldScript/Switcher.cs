using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Switcher : MonoBehaviour {

    public GameObject manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Switch(object sender, InteractableObjectEventArgs e)
    {
        manager.GetComponent<OpenWorldManager>().Click(gameObject);
    }
}
