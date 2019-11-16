using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class PlayerController : MonoBehaviour
{
    public CharacterController2D.CharacterCollisionState2D flags;
    public float walkSpeed = 6.0f;
    public float jumpSpeed =9.0f;
    public float gravity = 20.0f;
    public bool isGrounded;
    public bool isJumping;
    public bool isFacingRight;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController2D _characterController;


    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.x *= walkSpeed;

        if (isGrounded)
        {
            _moveDirection.y = 0;
            isJumping = false;

            if (_moveDirection.x < 0 )
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isFacingRight = false;
            }

            else if (_moveDirection.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFacingRight = true;
            }
            if (Input.GetButtonDown("Jump"))
            {

                _moveDirection.y = jumpSpeed;
                isJumping = true;
            }
        }
        else
        {
            if (Input.GetButtonUp("Jump"))
            {
                if (_moveDirection.y >0)
                {
                    _moveDirection.y = _moveDirection.y * 0.5f;
                }
            }
        }

        _moveDirection.y -= gravity * Time.deltaTime;
        _characterController.move(_moveDirection * Time.deltaTime);
        flags = _characterController.collisionState;

        isGrounded = flags.below;
        if(flags.above)
        {
            _moveDirection.y -= gravity * Time.deltaTime; 
        }
    }
}
