using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airEnemy : MonoBehaviour
{
    public GameObject Player,Game_Over;
    
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

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
   
}
