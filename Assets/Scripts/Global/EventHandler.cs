using UnityEngine.Events;

public class EventHandler
{
    
    public UnityEvent StartPlayerChasing;
    public UnityEvent StopPlayerChasing ;
    public UnityEvent PlayerFound;

    public EventHandler()
    {
        StartPlayerChasing = new();
        StopPlayerChasing = new();
        PlayerFound = new();
    }
}
