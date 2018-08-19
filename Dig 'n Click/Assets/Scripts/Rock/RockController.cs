using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour {

    public Text HealthDisplay;
    public float Health;
    public double Reward;

    private void Start()
    {
        int level = GameController.Instance.GetLevel();
        Health = 10 * Mathf.Pow(1.1f, level) - 1;
        Reward = Random.Range(1, 10) * level;
    }

    public void Hit(float strength)
    {
        if (strength < Health)
        {
            Health -= strength;
        }
        
    }

    private void Update()
    {
        HealthDisplay.text = "Rock's Health: " + Health;
    }
}
