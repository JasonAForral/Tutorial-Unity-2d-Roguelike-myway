﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

    public float restartLevelDelay = 1f;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    
    private Animator animator;
    private int food;

    private Vector2 touchOrigin = -Vector2.one;

    // Use this for initialization
    protected override void Start ()
    {
        //animator = GetComponent<Animator>();
        animator = GetComponentInChildren<Animator>();

        food = GameManager.instance.playerFoodPoints;

        UpdateFoodDisplay();

        base.Start();
    }

    private void OnDisable ()
    {
        GameManager.instance.playerFoodPoints = food;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        
        if (0 != horizontal)
            vertical = 0;

#else

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if (TouchPhase.Began == myTouch.phase)
            {
                touchOrigin = myTouch.position;
            }

            else if (TouchPhase.Ended == myTouch.phase && 0 <= touchOrigin.x)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = (x > 0 ? 1 : -1);
                else
                    vertical = (y > 0 ? 1 : -1);
            }
        }

#endif
        
        if (0 != horizontal || 0 != vertical)
        {
            AttemptMove<Wall>(horizontal, vertical);
        
        }
	}

    protected override void OnCantMove<T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");

        //throw new System.NotImplementedException();
    }

    private void OnTriggerEnter (Collider other)
    {
        switch (other.tag)
        {
            case "Exit":
                {
                    Invoke("Restart", restartLevelDelay);
                    enabled = false;
                    break;
                }
            case "Food":
                {
                    food += pointsPerFood;
                    UpdateFoodDisplay("+" + pointsPerFood + " ");
                    SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
                    other.gameObject.SetActive(false);
                    break;
                }
            case "Soda":
                {
                    food += pointsPerSoda;
                    UpdateFoodDisplay("+" + pointsPerSoda + " ");
                    SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
                    other.gameObject.SetActive(false);
                    break;
                }
        }
    }

    private void Restart ()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        UpdateFoodDisplay("-" + loss + " ");
        CheckIfGameOver();
    }

    private void UpdateFoodDisplay (string modifier = "")
    {
        foodText.text = modifier + "Food: " + food;

    }

    protected override void AttemptMove<T> (int xDir, int zDir)
    {
        
        food--;
        UpdateFoodDisplay();

        base.AttemptMove<T>(xDir, zDir);

        RaycastHit hit;

        if (Move(xDir, zDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void CheckIfGameOver ()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
