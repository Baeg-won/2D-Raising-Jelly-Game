using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelly : MonoBehaviour
{
    public int move_delay;
    public int move_time;

    float speed_x;
    float speed_y;
    bool isWandering;
    bool isWalking;

    SpriteRenderer sprite;
    Animator anim;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isWandering = false;
        isWalking = false;
    }

    void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());
        if (isWalking)
            Move();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bottom") || collision.gameObject.name.Contains("Top"))
            speed_y = -speed_y;
        else if (collision.gameObject.name.Contains("Left") || collision.gameObject.name.Contains("Right"))
            speed_x = -speed_x;
    }

    void Move()
    {
        if (speed_x != 0)
            sprite.flipX = speed_x < 0;

        transform.Translate(speed_x, speed_y, speed_y);
    }

    IEnumerator Wander()
    {
        move_delay = 6;
        move_time = 3;
        speed_x = Random.Range(-0.8f, 0.8f) * Time.deltaTime;
        speed_y = Random.Range(-0.8f, 0.8f) * Time.deltaTime;

        isWandering = true;

        yield return new WaitForSeconds(move_delay);

        isWalking = true;
        anim.SetBool("isWalk", true);

        yield return new WaitForSeconds(move_time);
        
        isWalking = false;
        anim.SetBool("isWalk", false);

        isWandering = false;
    }
}
