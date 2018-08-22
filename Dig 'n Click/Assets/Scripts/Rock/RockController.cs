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
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private OreDropper _dropper;
    private bool _isDestroyed;

    private void Start()
    {
        _isDestroyed = false;

        int level = GameController.Instance.GetLevel();
        _health = Math.Round(4 * Mathf.Pow(1.12f, level) + 6); //<--- public variables instead of plain numbers
        _maxHealth = _health;
        _reward = UnityEngine.Random.Range(1, 2) * level; //<--- as above, additionaly this range returns always 1

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = new Vector2(0, -InitialFallingSpeed);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Sprites[0];

        GameObject slider = GameObject.FindWithTag("Slider");
        if (slider != null)
            _slider = slider.GetComponent<Slider>();
        _hpLeftDisplay = _slider.GetComponentInChildren<Text>();
        UpdateSlider();

        GameObject dropper = GameObject.FindWithTag("Dropper");
        if (dropper != null)
            _dropper = dropper.GetComponent<OreDropper>();
    }

    public void Hit(int strength)
    {
        if (strength < _health)
        {
            _health -= strength;

            if (_health < _maxHealth / 4)
                _spriteRenderer.sprite = Sprites[3];
            else if (_health < _maxHealth / 2)
                _spriteRenderer.sprite = Sprites[2];
            else if (_health < _maxHealth * 3 / 4)
                _spriteRenderer.sprite = Sprites[1];

            UpdateSlider();
        }
        else if (!_isDestroyed)
        {
            _isDestroyed = true;
            _health = 0;
            UpdateSlider();

            _dropper.DropOre();
            GameController.Instance.AddMoney(_reward);
            GameController.Instance.SpawnRock();
            Destroy(gameObject);
        }
    }

    private void UpdateSlider()
    {
        _slider.value = Convert.ToSingle(_health / _maxHealth);
        _hpLeftDisplay.text =
            MoneyConverter.ConvertNumber(_health) + " / " + MoneyConverter.ConvertNumber(_maxHealth);
    }
}