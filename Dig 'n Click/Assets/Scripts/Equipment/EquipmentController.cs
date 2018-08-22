using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController Instance;
    public int Capacity;

    private SortedList<Ore, int> _items = new SortedList<Ore, int>(new OreCompareByValue());

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

    public bool AddItem(Ore itemToAdd, int amount = 1)
    {
        if (_items.ContainsKey(itemToAdd))
        {
            Debug.Log("Adding " + itemToAdd.Name + " in amount of " + amount);
            int newAmountOfItems = _items[itemToAdd] + amount;
            _items[itemToAdd] = newAmountOfItems;
            Debug.Log("New amount is: " + _items[itemToAdd]);
            return true;
        }

        if (IsFull())
        {
            Debug.Log("Inventory is full");
            return false;
        }

        Debug.Log("Adding new " + itemToAdd.Name + " in amount of " + amount);
        _items.Add(itemToAdd, amount);
        return true;
    }

    public bool RemoveItem(Ore itemToRemove, int amount = 1)
    {
        if (IsEmpty())
        {
            Debug.Log("Inventory is empty");
            return false;
        }

        if (!_items.ContainsKey(itemToRemove))
        {
            Debug.Log("Item to remove missing");
            return false;
        }

        int newAmountOfItems = _items[itemToRemove] - amount;
        if (newAmountOfItems < 0)
        {
            Debug.Log("New amount is negative");
            return false;
        }

        if (newAmountOfItems > 0)
        {
            Debug.Log("Removing " + itemToRemove.Name + " in amount of " + amount);
            _items[itemToRemove] = newAmountOfItems;
            Debug.Log("New amount is: " + _items[itemToRemove]);
        }
        else
        {
            Debug.Log("Amount of " + itemToRemove.Name + " is 0, removing from inventory");
            _items.Remove(itemToRemove);
        }

        return true;
    }

    private bool IsFull()
    {
        return _items.Count >= Capacity;
    }

    private bool IsEmpty()
    {
        return _items.Count <= 0;
    }
}