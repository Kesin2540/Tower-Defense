using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum gameState
{
    next, play, gameOver, win
}
public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private int totalWaves = 10;
    [SerializeField] private TextMeshProUGUI waveNumLbl;
    [SerializeField] private TextMeshProUGUI moneyLbl;
    [SerializeField] private TextMeshProUGUI escapedNumLbl;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int maxEnemiesOnScreen;
    [SerializeField] private int totalEnemies;
    [SerializeField] private int enemiesPerSpawn;
    [SerializeField] private TextMeshProUGUI nextWaveBtnLbl;
    [SerializeField] private Button nextWaveBtn;

    private int waveNum = 0;
    private int money = 10;
    private int escapedNum = 0;
    private int roundsEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemyToSpawn = 0;

    private gameState currentState = gameState.play;

    const float spawnDelay = 0.5f;

    public int TotalMoney
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            moneyLbl.text = money.ToString();
        }
    }
    public List<Enemy> EnemyList = new List<Enemy>();

    void Start()
    {
        nextWaveBtn.gameObject.SetActive(false);
        showMenu();
    }

    private void Update()
    {
        handleEscape();
    }

    IEnumerator spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    EnemyList.Add(newEnemy.GetComponent<Enemy>());

                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubMoney(int amount) 
    { 
        TotalMoney -= amount; 
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameState.gameOver:
                nextWaveBtnLbl.text = "Play Again!";
                break;
            case gameState.next:
                nextWaveBtnLbl.text = "Next Wave";
                break;
            case gameState.play:
                nextWaveBtnLbl.text = "Play";
                break;
            case gameState.win:
                nextWaveBtnLbl.text = "Play";
                break;
        }
        nextWaveBtn.gameObject.SetActive(true);
    }

    public void handleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDragSprite();
            TowerManager.Instance.TowerbtnPressed = null;
        }
    }
}
