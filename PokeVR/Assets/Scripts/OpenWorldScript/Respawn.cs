using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Respawn : MonoBehaviour {
    public GameObject objectToRespawn;
    public float RespawnTime;
    public float movementRadius;

    private SphereCollider sphereCollider;
    private float localTimer = 0;

	// Use this for initialization
	void Awake () {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = movementRadius;
        SetObject();
	}
	
	// Update is called once per frame
	void Update () {

        if (!CheckIfInBounds())
            localTimer += Time.deltaTime;
        
        if(localTimer >= RespawnTime )
        {
            localTimer = 0;
            SetObject();
        }
	}

    bool CheckIfInBounds()
    {
        Vector3 center = sphereCollider.gameObject.transform.position + sphereCollider.center;
        float radius = sphereCollider.radius;

        Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
        foreach (var item in allOverlappingColliders)
        {
            if (item.gameObject.tag == "Pokeball" && objectToRespawn.tag == "Pokeball")
                return true;
            if (item.gameObject.tag != "Wild")
                continue;
            if (item.gameObject.GetComponent<Information>().pokeName.Equals(objectToRespawn.GetComponent<Information>().pokeName))
                return true;
        }
        return false;
    }

    void SetObject()
    {
        GameObject go = Instantiate(objectToRespawn, sphereCollider.gameObject.transform.position + sphereCollider.center, Quaternion.identity);
        if(!go.tag.Equals("Pokeball"))
            go.transform.localScale = Vector3.one * 0.5f;
        else
            go.transform.localScale = Vector3.one * 0.25f;
        if (!go.tag.Equals("Pokeball"))
        {
            go.AddComponent<NavMeshAgent>();
            go.AddComponent<WildPokemonMovement>();
            go.GetComponent<WildPokemonMovement>().roamRadius = sphereCollider.radius;
        }
    }
}
