using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AI;

public class OpenWorldManager : MonoBehaviour {
    public TextAsset textAsset;
    public List<GameObject> capturedPoke = new List<GameObject>();
    public GameObject selectedPoke;
    PokemonStorage pokemonStorage;
    public GameObject player;
    public static int counter = 0;
    string path;

    private void Awake()
    {
        ++counter;
        Debug.Log("Executes");
        if (PlayerPrefs.GetInt("StarterSet") == 0)
        {
            PlayerPrefs.SetInt("StarterSet", 1);

            PokemonBox.AddItem(Resources.Load<GameObject>("PokemonPrefabs/Charmander").GetComponent<Information>().pokeName, 5, 100);
            PokemonBox.AddItem(Resources.Load<GameObject>("PokemonPrefabs/Bulbasaur").GetComponent<Information>().pokeName, 5, 100);
            PokemonBox.AddItem(Resources.Load<GameObject>("PokemonPrefabs/Squirtle").GetComponent<Information>().pokeName, 5, 100);
        }

        selectedPoke = Resources.Load<GameObject>("PokemonPrefabs/Charmander");
        
        path = Application.dataPath + "/Resources/SceneSwitch.txt";

        if (File.Exists(path))
        {
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
                    selectedPoke = Instantiate(Resources.Load<GameObject>("PokemonPrefabs/" + arr[0]));
                    selectedPoke.GetComponent<Information>().pokeName = arr[0];
                    selectedPoke.GetComponent<Information>().expNeeded = int.Parse(arr[2]);
                    Debug.Log(selectedPoke.GetComponent<Information>().level);
                    selectedPoke.GetComponent<Information>().level = int.Parse(arr[1]);
                }
            }
            Debug.Log("Mave");
            streamReader.Close();
        }
        else if (!selectedPoke.scene.IsValid())
        {
            Debug.Log(selectedPoke.GetComponent<Information>().pokeName);
            selectedPoke = Instantiate(selectedPoke);
            selectedPoke.GetComponent<Information>().level = 5;
            selectedPoke.GetComponent<Information>().expNeeded = 100;
        }
        Debug.Log(selectedPoke.GetComponent<Information>().pokeName);
        Debug.Log(gameObject.name + "   " + counter + " " + FindObjectsOfType<OpenWorldManager>().Length);
        selectedPoke.AddComponent<PokemonFollow>();
        selectedPoke.AddComponent<NavMeshAgent>();
        selectedPoke.transform.localScale = Vector3.one * 0.5f;
        selectedPoke.tag = "Mine";
        
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Position")))
        {
            string[] arr = PlayerPrefs.GetString("Position").Split(new char[] { ',' });
            Vector3 pos = new Vector3();
            Debug.Log(arr[0]);
            pos.x = float.Parse(arr[0]);
            pos.y = float.Parse(arr[1]);
            pos.z = float.Parse(arr[2]);
            player.transform.position = pos;
        }
    }

    // Use this for initialization
    void Start () {
        SphereCollider sphereCollider = player.GetComponent<SphereCollider>();
        Vector3 centre = player.transform.position + sphereCollider.center;
        float radius = sphereCollider.radius;
        Collider[] allColliders = Physics.OverlapSphere(centre, radius);

        foreach (var item in allColliders)
        {
            if(item.gameObject.tag.Equals("Wild"))
                DestroyImmediate(item);
        }
        File.Delete(path);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Encounter(GameObject enemy)
    {
        PlayerPrefs.SetString("Position", player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z);
        
        string path = Application.dataPath + "/Resources/SceneSwitch.txt";
        StreamWriter wf;
        if (!File.Exists(path))
            wf = File.CreateText(path);
        else
            wf = File.AppendText(path);
        wf.WriteLine("Player");
        wf.WriteLine(selectedPoke.GetComponent<Information>().pokeName + "," + selectedPoke.GetComponent<Information>().level + "," + selectedPoke.GetComponent<Information>().expNeeded);
        wf.WriteLine("Enemy");
        wf.WriteLine(enemy.GetComponent<Information>().pokeName + "," + enemy.GetComponent<Information>().level + "," + enemy.GetComponent<Information>().expNeeded);
        wf.Close();
        SceneManager.LoadScene("BattleScene");
    }

    public void Click(GameObject pokemon)
    {
        DestroyImmediate(selectedPoke);
        selectedPoke = Resources.Load<GameObject>("PokemonPrefabs/" + pokemon.GetComponent<Information>().pokeName);
        selectedPoke = Instantiate(selectedPoke, player.transform.position + player.transform.right + player.transform.up.normalized, Quaternion.identity);
        selectedPoke.GetComponent<Information>().expNeeded = pokemon.GetComponent<Information>().expNeeded;
        selectedPoke.GetComponent<Information>().level = pokemon.GetComponent<Information>().level;
        selectedPoke.AddComponent<PokemonFollow>();
        selectedPoke.transform.localScale = Vector3.one * 0.5f;
        selectedPoke.AddComponent<NavMeshAgent>();
        selectedPoke.tag = "Mine";
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("StarterSet");
    }

    void DestroyNearby()
    {

    }
}
