using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class BuildingButtonHandler : MonoBehaviour
{
    [SerializeField] BuildingObjectBase item;
    Tilemap tilemap;
    Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);

        tilemap = MapManager.Instance.tilemaps["WallMap"];
    }
    void ButtonClicked()
    {

        print(GameManager.Instance.fsm.currentStateType.ToString());
        BuildingCreator.Instance.ObjectSelected(item);
        
        BuildingCreator.Instance.TilemapSelected(tilemap);

        print("Button was clicked" + item.name);
    }
}
