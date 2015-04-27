using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public BoardManager boardScript;
    public Transform cameraTransform;

    private int level = 3;

    void Awake ()
    {
        boardScript = GetComponent<BoardManager>();
        cameraTransform.position = new Vector3(boardScript.columns * .5f - .5f, 0f, boardScript.rows * .5f - .5f);

        InitGame();
    }

    void InitGame ()
    {
        boardScript.SetupScene(level);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
