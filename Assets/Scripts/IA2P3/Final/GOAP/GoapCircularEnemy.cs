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

    public IsPlayerInRange isPlayerInRange;

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


        // EL ACTIONS , EL FROM Y EL TO TIENEN QUE SER IGUALES EN PLANANDEXECUTE(); Y ONLIPLAN(); Y REPLAN();

        // LOGRAR QUE EN EL FROM PUEDA PASARLE MINIMO UN BOOL, FLOAT, INT E STRING(O ENUM)
    }

    private void OnlyPlan()
    {


        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("DistanceFromPlayer", x=>x=0),

                                              new GOAPAction("Chase")
                                                 .Pre("DistanceFromPlayer", x=>(float)x < 15f)
                                                 .Pre("Healthy", x=>(int)x > 2)
                                                 .Effect("DistanceFromPlayer", x=>x = 20)
                                                 .Effect("isPlayerInRange", x=>x=0),

                                              new GOAPAction("RunAway")
                                                 .Pre("Healthy", x=>(int)x <= 2)
                                                 .Pre("isPlayerInRange", x=>(float)x <= chaseState.rangeDistance)
                                                 .Effect("Healthy", x=>x=3)
                                                 .Effect("isPlayerInRange", x=>x=20)
                                                 .Cost(2f),


                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", x=>(float)x <= chaseState.rangeDistance)
                                                 .Pre("hasAmmo",  x=>(int)x > 0)
                                                 .Effect("isPlayerAlive", x=> x = false),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", x=>(int)x <= 0)
                                                 .Effect("hasAmmo", x=>x=3)
                                          };

        // ESTOS SON LOS QUE YO TENGO AHORA
        var from = new GOAPState();
        from.values["DistanceFromPlayer"] = (_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerInRange"] =(_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerAlive"] = _player._playerModel.isAlive;
        from.values["hasAmmo"] = ammo;
        from.values["Healthy"] = life;

        // esto lo hice yo
        // from.values["DistanceFromPlayer"] = (_player.transform.position - transform.position).sqrMagnitude;

        //ESTO SON LOS ESTADOS QUE YO QUIERO
        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["Healthy"] = 3;

        //EMPIEZO DESDE EL FINAL Y VOY CHECKEANDO HASTA QUE LLEGO AL INICIO

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction>{
                                               new GOAPAction("Patrol")
                                                 .Effect("DistanceFromPlayer", x=>x=0)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("DistanceFromPlayer", x=>(float)x < 17f)
                                                 .Pre("Healthy", x=>(int)x > 2)
                                                 .Effect("isPlayerInRange", x=>x=0)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", x=>(int)x <= 0)
                                                 .Effect("hasAmmo", x=>x=3)
                                                 .LinkedState(rechargeState),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", x=>(float)x < chaseState.rangeDistance)
                                                 .Pre("hasAmmo",  x=>(int)x > 0)
                                                 .Effect("isPlayerAlive", x=> x = false)
                                                 .Effect("hasAmmo", x=>x=0)
                                                 .LinkedState(rangeAttackState),

                                               new GOAPAction("RunAway")
                                                 .Pre("Healthy",   x=>(int)x <= 2)
                                                 .Pre("DistanceFromPlayer",  x=>(float)x < 17f)
                                                 .Effect("Healthy", x=>x=3)
                                                 .Effect("isPlayerInRange",  x=>x=20)
                                                 .Effect("DistanceFromPlayer", x=>x =20)
                                                 .LinkedState(runAwayState)


                                          };

        var from = new GOAPState();
        from.values["DistanceFromPlayer"] = (_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerInRange"] = 0f;//(_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerAlive"] = true;//_player._playerModel.isAlive;
        from.values["hasAmmo"] = 50; //ammo;
        from.values["Healthy"] = 3;//life;


        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
       // to.values["Healthy"] = 3;

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
                                                 .Effect("DistanceFromPlayer", x=>x=0)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("DistanceFromPlayer",  x=>(float)x < 17f)
                                                 .Pre("Healthy", x=>(int)x > 2)
                                                 .Effect("isPlayerInRange", x=>x=0)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Recharge")
                                                 .Pre("hasAmmo", x=>(int)x <= 0)
                                                 .Effect("hasAmmo",  x=>x=3)
                                                 .LinkedState(rechargeState),

                                              new GOAPAction("Range Attack")
                                                 .Pre("isPlayerInRange", x=>(float)x < chaseState.rangeDistance)
                                                 .Pre("hasAmmo",  x=>(int)x <= 0)
                                                 .Effect("isPlayerAlive", x=>x=false)
                                                 .Effect("hasAmmo", x=>x=3)
                                                 .LinkedState(rangeAttackState),

                                              new GOAPAction("RunAway")
                                                 .Pre("Healthy",   x=>(int)x <= 2)
                                                 .Pre("DistanceFromPlayer",  x=>(float)x < 17f)
                                                 .Effect("Healthy", x=>x=3)
                                                 .Effect("isPlayerInRange", x=>x=20)
                                                 .Effect("DistanceFromPlayer", x=>x =20)
                                                 .LinkedState(runAwayState)



                                          };

        var from = new GOAPState();
        from.values["DistanceFromPlayer"] = (_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerInRange"] = (_player.transform.position - transform.position).sqrMagnitude;
        from.values["isPlayerAlive"] = _player._playerModel.isAlive;
        from.values["hasAmmo"] = ammo;
        from.values["Healthy"] = life;


        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;
        to.values["Healthy"] = 3;

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

        if (life <= 1)
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

public enum IsPlayerInRange
{
    Yes,
    No
}
