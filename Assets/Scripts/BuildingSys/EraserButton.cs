using UnityEngine;
using UnityEngine.UI;

public class EraserButton : MonoBehaviour
{
    [SerializeField] BuildingObjectBase item;
    Button Eraserbtn;
    private void Awake()
    {
        Eraserbtn = GetComponent<Button>();
        Eraserbtn.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        BuildingCreator.Instance.ObjectSelected(item);
    }
}
