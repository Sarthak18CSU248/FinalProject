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
    public float DoubleJumpSpeed = 4.0f;
    public float wallJumpSpeed = 1.5f;
    public float wallRunAmount = 2f;
    public float slopeSlideSpeed = 4.0f;
    public float GlideAmount = 2f;
    public float Glidetimer = 2.5f;
    public float creepSpeed = 3.0f;
    public float compare = 0.0f;


    public bool isGrounded;
    public bool isJumping;
    public bool isFacingRight;
    public bool doubleJumped;
    public bool WallJumped;
    public bool isWallRunning;
    public bool isSlopeSliding;
    public bool isGliding;
    public bool isDucking;
    public bool isCreeping;
    public LayerMask layerMask;

    public bool canWallRun=true;
    public bool CanWallJump = true;
    public bool canDoubleJump = true;
    public bool canjumpafterwalljump=true;
    public bool canrunafterwalljump = true;
    public bool canGlide = true;


    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController2D _characterController;
    private bool _lastJumpWallLeft;
    private float _slopeAngles;
    private Vector3 _slopeGrad;
    private bool _startGlide;
    private float _currentGlidetimer;
    private BoxCollider2D _boxCollider;
    private Vector3 _OriginalSize;
    private Vector3 _frontCorner;
    private Vector3 _backCorner;


    // Start is called before the first frame update
    void Start()
    {
        
        _characterController = GetComponent<CharacterController2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _OriginalSize = _boxCollider.size;
        _currentGlidetimer = Glidetimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (WallJumped == false)
        {
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.x *= walkSpeed;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 2f, layerMask);
        
        if (hit)
        {
            _slopeAngles = Vector2.Angle(hit.normal, Vector2.up);
            _slopeGrad = hit.normal;
            if (_slopeAngles > _characterController.slopeLimit)
            {
                isSlopeSliding = true;
            }
            else
            {
                isSlopeSliding = false;
            }
        }
        if (isGrounded)
        {
            _currentGlidetimer = Glidetimer;
            _moveDirection.y = 0;
            isJumping = false;
            doubleJumped = false;

            if (_moveDirection.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isFacingRight = false;
            }

            else if (_moveDirection.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isFacingRight = true;
            }
            if (isSlopeSliding)
            {
                _moveDirection = new Vector3(_slopeGrad.x * slopeSlideSpeed, -_slopeGrad.y * slopeSlideSpeed, 0f);
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
                if (_moveDirection.y > 0)
                {
                    _moveDirection.y = _moveDirection.y * 0.5f;

                }
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                {
                    if (canDoubleJump)
                    {
                        if (!doubleJumped)
                        {
                            _moveDirection.y = DoubleJumpSpeed;
                            doubleJumped = true;
                        }
                    }
                }
            }

        }

        if (canGlide ==true && Input.GetAxis("Vertical") > 0.5f && _characterController.velocity.y <0.2f)
        {
            if (_currentGlidetimer > 0)
            {

                isGliding = true;
                if (_startGlide)
                {
                    _moveDirection.y = 0f;
                    _startGlide = false;
                }
                _moveDirection.y -= GlideAmount * Time.deltaTime;
                _currentGlidetimer -= Time.deltaTime;
            }
            else
            {
                isGliding = false;
                _moveDirection.y -= gravity * Time.deltaTime;
            }
        }
        else
        {
            isGliding = false;
            _startGlide = true;
            _moveDirection.y -= gravity * Time.deltaTime;
        }

        _characterController.move(_moveDirection * Time.deltaTime);
        flags = _characterController.collisionState;

        isGrounded = flags.below;

        _frontCorner = new Vector3(transform.position.x + _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0);
        _backCorner = new Vector3(transform.position.x - _boxCollider.size.x / 2, transform.position.y + _boxCollider.size.y / 2, 0);
        RaycastHit2D hitheadf = Physics2D.Raycast(_frontCorner, Vector2.up, 2f, layerMask);
        RaycastHit2D hitheadb = Physics2D.Raycast(_backCorner, Vector2.up, 2f, layerMask);
        if (Input.GetAxis("Vertical") < 0 && (_moveDirection.x > 0f || _moveDirection.x < 0f))
        {
            if (!isDucking && !isCreeping)
            {
                _boxCollider.size = new Vector2(_boxCollider.size.x, _OriginalSize.y/2);
                 transform.position = new Vector3(transform.position.x, transform.position.y - (_OriginalSize.y / 4), 0);
                _characterController.recalculateDistanceBetweenRays();
            }
            isDucking = true;
            isCreeping = false;
        }
        else if (Input.GetAxis("Vertical") < 0 && _moveDirection.x == 0f)
        {

            if (!isDucking && !isCreeping)
            {
                _boxCollider.size = new Vector2(_boxCollider.size.x, _OriginalSize.y / 2);
                 transform.position = new Vector3(transform.position.x, transform.position.y - (_OriginalSize.y / 4), 0);
                _characterController.recalculateDistanceBetweenRays();
            }
            isDucking = false;
            isCreeping = true;
        }
        else 
        {

            if (!hitheadf.collider && !hitheadb.collider && (isDucking || isCreeping))
            {
                _boxCollider.size = new Vector2(_boxCollider.size.x, _OriginalSize.y);
                transform.position = new Vector3(transform.position.x, transform.position.y + (_OriginalSize.y / 4), 0);
                _characterController.recalculateDistanceBetweenRays();
                isDucking = false;
                isCreeping = false;
            }
        }
        if(flags.above)
        {
            _moveDirection.y -= gravity * Time.deltaTime; 
        }
        if(flags.left || flags.right)
        {
            if (canWallRun)
            {
                if (Input.GetAxis("Vertical") > 0 && isWallRunning ==true )
                {
                    _moveDirection.y = jumpSpeed / wallRunAmount;
                }
                StartCoroutine(wallrunwaiter());
            }
            if (CanWallJump)
            {
                if (Input.GetButtonDown("Jump") && WallJumped ==false && isGrounded ==false)
                {
                    if (_moveDirection.x < 0)
                    {
                        _moveDirection.x = jumpSpeed * wallJumpSpeed;
                        _moveDirection.y = jumpSpeed * wallJumpSpeed;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        _lastJumpWallLeft = false;

                    }

                    else if (_moveDirection.x > 0)
                    {
                        _moveDirection.x = -jumpSpeed * wallJumpSpeed;
                        _moveDirection.y = jumpSpeed * wallJumpSpeed;
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        _lastJumpWallLeft = true;

                    }
                    StartCoroutine(wallJumpwaiter());
                }
            }
        }
        else
        {
            if (canrunafterwalljump)
            {
                StopCoroutine(wallrunwaiter());
                isWallRunning = true;
            }
        }
    }
    IEnumerator wallJumpwaiter()
    {
        WallJumped = true;
        yield return new WaitForSeconds(0.5f);
        WallJumped = false;
    }

    IEnumerator wallrunwaiter()
    {
        isWallRunning = true;
        yield return new WaitForSeconds(0.5f);
        isWallRunning = false;
    }
}
