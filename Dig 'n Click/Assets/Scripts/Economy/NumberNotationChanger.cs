using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberNotationChanger : MonoBehaviour
{
    public Button NormalButton;
    public Button SimpleButton;
    public Button ScientificButton;
    public GameObject UpgradesPanel;

    private void Start()
    {
        if (MoneyConverter.Notation.Equals(MoneyConverter.Type.Normal))
            Normal();
        else if (MoneyConverter.Notation.Equals(MoneyConverter.Type.Scientific))
            Scientific();
        else
            Simple();
    }

    public void Normal()
    {
        MoneyConverter.ChangeNotation(MoneyConverter.Type.Normal);
        DisableButton(NormalButton);
        EnableButton(SimpleButton);
        EnableButton(ScientificButton);
        UpdateText();
    }

    public void Simple()
    {
        MoneyConverter.ChangeNotation(MoneyConverter.Type.Simple);
        DisableButton(SimpleButton);
        EnableButton(NormalButton);
        EnableButton(ScientificButton);
        UpdateText();
    }

    public void Scientific()
    {
        MoneyConverter.ChangeNotation(MoneyConverter.Type.Scientific);
        DisableButton(ScientificButton);
        EnableButton(NormalButton);
        EnableButton(SimpleButton);
        UpdateText();
    }

    private void DisableButton(Button buttonToDisable)
    {
        buttonToDisable.image.color = buttonToDisable.colors.disabledColor;
        buttonToDisable.interactable = false;
    }
    private void EnableButton(Button buttonToEnable)
    {
        buttonToEnable.image.color = buttonToEnable.colors.normalColor;
        buttonToEnable.interactable = true;
    }

    private void UpdateText()
    {
        var gameControllerInstance = GameController.Instance;
        gameControllerInstance.AddMoney(0);
        gameControllerInstance.CalculateNextLevelCost();
        gameControllerInstance.UpdateMiningPowerText();
        foreach (Transform childUpgradeTransform in UpgradesPanel.transform)
        {
            var upgradePanelHandler = childUpgradeTransform.gameObject.GetComponent<UpgradePanelHandler>();
            upgradePanelHandler.CalculateNextUpgradeCost();
        }
    }
}