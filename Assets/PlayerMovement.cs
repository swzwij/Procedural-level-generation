using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    //private Animator _animator;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float groundLinearDrag;
    private bool _facingRight = true;
    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;
    private float jumpPressedRemember = 0;
    private float jumpPressedRememberTime = 0.2f;
    private float airTime;

    [Header("Hang Time")]
    [SerializeField] private float hangTime;
    private float _hangCounter;

    [Header("Particles")]
    [SerializeField] private ParticleSystem flipDust;
    [SerializeField] private ParticleSystem jumpDust;
    [SerializeField] private ParticleSystem walkDust;

    private bool _canJump => jumpPressedRemember > 0 && _hangCounter > 0.1f;

    [Header("Ground Check")]
    [SerializeField] private float groundRaycastLength;
    [SerializeField] private Vector3 groundRaycastOffset;
    private bool _onGround;
    private bool _inAir;

    bool landedChecked;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //_animator.SetFloat("JumpVelocity", _rb.velocity.y);
        //_animator.SetFloat("Speed", Mathf.Abs(_horizontalDirection));
        //_animator.SetBool("OnGround", _onGround);

        CheckLand();
        CheckAirTime();

        jumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump")) jumpPressedRemember = jumpPressedRememberTime;

        _horizontalDirection = GetInput().x;

        if (_onGround)
        {
            _hangCounter = hangTime;
        }
        else _hangCounter -= Time.deltaTime;

        if (_canJump)
        {
            //_animator.SetBool("StartJump", true);
            Jump();
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * .5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && _facingRight || Input.GetKeyDown(KeyCode.A) && _facingRight) Flip();
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !_facingRight || Input.GetKeyDown(KeyCode.D) && !_facingRight) Flip();
    }

    private void FixedUpdate()
    {
        CheckGroundCollision();
        MoveCharacter();

        if (_onGround) ApplyGroundLinearDrag();
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * movementAcceleration);
        if (Mathf.Abs(_rb.velocity.x) > maxMoveSpeed) _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * maxMoveSpeed, _rb.velocity.y);
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection) _rb.drag = groundLinearDrag;
        else _rb.drag = 0f;
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = airLinearDrag;
    }

    private void Jump()
    {
        //_animator.SetBool("Landing", false);
        //PlayJumpDust();
        jumpPressedRemember = 0;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //_animator.SetBool("StartJump", false);
    }

    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0) _rb.gravityScale = fallMultiplier;
        else if (_rb.velocity.y > 0 && !Input.GetButtonDown("Jump")) _rb.gravityScale = lowJumpFallMultiplier;
        else _rb.gravityScale = 1f;
    }

    private void Flip()
    {
        //PlayFlipDust();
        _facingRight = !_facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    private void CheckGroundCollision()
    {
        _onGround = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer) ||
                    Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLength);
    }

    private void CheckAirTime()
    {
        if (_onGround)
        {
            airTime = 0f;
        }
        else
        {
            airTime += Time.deltaTime;
        }
    }
    private void CheckLand()
    {
        if (airTime > 0 && _onGround)
        {
            //_animator.SetBool("Landing", true);
            landedChecked = true;
        }
    }

    /*private void PlayFlipDust()
    {
        if (_onGround) flipDust.Play();
    }

    private void PlayJumpDust()
    {
        jumpDust.Play();
    }

    private void PlayWalkDust()
    {
        walkDust.Play();
    }*/
}
