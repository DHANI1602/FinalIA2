using System;
using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MonoBaseState, IState
{

    PlayerController _player;
    public float speed;
    private float waitTime;
    public float startWaitTime;

    public Waypoints[] moveSpots;
    private int randomSpot;
    GoapCircularEnemy gop;

    private void Awake() {
        gop = GetComponent<GoapCircularEnemy>();
        _player = FindObjectOfType<PlayerController>();
        waitTime = startWaitTime;
        randomSpot = UnityEngine.Random.Range(0, moveSpots.Length);
        moveSpots = FindObjectsOfType<Waypoints>();
      
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        gop.sprite.color = Color.white;
    }
    public override void UpdateLoop() {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].transform.position, speed * Time.deltaTime);
        if(Vector2.Distance(transform.position, moveSpots[randomSpot].transform.position) < 0.2f)
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
    }

    public override IState ProcessInput() {
      var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance < 17f) {
           // gop.sprite.color = Color.red;
            return Transitions["OnChaseState"];
        }


        return this;
    }
}
