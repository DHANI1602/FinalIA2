using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialGrid : MonoBehaviour
{
    public float x;
    public float y;
    public float cellWidth;
    public float cellHeight;

    public int width;
    public int height;

    private Dictionary<IGridEntity, Tuple<int, int>> lastPositions = new Dictionary<IGridEntity, Tuple<int, int>>();

    private HashSet<IGridEntity>[,] buckets;

    readonly public Tuple<int, int> Outside = Tuple.Create(-1, -1);
    readonly public IGridEntity[] Empty = new IGridEntity[0];


    private void Awake()
    {
        buckets = new HashSet<IGridEntity>[width, height];


        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                buckets[i, j] = new HashSet<IGridEntity>();
            }
        }

        var ents = RecursiveWalker(transform)
                  .Select(n => n.GetComponent<IGridEntity>())
                  .Where(n => n != null);

        foreach (var e in ents)
        {
            e.OnMove += UpdateEntity;
            UpdateEntity(e);
        }
    }

    public void UpdateEntity(IGridEntity entity)
    {
        var lastPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.Position);


        if (lastPos.Equals(currentPos))
            return;

        if (IsInsideGrid(lastPos))
        {
            buckets[lastPos.Item1, lastPos.Item2].Remove(entity);
        }


        if (IsInsideGrid(currentPos))
        {
            buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            lastPositions[entity] = currentPos;
        }
        else
            lastPositions.Remove(entity);
    }

    public IEnumerable<IGridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition)
    {
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), Mathf.Min(aabbFrom.y, aabbTo.y), 0);
        var to = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), Mathf.Max(aabbFrom.y, aabbTo.y), 0);

        var fromCoord = GetPositionInGrid(from);
        var toCoord = GetPositionInGrid(to);

        fromCoord = Tuple.Create(Util1.Clamp(fromCoord.Item1, width, 0), Util1.Clamp(fromCoord.Item2, height, 0));
        toCoord = Tuple.Create(Util1.Clamp(toCoord.Item1, width, 0), Util1.Clamp(toCoord.Item2, height, 0));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;

        var cols = Util.Generate(fromCoord.Item1, x => x + 1)
                       .TakeWhile(n => n < width && n <= toCoord.Item1);

        var rows = Util.Generate(fromCoord.Item2, y => y + 1)
                       .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany( col => rows.Select(row => Tuple.Create(col, row)));

        return cells
              .SelectMany(cell => buckets[cell.Item1, cell.Item2])
              .Where(e =>
                         from.x <= e.Position.x && e.Position.x <= to.x &&
                         from.y <= e.Position.y && e.Position.y <= to.y
                    )
              .Where(n => filterByPosition(n.Position));
    }

    public Tuple<int, int> GetPositionInGrid(Vector3 pos)
    {
        //quita la diferencia, divide segun las celdas y floorea
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.y - y) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position)
    {
        //si es menor a 0 o mayor a width o height, no esta dentro de la grilla
        return 0 <= position.Item1 && position.Item1 < width &&
               0 <= position.Item2 && position.Item2 < height;
    }

    void OnDestroy()
    {
        var ents = RecursiveWalker(transform).Select(n => n.GetComponent<IGridEntity>())
                                             .Where(n => n != null);

        foreach (var e in ents) e.OnMove -= UpdateEntity;
    }



    private static IEnumerable<Transform> RecursiveWalker(Transform parent)
    {
        foreach (Transform child in parent)
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            yield return child;
        }
    }


    public bool areGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;

    private void OnDrawGizmos()
    {
        var rows = Util.Generate(y, curr => curr + cellHeight)
                       .Select(row => Tuple.Create(new Vector3(x, row, 0),
                                                   new Vector3(x + cellWidth * width, row, 0)));

        var cols = Util.Generate(x, curr => curr + cellWidth)
                       .Select(col => Tuple.Create(new Vector3(col, y, 0),
                                                   new Vector3(col, y + cellHeight * height, 0)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

        foreach (var elem in allLines)
        {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }

        if (buckets == null || areGizmosShutDown) return;

        var originalCol = GUI.color;
        GUI.color = Color.red;
        if (!activatedGrid)
        {
            var allElems = new List<IGridEntity>();
            foreach (var elem in buckets)
                allElems = allElems.Concat(elem).ToList();

            int connections = 0;
            foreach (var entity in allElems)
            {
                foreach (var neighbour in allElems.Where(x => x != entity))
                {
                    Gizmos.DrawLine(entity.Position, neighbour.Position);
                    connections++;
                }

                if (showLogs)
                    Debug.Log("tengo " + connections + " conexiones por individuo");
                connections = 0;
            }
        }
        else
        {
            int connections = 0;
            foreach (var elem in buckets)
            {
                foreach (var ent in elem)
                {
                    foreach (var n in elem.Where(x => x != ent))
                    {
                        Gizmos.DrawLine(ent.Position, n.Position);
                        connections++;
                    }

                    if (showLogs)
                        Debug.Log("tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
        }

        GUI.color = originalCol;
        showLogs = false;
    }


}

