using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
    public Button[] RewardsSlotsButtons;
    public Sprite InactiveButtonImage;
    public Sprite ActiveButtonImage;
    public GameObject[] RewardAvailableExclamationMarks;
    public AudioClip ClaimSound;
    public float ClaimSoundVolume;

    private int _claimCount;
    private double _lastClaimTime;
    private bool _isInitialized;
    private int[] _rewards = new[] {1, 5, 10, 5, 5, 15, 25};

    private void Start()
    {
        CheckClaimCount();
        ToggleButtons();
        _isInitialized = true;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus || !_isInitialized) return;
        CheckClaimCount();
        ToggleButtons();
    }

    private void CheckClaimCount()
    {
        double timeSinceLastClaim = GetTimeSinceLastClaim();
        if (SecondsToDays(timeSinceLastClaim) > 2 || _claimCount >= 7)
        {
            _claimCount = 0;
            Debug.Log("_claimCount reset to 0");
        }
    }

    private void ToggleButtons()
    {
        int days;
        for (days = 0; _claimCount > days; ++days)
            SetButtonClaimed(RewardsSlotsButtons[days]);

        Debug.Log("Days since last claim: " + SecondsToDays(GetTimeSinceLastClaim()));
        if (SecondsToDays(GetTimeSinceLastClaim()) > 1)
        {
            SetButtonActive(RewardsSlotsButtons[days++]);
            ActivateExclamationMarks();
        }

        for (; days < RewardsSlotsButtons.Length; ++days)
        {
            SetButtonLocked(RewardsSlotsButtons[days]);
        }
    }

    private void ActivateExclamationMarks()
    {
        foreach (var exclamationMark in RewardAvailableExclamationMarks)
        {
            exclamationMark.SetActive(true);
        }
    }

    private double SecondsToDays(double seconds)
    {
        const double secondsInDay = 24 * 60 * 60;
        return seconds / secondsInDay;
    }

    private double GetTimeSinceLastClaim()
    {
        return GetCurrentTime() - _lastClaimTime;
    }
    
    private double GetCurrentTime()
    {
        return (DateTime.UtcNow - GameController.EpochTimeStart).TotalSeconds;
    }

    private void SetButtonClaimed(Button targetButton)
    {
        DeactivateButtonImage(targetButton);
        SetButtonText(targetButton, "Claimed");
        DeavtivateButtonPadlock(targetButton);
        targetButton.interactable = false;
    }

    private void SetButtonActive(Button targetButton)
    {
        ActivateButtonImage(targetButton);
        SetButtonText(targetButton, "Claim!");
        DeavtivateButtonPadlock(targetButton);
        targetButton.interactable = true;
    }

    private void SetButtonLocked(Button targetButton)
    {
        DeactivateButtonImage(targetButton);
        SetButtonText(targetButton, "");
        ActivateButtonPadlock(targetButton);
        targetButton.interactable = false;
    }

    private void ActivateButtonImage(Button targetButton)
    {
        targetButton.GetComponent<Image>().sprite = ActiveButtonImage;
    }

    private void DeactivateButtonImage(Button targetButton)
    {
        targetButton.GetComponent<Image>().sprite = InactiveButtonImage;
    }

    private void SetButtonText(Button targetButton, string text)
    {
        targetButton.GetComponentInChildren<Text>().text = text;
    }

    private void ActivateButtonPadlock(Button targetButton)
    {
        targetButton.transform.Find("Padlock").gameObject.SetActive(true);
    }

    private void DeavtivateButtonPadlock(Button targetButton)
    {
        targetButton.transform.Find("Padlock").gameObject.SetActive(false);
    }

    public int GetClaimCount()
    {
        return _claimCount;
    }

    public void LoadClaimCount(int count)
    {
        _claimCount = count;
    }

    public double GetLastClaimTime()
    {
        return _lastClaimTime;
    }

    public void LoadLastClaimTime(double time)
    {
        _lastClaimTime = time;
    }

    public void ClaimDayRewardByIndex(int dayIndex)
    {
        _lastClaimTime = GetCurrentTime();
        ++_claimCount;
        SetButtonClaimed(RewardsSlotsButtons[dayIndex]);
        DeactivateExclamationMarks();
        AddPrestigeCrystals(_rewards[dayIndex]);
        AudioController.Instance.PlayAudioEffect(ClaimSound, ClaimSoundVolume);
    }

    private void DeactivateExclamationMarks()
    {
        foreach (var exclamationMark in RewardAvailableExclamationMarks)
        {
            exclamationMark.SetActive(false);
        }
    }

    private void AddPrestigeCrystals(int amount)
    {
        GameController.Instance.AddPrestigeCrystals(amount);
        PrestigeController.Instance.UpdateOwnedAmount();
    }
}