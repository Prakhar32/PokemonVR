using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonEvolution : MonoBehaviour {
    public static TextAsset textAsset = Resources.Load<TextAsset>("Evolution");
	
    public static string CheckEvolution(string name, int level)
    {
        string[] rows = textAsset.text.Split(new char[] { '\n' });
        for(int i = 1; i < rows.Length; i++)
        {
            string[] column = rows[i].Split(new char[] { ',' });
            if (!column[0].Equals(name))
                continue;
            Debug.Log(level + " Level");
            //Debug.Log(column[2]);
            if (level >= int.Parse(column[2]))
                return column[1];
            else
                return name;
            
        }
        return name;
    }
    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
