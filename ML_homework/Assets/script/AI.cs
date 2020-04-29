using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class AI :/*MonoBehaviour*/ Agent
{
    private Rigidbody2D rig;
    private Animator ani;

    public GameObject camara;
    public GameObject point1;
    public GameObject point2;

    public GameObject[] virus;

    public Text timecount_text;
    public Text now_score;
    public Text last_score;

    public float speed;
    private bool jump;
    private bool can_jump;
    float timecount;
    int number;


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        timecount -= Time.deltaTime;
        timecount_text.text = timecount.ToString("F2");
        now_score.text = number.ToString();

        /*
        float movex = Input.GetAxis("Horizontal") * speed;
        if (Input.GetButtonDown("Jump") && can_jump) 
        {
            rig.velocity = new Vector2(movex, 100);
            ani.SetTrigger("jump");
        }
        else
        {
            rig.velocity = new Vector2(movex, rig.velocity.y);
        }*/

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
        //jump = false;
        camara.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "virus")
        {
            number += 1;
            collision.gameObject.SetActive(false);
        }
    }

    //AI
    
    public override void OnEpisodeBegin()
    {
        for(int i = 0; i < virus.Length; i++)
        {
            virus[i].transform.position = new Vector3(Random.Range(-38.0f, 38.0f), Random.Range(1.0f, 39f), 0);
            virus[i].SetActive(true);
        }

        transform.position = new Vector3(0, 1, 0);

        timecount = 120;
        number = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(new Vector2(transform.position.x, transform.position.y));
        sensor.AddObservation(rig.velocity);
        sensor.AddObservation(can_jump);
        sensor.AddObservation(jump);

        for(int i = 0; i < virus.Length; i++)
        {
            sensor.AddObservation(new Vector2(virus[i].transform.position.x, virus[i].transform.position.y));
        }

        sensor.AddObservation(timecount);
        sensor.AddObservation(number);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float movex = vectorAction[0] * speed;
        if (vectorAction[1] > 0) jump = true;

        if (jump && can_jump)
        {
            rig.velocity = new Vector2(movex, 100);
            ani.SetTrigger("jump");
        }
        else
        {
            rig.velocity = new Vector2(movex, rig.velocity.y);
        }

        Invoke("closejump", 0.01f);

        //rndturn
        if (timecount <= 0)
        {
            float scorepoint = -(5 - number);
            SetReward(scorepoint);
            last_score.text = scorepoint.ToString();
            EndEpisode();
        }
        if (number >= 5)
        {
            float scorepoint;
            if (timecount > 100)
            {
                scorepoint = 5;
            }
            else
            {
                scorepoint = timecount / 20;
            }
            SetReward(scorepoint);
            last_score.text = scorepoint.ToString();
            EndEpisode();
        }
        
    }
    
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        if (Input.GetButton("Jump"))
        {
            actionsOut[1] = 1;
        }
        else
        {
            actionsOut[1] = -1;
        }
    }

    public void closejump()
    {
        jump = false;
    }
}
