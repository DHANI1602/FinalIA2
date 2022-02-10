using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc2 : MonoBehaviour
{
    public float speed;
    public float radius;
    public float rangeAttack;
    public LayerMask player;
    StateMachine _sm;
    public GameObject explosion;
    public GameObject motorfire;

    private void Awake()
    {
        _sm = new StateMachine();
        _sm.AddState(new StateHauntingNpc2(_sm, this));
        _sm.AddState(new StateKamikaze(_sm, this));
        _sm.SetState<StateHauntingNpc2>();
    }

    private void Update()
    {
        _sm.Update();

        var targetPos = Target();
        var thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public Vector3 Target()
    {
        var target = Physics2D.OverlapCircle(transform.position, radius, player);
        return target.transform.position;
    }

    public bool RangeAttack()
    {
        return Vector3.Distance(Target(), transform.position) < rangeAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }
}
