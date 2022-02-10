using System.Collections.Generic;

public class Memento<TSnapshot>
{
    private List<TSnapshot> _snapshots = new List<TSnapshot>();

    public void Record(TSnapshot snapshot)
    {
        _snapshots.Add(snapshot);
    }

    public TSnapshot Remember()
    {
        var snapshot = _snapshots[_snapshots.Count - 1];
        _snapshots.RemoveAt(_snapshots.Count - 1);
        return snapshot;
    }

    public bool CanRemember()
    {
        return _snapshots.Count > 0;
    }
}
