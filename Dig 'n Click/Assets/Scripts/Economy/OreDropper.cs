using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OreDropper : MonoBehaviour
{
    public RectTransform TextSpawn;
    public GameObject DropText;
    public float TextSpawnRadius;
    public float DropChance;

    private Dictionary<Ore, int> _droppedOres;

    public Dictionary<Ore, int> DropOre(int repeat = 1)
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

        _droppedOres = new Dictionary<Ore, int>();
        if (dropableOres.Count <= 0) return _droppedOres;

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

        bool isInventoryFull = false;
        List<Ore> notAddedToInventory = new List<Ore>();
        foreach (var element in _droppedOres)
        {
            if (EquipmentController.Instance.AddItem(element.Key, element.Value))
                InstantiateDropText(element.Key, element.Value);
            else
            {
                notAddedToInventory.Add(element.Key);
                isInventoryFull = true;
            }
        }

        if(isInventoryFull)
            InstantiateInventoryIsFullText();
        
        foreach (var element in notAddedToInventory)
            _droppedOres.Remove(element);

        return _droppedOres;
    }

    private void InstantiateDropText(Ore dropedOre, int amount = 1)
    {
        Text createdText = CreateTextGameobject();
        createdText.text = amount.ToString("+#;-#;0") + " " + dropedOre.Name;
        createdText.color = dropedOre.DropTextColor;
    }

    private void InstantiateInventoryIsFullText()
    {
        Text createdText = CreateTextGameobject();
        createdText.text = "Inventory is full!";
        createdText.color = Color.red;
    }

    private Text CreateTextGameobject()
    {
        GameObject instantiatedTextGameObject = Instantiate(DropText, TextSpawn);
        Text text = instantiatedTextGameObject.GetComponent<Text>();
        Vector3 randomPosition = Random.insideUnitSphere * TextSpawnRadius;
        text.rectTransform.position = new Vector3(TextSpawn.position.x + randomPosition.x,
            TextSpawn.position.y + randomPosition.y, text.transform.position.z);
        return text;
    }

    private bool IsInRangeInclusive(float value, float rangeMin, float rangeMax)
    {
        return rangeMin <= value && value <= rangeMax;
    }
}