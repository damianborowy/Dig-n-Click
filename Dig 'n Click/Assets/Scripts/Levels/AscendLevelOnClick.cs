using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class AscendLevelOnClick : MonoBehaviour
{
    public BackgroundScroller Scroller;

    public void OnClick()
    {
        if (GameController.Instance.GetNextLevelCost() <= GameController.Instance.GetMoney())
        {
            if (!Scroller.IsBackgroundScrolling())
                StartCoroutine(Ascend());
        }
    }

    private IEnumerator Ascend()
    {
        GameController.Instance.AddLevel();
        var rock = GameObject.FindWithTag("Rock");
        var shadow = GameObject.FindWithTag("Ground");
        if (rock != null)
            Destroy(rock.gameObject);
        if (shadow != null)
            shadow.SetActive(false);
        Scroller.ScrollBackground();
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