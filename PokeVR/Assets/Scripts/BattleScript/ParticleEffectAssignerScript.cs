using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectAssignerScript : MonoBehaviour {
    public GameObject Target;
    public ParticleSystem ps;
    private float startTime;
    private ParticleSystem p;
    public int power;
    public int lowestPower;
    public ParticlePosition particlePosition;

    // Use this for initialization
    void Start () {
        //Target = GameObject.FindGameObjectWithTag("Wild");
        p = Instantiate(ps, transform.forward, Quaternion.identity);
        Positioning();
        p.gameObject.transform.LookAt(Target.transform);
        increaseIntensity();
	}

    void Positioning()
    {
        if (particlePosition == ParticlePosition.Above)
            p.gameObject.transform.position = Target.transform.up * 7 + Target.transform.position;
        if (particlePosition == ParticlePosition.On)
            p.gameObject.transform.position = Target.transform.position;
        if (particlePosition == ParticlePosition.Front)
            p.gameObject.transform.position = transform.position + transform.forward.normalized + transform.up.normalized;

        StartCoroutine("Damage");
    }

    void increaseIntensity()
    {
        
        //p.emissionRate = (float)power / (float)lowestPower;
    }
	
	// Update is called once per frame
	void Update () {
        startTime += Time.deltaTime;
        
        if(startTime > 5)
        {
            Debug.Log(p == null);
            Destroy(p.gameObject);
            gameObject.GetComponent<MoveClassifier>().DestroyMethods();
            Destroy(gameObject.GetComponent<ParticleEffectAssignerScript>());
        }
	}
    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(3);
        Target.transform.GetChild(0).GetComponent<Animator>().SetBool("Damage", true);
        yield return new WaitForSeconds(0.8f);
        Target.transform.GetChild(0).GetComponent<Animator>().SetBool("Damage", false);
    }

}
