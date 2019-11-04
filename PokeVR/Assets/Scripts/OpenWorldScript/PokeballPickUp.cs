using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PokeballPickUp : MonoBehaviour {

    
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += gameObject.GetComponent<PokeballPickUp>().IncreasePokeballs;
        //Destroy(gameObject.GetComponent<WildPokemonMovement>());
    }
	
    void IncreasePokeballs(object sender, InteractableObjectEventArgs e)
    {
        PokemonBox.pokeBallCount++;
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
