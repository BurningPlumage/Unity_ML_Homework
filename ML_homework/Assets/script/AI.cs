using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class AI : MonoBehaviour//Agent
{
    private Rigidbody2D rig;
    private Animator ani;

    public GameObject camara;
    public GameObject point1;
    public GameObject point2;

    public Text timecount_text;
    public Text now_score;
    public Text last_score;

    public float speed;
    private bool jump;
    private bool can_jump;
    float timecount;
    int score;


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        timecount -= Time.deltaTime;
        timecount_text.text = timecount.ToString("F2");
        now_score.text = score.ToString();


        float movex = Input.GetAxis("Horizontal") * speed;
        if (Input.GetButtonDown("Jump") && can_jump) 
        {
            rig.velocity = new Vector2(movex, 100);
            ani.SetTrigger("jump");
        }
        else
        {
            rig.velocity = new Vector2(movex, rig.velocity.y);
        }

        ani.SetFloat("move", Mathf.Abs(rig.velocity.x));
        if (rig.velocity.x > 1)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }else if (rig.velocity.x < -1)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }


        can_jump = Physics2D.OverlapArea(point1.transform.position, point2.transform.position, LayerMask.GetMask("floor"));
        ani.SetBool("stand", can_jump);
    }


    private void LateUpdate()
    {
        jump = false;
        camara.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "virus")
        {
            score += 1;
            collision.gameObject.SetActive(false);
        }
    }
}
