using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator Anim;
    public Transform GroundCheck;
    public LayerMask Ground;
    private Vector2 moveVector;
    private TetrisHold tetrisHold;


    public bool TakeTetris = false;
    public bool OnGround;
    private bool jumpControl;
    private bool faceRight = true;

    public float jumpForce = 160f;
    private float jumpTime = 0;
    private float jumpControlTime = 0.7f;
    private float GroundCheckRadius;

    private int speed = 3;

    void Start()
    {
        tetrisHold = GetComponent<TetrisHold>();
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
    }

    public void Walk()
    {
        Anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    public void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (OnGround) { jumpControl = true; }
        }
        else { jumpControl = false; }
        if (jumpControl)
        {
            if ((jumpTime += Time.fixedDeltaTime) < jumpControlTime)
            {
                rb.AddForce(Vector2.up * jumpForce / (jumpTime * 10));
            }
        }
        else { jumpTime = 0; }
    }

    public void CheckingGround()
    {
        OnGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        Anim.SetBool("onGround", OnGround);
    }

    public void HoldTetris()
    {
        Anim.SetBool("takeTetris", tetrisHold.Hold);
    }
}
