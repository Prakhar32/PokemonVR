using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : MonoBehaviour {
    public ElementType elementType;
    public int power;
    public ParticleSystem ps;
    public ParticlePosition particlePosition;
    private int lpow;
    public GameObject Target;

    private void Start()
    {
       // Target = GameObject.FindGameObjectWithTag("Wild");    
    }
    
    public void NextStep()
    {
        if(elementType == ElementType.Fire)
        {
            if(power == 40)
                ps = Resources.Load<ParticleSystem>("MovesEffect/Ember");
            else
                ps = Resources.Load<ParticleSystem>("MovesEffect/FlameThrower");
            lpow = (int)LowestValPerElement.Fire;
        }

        if (elementType == ElementType.Water)
        {
            if (power == 65)
                ps = Resources.Load<ParticleSystem>("MovesEffect/BubbleBeam");
            else
                ps = Resources.Load<ParticleSystem>("MovesEffect/WaterGun");
            lpow = (int)LowestValPerElement.Fire;
        }

        if(elementType == ElementType.Ice)
        {
            if (power == 90)
                ps = Resources.Load<ParticleSystem>("MovesEffect/IceLance");
            lpow = (int)LowestValPerElement.Water;
        }

        if(elementType == ElementType.Poison)
        {
            ps = Resources.Load<ParticleSystem>("MovesEffect/PoisonGas");
            lpow = (int)LowestValPerElement.Poison;
        }

        if(elementType == ElementType.Grass)
        {
            ps = Resources.Load<ParticleSystem>("MovesEffect/RazorLeaf");
            lpow = (int)LowestValPerElement.Poison;
        }
        if(elementType == ElementType.Flying)
        {
            if(power < 100)
                ps = Resources.Load<ParticleSystem>("MovesEffect/WeakWind");
            else
                ps = Resources.Load<ParticleSystem>("MovesEffect/StrongWind");
            lpow = (int)LowestValPerElement.FLying;
        }
        if(elementType == ElementType.Psychic)
        {
            ps = Resources.Load<ParticleSystem>("MovesEffect/Confusion");
            lpow = (int)LowestValPerElement.Psychic;
        }
        if(elementType == ElementType.Electric)
        {
            ps = Resources.Load<ParticleSystem>("MovesEffect/Lightning");
            lpow = (int)LowestValPerElement.Electric;
        }

        Helper(ps, power);
    }

    void Helper(ParticleSystem ps, int power)
    {
        gameObject.AddComponent<ParticleEffectAssignerScript>();
        gameObject.GetComponent<ParticleEffectAssignerScript>().ps = ps;
        gameObject.GetComponent<ParticleEffectAssignerScript>().power = power;
        gameObject.GetComponent<ParticleEffectAssignerScript>().particlePosition = particlePosition;
        gameObject.GetComponent<ParticleEffectAssignerScript>().lowestPower = lpow;
        gameObject.GetComponent<ParticleEffectAssignerScript>().Target = Target;
        StartCoroutine("Damage");
    }

    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(3);
        Target.GetComponent<Information>().Hit(power, elementType, gameObject.GetComponent<Information>().level);
    }
}
