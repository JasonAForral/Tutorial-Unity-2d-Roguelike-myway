using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    private void OnTriggerEnter (Collider other)
    {
        Debug.Log("anything");
        Debug.Log(other.tag);
        //if (other.CompareTag("Exit"))
        //{
        //    Invoke("Restart", restartLevelDelay);
        //    enabled = false;
        //}
        //else if (other.CompareTag("Food"))
        //{
        //    food += pointsPerFood;
        //    gameObject.SetActive(false);
        //}
        //else if (other.CompareTag("Soda"))
        //{
        //    food += pointsPerFood;
        //    gameObject.SetActive(false);
        //}
        
    }

    private void OnCollisionEnter (Collision col)
    {
        
    }
}
