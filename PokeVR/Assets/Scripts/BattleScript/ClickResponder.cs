using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickResponder : MonoBehaviour {

    public GameObject Player;
    public GameObject CPU;
    public GameObject gameManager;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Mine");
        CPU = GameObject.FindGameObjectWithTag("Wild");
        gameManager = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Clicked()
    {
        //Player.GetComponent<MoveClassifier>().DataExtracter(transform.GetComponentInChildren<TextMeshPro>().text);
        gameManager.GetComponent<GameManager>().Listen(transform.GetComponentInChildren<TextMeshPro>().text);
        gameObject.transform.parent.gameObject.SetActive(false);
        Debug.LogError("Executed");
    }
}
