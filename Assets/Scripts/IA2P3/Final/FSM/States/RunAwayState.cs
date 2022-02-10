using System;
using FSM;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunAwayState : MonoBaseState, IState
{

    public override event Action OnNeedsReplan;

    private PlayerController _player;

    public float lifeTimer = 2;
    float currentLifeTimer;

    private float waitTime;
    public float startWaitTime;

    private float _lastAttackTime;
    float speed = 4;
    public RunWaypoints[] moveSpots;
    private int randomSpot;
    GoapCircularEnemy gop;


    private void Awake()
    {
        gop = GetComponent<GoapCircularEnemy>();
        _player = FindObjectOfType<PlayerController>();
        waitTime = startWaitTime;
        randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
        moveSpots = FindObjectsOfType<RunWaypoints>();
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        gop.sprite.color = Color.green;
    }

    public override void UpdateLoop()
    {

        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].transform.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        if (gop.life < 3)
        {
            if (currentLifeTimer < lifeTimer)
            {

                currentLifeTimer += 1;
            }
            else
            {
                gop.life += 1;
                currentLifeTimer = 0;
            }
        }

        else if (gop.life >= 3)
        {
            currentLifeTimer = 0;

            OnNeedsReplan?.Invoke();
        }

    }

    public override IState ProcessInput()
    {
        return this;
    }
}