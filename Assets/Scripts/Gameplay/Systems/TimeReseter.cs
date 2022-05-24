using Gamebase;
using UnityEngine;

public class TimeReseter : MonoBehaviour
{
    void Start()
    {
        GamebaseSystems.Instance.GlobalEventsSystem.Subscribe(GlobalEventType.ReturnTimeScale, Reset);
    }
    
    public void Reset()
    {
        Time.timeScale = 1;
    }
}
