using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class GoapCircularEnemy : MonoBehaviour
{

    public PatrolState patrolState;
    public ChaseState chaseState;
    public RunAwayState runAwayState;
    public RangeAttackState rangeAttackState;
    public RechargeWeaponState rechargeState;

    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;
    PlayerController _player;
    public GameObject fire;

    public int life = 3;
    public int ammo = 3;
    public SpriteRenderer sprite;

    Action<IEnumerable<GOAPAction>> goapToPlan;

    private void Awake()
    {
        goapToPlan += CallConfigure;
        _player = FindObjectOfType<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        rangeAttackState.OnNeedsReplan += OnReplan;
        runAwayState.OnNeedsReplan += OnReplan;

        //OnlyPlan();
        PlanAndExecute();
    }

    private void OnlyPlan()
    {


        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Pre("Healthy", true)
                                                 .Effect("isPlayerInSight", false)
                                                 .Effect("isPlayerInRange", true),

                                              new GOAPAction("RunAway")
                                                 .Pre("Healthy",   false)
                                                 .Pre("isPlayerInRange", true)
                                                 .Effect("Healthy", true)
                                                 .Effect("isPlayerInRange", false)
                                                 .Cost(2f),


                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", true)
                                                 .Pre("hasAmmo",  true)
                                                 .Effect("isPlayerAlive", false),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", false)
                                                 .Effect("hasAmmo", true)
                                          };
        var from = new GOAPState();
        from.values["isPlayerInSight"] = (_player.transform.position - transform.position).sqrMagnitude < 15f;  
        from.values["isPlayerInRange"] = (_player.transform.position - transform.position).sqrMagnitude < chaseState.rangeDistance; 
        from.values["isPlayerAlive"] = true;
        from.values["hasAmmo"] = ammo > 0;
        from.values["Healthy"] = life > 2;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["Healthy"] = true;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                               new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Pre("Healthy", true)
                                                 .Effect("isPlayerInRange", true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", false)
                                                 .Effect("hasAmmo", true)
                                                 .LinkedState(rechargeState),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", true)
                                                 .Pre("hasAmmo",  true)
                                                 .Effect("isPlayerAlive", false)
                                                 .Effect("hasAmmo", false)
                                                 .LinkedState(rangeAttackState),

                                               new GOAPAction("RunAway")
                                                 .Pre("Healthy",   false)
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("Healthy", true)
                                                 .Effect("isPlayerInRange", false)
                                                 .Effect("isPlayerInSight", false)
                                                 .LinkedState(runAwayState)


                                          };

        var from = new GOAPState();
        from.values["isPlayerInSight"] = (_player.transform.position - transform.position).sqrMagnitude < 17f;
        from.values["isPlayerInRange"] = (_player.transform.position - transform.position).sqrMagnitude < chaseState.rangeDistance;
        from.values["isPlayerAlive"] = true;
        from.values["hasAmmo"] = ammo > 0;
        from.values["Healthy"] = life > 2;


        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["Healthy"] = true;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);


    }

    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        var actions = new List<GOAPAction>{
                                               new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Pre("Healthy", true)
                                                 .Effect("isPlayerInRange", true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", false)
                                                 .Effect("hasAmmo", true)
                                                 .LinkedState(rechargeState),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", true)
                                                 .Pre("hasAmmo",  true)
                                                 .Effect("isPlayerAlive", false)
                                                 .Effect("hasAmmo", false)
                                                 .LinkedState(rangeAttackState),

                                              new GOAPAction("RunAway")
                                                 .Pre("Healthy",   false)
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("Healthy", true)
                                                 .Effect("isPlayerInRange", false)
                                                 .Effect("isPlayerInSight", false)
                                                 .LinkedState(runAwayState)
                                                 


                                          };

        var from = new GOAPState();
        from.values["isPlayerInSight"] = (_player.transform.position - transform.position).sqrMagnitude < 17f;
        from.values["isPlayerInRange"] = (_player.transform.position - transform.position).sqrMagnitude < chaseState.rangeDistance;
        from.values["isPlayerAlive"] = true;
        from.values["hasAmmo"] = ammo > 0;
        from.values["Healthy"] = life > 2;


        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["Healthy"] = true;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);



    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        if (_fsm != null)
        {
            _fsm.Active = false;
        }
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
    public void CallConfigure(IEnumerable<GOAPAction> plan)
    {
        ConfigureFsm(plan);
        
    }
    public void TakeDamage()
    {
        life -= 1;

        if(life <= 1)
        {
            OnReplan();
        }
        if (life <= 0)
        {
            Instantiate(fire, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }



}
