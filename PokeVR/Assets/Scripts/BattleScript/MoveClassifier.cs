using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MoveClassifier : MonoBehaviour {
    public TextAsset textAsset;
    private string[] medium;
    public Dictionary<string, string> moveDescription = new Dictionary<string, string>();
    public GameObject Target;
    public GameObject manager;

	// Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController");
        textAsset = Resources.Load<TextAsset>("PokeMoveDescript");
        medium = textAsset.text.Split(new char[] { '\n' });
        for(int i = 1; i < medium.Length; i++)
        {
            string str = medium[i];int j = str.IndexOf(',');
            //Debug.Log(str.Substring(j + 1, str.Length - j - 1));
            moveDescription.Add(str.Substring(0, j), str.Substring(j + 1, str.Length - j - 1));
        }

        if (gameObject.tag == "Mine")
            Target = GameObject.FindGameObjectWithTag("Wild");
        else
            Target = GameObject.FindGameObjectWithTag("Mine");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        string moveName = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        DataExtracter(moveName);
    }

    public void DataExtracter(string moveName)
    {
        manager.GetComponent<GameManager>().rightController.transform.parent.gameObject.SetActive(false);

        bool present = false;
        foreach (string item in GetComponent<Information>().moves)
        {
            if (item.Equals(moveName))
                present = true;
        }
        if (!present)
        {
            Debug.Log("Not Present");
            return;
        }

        Store store = manager.GetComponent<GameManager>().ReturnStore(moveName);
        Debug.Log("Moved " + store.moveType);
        
        Classifier(store.moveType, store.elementType, (int)store.power, store.particlePosition);
    }

    void Classifier(MoveType moveType, ElementType elementType, int power, ParticlePosition particlePosition)
    {
        if(moveType == MoveType.Special)
        {
            gameObject.AddComponent<SpecialMove>();
            gameObject.GetComponent<SpecialMove>().elementType = elementType;
            gameObject.GetComponent<SpecialMove>().power = power;
            gameObject.GetComponent<SpecialMove>().particlePosition = particlePosition;
            gameObject.GetComponent<SpecialMove>().Target = Target;
            gameObject.GetComponent<SpecialMove>().NextStep();
        }

        if (moveType == MoveType.Physical)
        {
            gameObject.AddComponent<PhysicalMove>();
            gameObject.GetComponent<PhysicalMove>().elementType = elementType;
            gameObject.GetComponent<PhysicalMove>().power = power;
            gameObject.GetComponent<PhysicalMove>().particlePosition = particlePosition;
            gameObject.GetComponent<PhysicalMove>().target = Target;
            gameObject.GetComponent<PhysicalMove>().NextStep();
        }
        
    }

    public void DestroyMethods()
    {
        manager.GetComponent<GameManager>().switchTurn();
        if (GetComponent<PhysicalMove>() != null)
            Destroy(GetComponent<PhysicalMove>());
        if (GetComponent<SpecialMove>() != null)
            Destroy(GetComponent<SpecialMove>());
    }
}
