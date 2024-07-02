using System.Collections;
using System.Collections.Generic;
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

    public int TotalEscaped
    {
        get
        {
            return escapedNum;
        }
        set
        {
            escapedNum = value;
        }
    }

    public int RoundsEscaped
    {
        get
        {
            return roundsEscaped;
        }
        set
        {
            roundsEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }

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
        // Ensure moneyLbl is assigned if not done in the editor
        if (moneyLbl == null)
        {
            moneyLbl = GameObject.FindGameObjectWithTag("moneylbl").GetComponent<TextMeshProUGUI>();
            escapedNumLbl = GameObject.Find("Escaped_lbl").GetComponent<TextMeshProUGUI>();
        }

        nextWaveBtn.gameObject.SetActive(false);
        showMenu();
    }


    IEnumerator spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
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
        foreach (Enemy enemy in EnemyList)
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

    public void IsWaveOver()
    {
        escapedNumLbl.text = "Escaped " + TotalEscaped + "/10";
        if ((RoundsEscaped + TotalKilled) == totalEnemies)
        {
            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentState = gameState.gameOver;
        }
        else if (waveNum == 0 && (TotalKilled + RoundsEscaped) == 0)
        {
            currentState = gameState.play;
        }
        else if (waveNum >= totalWaves)
        {
            currentState = gameState.win;
        }
        else
        {
            currentState = gameState.next;
        }
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

    public void playBtnPressed()
    {
        switch (currentState)
        {
            case gameState.next:
                waveNum += 1;
                totalEnemies += waveNum;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                moneyLbl.text = TotalMoney.ToString();
                escapedNumLbl.text = "Escaped " + TotalEscaped + "/10";
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundsEscaped = 0;
        waveNumLbl.text = "Wave " + (waveNum + 1);
        StartCoroutine(spawn());
        nextWaveBtn.gameObject.SetActive(false);
    }
}
