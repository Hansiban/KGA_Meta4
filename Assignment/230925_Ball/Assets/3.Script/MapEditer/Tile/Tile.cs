using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile_Type
{
    Empty = 0,
    Base,
    Broke,
    Boom,
    Jump,
    Staight_Left,
    Staight_Right,
    Blink,
    Item_Coin = 10,
    Player=100
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite[] TileImage; //이미지 배열
    [SerializeField] private Sprite[] ItemImage; //아이템 이미지
    [SerializeField] private Sprite Player; //아이템 이미지

    private Tile_Type type;
    private SpriteRenderer renderer;

    public void Setup(Tile_Type type)
    {
        renderer = GetComponent<SpriteRenderer>();
        this.type = type;
    }
    public Tile_Type Tiletype
    {
        get => type;
        set
        {
            type = value;
            //type에 따른 이미지 선택
            if ((int)type < (int)Tile_Type.Item_Coin)
            {
                renderer.sprite = TileImage[(int)type];
            }
            else if ((int)type < (int)Tile_Type.Player)
            {
                renderer.sprite = ItemImage[(int)type - (int)Tile_Type.Item_Coin];
            }
            else if ((int)type == (int)Tile_Type.Player)
            {
                renderer.sprite = Player;
            }
        }
    }


}
