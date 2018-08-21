using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDropper : MonoBehaviour
{
    public List<Ore> Ores;

    private void Start()
    {
        Ores.Sort(new OreCompareByDropChance());
    }

    public void DropOre()
    {
        float rand = Random.value;
        foreach (var element in Ores)
        {
            if (rand < element.DropChance && IsInRange(GameController.Instance.GetLevel(), element.MinLevel, element.MaxLevel))
            {
                //EQ.AddOre(element);
                break;
            }
        }
    }

    private bool IsInRange(float value, float rangeMin, float rangeMax)
    {
        return rangeMin < value && value < rangeMax;
    }
}