using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using Global;

public class PlayerController : MonoBehaviour
{
    public GameObject player,checkpoint1,checkpoint2,greenflag1,greenflag2,speedcoin,speedcoin1,speedcoin2,speedcoin3;
    public enum GroundType
    {
        none,
        LevelGeometry,
        OneWayPlatform,
        MovingPlatform,
        CollapsingPlatform
    }
    public airEnemy air_enemy;
    

    public CharacterController2D.CharacterCollisionState2D flags;
    public CollapsingPlatform collapse;
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
    private Vector3 _frontBottomCorner;
    private Vector3 _backBottomCorner;
    private Animator _animator;
    private bool _abletoWallRun;
    public GroundType _groundType;
    private GameObject _tempOneWayPlatform;
    private GameObject _tempMovingPlatform;
    private EffectorType _currentEffectorType = EffectorType.None;
    private Vector3 _currenteffectorAdjustment = Vector3.zero;
    public Vector3 checkpoint_coardinates1 = Vector3.zero;
    public Vector3 checkpoint_coardinates2 = Vector3.zero;
    public Vector3 speedcoin_coardinates1 = Vector3.zero;
    public Vector3 speedcoin_coardinates2 = Vector3.zero;
    public Vector3 speedcoin_coardinates3 = Vector3.zero;
    public Vector3 speedcoin_coardinates4 = Vector3.zero;

    public bool check1, check2; 

    void Start()
    {
        
        _characterController = GetComponent<CharacterController2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _OriginalSize = _boxCollider.size;
        _currentGlidetimer = Glidetimer;
        _animator = GetComponentInChildren<Animator>();
        checkpoint_coardinates1 = checkpoint1.transform.position;
        checkpoint_coardinates2 = checkpoint2.transform.position;
        speedcoin_coardinates1 = speedcoin.transform.position;
        speedcoin_coardinates2 = speedcoin1.transform.position;
        speedcoin_coardinates3 = speedcoin2.transform.position;
        speedcoin_coardinates4 = speedcoin3.transform.position;
    }


    void Update()
    {
        
        if (WallJumped == false)
        {
            _moveDirection.x = Input.GetAxis("Horizontal");
            if (isCreeping)
            {
                _moveDirection.x *= creepSpeed;
            }
            else
            {


                _moveDirection.x *= walkSpeed;
            }
        }

        GetGroundType();

       
        if (isGrounded)
        {
            float EPSILON = 0.5f;
            if (System.Math.Abs(player.transform.position.x - checkpoint_coardinates1.x) <= EPSILON)
            {
                checkpoint1.SetActive(false);
                greenflag1.SetActive(true);
                check1 = true;
            }
            if (System.Math.Abs(player.transform.position.x - checkpoint_coardinates2.x) <= EPSILON)
            {
                checkpoint2.SetActive(false);
                greenflag2.SetActive(true);
                check2 = true;
            }
            if (air_enemy.istriggered == true )
            {
                if (check1 == true && check2 == false)
                {
                   _moveDirection.x = checkpoint_coardinates1.x;
                   _moveDirection.y = checkpoint_coardinates1.y;
                   _moveDirection.z = 0;


                }
            }
            float a = 0.5f;
            if (System.Math.Abs(player.transform.position.x - speedcoin_coardinates1.x) <= a)
            {
                speedcoin.SetActive(false);
                StartCoroutine(Walking());
                
            }
            if (System.Math.Abs(player.transform.position.x - speedcoin_coardinates2.x) <= a)
            {
                speedcoin1.SetActive(false);
                StartCoroutine(Walking());

            }
            if (System.Math.Abs(player.transform.position.x - speedcoin_coardinates3.x) <= a)
            {
                speedcoin2.SetActive(false);
                StartCoroutine(Walking());

            }
            if (System.Math.Abs(player.transform.position.x - speedcoin_coardinates4.x) <= a)
            {
                speedcoin3.SetActive(false);
                StartCoroutine(Walking());

            }

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
                if(isDucking && _groundType.Equals(GroundType.OneWayPlatform))
                {
                    StartCoroutine(DisableOneWayPlatform());
                }
                _moveDirection.y = jumpSpeed;
                isJumping = true;
                _abletoWallRun = true;
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
                        if (!isGrounded && _groundType.Equals(GroundType.MovingPlatform))
                        {
                            doubleJumped = false;
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

        if(!_currentEffectorType.Equals(EffectorType.None))
        {
            EffectorAdjustment();
        }

        _characterController.move(_moveDirection * Time.deltaTime);
        flags = _characterController.collisionState;

       

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
            isDucking = false;
            isCreeping = true;
        }
        else if (Input.GetAxis("Vertical") < 0 && _moveDirection.x == 0f)
        {

            if (!isDucking && !isCreeping)
            {
                _boxCollider.size = new Vector2(_boxCollider.size.x, _OriginalSize.y / 2);
                 transform.position = new Vector3(transform.position.x, transform.position.y - (_OriginalSize.y / 4), 0);
                _characterController.recalculateDistanceBetweenRays();
            }
            isDucking = true;
            isCreeping = false;
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
                if (Input.GetAxis("Vertical") > 0 && _abletoWallRun ==true && isGrounded==false)
                {
                    _moveDirection.y = jumpSpeed / wallRunAmount;
                    StartCoroutine(wallrunwaiter());
                }
                
                if (flags.left)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }

                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
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

                    if (canrunafterwalljump==false)
                    {
                        _abletoWallRun = false;
                    }
                }
            }
        }
        else
        {
            if (canrunafterwalljump)
            {
                StopCoroutine(wallrunwaiter());
                _abletoWallRun = true;
                isWallRunning = false;
            }

        }

        isGrounded = flags.below;
        if(isGrounded)
        {
            GetGroundType();
        }
        else if(!isGrounded && !flags.wasGroundedLastFrame)
        {
            ClearGroundType();
        }
        updateanimator();
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
        if (WallJumped == false)
        {
            _abletoWallRun = false;
        }
    }
    IEnumerator DisableOneWayPlatform()
    {
        if(_tempOneWayPlatform)
        {
            _tempOneWayPlatform.GetComponent<EdgeCollider2D>().enabled = false;
            yield return new WaitForSeconds(1f);
            
        }
        if (_tempOneWayPlatform)
        {
            _tempOneWayPlatform.GetComponent<EdgeCollider2D>().enabled = true;
            _tempOneWayPlatform = null;

        }
    }
    IEnumerator Walking()
    {
        walkSpeed = (walkSpeed * 1.5f);
        yield return new WaitForSeconds(4f);
        walkSpeed = 6.0f; 
    }
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Triggered");
        Effectors effector = collider.gameObject.GetComponent<Effectors>();
        if(effector)
        {
            _currentEffectorType = effector.effectortype;
            _currenteffectorAdjustment = effector.effectorAdjustment;
        }
    }
    
    private void GetGroundType()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 2f, layerMask);
        if (!hit)
        {
            _frontBottomCorner = new Vector3(transform.position.x + _boxCollider.size.x / 2, transform.position.y, 0);

            _backBottomCorner = new Vector3(transform.position.x - _boxCollider.size.x / 2, transform.position.y, 0);
            RaycastHit2D hitFrontFloor = Physics2D.Raycast(_frontBottomCorner, -Vector2.up, 2f, layerMask);
            RaycastHit2D hitBackFloor = Physics2D.Raycast(_backBottomCorner, -Vector2.up, 2f, layerMask);

            if (!hitFrontFloor.collider && hitBackFloor.collider)
            {
                hit = hitBackFloor;
            }
            else if (hitFrontFloor.collider && !hitBackFloor.collider)
            {
                hit = hitFrontFloor;
            }
        }

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
            string layerName = LayerMask.LayerToName(hit.transform.gameObject.layer);
            if (layerName == "OneWayPlatform")
            {
                _groundType = GroundType.OneWayPlatform;
                if (!_tempOneWayPlatform)
                {
                    _tempOneWayPlatform = hit.transform.gameObject;
                }
            }
            else if (layerName == "MovingPlatform")
            {
                _groundType = GroundType.MovingPlatform;
                if (!_tempMovingPlatform)
                {
                    Debug.Log("Hi");
                    _tempMovingPlatform = hit.transform.gameObject;
                    transform.SetParent(hit.transform);
                }
            }
            else if (layerName == "LevelGeometry")
            {
                _groundType = GroundType.LevelGeometry;
            }
            else if (layerName == "CollapsingPlatform")
            {
                _groundType = GroundType.CollapsingPlatform;
                hit.transform.gameObject.GetComponent<CollapsingPlatform>().CollapsePlatform();
                
                
            }
            
        }
        
    }

    private void ClearGroundType()
    {
        _groundType = GroundType.none;
        if(_tempMovingPlatform)
        {
            _tempMovingPlatform = null;
        }
        transform.SetParent(null);
    }
    private void EffectorAdjustment()
    {
        if (_currentEffectorType.Equals(EffectorType.TractorBeam))
        {
            _moveDirection.y = _currenteffectorAdjustment.y;
        }
        else if (_currentEffectorType.Equals(EffectorType.FloatZone))
        {
            _moveDirection += _currenteffectorAdjustment;
        }
        else if (_currentEffectorType.Equals(EffectorType.Ladder))
        {
            _moveDirection.y = 0;
            if(Input.GetAxis("Vertical")>0)
            {
                _moveDirection.y = _currenteffectorAdjustment.y;
            }
            else if (Input.GetAxis("Vertical") < 0 )
            {
                _moveDirection.y = -_currenteffectorAdjustment.y;
            }
        }
    }
    private void updateanimator()
    {
        _animator.SetFloat("MovementX", Mathf.Abs(_moveDirection.x));
        _animator.SetFloat("MovementY", _moveDirection.y);
        _animator.SetBool("isGrounded", isGrounded);
        _animator.SetBool("isJumping", isJumping);
        _animator.SetBool("isWallJumped", WallJumped);
        _animator.SetBool("isWallRunning",isWallRunning);
        _animator.SetBool("isGliding", isGliding);
        _animator.SetBool("isDucking", isDucking);
        _animator.SetBool("isCreeping", isCreeping);
        _animator.SetBool("isSlopeSliding", isSlopeSliding);
    }
    
}
