using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    public static PixelController player;

    private void Awake()
    {
        if (player == null)
            player = this;
    }


    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float speed = 10f;
    [SerializeField] private int maxHP = 10;
    private int currentHP;

    private bool flipped;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentHP = maxHP;
    }

    private void Update()
    {
        if (rb.velocity.x > 0.1f && flipped || rb.velocity.x < -0.1f && !flipped)
        {
            Flip();
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("attack");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            currentHP = 0;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
        flipped = !flipped;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("eAttack"))
        {
            TakeDamage(3);
            anim.SetTrigger("hit");
        }
    }

    private void FixedUpdate()
    {
        Vector2 move = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        rb.velocity = move * speed;
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.magnitude));
    }
}
