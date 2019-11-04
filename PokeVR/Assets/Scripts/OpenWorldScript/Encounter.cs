using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour {

    public GameObject OWgameManager;
    public bool hasCollidedOnce;
    public GameObject collidedObj;

	// Use this for initialization
	void Start () {
        OWgameManager = GameObject.FindGameObjectWithTag("GameController");
        hasCollidedOnce = false;
        Debug.Log("Encounter " + gameObject.name);
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("vcv");
        if (collision.gameObject.tag.Equals("Wild"))
        {
            if(hasCollidedOnce)
                OWgameManager.GetComponent<OpenWorldManager>().Encounter(collision.gameObject);
            else
            {
                collision.gameObject.transform.position -= collision.gameObject.transform.forward * 2;
                StartCoroutine("SlowDown");
                hasCollidedOnce = true;
                collidedObj = collision.gameObject;
            }
        }
    }

    private IEnumerator SlowDown()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(5);
        Time.timeScale = 1;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Wild"))
        {
            Debug.LogError("asas");
            OWgameManager.GetComponent<OpenWorldManager>().Encounter(collision.gameObject);
        }
    }
    
    private void Update()
    {
        /*SphereCollider Item = gameObject.GetComponent<SphereCollider>();
        Vector3 center = Item.gameObject.transform.position + Item.center;
        float radius = Item.radius;

        Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
        foreach (var item in allOverlappingColliders)
        {
            if (item.gameObject.tag.Equals("Wild"))
                OWgameManager.GetComponent<OpenWorldManager>().Encounter(item.gameObject);
        }*/
       
        if (hasCollidedOnce)
        {
            WildPokemonMovement wpm = collidedObj.GetComponent<WildPokemonMovement>();
            if (Vector3.Distance(wpm.startPosition, transform.position) > wpm.roamRadius)
            {
                Time.timeScale = 1;
                collidedObj = null;
                hasCollidedOnce = false;
            }
        }
    }
}
