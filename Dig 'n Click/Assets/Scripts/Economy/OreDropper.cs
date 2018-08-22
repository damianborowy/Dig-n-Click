using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDropper : MonoBehaviour
{
    public float DropChance;
    public List<Ore> Ores;

    public void DropOre()
    {
        if (Random.value > DropChance)
        {
            Debug.Log("No drop");
            return;
        }

        List<Ore> dropableOres = new List<Ore>();
        float dropableOresTotalWeight = 0;
        foreach (var element in Ores)
        {
            if (IsInRangeInvclusive(GameController.Instance.GetLevel(), element.MinLevel, element.MaxLevel))
            {
                Debug.Log("Adding " + element.Name + " to dropableOres");
                dropableOres.Add(element);
                dropableOresTotalWeight += element.DropWeight;
            }
        }
        Debug.Log("dropableOresTotalWeight: " + dropableOresTotalWeight);

        float rand = Random.Range(0, dropableOresTotalWeight);
        Debug.Log("Trying to drop ore with rand: " + rand);
        float workingWeight = 0;
        foreach (var element in dropableOres)
        {
            workingWeight += element.DropWeight;
            Debug.Log("workingWeight: " + workingWeight);
            if (rand <= workingWeight)
            {
                Debug.Log("Dropped ore: " + element.Name);
                EquipmentController.Instance.AddItem(element);
                break;
            }
        }
    }

    private bool IsInRangeInvclusive(float value, float rangeMin, float rangeMax)
    {
        return rangeMin <= value && value <= rangeMax;
    }
}