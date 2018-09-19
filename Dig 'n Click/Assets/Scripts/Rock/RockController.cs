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
    public float PickaxeSoundVolume;
    public AudioClip[] DestroySound;
    public float DestroySoundVolume;

    private bool _canBeHit;
    private bool _areHitParticlesOn;
    private bool _areDestroyParticlesOn;
    private Slider _slider;
    private Text _hpLeftDisplay;
    private double _reward;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private OreDropper _dropper;
    private Transform _particlesSpawn;
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
        LoadPlayerPrefs();
        SetRockHealth();
        SetRockReward();
        SetNeededGameObjects();
    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("HIT"))
            _areHitParticlesOn = PlayerPrefs.GetInt("HIT") == 1;
        if (PlayerPrefs.HasKey("DESTROY"))
            _areDestroyParticlesOn = PlayerPrefs.GetInt("DESTROY") == 1;
    }

    private void SetRockHealth()
    {
        int level = GameController.Instance.GetLevel();
        Health = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias,
            BasicEconomyValues.HealthExponentialMultiplier, level);
        MaxHealth = Health;
    }

    private void SetRockReward()
    {
        int level = GameController.Instance.GetLevel();
        if (level == 1)
            _reward = 1;
        else
            _reward = BasicEconomyValues.MoneyReward(level);
    }

    private void SetNeededGameObjects()
    {
        SetSlider();
        SetHPDisplay(_slider);
        SetDropper();
        SetParticlesSpawn();
    }

    private void SetSlider()
    {
        GameObject slider = GameObject.FindWithTag("Slider");
        if (slider != null)
            _slider = slider.GetComponent<Slider>();
    }

    private void SetHPDisplay(Slider slider)
    {
        if (slider != null)
            _hpLeftDisplay = slider.GetComponentInChildren<Text>();
        UpdateSlider();
    }

    private void SetDropper()
    {
        GameObject dropper = GameObject.FindWithTag("Dropper");
        if (dropper != null)
            _dropper = dropper.GetComponent<OreDropper>();
    }

    private void SetParticlesSpawn()
    {
        GameObject particlesSpawn = GameObject.FindWithTag("ParticlesSpawn");
        if (particlesSpawn != null)
            _particlesSpawn = particlesSpawn.transform;
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
            UpdateRockVisuals();
        }
        else if (!_isDestroyed)
        {
            DestroyRockAndSpawnNew();
        }
    }

    private void UpdateRockVisuals()
    {
        if (Health < MaxHealth / 4)
            _spriteRenderer.sprite = Sprites[3];
        else if (Health < MaxHealth / 2)
            _spriteRenderer.sprite = Sprites[2];
        else if (Health < MaxHealth * 3 / 4)
            _spriteRenderer.sprite = Sprites[1];

        UpdateSlider();
    }

    private void DestroyRockAndSpawnNew()
    {
        _isDestroyed = true;
        AddRockRewards();
        CreateDestroyedRockVisualsAndAudio();
        GameController.Instance.SpawnRock();
        Destroy(gameObject);
    }

    private void CreateDestroyedRockVisualsAndAudio()
    {
        Health = 0;
        UpdateSlider();
        CreateDestroyParticles();
        PlayDestroySound();
    }

    private void AddRockRewards()
    {
        _dropper.DropOre();
        GameController.Instance.AddMoney(_reward);
    }

    private void CreateDestroyParticles()
    {
        if (!_areDestroyParticlesOn) return;
        GameObject instantiatedParticles = Instantiate(DestroyParticles, _particlesSpawn);
        instantiatedParticles.transform.position = transform.position;
    }

    private void PlayDestroySound()
    {
        AudioClip randomDestroySound = DestroySound[Random.Range(0, PickaxeSound.Length)];
        AudioController.Instance.PlayAudioEffect(randomDestroySound, DestroySoundVolume);
    }

    private void UpdateSlider()
    {
        _slider.value = Convert.ToSingle(Health / MaxHealth);
        _hpLeftDisplay.text =
            MoneyConverter.ConvertNumber(Health) + " / " + MoneyConverter.ConvertNumber(MaxHealth);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_canBeHit) return;

        PlayPickaxeSound();
        Vector3 hitPosition = eventData.pointerCurrentRaycast.worldPosition;
        CreateHitParticles(hitPosition);
        Hit(GameController.Instance.GetStrength());
    }

    private void CreateHitParticles(Vector3 hitPosition)
    {
        if (!_areHitParticlesOn) return;
        GameObject instantiatedParticles = Instantiate(HitParticles, _particlesSpawn);
        instantiatedParticles.transform.position = hitPosition;
    }

    private void PlayPickaxeSound()
    {
        AudioClip randomPickaxeSound = PickaxeSound[Random.Range(0, PickaxeSound.Length)];
        AudioController.Instance.PlayAudioEffect(randomPickaxeSound, PickaxeSoundVolume);
    }
}