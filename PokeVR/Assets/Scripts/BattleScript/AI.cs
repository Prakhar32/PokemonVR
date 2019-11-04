using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    public List<string> moves;
    public GameObject Target;
    public GameObject manager;

	// Use this for initialization
	void Start () {
        Target = GameObject.FindGameObjectWithTag("Mine");
        manager = GameObject.FindGameObjectWithTag("GameController");
        moves = manager.GetComponent<GameManager>().Retrieve(gameObject.GetComponent<Information>().pokeName, gameObject.GetComponent<Information>().level);
    }
	
    public void LoadMoves()
    {
        moves = manager.GetComponent<GameManager>().Retrieve(gameObject.GetComponent<Information>().pokeName, gameObject.GetComponent<Information>().level);
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void MoveSelection()
    {
        //LoadMoves();
        string[] m = moves.ToArray(); Debug.Log(moves.ToString());
        float[] moveDamage = new float[m.Length];float sum = 0; 
        for(int i = 0; i < m.Length; i++)
        {
            Store store = manager.GetComponent<GameManager>().ReturnStore(m[i]);
            float effect = manager.GetComponent<GameManager>().Effectiveness(store.elementType, Target.GetComponent<Information>().typeing);
            moveDamage[i] = store.power * effect;sum += store.power * effect;
            //dict.Add(move, prop.power * effect);
            Debug.Log(m[i]);
        }

        float[] probExecute = new float[moveDamage.Length];
        for(int i = 0; i < moveDamage.Length; i++)
        {
            probExecute[i] = moveDamage[i] / sum + 0.2f;
        }

        string selected = m[Prob(probExecute)];
        Debug.Log(selected);
        gameObject.GetComponent<MoveClassifier>().DataExtracter(selected);
    }

    int Prob(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
