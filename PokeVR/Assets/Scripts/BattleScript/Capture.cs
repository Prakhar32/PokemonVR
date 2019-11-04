using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : MonoBehaviour {
    public GameObject manager;
    public bool hasThrown = false;

	// Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasThrown)
        {
            manager.GetComponent<GameManager>().pokeBallCount--;
            hasThrown = true;
        }

        if (collision.gameObject.tag.Equals("Wild"))
        {
            Debug.Log("jnkjasfdsa");
            float enemyHealth = collision.gameObject.GetComponent<Information>().HealthPoints;
            float enemyMaxHealth = collision.gameObject.GetComponent<Information>().maxHealth;
            float prob = ((3 * enemyMaxHealth - 2 * enemyHealth) / (3 * enemyMaxHealth * 5)) * 2;
            if (Random.Range(0.0f, 1.0f) < prob)
                ifCaught(true);
            else
                ifCaught(false);

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void ifCaught(bool caught)
    {
        manager.GetComponent<GameManager>().Capture(caught);
    }
}
