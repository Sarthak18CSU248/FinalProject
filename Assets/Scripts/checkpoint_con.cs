using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint_con : MonoBehaviour
{
    private PlayerController player;
    public GameObject Player ,checkpoint;
    public Sprite YellowFlag, GreenFlag;
    private SpriteRenderer checkpoint_renderer;
    public bool CheckPoint_reached = false;
    // Start is called before the first frame update
    void Start()
    {
        checkpoint_renderer = checkpoint.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         if(Vector3.Distance(Player.transform.position, checkpoint.transform.position) <= 1f)
            {
                CheckPoint_reached = true;
                checkpoint_renderer.sprite = GreenFlag;
            }
        
    }
  
}
