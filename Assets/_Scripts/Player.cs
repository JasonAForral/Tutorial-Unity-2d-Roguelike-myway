using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;



	// Use this for initialization
    protected override void Start ()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable ()
    {
        GameManager.instance.playerFoodPoints = food;
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.playersTurn) 
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (0 != horizontal)
            vertical = 0;

        if (0 != horizontal || 0 != vertical)
            AttemptMove<Wall>(horizontal, vertical);
	}

    protected override void OnCantMove<T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");

        throw new System.NotImplementedException();
    }

    private void onTriggerEnter (Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.CompareTag("Food"))
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Soda"))
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
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
        CheckIfGameOver();
    }

    protected override void AttemptMove<T> (int xDir, int zDir)
    {
        food--;
        base.AttemptMove<T>(xDir, zDir);

        RaycastHit hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void CheckIfGameOver ()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
