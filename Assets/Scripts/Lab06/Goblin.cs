using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Subject
{
    Rigidbody2D rb;
    Animator anim;

    private bool flipped;


    [SerializeField] private float speed = 3f;
    [SerializeField] private int maxHP = 10;
    private int currentHP;

    public float attackRange = 0.6f;
    [SerializeField] private float attackCooldwn = 2f;

    EnemyState state = EnemyState.Idle;

    List<GameObject> waypoints = new List<GameObject>();

    GameObject target;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentHP = maxHP;

        foreach (GameObject path in GameObject.FindGameObjectsWithTag("waypoint"))
        {
            waypoints.Add(path);
        }
    }

    private void Update()
    {
        anim.SetBool("move", state == EnemyState.Moving);

        if (target == null)
            return;

        if ((transform.position.x > target.transform.position.x && !flipped || transform.position.x < target.transform.position.x && flipped) && target != null)
        {
            Flip();
        }

        
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Idle:

                if (target == null)
                    target = NextPath();

                rb.MovePosition(MoveDirection());
                state = EnemyState.Moving;

                break;

            case EnemyState.Moving:

                if (target.GetComponent<PixelController>())
                {
                    if (Vector2.Distance(transform.position, target.transform.position) > attackRange)
                    {
                        rb.MovePosition(MoveDirection());
                    }

                    else
                    {
                        StartCoroutine(Attack());
                    }
                }

                else

                {
                    if (Vector2.Distance(transform.position, target.transform.position) > 0)
                    {
                        rb.MovePosition(MoveDirection());
                    }

                    else
                    {
                        rb.MovePosition(MoveDirection());
                        target = NextPath();
                    }
                }
                break;
        }
    }

    Vector2 MoveDirection()
    {
        if (target != null)
            return Vector2.MoveTowards(transform.position, target.transform.position, Time.fixedDeltaTime * speed);

        return Vector2.zero;
    }

    IEnumerator Attack()
    {
        state = EnemyState.Attacking;
        anim.SetTrigger("attack");

        yield return new WaitForSeconds(attackCooldwn);

        state = EnemyState.Idle;
        target = null;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;
        flipped = !flipped;
    }

    public void TargetPlayer()
    {
        target = PixelController.player.gameObject;
    }

    private GameObject NextPath()
    {
        int rand = Random.Range(0, waypoints.Count);
        return waypoints[rand];
    }

    private void TakeDamage()
    {
        currentHP -= 5;

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            ObjectPooler.instance.AddToPool(this.tag, gameObject);
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
        ObjectPooler.instance.AddToPool(this.tag, gameObject);

        Notify(this.gameObject, NotificationType.death);               
    }

    public void Respawn()
    {
        state = EnemyState.Idle;
        currentHP = maxHP;
        target = null;

        //Registering all present observers to this subject's list.
        foreach(Observer observer in FindObjectsOfType<Observer>())
        {
            RegisterObserver(observer);
        }

        // Notifies all other subjects to register this observer to their list.
        Notify(gameObject, NotificationType.respawn);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pAttack"))
        {
            TakeDamage();
            Notify(collision.transform.parent.gameObject, NotificationType.damaged);
            Debug.Log("hit");
        }
    }
}


