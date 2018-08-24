using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController Instance;
    public int Capacity;
    public RectTransform Canvas;
    public GameObject DropText;
    public float TextSpawnRadius;
    public SortedList<Ore, int> Items = new SortedList<Ore, int>(new OreCompareByValue());

    private List<GameObject> _equipmentSlots = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateItemSlots();
    }

    public void AddItemSlot(GameObject slot)
    {
        _equipmentSlots.Add(slot);
    }

    private void UpdateItemSlots(Ore oreToUpdate = null)
    {
        if (oreToUpdate != null)
        {
            int oreIndex = Items.IndexOfKey(oreToUpdate);
            GameObject amountInSlot = _equipmentSlots[oreIndex].transform.Find("Amount").gameObject;
            Text itemAmount = amountInSlot.GetComponent<Text>();
            itemAmount.text = Items.ElementAt(oreIndex).Value.ToString();
            Debug.Log("Slot updated (oreToUpdate!=null)");
        }
        else
            for (int i = 0; i < Items.Count; ++i) //full update
            {
                GameObject itemInSlot = _equipmentSlots[i].transform.Find("Item").gameObject;
                Image itemImage = itemInSlot.GetComponent<Image>();
                itemImage.sprite = Items.ElementAt(i).Key.OreSprite;
                GameObject amountInSlot = _equipmentSlots[i].transform.Find("Amount").gameObject;
                Text itemAmount = amountInSlot.GetComponent<Text>();
                itemAmount.text = Items.ElementAt(i).Value.ToString();
                Debug.Log("Item slot updated");
            }
    }

    public bool AddItem(Ore itemToAdd, int amount = 1)
    {
        if (Items.ContainsKey(itemToAdd))
        {
            Debug.Log("Adding " + itemToAdd.Name + " in amount of " + amount);
            InstantiateDropText(itemToAdd);
            int newAmountOfItems = Items[itemToAdd] + amount;
            Items[itemToAdd] = newAmountOfItems;
            UpdateItemSlots(itemToAdd);
            Debug.Log("New amount is: " + Items[itemToAdd]);
            return true;
        }

        if (IsFull())
        {
            Debug.Log("Inventory is full");
            InstantiateInventoryIsFullText();
            return false;
        }

        Debug.Log("Adding new " + itemToAdd.Name + " in amount of " + amount);
        InstantiateDropText(itemToAdd);
        Items.Add(itemToAdd, amount);
        UpdateItemSlots();
        return true;
    }

    public bool RemoveItem(Ore itemToRemove, int amount = 1)
    {
        if (IsEmpty())
        {
            Debug.Log("Inventory is empty");
            return false;
        }

        if (!Items.ContainsKey(itemToRemove))
        {
            Debug.Log("Item to remove missing");
            return false;
        }

        int newAmountOfItems = Items[itemToRemove] - amount;
        if (newAmountOfItems < 0)
        {
            Debug.Log("New amount is negative");
            return false;
        }

        if (newAmountOfItems > 0)
        {
            Debug.Log("Removing " + itemToRemove.Name + " in amount of " + amount);
            Items[itemToRemove] = newAmountOfItems;
            UpdateItemSlots(itemToRemove);
            Debug.Log("New amount is: " + Items[itemToRemove]);
        }
        else
        {
            Debug.Log("Amount of " + itemToRemove.Name + " is 0, removing from inventory");
            Items.Remove(itemToRemove);
            UpdateItemSlots();
        }

        return true;
    }

    private void InstantiateDropText(Ore dropedOre)
    {
        GameObject instantiatedTextGameObject = Instantiate(DropText, Canvas);
        Text text = instantiatedTextGameObject.GetComponent<Text>();
        Vector3 randomPosition = Random.insideUnitSphere * TextSpawnRadius;
        text.rectTransform.position = new Vector3(Canvas.position.x + randomPosition.x,
            Canvas.position.y + randomPosition.y, text.transform.position.z);
        text.text = "+1 " + dropedOre.Name;
        text.color = dropedOre.DropTextColor;
    }

    private void InstantiateInventoryIsFullText()
    {
        GameObject instantiatedTextGameObject = Instantiate(DropText, Canvas);
        Text text = instantiatedTextGameObject.GetComponent<Text>();
        Vector3 randomPosition = Random.insideUnitSphere * TextSpawnRadius;
        text.rectTransform.position = new Vector3(Canvas.position.x + randomPosition.x,
            Canvas.position.y + randomPosition.y, text.transform.position.z);
        text.text = "Inventory is full!";
        text.color = Color.red;
    }

    private bool IsFull()
    {
        return Items.Count >= Capacity;
    }

    private bool IsEmpty()
    {
        return Items.Count <= 0;
    }
}