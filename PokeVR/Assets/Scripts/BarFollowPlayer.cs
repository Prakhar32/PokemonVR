using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarFollowPlayer : MonoBehaviour {
    public GameObject Player;

	// Use this for initialization
	void Start () {
        if(SceneManager.GetActiveScene().name.Equals("OpenWorldScene"))
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        Player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.rotation = Quaternion.LookRotation(transform.position - Player.transform.position);
    }
}
