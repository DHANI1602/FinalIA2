using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class BFS<T>
{
    public event Action<IEnumerable<T>> OnPathCompleted;
    public IEnumerator CalculatePath(T start, Func<T, bool> isGoal, Func<T, IEnumerable<T>> explode)
    {
        var path = new List<T>() { start };
        var queue = new Queue<T>();

        queue.Enqueue(start);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        while (queue.Count > 0)
        {
            if (stopwatch.ElapsedMilliseconds >= 1 / 60f)
            {
                yield return null;
                stopwatch.Restart();
            }
            var dequeued = queue.Dequeue();

            if (isGoal(dequeued))
            {
                OnPathCompleted?.Invoke(path);
                yield break;
            }

            var toEnqueue = explode(dequeued);
            foreach (var n in toEnqueue)
            {
                path.Add(n);
                queue.Enqueue(n);
            }

        }
        OnPathCompleted?.Invoke(path);
        yield break;
    }

}

