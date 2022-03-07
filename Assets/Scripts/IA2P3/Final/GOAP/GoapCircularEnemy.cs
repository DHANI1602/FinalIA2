using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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

    public HasAmmo hasAmmo = global::HasAmmo.Yes;
    public SpriteRenderer sprite;

    Action<IEnumerable<GOAPAction>> goapToPlan;

    //Variables de estado
    private const string DistanceFromPlayer = "DistanceFromPlayer";
    private const string Healthy = "Healthy";
    private const string HasAmmo = "HasAmmo";
    private const string IsPlayerAlive = "IsPlayerAlive";

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

        PlanAndExecute();

    }


    private void PlanAndExecute()
    {
        Debug.Log("GoapCircularEnemy :: PlanAndExecute");
        var actions = new List<GOAPAction>();
        var from = new GOAPState();
        var to = new GOAPState();

        DoPlanning(out actions, out from, out to);

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);
    }

    //Saco la implementacion del plan para evitar duplicar codigo en PlanAndExecute y OnReplan, asi ambos comparten plan
    private void DoPlanning(out List<GOAPAction> actions, out GOAPState from, out GOAPState to)
    {
        actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect(DistanceFromPlayer, x=> x.SetValue(0f))
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre(DistanceFromPlayer, x=>x.GetValue<float>() < 3f)
                                                 .Pre(Healthy, x=>x.GetValue<int>() > 2)
                                                 .Effect(DistanceFromPlayer, x=>x.SetValue(29f))
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Recharge")
                                                 .Pre(HasAmmo, x=> x.GetValue<HasAmmo>() == global::HasAmmo.No)
                                                 .Effect(HasAmmo, x=> x.SetValue(global::HasAmmo.Yes))
                                                 .LinkedState(rechargeState),

                                              new GOAPAction("RunAway")
                                                 .Pre(Healthy,   x=>x.GetValue<int>() < 3)
                                                 .Effect(Healthy, x=> x.SetValue(3))
                                                 .LinkedState(runAwayState),

                                              new GOAPAction("Range Attack")
                                                 .Pre(DistanceFromPlayer, x=> x.GetValue<float>() >= chaseState.rangeDistance)
                                                 .Pre(HasAmmo,  x=>x.GetValue<HasAmmo>() == global::HasAmmo.Yes )
                                                 .Effect(IsPlayerAlive, x=> x.SetValue(false))
                                                 .Effect(HasAmmo, x=>x.SetValue(global::HasAmmo.No))
                                                 .LinkedState(rangeAttackState)
        };


        from = new GOAPState();
        from.values[DistanceFromPlayer] = new GOAPVariable((_player.transform.position - transform.position).sqrMagnitude); 
        from.values[IsPlayerAlive] = new GOAPVariable(_player._playerModel.isAlive); 
        from.values[HasAmmo] = new GOAPVariable(hasAmmo); 
        from.values[Healthy] = new GOAPVariable(life); 

        to = new GOAPState();
        to.values[IsPlayerAlive] = new GOAPVariable(false);
        to.values[Healthy] = new GOAPVariable(3);
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

        Debug.Log("GoapCircularEnemy :: OnReplan");


        var actions = new List<GOAPAction>();
        var from = new GOAPState();
        var to = new GOAPState();

        DoPlanning(out actions, out from, out to);

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



public class GOAPVariable
{
    public object value;

    public GOAPVariable(object value)
    {
        this.value = value;
    }

    public T GetValue<T>()
    {
        return (T)value;
    }

    public GOAPVariable Clone()
    {
        return new GOAPVariable(value);
    }

    public void SetValue<T>(T newValue)
    {
        //Debug.LogWarning($"old: {value}, new: {newValue}");
        value = newValue;
    }
}
