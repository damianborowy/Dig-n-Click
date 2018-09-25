using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RandomRewardSpawner : MonoBehaviour
{
    public GameObject Candy;
    public GameObject AdCandy;
    public float SpawnTimeLowerBound;
    public float SpawnTimeUpperBound;
    public float AdvertisementChance;

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
            yield return new WaitForSeconds(Random.Range(SpawnTimeLowerBound, SpawnTimeUpperBound));
            SpawnCandy();
        }
    }

    private void SpawnCandy()
    {
        GameObject candyToSpawn = PickCandyToSpawn();
        SpawnCandyAtRandomPosition(candyToSpawn);
    }

    private GameObject PickCandyToSpawn()
    {
        return ShouldAdBeShown() ? AdCandy : Candy;
    }

    private bool ShouldAdBeShown()
    {
        return Random.value < AdvertisementChance && Advertisement.IsReady();
    }

    private void SpawnCandyAtRandomPosition(GameObject candyToSpawn)
    {
        GameObject candygGameObject = Instantiate(candyToSpawn, _rectTransform);
        candygGameObject.transform.position = GetRandomPosition();
        candygGameObject.transform.rotation = GetRandomRotation();
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