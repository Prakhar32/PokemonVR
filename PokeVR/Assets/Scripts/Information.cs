using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRTK;

public class Information : MonoBehaviour {

    public int evolutionStage = 1;
    public int level = 2;
    public int HealthPoints;
    public ElementType[] typeing;
    private TextAsset textAsset;
    public int expNeeded;
    public List<string> moves = new List<string>();
    public GameObject manager;
    public Image image;
    public int maxHealth;
    public string pokeName;
    public TextMeshProUGUI textMeshPro;
    public TextMeshPro damageText;
    public GameObject Player;
    public Direction dir;

    //First key indicates MoveType hit second type denotes type of Pokemon
    private Dictionary<ElementType, Dictionary<ElementType, float>> dictionary = new Dictionary<ElementType, Dictionary<ElementType, float>>();
    private Color superEffective;
    private Color normallyEffective;
    private Color notVeryEffective;

	// Use this for initialization
	void Start () {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        expNeeded = (int)Mathf.Pow(level, 3); 
        HealthPoints = (int)Mathf.Exp((float)level / 12) * 6 + 20;
        maxHealth = HealthPoints;
        manager = GameObject.FindGameObjectWithTag("GameController");

        textAsset = Resources.Load<TextAsset>("TypeEffectiveness");
        textMeshPro.text = "Level " + level;
        textMeshPro.color = Color.yellow;
        image = transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        //Debug.Log(transform.GetChild(1).GetChild(0).GetChild(0).name);

        superEffective = Color.red;
        normallyEffective = Color.white;
        notVeryEffective = Color.green;

        //GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += Information_InteractableObjectUsed;
	}

    public void Information_InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Executed");
        if (!transform.GetChild(1).GetChild(2).gameObject.activeSelf)
            transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        else
            transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
    }

    public void CalcMoves()
    {
       moves = manager.GetComponent<GameManager>().Retrieve(pokeName, level);
    }

	// Update is called once per frame
	void Update () {
        //CalcMoves();
        Player = GameObject.FindGameObjectWithTag("Player");
        
	}

    public void Hit(int power, ElementType elementType, int opponentLevel)
    {
        float effect = 1;
        effect = manager.GetComponent<GameManager>().Effectiveness(elementType, typeing);

        if (effect >= 2)
            damageText.color = superEffective;
        else if (effect == 1)
            damageText.color = normallyEffective;
        else
            damageText.color = notVeryEffective;

        float damage = (((2 * opponentLevel / 5 + 2) * power) / 50) * effect;
        TakeDamage(damage);
        StartCoroutine("ReduceHealth");
    }
    
    void TakeDamage(float damage)
    {
        HealthPoints -= (int)damage;
        image.fillAmount = (float)HealthPoints / maxHealth;
        damageText.text = damage + "";
        //StartCoroutine("ReduceHealth");
    }

    IEnumerator ReduceHealth()
    {
        PositionText();
        //image.transform.parent.parent.gameObject.SetActive(true);
        damageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        damageText.gameObject.SetActive(false);
        //image.transform.parent.parent.gameObject.SetActive(false);
    }

    void PositionText()
    {
        Vector3 frontPosition = transform.position;
        Vector3 rightPosition = transform.position + transform.right;
        Vector3 leftPosition = transform.position - transform.right;

        float winnerDistance = Vector3.Distance(Player.transform.position, frontPosition);
        Direction dir = Direction.Front;
        
       if(Vector3.Distance(Player.transform.position , rightPosition) < winnerDistance)
        {
            winnerDistance = Vector3.Distance(Player.transform.position, rightPosition);
            dir = Direction.Right;
        }

        if (Vector3.Distance(Player.transform.position, leftPosition) < winnerDistance)
        {
            winnerDistance = Vector3.Distance(Player.transform.position, leftPosition);
            dir = Direction.Left;
        }

        if(dir == Direction.Left)
            damageText.transform.Translate(-gameObject.transform.right);
        if (dir == Direction.Right)
            damageText.transform.Translate(gameObject.transform.right);
    }
}
