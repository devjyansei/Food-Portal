using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    int score = 0;
    float nextThrowTime = 0f;

    GameObject lastSpawnedItemPrefab;
    GameObject nextItemPrefab;
    GameObject indicatorPrefab;

    [SerializeField] List<itemSO> itemSOList = new List<itemSO>();
    [SerializeField] List<Item> allItemsList = new List<Item>();
    [SerializeField] GameObject[] spawnableItemsList;
    [SerializeField] GameObject[] indicatorsList;
    [SerializeField] SpawnerController spawnerController;
    [SerializeField] ParticleSystem portalParticle;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverPanel;


    private PlayerStates playerState;
    public enum PlayerStates
    {
        canThrow,
        waiting

    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    
    void Start()
    {
        playerState = PlayerStates.canThrow;
        SpawnAndSetFirstItemRandom();
        SetNextItemRandom();
        score = 0;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) &&  Time.time > nextThrowTime +0.1f && playerState == PlayerStates.canThrow)
        {
            StartCoroutine("ThrowDelayer");
        }
    }
    IEnumerator ThrowDelayer()
    {
        playerState = PlayerStates.waiting;
        ThrowCurrentItem();
        yield return new WaitForSeconds(spawnerController.GetThrowRate());
        SpawnNextItem();
        SetNextItemRandom();
        playerState = PlayerStates.canThrow;
    }
    public void Explode(Item item1,Item item2,Vector3 collisionPoint)
    {

        if (item1.GetIsExploded() == false && item2.GetIsExploded() == false)
        {

            if (item1.itemSO.level >= itemSOList.Count)
            {
                return;
                
            }
            int gainedPoint = item1.itemSO.point * item2.itemSO.point;

            score += gainedPoint;
            UpdateScore();

            GameObject higherLevelItem = allItemsList[item1.itemSO.level].itemSO.prefab;

            
            Destroy(item1.gameObject);
            Destroy(item2.gameObject);

            if (item1.itemSO.level != allItemsList.Count)
            {
                GameObject createdItem = Instantiate(higherLevelItem, collisionPoint, transform.rotation);
                createdItem.GetComponent<Item>().SetCreatedByExplosion(true);

                item1.SetIsExploded(true);
                item2.SetIsExploded(true);

            }
                              
        }
        
    }

    public void SetNextItemRandom()
    {  
        nextItemPrefab = spawnableItemsList[Random.Range(0, spawnableItemsList.Length)];
        UpdateIndicatorPrefab();
    }

    public void UpdateIndicatorPrefab()
    {
        if (indicatorPrefab != null)
        {
            Destroy(indicatorPrefab);     
        }
        indicatorPrefab = Instantiate(indicatorsList[nextItemPrefab.GetComponent<Item>().itemSO.level - 1]);
        
    }
    private void SpawnAndSetFirstItemRandom()
    {
        lastSpawnedItemPrefab = Instantiate(spawnableItemsList[Random.Range(0,spawnableItemsList.Length)], spawnerController.GetItemSpawnPoint().position, transform.rotation);
        ChangePortalsColorAccordingToItem(lastSpawnedItemPrefab.GetComponent<Item>());        
       
        AttachItemToSpawner();
    }
    private void SpawnNextItem()
    {
        lastSpawnedItemPrefab = Instantiate(nextItemPrefab, spawnerController.GetItemSpawnPoint().position, transform.rotation);
        ChangePortalsColorAccordingToItem(lastSpawnedItemPrefab.GetComponent<Item>());
        
        AttachItemToSpawner();
    }
    private void ThrowCurrentItem()
    {
        Item lastSpawnedItem = lastSpawnedItemPrefab.GetComponent<Item>();
        lastSpawnedItem.OpenGravity();
        lastSpawnedItemPrefab.transform.SetParent(null);
        lastSpawnedItem.SetIsThrowed(true);
        nextThrowTime = Time.time + spawnerController.GetThrowRate();
    }
    private void AttachItemToSpawner()
    {
        lastSpawnedItemPrefab.transform.SetParent(spawnerController.GetItemSpawnPoint());
    }


    private void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    private void ChangePortalsColorAccordingToItem(Item item)
    {
        ParticleSystem.MainModule particleMain = portalParticle.main;
        ParticleSystem[] childrenParticles = portalParticle.gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in childrenParticles)
        {
            var main = particle.main;
            main.startColor = item.itemSO.color;
        }
        particleMain.startColor = item.itemSO.color;
        
    }
    
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    
}
