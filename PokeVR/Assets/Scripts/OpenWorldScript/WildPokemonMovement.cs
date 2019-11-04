using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WildPokemonMovement : MonoBehaviour {

    //public Vector3 targetPosition;
    public float roamRadius = 10;
    public Vector3 startPosition;
    NavMeshAgent _nav;
    public Vector3 finalPosition;
    public GameObject Player;
    public bool targetAcquired;

	// Use this for initialization
	void Start () {
        startPosition = gameObject.transform.position;
        _nav = GetComponent<NavMeshAgent>();
        transform.GetChild(0).GetComponent<Animator>().SetBool("Move", true);
        InvokeRepeating("randomMovement", 0, 3.5f);
	}
	
	// Update is called once per frame
	void Update () {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(Player.transform.position, startPosition) < roamRadius)
            targetAcquired = true;
    }

    public void randomMovement()
    {
        if (!targetAcquired)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += startPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(new Vector3(randomDirection.x, 0, randomDirection.z), out hit, roamRadius, 1);
            finalPosition = hit.position;
            _nav.destination = finalPosition;
        }
        else
        {
            _nav.destination = Player.transform.position;
        }
    }
}
