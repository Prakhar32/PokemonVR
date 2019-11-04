using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokeBall_Display : MonoBehaviour {

    public GameObject manager;
	// Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<TextMeshPro>().text = manager.GetComponent<GameManager>().pokeBallCount + "";

        if (manager.GetComponent<GameManager>().pokeBallCount <= 0)
            gameObject.SetActive(false);
	}
}
