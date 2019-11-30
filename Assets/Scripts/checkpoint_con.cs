using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint_con : MonoBehaviour
{
    private PlayerController player;
    public bool Checkpoint_Reached;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player.isGrounded==true && collision.tag == "Player")
        {
            Debug.Log("Hello");
            Checkpoint_Reached = true;

        }
    }
}
