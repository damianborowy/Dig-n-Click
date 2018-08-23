using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    public BackgroundScroller Scroller;
    public Direction ChangeDirection;

    private Button _arrow;

    public void OnClick()
    {
        if (Scroller.IsBackgroundScrolling()) return;

        int levelToMoveTo = GameController.Instance.GetLevel();

        if (ChangeDirection == Direction.Down && levelToMoveTo < GameController.Instance.GetMaxLevel())
            ChangeLevel(++levelToMoveTo);
        else if (ChangeDirection == Direction.Up && levelToMoveTo > 1)
            ChangeLevel(--levelToMoveTo);
    }

    private void ChangeLevel(int levelToMoveTo)
    {
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