using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessibleMoves {
    public Dictionary<string, int> lookUpTable = new Dictionary<string, int>();
    public TextAsset textAsset;

    public AccessibleMoves()
    {
        textAsset = Resources.Load<TextAsset>("Learnable Moves");
        string[] rows = textAsset.text.Split('\n');
        for (int i = 1; i < rows.Length; i++)
        {
            lookUpTable.Add(rows[i].Substring(0, rows[i].IndexOf(',')), i);
        }
    }

    public List<string> Retrieve(string name, int level)
    {
        List<string> list = new List<string>();
        string[] rows = textAsset.text.Split('\n');
        int index = 10; lookUpTable.TryGetValue(name, out index);

        //Debug.Log(lookUpTable.Count);
        foreach (string str in lookUpTable.Keys)
        {
           // Debug.Log(str);
        }
        Debug.Log(index + "" + name);
        string[] columnI = rows[index].Split(',');
        string[] column0 = rows[0].Split(',');
       // Debug.Log(columnI.ToString());

        for (int i = 1; i < column0.Length; i++)
        {
            // Debug.Log(column0[i]);
            if (int.Parse(column0[i]) <= level)
            {
                //Debug.Log(columnI[i]); columnI[i].Trim();
                if (!string.IsNullOrEmpty(columnI[i]))
                {
                    list.Add(columnI[i]);// Debug.Log(columnI[i] + "   " + column0[i]);
                }
            }
        }
        Debug.Log("Log");
        return list;
    }

}
