using System.Collections;

public interface IReminder
{
    void MakeSnapshot();
    void Rewind();

    IEnumerator StartToRecord();
}