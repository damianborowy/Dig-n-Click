using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreDropper : MonoBehaviour
{
    public float DropChance;
    public RectTransform Canvas;
    public GameObject DropText;
    public float TextSpawnRadius;
    public List<Ore> Ores;

    public void DropOre()
    {
        if (Random.value > DropChance) return;

        List<Ore> dropableOres = new List<Ore>();
        float dropableOresTotalWeight = 0;
        foreach (var element in Ores)
        {
            if (IsInRangeInvclusive(GameController.Instance.GetLevel(), element.MinLevel, element.MaxLevel))
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
                InstantiateDropText(element);
                EquipmentController.Instance.AddItem(element);
                break;
            }
        }
    }

    private void InstantiateDropText(Ore dropedOre)
    {
        GameObject instantiatedTextGameObject = Instantiate(DropText, Canvas);
        Text text = instantiatedTextGameObject.GetComponent<Text>();
        Vector3 randomPosition = Random.insideUnitSphere * TextSpawnRadius;
        text.rectTransform.position = new Vector3(Canvas.position.x + randomPosition.x, Canvas.position.y + randomPosition.y, text.transform.position.z);
        text.text = "+1 " + dropedOre.Name;
        text.color = dropedOre.DropTextColor;
    }

    private bool IsInRangeInvclusive(float value, float rangeMin, float rangeMax)
    {
        return rangeMin <= value && value <= rangeMax;
    }
}