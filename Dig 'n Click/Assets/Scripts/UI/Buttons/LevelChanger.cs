using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    public BackgroundScroller Scroller;
    public Direction ChangeDirection;

    private Button _arrow;
    private bool _isPricetagHidden;

    public void OnClick()
    {
        if (Scroller.IsBackgroundScrolling()) return;

        int currentLevel = GameController.Instance.GetLevel();

        if (ChangeDirection == Direction.Down && currentLevel < GameController.Instance.GetMaxLevel())
        {
            ChangeLevel(++currentLevel);
        }
        else if (ChangeDirection == Direction.Down &&
                 GameController.Instance.GetNextLevelCost() <= GameController.Instance.GetMoney() && !_isPricetagHidden)
            GameController.Instance.AddMaxLevel();
        else if (ChangeDirection == Direction.Up && currentLevel > 1)
            ChangeLevel(--currentLevel);
    }

    public void UpdatePricetag() //only for down arrow
    {
        if (GameController.Instance.GetLevel() != GameController.Instance.GetMaxLevel())
        {
            transform.GetChild(0).localScale = new Vector3(0, 0, 0);
            _isPricetagHidden = true;
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            _isPricetagHidden = false;
        }
    }

    private void ChangeLevel(int levelToMoveTo)
    {
        AutoMiner.Instance.StopMiner();
        GameController.Instance.SetLevel(levelToMoveTo);
        StartCoroutine(Ascend());
    }

    private IEnumerator Ascend()
    {
        var rock = GameObject.FindWithTag("Rock");
        var shadow = GameObject.FindWithTag("Shadow");
        if (rock != null)
            Destroy(rock.gameObject);
        if (shadow != null)
            shadow.SetActive(false);
        Scroller.ScrollBackground(ChangeDirection);
        while (Scroller.IsBackgroundScrolling())
        {
            yield return null;
        }

        if (shadow != null)
            shadow.SetActive(true);
        if (GameObject.FindWithTag("Rock") == null)
            GameController.Instance.SpawnRock();
    }
}