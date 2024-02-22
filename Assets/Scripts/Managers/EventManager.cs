using System.Collections.Generic;

public delegate void MyDelegate();
public enum  EventType
{
    OnMouseClick,
}
public class EventManager : Single<EventManager>
{
    public Dictionary<EventType, Dictionary<string,Lisener>> eventDic;

    public void AddEvent(EventType eventType)
    {
        if(!eventDic.ContainsKey(eventType))
            eventDic.Add(eventType, new Dictionary<string, Lisener>());
    }

    public void AddLisener(Lisener lisener)
    {
        EventType eventType = lisener.eventType;
        if (!eventDic.ContainsKey(eventType))
        {
            AddEvent(eventType);
        }
        eventDic[eventType][lisener.name] = lisener;
    }

    //发送事件,事件被触发时执行操作。
    public void SendEvent(EventType eventType)
    {
        if (eventDic.ContainsKey(eventType))
        {
            foreach (var lisener in eventDic[eventType].Values)
            {
                if (lisener.active)
                {
                    lisener.CallBack();
                }
            }
        }
    }
    public void RemoveLisener(EventType eventType,string lisenerName)
    {
        if (eventDic.ContainsKey(eventType) && eventDic[eventType].ContainsKey(lisenerName))
        {
            eventDic[eventType].Remove(lisenerName);
        }
    }
    public void RemoveEvent(EventType eventType)
    {
        if (eventDic.ContainsKey(eventType))
        {
            eventDic.Remove(eventType);
        }
    }

    public void Clear()
    {
        eventDic.Clear();
    }
}

public class Lisener
{
    public bool active;
    public string name;
    public EventType eventType;
    public MyDelegate CallBack;
    public Lisener(string name,EventType eventType, MyDelegate CallBack)
    {
        this.name = name;
        this.eventType = eventType;
        this.CallBack = CallBack;
        active = true;
    }
   

}

