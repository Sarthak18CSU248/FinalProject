using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airEnemy : MonoBehaviour
{
    public GameObject Player,Game_Over,airenemy, checkpoint1, checkpoint2;
    public PlayerController player;
    public bool istriggered=false;
    public enum airenemystate
    {
        Stop ,
        Dash,
        Move,
        Animated,
        MoveTowards

    }
    public enum collisionBehaviour
    {
        None,
        Rebound,
        Fall,
        Explode,
        Disappear
    }
    public void GameOver()
    {
        Game_Over.SetActive(true);
        Player.SetActive(false);
        airenemy.SetActive(false);
        

    }
    public airenemystate AirenemyState;
    public collisionBehaviour Collission_Behaviour;

    public bool usePhysics = true;
    private Rigidbody2D rb;
    public float thrust = 10f;
    public float rotationSpeed = 10f;
    public bool autoTargetPlayer = true;
    public Transform target;
    private bool _tracking = true;
    public LayerMask lm;
    public float dashAmount;
    public Vector3 player_coardinates = Vector3.zero;
    public Vector3 airenemy_coardinates = Vector3.zero;
    public Vector3 checkpoint_coardinates1 = Vector3.zero;
    public Vector3 checkpoint_coardinates2 = Vector3.zero;
    public bool check1;
    public bool check2;
    public Rigidbody2D air_enemy;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player_coardinates = Player.transform.position;
        airenemy_coardinates = airenemy.transform.position;
        checkpoint_coardinates1 = checkpoint1.transform.position;
        checkpoint_coardinates2 = checkpoint2.transform.position;
        air_enemy = airenemy.GetComponent<Rigidbody2D>();
        if (autoTargetPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AirenemyState.Equals(airenemystate.Stop))
        {
            rb.velocity = new Vector2(0, 0);
            rb.angularVelocity = 0f;
        }
       
        if (AirenemyState.Equals(airenemystate.Dash))
        {
            if(_tracking)
                Lookat2D(target);
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, lm);
            if (hit)
            {
                if (hit.collider.tag == "Player" && _tracking)
                {
                    StartCoroutine("Dash");
                }
            }
        }

        if (AirenemyState.Equals(airenemystate.Move))
        {
            Move(transform.right);
        }

        if (AirenemyState.Equals(airenemystate.MoveTowards))
        {
            MoveTowards(target);
        }


    }
    public void Move(Vector3 movedirection)
    {
       if(usePhysics)
        {
            rb.AddForce(movedirection * thrust);
        }
       else
        {
            rb.MovePosition(transform.position + (movedirection * thrust) * Time.deltaTime);
        }
        if (Vector3.Distance(Player.transform.position, rb.transform.position) <= 0.1f)
        {
            istriggered = true;
        
        }
        if (istriggered)
        {
            
            
                GameOver();
            
        }
        
    }
    


    public void MoveTowards(Transform target)
    {
        Lookat2D(target);
        Move(transform.right);
    }
    public void Lookat2D(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);
    }
   
    IEnumerator Dash()
    {
        _tracking = false;
        yield return new WaitForSeconds(1f);
        rb.AddForce(transform.right * dashAmount, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0f;
        _tracking = true;
    }
    IEnumerator AirEnemyWaiter()
    {
        air_enemy.simulated = false;
        airenemy_coardinates = new Vector3(41.5f, 14.1f, airenemy_coardinates.z);
        yield return new WaitForSeconds(0.3f);
        air_enemy.simulated = true;


    }

}
