using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDropper : MonoBehaviour
{
    public float DropChance;

    public void DropOre(int repeat = 1)
    {
        for (int i = 0; i < repeat; ++i)
        {
            var ores = OresList.Instance.Ores;

            if (Random.value > DropChance) continue;

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

            float rand = Random.Range(0, dropableOresTotalWeight);
            float workingWeight = 0;
            foreach (var element in dropableOres)
            {
                workingWeight += element.DropWeight;
                if (rand <= workingWeight)
                {
                    EquipmentController.Instance.AddItem(element);
                    break;
                }
            }
        }
    }

    private bool IsInRangeInclusive(float value, float rangeMin, float rangeMax)
    {
        return rangeMin <= value && value <= rangeMax;
    }
}