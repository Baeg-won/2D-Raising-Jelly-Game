using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jelly : MonoBehaviour
{
    public int id;
    public int level;
    public float exp;
    public float required_exp;
    public float max_exp;

    public GameManager game_manager;
    public GameObject left_top;
    public GameObject right_bottom;

    float pick_time;

    int move_delay;
    int move_time;

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

    void Update()
    {
        if(exp < max_exp)
            exp += Time.deltaTime;

        if (exp > required_exp * level && level < 3)
            game_manager.ChangeAc(anim, ++level);
    }

    void FixedUpdate()
    {
        if (!isWandering)
            StartCoroutine(Wander());
        if (isWalking)
            Move();

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x)
            speed_x = -speed_x;
        if (pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            speed_y = -speed_y;
    }

    void OnMouseDown()
    {
        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        if(exp < max_exp) ++exp;

        game_manager.GetJelatin(id, level);
    }

    void OnMouseDrag()
    {
        pick_time += Time.deltaTime;

        if (pick_time < 0.1f) return;

        isWalking = false;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doTouch");

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mouse_pos.x, mouse_pos.y, mouse_pos.y));

        transform.position = point;
    }

    void OnMouseUp()
    {
        pick_time = 0;

        if (game_manager.isSell) {
            game_manager.GetGold(id, level);

            Destroy(gameObject);
        }

        float pos_x = transform.position.x;
        float pos_y = transform.position.y;

        if (pos_x < left_top.transform.position.x || pos_x > right_bottom.transform.position.x ||
            pos_y > left_top.transform.position.y || pos_y < right_bottom.transform.position.y)
            transform.position = new Vector3(0, -1, 0);
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
