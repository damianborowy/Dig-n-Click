using System;
using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    public float InitialFallingSpeed;
    public Sprite[] Sprites;

    private Slider _slider;
    private Text _hpLeftDisplay;
    private double _health;
    private double _maxHealth;
    private double _reward;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _isDestroyed;

    private void Start()
    {
        _isDestroyed = false;

        _slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        _hpLeftDisplay = _slider.GetComponentInChildren<Text>();

        int level = GameController.Instance.GetLevel();
        _health = Math.Round(4 * Mathf.Pow(1.12f, level) + 6);
        _maxHealth = _health;
        _reward = UnityEngine.Random.Range(1, 2) * level;

        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(0, -InitialFallingSpeed);

        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = Sprites[0];
    }

    public void Hit(int strength)
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
        }
        else if (!_isDestroyed)
        {
            _isDestroyed = true;
            GameController.Instance.AddMoney(_reward);
            GameController.Instance.SpawnRock();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _slider.value = Convert.ToSingle(_health / _maxHealth);
        _hpLeftDisplay.text =
            MoneyConverter.ConvertNumber(_health) + " / " + MoneyConverter.ConvertNumber(_maxHealth);
    }
}