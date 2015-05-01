using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

    public float restartLevelDelay = 1f;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public Text foodText;
    
    private Animator animator;
    private int food;

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

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        
        if (0 != horizontal)
            vertical = 0;

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
        Debug.Log(other.tag);
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
                    other.gameObject.SetActive(false);
                    break;
                }
            case "Soda":
                {
                    food += pointsPerSoda;
                    UpdateFoodDisplay("+" + pointsPerSoda + " ");
                    other.gameObject.SetActive(false);
                    break;
                }
        }

        // if then chain
        //Debug.Log(other.tag);
        //if (other.CompareTag("Exit"))
        //{
        //    Invoke("Restart", restartLevelDelay);
        //    enabled = false;
        //}
        //else if (other.CompareTag("Food"))
        //{
        //    food += pointsPerFood;
        //    other.gameObject.SetActive(false);
        //}
        //else if (other.CompareTag("Soda"))
        //{
        //    food += pointsPerFood;
        //    other.gameObject.SetActive(false);
        //}
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
            // call sfx
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void CheckIfGameOver ()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
