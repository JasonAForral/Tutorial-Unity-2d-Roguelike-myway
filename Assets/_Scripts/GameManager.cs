using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    public float levelStartDelay = 2f;
    

    private Transform cameraTransform;



    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    
    void Awake ()
    {


        if (null == instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        
        cameraTransform = GameObject.FindGameObjectWithTag("Camera Pan").transform;
        cameraTransform.position = new Vector3(boardScript.columns * .5f - .5f, 0f, boardScript.rows * .5f - .5f);

        InitGame();
    }

    void InitGame ()
    {
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    public void GameOver ()
    {
        enabled = false;
    }
	
	// Update is called once per frame
    void Update ()
    {
        if (playersTurn || enemiesMoving)
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
