using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisHold : MonoBehaviour
{
    RaycastHit2D hit;
    public Transform HoldPoint;
    public bool Hold;
    public float Radius;
    public float Distance;
    public float Throw;
    private float rotate = 90;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!Hold)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.CircleCast(transform.position, Radius, Vector2.right * transform.localScale.x, Distance);
                if (hit.collider != null && hit.collider.tag == "Tetris")
                {                    
                    Hold = true;
                    FreezeTetris();
                }
            }
            else
            {
                Hold = false;
                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null) 
                {
                    AnFreezeTetris();
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * Throw;
                }
            }
        }
        if (Hold)
        {
            hit.collider.gameObject.transform.position = HoldPoint.position;
            if (HoldPoint.position.x > transform.position.x && Hold == true)
            {
                hit.collider.gameObject.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            else if (HoldPoint.position.x < transform.position.x && Hold == true)
            {
                hit.collider.gameObject.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Hold)
            {
                hit.collider.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, rotate);
                rotate += 90;
            }
        }
    }
   
    private void FreezeTetris()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void AnFreezeTetris()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
