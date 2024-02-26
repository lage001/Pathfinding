using UnityEngine;
using UnityEngine.Tilemaps;


public enum Category
{
    Wall,
    Floor,
    Eraser,
    Start,
    Target,
}
[CreateAssetMenu(fileName ="Buildable",menuName ="BuildingObjects/Create Buildable")]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] Category category;
    [SerializeField] TileBase tileBase;
    public bool walkable;
    public TileBase TileBase
    {
        get
        {
            return tileBase;
        }
    }
    public Category Category
    {
        get
        {
            return category;
        }
    }

}
