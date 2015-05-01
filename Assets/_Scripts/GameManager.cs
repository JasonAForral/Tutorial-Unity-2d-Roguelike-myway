using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;


    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private Transform cameraTransform;

    void Awake ()
    {


        if (null == instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        

        InitGame();
    }

    private void OnLevelWasLoaded (int index)
    {
        level++;

        InitGame();
    }

    void InitGame ()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("Camera Pan").transform;
        cameraTransform.position = new Vector3(boardScript.columns * .5f - .5f, 0f, boardScript.rows * .5f - .5f);

        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = levelImage.GetComponentInChildren<Text>();
        levelText.text = System.String.Concat("Day " + level);
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage ()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver ()
    {
        levelText.text = System.String.Concat("After ", level, " days, you starved.");
        levelImage.SetActive(true);

        enabled = false;
    }
	
	// Update is called once per frame
    void Update ()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        return;

        StartCoroutine(MoveEnemeis());
    }

    public void AddEnemyToList (Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemeis ()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        if (0 == enemies.Count)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}
