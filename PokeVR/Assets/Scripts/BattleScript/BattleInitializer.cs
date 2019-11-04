using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using VRTK;

public class BattleInitializer : MonoBehaviour {
    public Transform playerPositon;
    public Transform CPUPosition;
    public Transform PlayerPokemonPosition;
    public GameObject CPU;
    public GameObject Pokemon;
    public GameObject Player;
    public GameObject HealthPrefab;
    public TextMeshPro damageTextPrefab;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        string path = Application.dataPath + "/Resources/SceneSwitch.txt";

        //Reading from file at start of new Scene
        if (File.Exists(path))
        {
            Debug.Log("Mave");
            StreamReader streamReader = File.OpenText(path);
            string s = ""; bool playerRead = true;
            while ((s = streamReader.ReadLine()) != null)
            {
                if (s.Equals("Player"))
                    continue;
                if (s.Equals("Enemy"))
                {
                    playerRead = false; continue;
                }

                if (playerRead)
                {
                    string[] arr = s.Split(new char[] { ',' });
                    initValues(Pokemon, arr[0], int.Parse(arr[2]), int.Parse(arr[1]), PlayerPokemonPosition, "Mine");
                    /*Pokemon = Instantiate(Resources.Load<GameObject>("PokemonPrefabs/" + arr[0]), PlayerPokemonPosition.position, PlayerPokemonPosition.rotation);
                    Pokemon.GetComponent<Information>().pokeName = arr[0];
                    Pokemon.GetComponent<Information>().expNeeded = int.Parse(arr[2]);
                    Pokemon.GetComponent<Information>().level = int.Parse(arr[1]);
                    Pokemon.tag = "Mine";
                    *///Pokemon.GetComponent<Information>().level = PlayerPrefs.GetInt("PlayerLevel");
                }
                else
                {
                    string[] arr = s.Split(new char[] { ',' });
                    initValues(CPU, arr[0], int.Parse(arr[2]), int.Parse(arr[1]), CPUPosition, "Wild");
                    /*CPU = Instantiate(Resources.Load<GameObject>("PokemonPrefabs/" + arr[0]), CPUPosition.position, CPUPosition.rotation);
                    CPU.GetComponent<Information>().pokeName = arr[0];
                    CPU.GetComponent<Information>().expNeeded = int.Parse(arr[2]);
                    CPU.GetComponent<Information>().level = int.Parse(arr[1]);
                    CPU.tag = "Wild";
                  */  //CPU.GetComponent<Information>().level = PlayerPrefs.GetInt("Enemylevel");
                }
            }
            streamReader.Close();
            File.Delete(path);
        }
        else
        {
            Pokemon = GameObject.FindGameObjectWithTag("Mine");
            CPU = GameObject.FindGameObjectWithTag("Wild");
        }
       
        Ininitalizer(Pokemon);
        Ininitalizer(CPU);
    }

    public void Ininitalizer(GameObject go)
    {
        TextMeshPro damage = Instantiate(damageTextPrefab, go.transform.forward + go.transform.up * 2.5f + go.transform.position, Quaternion.identity);
        go.GetComponent<Information>().damageText = damage;
        go.AddComponent<VRTK_InteractableObject>();
        go.GetComponent<VRTK_InteractableObject>().isUsable = true;
        go.GetComponent<VRTK_InteractableObject>().disableWhenIdle = false;
        //go.AddComponent<VRTK_InteractUse>();
        go.GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += go.GetComponent<Information>().Information_InteractableObjectUsed;
        go.GetComponent<VRTK_InteractableObject>().enabled = true;
        // go.GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += BattleInitializer_InteractableObjectUsed; 
        go.GetComponentInChildren<TextMeshProUGUI>().text = go.GetComponent<Information>().level + "";
    }

    public void initValues(GameObject go, string pokeName, int expNeeded, int level, Transform trans, string tag) 
    {
        go = Instantiate(Resources.Load<GameObject>("PokemonPrefabs/" + pokeName), trans.position, trans.rotation);
        go.GetComponent<Information>().pokeName = pokeName;
        go.GetComponent<Information>().level = level;
        go.GetComponent<Information>().expNeeded = expNeeded;
        go.tag = tag;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
