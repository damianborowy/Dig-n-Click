using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDropper : MonoBehaviour
{
    public float DropChance;

    private Dictionary<Ore, int> _droppedOres;

    public void DropOre(int repeat = 1)
    {
        var ores = OresList.Instance.Ores;

        List<Ore> dropableOres = new List<Ore>();
        float dropableOresTotalWeight = 0;
        foreach (var element in ores)
        {
            if (IsInRangeInclusive(GameController.Instance.GetLevel(), element.MinLevel, element.MaxLevel))
            {
                dropableOres.Add(element);
                dropableOresTotalWeight += element.DropWeight;
            }
        }
        if (dropableOres.Count <= 0) return;

        _droppedOres = new Dictionary<Ore, int>();
        for (int i = 0; i < repeat; ++i)
        {
            if (Random.value > DropChance) continue;

            float rand = Random.Range(0, dropableOresTotalWeight);
            float workingWeight = 0;
            foreach (var element in dropableOres)
            {
                workingWeight += element.DropWeight;
                if (rand <= workingWeight)
                {
                    if (_droppedOres.ContainsKey(element))
                        ++_droppedOres[element];
                    else
                        _droppedOres.Add(element, 1);
                    break;
                }
            }
        }

        foreach (var element in _droppedOres)
        {
            EquipmentController.Instance.AddItem(element.Key, element.Value);
        }
    }

    private bool IsInRangeInclusive(float value, float rangeMin, float rangeMax)
    {
        return rangeMin <= value && value <= rangeMax;
    }
}