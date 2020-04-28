using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class AI : MonoBehaviour//Agent
{
    private Rigidbody2D rig;

    public GameObject point1;
    public GameObject point2;

    public Text timecount_text;

    private bool jump;
    private bool can_jump;
    float timecount;


    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timecount -= Time.deltaTime;
        timecount_text.text = timecount.ToString("F2");
    }

    private void FixedUpdate()
    {
        can_jump = Physics2D.OverlapArea(point1.transform.position, point2.transform.position, LayerMask.GetMask("floor"));
    }

    private void LateUpdate()
    {
        jump = false;
    }
}
