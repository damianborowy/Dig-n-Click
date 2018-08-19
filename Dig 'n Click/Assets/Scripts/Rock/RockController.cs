using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    public float InitialFallingSpeed;
    public Sprite[] Sprites;

    private Slider _slider;
    private Text _hpLeftDisplay;
    private float _health;
    private float _maxHealth;
    private double _reward;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private Slider _slider;
    private Text _hpLeftDisplay;
    private float _health;
    private float _maxHealth;
    private double _reward;
    private Rigidbody2D _rb;

    private void Start()
    {
        _slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        _hpLeftDisplay = _slider.GetComponentInChildren<Text>();

        int level = GameController.Instance.GetLevel();
        _health = 10 * Mathf.Pow(1.1f, level) - 1;
        _maxHealth = _health;
        _reward = Random.Range(1, 10) * level;

        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(0, -InitialFallingSpeed);

        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = Sprites[0];
    }

    public void Hit(float strength)
    {
        if (strength < _health)
        {
            _health -= strength;

            if (_health < _maxHealth / 4)
                _sr.sprite = Sprites[3];
            else if (_health < _maxHealth / 2)
                _sr.sprite = Sprites[2];
            else if (_health < _maxHealth * 3 / 4)
                _sr.sprite = Sprites[1];

                Debug.Log("Rock's Health: " + _health);
        }
        else
        {
            Debug.Log("Rock destroyed");

            GameController.Instance.AddMoney(_reward);
            GameController.Instance.SpawnRock();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _slider.value = _health / _maxHealth;
        _hpLeftDisplay.text = _health + " / " + _maxHealth;
    }
}