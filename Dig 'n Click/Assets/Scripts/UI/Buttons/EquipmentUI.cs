using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject Slot;
    public GameObject ScrollableSlots;
    public List<BookmarkChanger> Bookmarks;
    public AudioClip BookmarkSound;
    public float BookmarkSoundVolume;

    private MovingUIHandler _movingUIHandler;

    private void Awake()
    {
        _movingUIHandler = transform.parent.gameObject.GetComponent<MovingUIHandler>();
        for (int i = 0; i < EquipmentController.Instance.Capacity; ++i)
        {
            GameObject slotGameObject = Instantiate(Slot, ScrollableSlots.transform.Find("SlotsPanel"));
            SlotController slotController = slotGameObject.GetComponent<SlotController>();
            EquipmentController.Instance.AddItemSlot(slotController);
        }
    }

    private void Start()
    {
        ChangeToBookmarkByIndex(0);
    }

    public void ChangeToBookmarkByIndex(int index)
    {
        for (int i = 0; i < Bookmarks.Count; ++i)
        {
            if (i == index)
                Bookmarks[i].Activate();
            else
                Bookmarks[i].Deactivate();
        }
    }

    public void PlayBookmarkSound()
    {
        AudioController.Instance.PlayAudioEffect(BookmarkSound, BookmarkSoundVolume);
    }

    public void Toggle()
    {
        _movingUIHandler.Move(MovingUIHandler.Type.Equipment);

        GameObject sellButtonGameObject = GameObject.FindWithTag("SellButton");
        if (sellButtonGameObject != null)
            sellButtonGameObject.GetComponent<SellButtonController>().Clear();
    }
}

[System.Serializable]
public class BookmarkChanger
{
    public GameObject Slots;
    public Button Bookmark;

    public void Activate()
    {
        Slots.SetActive(true);
        Bookmark.interactable = false;
    }

    public void Deactivate()
    {
        Slots.SetActive(false);
        Bookmark.interactable = true;
    }
}