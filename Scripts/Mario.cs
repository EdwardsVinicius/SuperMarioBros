using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public GameObject mario;
    Rigidbody2D rb;
    Animator animator;
    public float speed = 1;
    bool jumping = false;
    bool facingRight = true;

    public float jumpVelocity = 3f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        
        float horizontal = Input.GetAxisRaw("Horizontal") * speed;

        position.x += horizontal * Time.deltaTime * speed;
        transform.position = position;


        animator.SetFloat("speed", Mathf.Abs(horizontal));
        jump();
        
        if (horizontal > 0 && !facingRight)
            flip();
        else if (horizontal < 0 && facingRight)
            flip();
    }

    void flip() {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;

        scale.x *= -1;
        transform.localScale = scale;
    }

    void jump() {
        if (Input.GetKey(KeyCode.Space)) {
            if (jumping) return;

            animator.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpVelocity;

            if (rb.velocity.y < 0){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void onLanding() {
        animator.SetBool("isJumping", false);
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        jumping = false;
        onLanding();     
    }

    private void OnCollisionExit2D(Collision2D collision) {
        jumping = true;
    }
}
