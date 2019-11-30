using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCollision : MonoBehaviour
{
    public bool istriggered;
    public GameObject SirenEnemy, Player;
    public void DestroySiren()
    {
        if (Vector3.Distance(SirenEnemy.transform.position, Player.transform.position) <= 0.5f)
        {
            istriggered = true;
            Debug.Log("Hello");
        }
    }
   
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("Hello");
            istriggered = true;
        }
    }*/
}