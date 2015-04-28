using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    private Transform cameraTransform;

    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private int level = 3;

    void Awake ()
    {


        if (null == instance)
            instance = this;
        else if (null != this)
            Destroy(gameObject);

        boardScript = GetComponent<BoardManager>();
        cameraTransform = GameObject.FindGameObjectWithTag("Camera Pan").transform;
        cameraTransform.position = new Vector3(boardScript.columns * .5f - .5f, 0f, boardScript.rows * .5f - .5f);

        InitGame();
    }

    void InitGame ()
    {
        boardScript.SetupScene(level);
    }

    public void GameOver ()
    {
        enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
