using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleQuery : MonoBehaviour
{
   public SpatialGrid grid;

    public float radius;

    public IEnumerable<IGridEntity> Query()
    {
        var from = transform.position - new Vector3(radius, radius, 0);
        var to = transform.position + new Vector3(radius, radius, 0);
        return grid.Query(from, to, pos => (transform.position - pos).sqrMagnitude < radius * radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
