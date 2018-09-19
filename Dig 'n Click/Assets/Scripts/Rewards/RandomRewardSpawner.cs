using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRewardSpawner : MonoBehaviour
{
    public GameObject Candy;
    public float SpawnChance;
    public float SpawnInterval;
    public AudioClip CandySound;
    public float CandySoundVolume;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCandies());
    }

    private IEnumerator SpawnCandies()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);
            if (Random.value < SpawnChance)
                SpawnCandyAtRandomPosition();
        }
    }

    private void SpawnCandyAtRandomPosition()
    {
        GameObject candygGameObject = Instantiate(Candy, _rectTransform);
        candygGameObject.transform.position = GetRandomPosition();
        candygGameObject.transform.rotation = GetRandomRotation();
        AudioController.Instance.PlayAudioEffect(CandySound, CandySoundVolume);
    }

    private Vector3 GetRandomPosition()
    {
        float spawnWidth = _rectTransform.rect.width;
        float spawnHeight = _rectTransform.rect.height;
        float targetX = _rectTransform.position.x + Random.Range(-spawnWidth / 2, spawnWidth / 2);
        float targetY = _rectTransform.position.y + Random.Range(-spawnHeight / 2, spawnHeight / 2);
        return new Vector3(targetX, targetY, 0);
    }

    private Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, 0, Random.rotation.eulerAngles.z);
    }
}