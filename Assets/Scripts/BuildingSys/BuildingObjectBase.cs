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
    //Tile image
    public TileBase TileBase
    {
        get
        {
            return tileBase;
        }
    }
    //The function of tile
    public Category Category
    {
        get
        {
            return category;
        }
    }

}
