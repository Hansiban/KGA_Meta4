using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType_G
{
    Empty = 0,
    Base,
    Broke,
    Boom,
    Jump,
    StraightLeft,
    StraightRight,
    Blink,
    LastIndex
}
public enum collisionDirection { up = 0, down }
#region ...
//public class Tile_G : MonoBehaviour
//{
//    [SerializeField] private Sprite[] Image;
//    private SpriteRenderer renderer;
//    private TileType_G tiletype;
//    public TileType_G Tiletype
//    {
//        get => tiletype;
//        set
//        {
//            tiletype = value;
//            renderer.sprite = Image[(int)tiletype - 1];
//        }
//    }

//    public void Setup(TileType_G tile)
//    {
//        renderer = GetComponent<SpriteRenderer>();
//        tiletype = tile;
//    }

//}
#endregion
public abstract class Tile_G : MonoBehaviour
{
    protected Movement2D movement;
    public virtual void Setup(Movement2D movement2D) //구현 선택
    {
        movement = movement2D;
    }
    public abstract void collsition(collisionDirection direction); //무조건 구현
    
}
