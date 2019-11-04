using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRTK;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Specialized;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public TextAsset textAsset;
    public GameObject Player;
    public GameObject CPU;
    public Dictionary<string, int> lookUpTable = new Dictionary<string, int>();
    public GameObject panel1;
    public GameObject panel2;
    public GameObject secondButtonPrefab;
    public List<GameObject> pokeSent = new List<GameObject>();
    public SizeChange sizeChange;
    public Turn turn;
    public TextAsset textAssetElement;
    public Decider decider;
    public EffectDecider effectDecider;
    public AccessibleMoves accessibleMoves;
    public Dictionary<ElementType, List<string>> dict = new Dictionary<ElementType, List<string>>();
    public Vector3 initScale;
    public GameObject dropZone;
    public GameObject radialButtonPrefab;
    public GameObject rightController;
    public Material mat;
    public int pokeBallCount;

    private Material[] materials;
    private PokemonStorage pokemonStorage;

    // Use this for initialization
    void Start () {
        textAsset = Resources.Load<TextAsset>("Learnable Moves");
        textAssetElement = Resources.Load<TextAsset>("TypeEffectiveness");
        Player = GameObject.FindGameObjectWithTag("Mine");
        CPU = GameObject.FindGameObjectWithTag("Wild");
        CPU.AddComponent<AI>();
        decider = new Decider();
        effectDecider = new EffectDecider();
        pokeSent.Add(Player);
        turn = Turn.PlayerTurn;
        sizeChange = SizeChange.None;
        accessibleMoves = new AccessibleMoves();
        pokeBallCount = PokemonBox.pokeBallCount;
        
        string[] rows = textAsset.text.Split('\n');
        for(int i = 1; i < rows.Length; i++)
        {
            lookUpTable.Add(rows[i].Substring(0, rows[i].IndexOf(',')), i);
        }
        if (Player.GetComponent<Information>().moves.Count == 0)
            Player.GetComponent<Information>().moves = Retrieve(Player.GetComponent<Information>().pokeName, Player.GetComponent<Information>().level);
        CPU.GetComponent<Information>().moves = Retrieve(CPU.GetComponent<Information>().pokeName, CPU.GetComponent<Information>().level);
        ManageDisplay();
	}
    
    void ManageDisplay()
    {
        for (int i = 0; i < Player.GetComponent<Information>().moves.Count; i++)
        {
            Store store = ReturnStore(Player.GetComponent<Information>().moves[i]);
            List<string> list = new List<string>();
            if (!dict.ContainsKey(store.elementType))
            {
                List<string> str = new List<string>();
                str.Add(Player.GetComponent<Information>().moves[i]);
                dict.Add(store.elementType, str);
            }

            else
            {
                List<string> str;
                dict.TryGetValue(store.elementType, out str);
                str.Add(Player.GetComponent<Information>().moves[i]);
                dict[store.elementType] = str;
            }
        }
        DisplayFirstMenu();
    }
	
    public Store ReturnStore(string move)
    {
        return decider.returDesc(move);
    }

    public float Effectiveness(ElementType moveType, ElementType[] pokeType)
    {
        return effectDecider.Effectiveness(moveType, pokeType);
    }

    public List<string> Retrieve(string name, int level)
    {

        Debug.Log(name);
        return accessibleMoves.Retrieve(name, level);
    }

    public void switchTurn()
    {
        if (turn == Turn.PlayerTurn)
        {
            turn = Turn.CpuTurn; CPU.GetComponent<AI>().MoveSelection(); rightController.transform.parent.gameObject.SetActive(false);
            SphereCollider Item = dropZone.GetComponent<SphereCollider>();
            Vector3 center = Item.gameObject.transform.position + Item.center;
            float radius = Item.radius;

            Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
            foreach (var item in allOverlappingColliders)
            {
                item.gameObject.SetActive(false);
            }

            dropZone.SetActive(false);
        }
        else
        {
            turn = Turn.PlayerTurn;
            if(pokeBallCount > 0)
                dropZone.SetActive(true);
            rightController.transform.parent.gameObject.SetActive(true);
            DisplayFirstMenu();
        }
    }

    
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
          //  Victory();

        if(pokeBallCount <= 0)
            dropZone.SetActive(false);

        if (sizeChange == SizeChange.Shrink)
        {
            for (int i = 0; i < CPU.GetComponentInChildren<SkinnedMeshRenderer>().materials.Length; i++)
                CPU.GetComponentInChildren<SkinnedMeshRenderer>().materials[i] = mat;

            if (CPU.transform.lossyScale.x <= 0.0001f)
            {
                sizeChange = SizeChange.None;    
            }

            CPU.transform.localScale = CPU.transform.localScale - new Vector3(0.005f, 0.005f, 0.005f);
        }

        if (sizeChange == SizeChange.Expand)
        {
            CPU.GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

            if (CPU.transform.lossyScale.x >= initScale.x)
            {
                sizeChange = SizeChange.None;
            }

            CPU.transform.localScale = CPU.transform.localScale + new Vector3(0.005f, 0.005f, 0.005f);
        }

        if (CPU.GetComponent<Information>().HealthPoints <= 0)
            Victory();

        if (Player.GetComponent<Information>().HealthPoints <= 0)
            Defeat();
    }

    public void DisplayFirstMenu()
    {
        Debug.Log("menu");
        rightController.GetComponent<VRTK_RadialMenu>().buttons.Clear();
        int index = 0; Debug.Log(dict.Keys.Count);
        
        ElementType[] keys = new ElementType[dict.Keys.Count];
        dict.Keys.CopyTo(keys, 0);
        FillButtons(keys.Select(a => a.ToString()).ToArray());
        //Debug.Log(keys.Length);
        for (index = 0; index < keys.Length; index++)
        {
            ElementType el = keys[index]; Debug.Log(el.ToString() + "  " + index + " val " + dict[el][0]);
            rightController.GetComponent<VRTK_RadialMenu>().buttons[index].OnClick.AddListener(()=>Listen(el.ToString()));
           // Debug.Log(Resources.Load<Material>("MoveMaterial/" + el.ToString()) == null);
           // rightController.GetComponent<VRTK_RadialMenu>().menuButtons[index].GetComponent<UICircle>().material = Resources.Load<Material>("MoveMaterial/" + el.ToString());
        }
    }

    public void Listen(string s)
    {
        Debug.LogError("asdsa");
        ClearMenu();
        IEnumerator coroutine = SecondMenu(s);
        StartCoroutine(coroutine);
    }

    public void ClearMenu()
    {
        rightController.GetComponent<VRTK_RadialMenu>().currentPress = -1;
      
        foreach (GameObject go in rightController.GetComponent<VRTK_RadialMenu>().menuButtons)
        {
            Destroy(go);
        }
        rightController.GetComponent<VRTK_RadialMenu>().buttons.Clear();
        rightController.GetComponent<VRTK_RadialMenu>().menuButtons.Clear();
        Debug.Log("asdchjog");
        rightController.GetComponent<VRTK_RadialMenu>().currentHover = -1;
    }

    public void FillButtons(string[] val)
    {
        for (int i = 0; i < val.Length; i++)
        {
            rightController.GetComponent<VRTK_RadialMenu>().buttons.Add(new VRTK_RadialMenu.RadialMenuButton());
            //Debug.Log(val[i].ToString());
        }
        rightController.GetComponent<VRTK_RadialMenu>().RegenerateButtons();

        for (int i = 0; i < val.Length; i++)
        {
            rightController.GetComponent<VRTK_RadialMenu>().menuButtons[i].GetComponentInChildren<TextMeshPro>().text = val[i];
            rightController.GetComponent<VRTK_RadialMenu>().menuButtons[i].GetComponentInChildren<TextMeshPro>().gameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 90));
            rightController.GetComponent<VRTK_RadialMenu>().menuButtons[i].GetComponentInChildren<TextMeshPro>().gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-230, 1, 0);
        }
        rightController.GetComponent<VRTK_RadialMenu>().ToggleMenu();
        rightController.GetComponent<VRTK_RadialMenu>().enabled = true;
        
    }

    public IEnumerator Delay(string[] s)
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator SecondMenu(string s)
    {
        Debug.Log(s);
        ElementType elem = (ElementType)System.Enum.Parse(typeof(ElementType), s);
        List<string> moves = dict[elem];
        
        FillButtons(moves.ToArray());
        for (int i = 0; i < moves.Count; i++)
        {
            string move = moves[i];
            Debug.Log(move);
            rightController.GetComponent<VRTK_RadialMenu>().buttons[i].OnClick.AddListener(() => Player.GetComponent<MoveClassifier>().DataExtracter(move));
            Debug.Log("condn");
        }
        yield return null;
    }

    public void Capture(bool decision)
    {
        Debug.Log("asfczx");
        sizeChange = SizeChange.Shrink;
        materials = CPU.GetComponentInChildren<SkinnedMeshRenderer>().materials;
        initScale = CPU.transform.localScale;
        IEnumerator capture = captureDecider(decision);
        StartCoroutine(capture);
    }

    IEnumerator captureDecider(bool decision)
    {
        yield return new WaitForSeconds(4);
        if (!decision)
            sizeChange = SizeChange.Expand;
        else
            Captured();
    }

    public void Captured()
    {
        string path = Application.dataPath + "/Resources/Captured.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(CPU.name + "," + CPU.GetComponent<Information>().level);
        wf.Close();
        PokemonBox.AddItem(CPU.GetComponent<Information>().pokeName, CPU.GetComponent<Information>().level, CPU.GetComponent<Information>().expNeeded);
        Debug.Log("Captured");
        SceneManager.LoadScene("OpenWorldScene");
    }

    void Victory()
    {
        ExperienceGained();
        Information info = Player.GetComponent<Information>();
        string newName =  PokemonEvolution.CheckEvolution(info.pokeName, info.level);
        int level = info.level;int expNeeded = info.expNeeded;
        if(!newName.Equals(info.pokeName))
        {
            PokemonBox.RemoveMember(info.pokeName, info.level, info.expNeeded);
            PokemonBox.AddItem(newName, info.level, info.expNeeded);
            Debug.Log(newName);
            pokeSent.Remove(Player);
            Player = Instantiate(Resources.Load<GameObject>("PokemonPrefabs/" + newName));
            Player.GetComponent<Information>().level = level;
            Player.GetComponent<Information>().expNeeded = expNeeded;
           // Debug.Log(Player.GetComponent<Information>().pokeName);
            pokeSent.Add(Player);
        }
        string path = Application.dataPath + "/Resources/SceneSwitch.txt";
        StreamWriter wf;
        wf = File.CreateText(path);
        wf.WriteLine("Player");
        foreach (GameObject go in pokeSent)
        {
            //wf = File.AppendText(path);
            wf.WriteLine(go.GetComponent<Information>().pokeName + "," + go.GetComponent<Information>().level + "," + go.GetComponent<Information>().expNeeded);
            //wf.Close();
            Debug.Log(go.GetComponent<Information>().level);
            SceneChangeInfoStorafe.playerPoke = go.GetComponent<Information>().pokeName;
            SceneChangeInfoStorafe.level = go.GetComponent<Information>().level;
            SceneChangeInfoStorafe.expNeeded = go.GetComponent<Information>().expNeeded;
        }
        wf.Close();
        SceneManager.LoadScene("OpenWorldScene");
    }

    void ExperienceGained()
    {
        Information info;
        //   = Player.GetComponent<Information>();
        int le = CPU.GetComponent<Information>().level;
        //int lp = info.level;
        //int prevExp = info.expNeeded;
        int lp = 0;

        List<Stor> prevlist = new List<Stor>();
        for (int i = 0; i < pokeSent.Count; i++)
        {
            GameObject item = pokeSent[i];
            Information ifo = item.GetComponent<Information>();
            prevlist.Add(new Stor(ifo.pokeName, ifo.level, ifo.expNeeded));
            lp += ifo.level;
        }

        //lp denotes avg level of Player
        lp /= pokeSent.Count;
        //total exp gained
        int expGained = (int)((le / 5) * Mathf.Pow((2 * le + 10), 2.5f) / Mathf.Pow((le + lp + 10), 2.5f) + 1) * 2;

        for (int i = 0; i < pokeSent.Count; i++)
        {
            GameObject go = pokeSent[i];
            int expEach = expGained / pokeSent.Count;
            info = go.GetComponent<Information>();
            do
            {
                expEach -= info.expNeeded;
                info.level++;
                info.expNeeded = (int)Mathf.Pow(Player.GetComponent<Information>().level, 3);
            } while (expEach > info.expNeeded);
        }

        for (int i = 0; i < pokeSent.Count; i++)
        {
            GameObject go = pokeSent[i];
            info = go.GetComponent<Information>();
            PokemonBox.ModifyValue(info.pokeName, prevlist[i].level, prevlist[i].expRequired, info.level, info.expNeeded);
        }
        //PokemonBox.ModifyValue(info.pokeName, lp, prevExp, info.level, info.expNeeded);
    }

    void Defeat()
    {
        SceneManager.LoadScene("OpenWorldScene");
    }
}
