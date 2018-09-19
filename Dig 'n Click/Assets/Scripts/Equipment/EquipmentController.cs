using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController Instance;
    public int Capacity;
    public SellButtonController SellButton;
    public SortedList<Ore, int> Items = new SortedList<Ore, int>(new OreCompareByValue());

    private List<SlotController> _equipmentSlots = new List<SlotController>();

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

    public void AddItemSlot(SlotController slot)
    {
        _equipmentSlots.Add(slot);
    }

    public void UpdateItemSlots(Ore oreToUpdate = null)
    {
        if (oreToUpdate != null)
        {
            int oreIndex = Items.IndexOfKey(oreToUpdate);
            int amount = Items.ElementAt(oreIndex).Value;
            _equipmentSlots[oreIndex].UpdateOre(amount);
        }
        else
        {
            for (int i = 0; i < Items.Count; ++i) //full update for filled slots
            {
                Ore ore = Items.ElementAt(i).Key;
                int amount = Items.ElementAt(i).Value;
                _equipmentSlots[i].AssignOre(ore, amount);
            }

            if (SellButton.isActiveAndEnabled) //sell button position update
                    SellButton.UpdatePosition(_equipmentSlots);

            for (int i = Items.Count; i < _equipmentSlots.Count; ++i) //update for empty slots
            {
                if (_equipmentSlots[i].IsEmpty())
                    break;
                _equipmentSlots[i].ClearSlot();
            }
        }
    }

    public bool AddItem(Ore itemToAdd, int amount = 1)
    {
        if (amount <= 0)
            return false;

        if (Items.ContainsKey(itemToAdd))
        {
            Items[itemToAdd] += amount;
            UpdateItemSlots(itemToAdd);
            SellButton.UpdateMaxValueInSellHandler(); //update sell slider max value dynamically
            return true;
        }

        if (IsFull())
            return false;

        Items.Add(itemToAdd, amount);
        UpdateItemSlots();
        return true;
    }

    public bool RemoveItem(Ore itemToRemove, int amount)
    {
        if (amount <= 0)
            return false;

        if (IsEmpty())
            return false;

        if (!Items.ContainsKey(itemToRemove))
            return false;

        int newAmountOfItems = Items[itemToRemove] - amount;
        if (newAmountOfItems < 0)
            return false;

        if (newAmountOfItems > 0)
        {
            Items[itemToRemove] = newAmountOfItems;
            UpdateItemSlots(itemToRemove);
        }
        else
        {
            Items.Remove(itemToRemove);
            UpdateItemSlots();
        }

        return true;
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