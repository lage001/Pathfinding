using UnityEngine;

public class SingletonBase<T>:MonoBehaviour where T:MonoBehaviour
{

    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                }
            }
            
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            print(typeof(T).ToString());
            print(instance.ToString());
            Destroy(gameObject);
        }
    }
}
