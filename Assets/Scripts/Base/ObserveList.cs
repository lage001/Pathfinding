using System.Collections.Generic;


public delegate void VoidCallBack();
public class ObserveList<T> : List<T>
{
    public VoidCallBack OnChange;
    public ObserveList() :base()
    {
    }

    public new void Add(T item)
    {
        base.Add(item);
        OnChange?.Invoke();
    }
    public new void Remove(T item)
    {
        base.Remove(item);
        OnChange?.Invoke();
    }


}
