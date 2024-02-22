using UnityEngine;

public class TestScript : MonoBehaviour
{

    public ObserveList<int> L;
    
    private void Awake()
    {
        L = new ObserveList<int>() {1,2,3 };
        L.OnChange += TestFunc;
        L.Remove(1);
        L.Add(4);
        print(L.Contains(1));
    }
    void TestFunc()
    {
        foreach (var item in L)
        {
            print(item);
        }
    }
}
