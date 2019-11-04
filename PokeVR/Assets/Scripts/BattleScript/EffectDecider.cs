using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDecider{
    private Dictionary<ElementType, Dictionary<ElementType, float>> dictionary = new Dictionary<ElementType, Dictionary<ElementType, float>>();
    public TextAsset textAssetElement;

    public EffectDecider()
    {
        textAssetElement = Resources.Load<TextAsset>("TypeEffectiveness");
        string[] rows = textAssetElement.text.Split(new char[] { '\n' });
        for (int i = 1; i < rows.Length; i++)
        {
            //Debug.Log(rows[i]);
            string firstDictKeyStr = rows[i].Substring(0, rows[i].IndexOf(','));
            ElementType firstDictKey = (ElementType)System.Enum.Parse(typeof(ElementType), firstDictKeyStr);
            string[] columns = rows[i].Split(new char[] { ',' });
            Dictionary<ElementType, float> mapping = new Dictionary<ElementType, float>();
            for (int j = 1; j < columns.Length; j++)
            {
                string[] types = rows[0].Split(new char[] { ',' });
                mapping.Add((ElementType)System.Enum.Parse(typeof(ElementType), types[j]), float.Parse(columns[j]));
            }
            dictionary.Add(firstDictKey, mapping);
        }
    }

    public float Effectiveness(ElementType moveType, ElementType[] pokeType)
    {
        float effect = 1;
        for (int i = 0; i < pokeType.Length; i++)
        {
            float currentVal;
            Dictionary<ElementType, float> dict;
            dictionary.TryGetValue(moveType, out dict);
            dict.TryGetValue(pokeType[i], out currentVal);
            effect *= currentVal;
        }
        return effect;
    }
}
