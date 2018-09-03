using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilitesHandler : MonoBehaviour
{
    public GameObject ProbabilitySlot;

    private OreDropper _oreDropper;

    private void Start()
    {
        _oreDropper = GameObject.FindWithTag("Dropper").GetComponent<OreDropper>();
        UpdateProbabilities();
    }

    public void UpdateProbabilities()
    {
        List<Ore> droppbaleOres = _oreDropper.GetDroppableOres();
        float droppableOresTotalWeight = OreDropper.GetTotalWeight(droppbaleOres);

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var element in droppbaleOres)
        {
            GameObject probabilitySlotGameObject = Instantiate(ProbabilitySlot, transform);
            probabilitySlotGameObject.transform.Find("Ore").GetComponent<Image>().sprite = element.OreSprite;
            float dropProbability = element.DropWeight / droppableOresTotalWeight * _oreDropper.DropChance;
            probabilitySlotGameObject.transform.Find("ProbabilityText").GetComponentInChildren<Text>().text = dropProbability.ToString("P");
        }
    }
}