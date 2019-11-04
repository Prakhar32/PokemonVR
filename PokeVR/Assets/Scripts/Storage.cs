using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Physical, Special
}

public enum ElementType
{
    Fire, Water, Ice, Poison, Grass, Normal, Flying, Ground, Rock, Psychic, Dark, Bug, Electric
}

public enum LowestValPerElement
{
    Fire = 40, Water = 40, Ice = 90, Poison = 40, Grass = 55, Normal = 40, FLying = 35, Ground = 60, Rock = 60, Psychic = 50, Dark = 60, Bug = 60, Electric = 40
}

public enum ParticlePosition
{
    Above, Front, On, None
}

public enum Turn
{
    CpuTurn, PlayerTurn
}

public enum SizeChange
{
    Shrink, Expand, None
}

public class Property
{
    public ElementType elementType;
    public float power;

    public Property(ElementType element, float pow)
    {
        elementType = element;
        pow = power;
    }
}

public class Store
{
    public MoveType moveType;
    public ElementType elementType;
    public float power;
    public ParticlePosition particlePosition;

    public Store(MoveType m, ElementType e, float p, ParticlePosition par)
    {
        power = p; elementType = e; moveType = m; particlePosition = par;
    }
}

public class Decider
{
    private TextAsset textAsset;
    private string[] medium;
    public Dictionary<string, string> moveDescription = new Dictionary<string, string>();
    Store store;

    public Decider()
    {
        textAsset = Resources.Load<TextAsset>("PokeMoveDescript");
        medium = textAsset.text.Split(new char[] { '\n' });
        for (int i = 1; i < medium.Length; i++)
        {
            string str = medium[i]; int j = str.IndexOf(',');
            //Debug.Log(str.Substring(j + 1, str.Length - j - 1));
            moveDescription.Add(str.Substring(0, j), str.Substring(j + 1, str.Length - j - 1));
            
        }
    }

    public Store returDesc(string move)
    {
        string s = "a";Debug.Log(move);
        foreach(string val in moveDescription.Values)
        {
           // Debug.Log(val);
        }
        moveDescription.TryGetValue(move, out s);
        string[] arr = s.Split(new char[] { ',' });
        Debug.Log(arr[0]);
        MoveType moveType = (MoveType)System.Enum.Parse(typeof(MoveType), arr[0]);
        ElementType elementType = (ElementType)System.Enum.Parse(typeof(ElementType), arr[1]);
        float power = float.Parse(arr[2]);
        ParticlePosition particlePosition = (ParticlePosition)System.Enum.Parse(typeof(ParticlePosition), arr[3]);
        store = new Store(moveType, elementType, power, particlePosition);
        return store;
    }
}

public class Storage : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum Registration
{
    Registered, NotRegistered
}

/*public class DamageType
{
     
}*/

public enum Direction
{
    Front, Left, Right 
}
