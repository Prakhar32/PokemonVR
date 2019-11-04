using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonStorage {

    public static Dictionary<string, List<int>> pairs = new Dictionary<string, List<int>>();
    

    public static void AddItem(string name, int level)
    {
        if (!pairs.ContainsKey(name))
        {
            List<int> l = new List<int>(); l.Add(level);
            pairs.Add(name, l);
        }
        else
            pairs[name].Add(level);
    }

    public Dictionary<string, List<int>> retDict()
    {
        return pairs;
    }
}

public class Stor
{
    public string name;
    public int level;
    public int expRequired;
    public int Health;

    public Stor(string name, int level, int expRequired)
    {
        this.name = name; this.level = level; this.expRequired = expRequired;
        Health = (int)Mathf.Exp((float)level / 12) * 6 + 20;
    }
}

public class PokemonBox
{
    public static Dictionary<string, Dictionary<int, List<int>>> pairOfPairs = new Dictionary<string, Dictionary<int, List<int>>>();
    public static List<Stor> queue = new List<Stor>();
    public static int pokeBallCount = 40;

    public static void AddItem(string name, int level, int expRequired)
    {
       /* if (!pairOfPairs.ContainsKey(name))
        {
            List<int> list = new List<int>();
            list.Add(expRequired);
            Dictionary<int, List<int>> intermediatery = new Dictionary<int, List<int>>();
            intermediatery.Add(level, list);
            pairOfPairs.Add(name, intermediatery);
        }
        else if (!pairOfPairs[name].ContainsKey(level))
        {
            List<int> list = new List<int>();
            list.Add(expRequired);
            pairOfPairs[name].Add(level, list);
        }
        else
            pairOfPairs[name][level].Add(expRequired);*/
        queue.Add(new Stor(name, level, expRequired));
    }

    public static void ModifyValue(string name, int prevlevel, int prevexpNeeded, int newLevel, int newExpNeeded)
    {
        for(int i = 0; i < queue.Count; i++)
        {
            if(queue[i].name.Equals(name) && queue[i].level == prevlevel && queue[i].expRequired == prevexpNeeded)
            {
                queue[i].expRequired = newExpNeeded; queue[i].level = newLevel;
            }
        }
    }

    public static void ModifyHealth(string name, int level, int expRequired, int healthNew)
    {
        for (int i = 0; i < queue.Count; i++)
        {
            if (queue[i].name.Equals(name) && queue[i].level == level && queue[i].expRequired == expRequired)
            {
                queue[i].Health = healthNew;
            }
        }
    }

    public static void RemoveMember(string name, int level, int expNeeded)
    {
        foreach (var item in queue)
        {
            if (item.name.Equals(name) && item.level == level && item.expRequired == expNeeded)
            { queue.Remove(item); break; }
        }
    }
}

public class SceneChangeInfoStorafe
{
    public static Vector3 playerPosition;

    public static string playerPoke;
    public static int level;
    public static int expNeeded;

    public static string enemyPoke;
}
