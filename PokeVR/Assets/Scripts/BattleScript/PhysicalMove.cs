using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMove : MonoBehaviour {

    public ElementType elementType;
    public int power;
    public ParticleSystem ps;
    public GameObject target;
    private bool walkTowards;
    private bool walkAway;
    private bool moveBack;
    private Vector3 initPos;
    public ParticlePosition particlePosition;

	// Use this for initialization
	void Start () {
        initPos = gameObject.transform.position;
        //target = GameObject.FindGameObjectWithTag("Wild");
    }
	
	// Update is called once per frame
	void Update () {
        if (walkTowards)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.01f);
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Move", true);
            if(Vector3.Distance(transform.position, target.transform.position) < 2f)
            {
                walkTowards = false;
                gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Move", false);
                StartCoroutine("move");
            }
        }

        if (moveBack)
        { 
            transform.position = Vector3.Lerp(transform.position, initPos, 0.01f);
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Move", true);
            if (Vector3.Distance(transform.position, initPos) < 0.3f)
            {
                moveBack = false; transform.Rotate(new Vector3(0, 180, 0));
                gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Move", false);
                gameObject.GetComponent<MoveClassifier>().DestroyMethods();
            }
        }
	}

    public void NextStep()
    {
        if(elementType == ElementType.Normal || elementType == ElementType.Flying || elementType == ElementType.Dark)
        {
            walkTowards = true;
        }

        if(elementType == ElementType.Rock)
        {
            Debug.Log("asdas");
            if (power == 100)
            {
                ps = Resources.Load<ParticleSystem>("MovesEffect/Stone Edge");
                Debug.Log(power);
            }
            else
                ps = Resources.Load<ParticleSystem>("MovesEffect/RockSlide");
        }

        if (elementType == ElementType.Ground)
        {
            if (power == 100)
                ps = (ParticleSystem)Resources.Load("MovesEffect/EarthQuake");
        }

        if (elementType == ElementType.Grass)
        {
            if (power == 55)
                ps = (ParticleSystem)Resources.Load("MovesEffect/RazorLeaf");
        }
        else
        {
            walkTowards = true;
        }
        if (ps != null)
        {
            //Debug.Log("asdas");
            gameObject.AddComponent<ParticleEffectAssignerScript>();
            gameObject.GetComponent<ParticleEffectAssignerScript>().ps = ps;
            gameObject.GetComponent<ParticleEffectAssignerScript>().power = power;
            gameObject.GetComponent<ParticleEffectAssignerScript>().particlePosition = particlePosition;
            Debug.Log(target == null);
            target.GetComponent<Information>().Hit(power, elementType, gameObject.GetComponent<Information>().level);
        }
    }

    public IEnumerator move()
    {
        target.transform.GetChild(0).GetComponent<Animator>().SetBool("Damage", true);
        yield return new WaitForSeconds(1);
        moveBack = true;
        transform.Rotate(new Vector3(0, 180, 0));
        target.transform.GetChild(0).GetComponent<Animator>().SetBool("Damage", false);
        target.gameObject.GetComponent<Information>().Hit(power, elementType, gameObject.GetComponent<Information>().level);
    }
}
