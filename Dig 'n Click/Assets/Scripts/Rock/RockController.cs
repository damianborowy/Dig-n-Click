using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class RockController : MonoBehaviour, IPointerDownHandler
{
    public float InitialFallingSpeed;
    public Sprite[] Sprites;
    public GameObject HitParticles;
    public GameObject DestroyParticles;
    public double Health;
    public double MaxHealth;
    public AudioClip[] PickaxeSound;
    public AudioClip[] DestroySound;

    private bool _canBeHit;
    private Slider _slider;
    private Text _hpLeftDisplay;
    private double _reward;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private OreDropper _dropper;
    private bool _isDestroyed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = new Vector2(0, -InitialFallingSpeed);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Sprites[0];
    }

    private void Start()
    {
        _isDestroyed = false;

        int level = GameController.Instance.GetLevel();
        Health = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, level);
        MaxHealth = Health;
        _reward = BasicEconomyValues.MoneyReward(level);
        
        GameObject slider = GameObject.FindWithTag("Slider");
        if (slider != null)
            _slider = slider.GetComponent<Slider>();
        _hpLeftDisplay = _slider.GetComponentInChildren<Text>();
        UpdateSlider();

        GameObject dropper = GameObject.FindWithTag("Dropper");
        if (dropper != null)
            _dropper = dropper.GetComponent<OreDropper>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _canBeHit = true;
    }

    public void Hit(double strength)
    {
        if (!_canBeHit) return;

        if (strength < Health)
        {
            Health -= strength;

            if (Health < MaxHealth / 4)
                _spriteRenderer.sprite = Sprites[3];
            else if (Health < MaxHealth / 2)
                _spriteRenderer.sprite = Sprites[2];
            else if (Health < MaxHealth * 3 / 4)
                _spriteRenderer.sprite = Sprites[1];

            UpdateSlider();
        }
        else if (!_isDestroyed)
        {
            _isDestroyed = true;
            Health = 0;
            UpdateSlider();
            Instantiate(DestroyParticles, transform.position, Quaternion.identity);
            AudioController.Instance.PlayDestroySound(DestroySound[Random.Range(0, DestroySound.Length)]);

            _dropper.DropOre();
            GameController.Instance.AddMoney(_reward);
            GameController.Instance.SpawnRock();
            Destroy(gameObject);
        }
    }

    private void UpdateSlider()
    {
        _slider.value = Convert.ToSingle(Health / MaxHealth);
        _hpLeftDisplay.text =
            MoneyConverter.ConvertNumber(Health) + " / " + MoneyConverter.ConvertNumber(MaxHealth);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_canBeHit) return;

        AudioController.Instance.PlayPickaxeSound(PickaxeSound[Random.Range(0, PickaxeSound.Length)]);
        Instantiate(HitParticles, eventData.pointerCurrentRaycast.worldPosition, Quaternion.identity);
        Hit(GameController.Instance.GetStrength());
    }
}