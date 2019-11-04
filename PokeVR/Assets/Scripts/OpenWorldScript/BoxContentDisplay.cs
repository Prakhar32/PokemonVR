using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRTK;

public class BoxContentDisplay : MonoBehaviour {

    public GameObject Box;
    public int rowSize;
    public int columnSize;
    public float rowSpacing;
    public float columnSpacing;
    public bool isVisible;
    public bool firstset;
    public GameObject manager;
    
    private int rowIndex;
    private int columnIndex;
    private GameObject[,] pokeInBox;
    private GameObject go;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        pokeInBox = new GameObject[rowSize, columnSize];
        Debug.Log(PokemonBox.queue.Count);rowIndex = 0;columnIndex = 0;
        Debug.Log(PokemonBox.queue[2].name);
        foreach (Stor item in PokemonBox.queue)
        {
             go = Instantiate<GameObject>(Resources.Load<GameObject>("PokemonPrefabs/" + item.name), Box.transform);
            go.GetComponent<Information>().level = item.level;
            go.GetComponent<Information>().expNeeded = item.expRequired;
            //TextMeshPro textMeshPro = Instantiate()
            Debug.Log(item.name);
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Destroy(go.GetComponent<Rigidbody>());
            go.AddComponent<VRTK_InteractableObject>();
            go.GetComponent<VRTK_InteractableObject>().isUsable = true;
            go.AddComponent<Switcher>();
            go.GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += go.GetComponent<Switcher>().Switch;        
            AddItems(go);
        }
    }
    
    void AddItems(GameObject go)
    {
        if (rowIndex == 0 && columnIndex == 0 && !firstset)
        {
            go.transform.localPosition = Vector3.zero + Box.transform.right.normalized * (columnSize / 2) * columnSpacing * 0.08f;
            pokeInBox[rowIndex, columnIndex] = go;
            firstset = true;
        }

        else if (columnIndex == columnSize)
        {
            columnIndex = 0;
            //go.transform.localPosition = pokeInBox[rowIndex, columnIndex].transform.localPosition + Vector3.forward.normalized * rowSpacing;
            rowIndex++;
            pokeInBox[rowIndex, columnIndex] = go;
        }
        else
        {
            columnIndex++;
            pokeInBox[rowIndex, columnIndex] = go;
            Debug.Log("mmmm");
        }

        go.transform.position = pokeInBox[0, 0].transform.position + rowIndex * pokeInBox[0, 0].transform.forward.normalized * rowSpacing + columnIndex * columnSpacing * pokeInBox[0, 0].transform.right;
        Debug.Log(rowIndex + "   " + columnIndex);
        
    }
    
    private void BoxContentDisplay_InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log(sender.ToString());
        manager.GetComponent<OpenWorldManager>().Click(((GameObject)sender));
    }

    public void ToggleDisplay()
    {
        if (!isVisible)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        Box.SetActive(true);
    }

    public void Hide()
    {
        Box.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
