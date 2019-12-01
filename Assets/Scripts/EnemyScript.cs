using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class EnemyScript : MonoBehaviour
{
    public enum GroundState
    {
        Stop,
        MoveForward,
        Jump,
        Patrol,
        Dash
    }
    public void GameOver()
    {
        Game_Over.SetActive(true);


    }
    public GroundState groundstate;

    public GameObject Player, rb,Game_Over;
    
    public bool isGrounded;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController2D _characterController;
    public CharacterController2D.CharacterCollisionState2D flags;
    public float moveSpeed=3f;
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    private bool isFacingLeft = false;
    public bool aTurn = true;
    public bool jumpforward = true;
    public bool istriggered = true;
    public bool jumpandWait = true;
   

    // Start is called before the first frame update
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            if (groundstate.Equals(GroundState.Stop))
            {
                _moveDirection = Vector3.zero;
            }
            else if (groundstate.Equals(GroundState.MoveForward))
            {
                if(aTurn)
                {
                    if (flags.left && isFacingLeft)
                    {
                        Turn();
                    }
                    else if (flags.right && !isFacingLeft)
                    {
                        Turn();
                    }
                }
                if (isFacingLeft)
                {
                    _moveDirection.x = -moveSpeed;
                }
                else 
                {
                    _moveDirection.x = moveSpeed;
                }
                
                if (Vector3.Distance(Player.transform.position, rb.transform.position) <= 0.1f)
                {
                    istriggered = true;
                    GameOver();


                }
            }
            else if (groundstate.Equals(GroundState.Dash))
            {

            }
            else if (groundstate.Equals(GroundState.Jump))
            {
                
                _moveDirection.y = jumpSpeed;
                if(jumpforward && isFacingLeft)
                {
                    _moveDirection.x = -moveSpeed;
                }
                else if(jumpforward && !isFacingLeft)
                {
                    _moveDirection.x = moveSpeed;
                }
                if (jumpandWait)
                {
                    StartCoroutine(JumpAndWait());
                }
                if (Vector3.Distance(Player.transform.position, rb.transform.position) <= 0.1f)
                {
                    istriggered = true;
                    GameOver();



                }
            }

        }
        _moveDirection.y -= gravity * Time.deltaTime;
        _characterController.move(_moveDirection * Time.deltaTime);
        flags = _characterController.collisionState;

        isGrounded = flags.below;
    }
    public void Turn()
    {
        if (isFacingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingLeft = false;
        }
        else if (!isFacingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingLeft = true;

        }

    }
    IEnumerator JumpAndWait()
    {
        groundstate = GroundState.Stop;
        groundstate = GroundState.MoveForward;
        yield return new WaitForSeconds(1.5f);
        groundstate = GroundState.Jump;

    }
}
