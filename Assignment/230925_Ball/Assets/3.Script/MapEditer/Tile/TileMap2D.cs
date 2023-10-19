using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMap2D : MonoBehaviour
{
    [Header("Game�� ���� Check!")]
    public bool isGame = false;

    [Header("Game")]
    [SerializeField] private GameObject[] Tile_Prefabs_G;
    [SerializeField] private GameObject ItemPrefabs;

    private int MaxCoin = 0;
    private int CurrentCoin = 0;
    [SerializeField] private Stage_UI stageUI;
    [SerializeField] private Stagecontroller stagecontroller;

    [SerializeField] private List<TileBlink> blinkTiles;
    [SerializeField] private Movement2D movement;

    //----------------------------------------------
    [Header("MapEditer")]
    [SerializeField] private GameObject TilePrefabs;

    [Header("InputField")]
    [SerializeField] private InputField input_Width;
    [SerializeField] private InputField input_Height;

    public int width { get; private set; } = 10;
    public int height { get; private set; } = 10;

    public List<Tile> tileList { get; private set; }

    private MapData mapdata;


    private void Awake()
    {
        if (isGame) return;
        input_Width.text = width.ToString();
        input_Height.text = width.ToString();
        tileList = new List<Tile>();
        mapdata = new MapData();
    }

    #region �� ������

    //��ư �̺�Ʈ
    public void Generate_tilemap()
    {
        blinkTiles = new List<TileBlink>();
        if (int.TryParse(input_Width.text, out int _width) && int.TryParse(input_Height.text, out int _hieght))
        {
            width = _width;
            height = _hieght;
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (height * 0.5f - 0.5f) - y, 0);
                SpawnTile(Tile_Type.Empty, position);
            }
        }

        mapdata.Mapsize.x = width;
        mapdata.Mapsize.y = height;

        mapdata.Mapdata = new int[tileList.Count];
    }

    private void SpawnTile(Tile_Type type, Vector3 position)
    {
        GameObject clone = Instantiate(TilePrefabs, position, Quaternion.identity);
        clone.name = "Tile";
        clone.transform.SetParent(transform); //Ÿ�� �� ������Ʈ�� ���
        Tile tile = clone.GetComponent<Tile>();
        tile.Setup(type);

    }

    public MapData GetMapData()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i].Tiletype != Tile_Type.Player)
            {
                mapdata.Mapdata[i] = (int)tileList[i].Tiletype;
            }
            else //�÷��̾���
            {
                mapdata.Mapdata[i] = (int)Tile_Type.Empty;
                int x = (int)tileList[i].transform.position.x;
                int y = (int)tileList[i].transform.position.y;

                mapdata.PlayerPosition = new Vector2Int(x, y);

            }
        }
        return mapdata;
    }
    #endregion
    #region ����
    public void Generate_tilemap(MapData map)
    {
        blinkTiles = new List<TileBlink>();
        int width = map.Mapsize.x;
        int height = map.Mapsize.y;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (map.Mapdata[index].Equals((int)TileType_G.Empty))
                {
                    continue;
                }
                //�����Ǵ� Ÿ�� �� �߾��� 0 0 0�� ��ġ
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (height * 0.5f - 0.5f) - y, 0);
                if (map.Mapdata[index] > (int)TileType_G.Empty && map.Mapdata[index] < (int)TileType_G.LastIndex)
                {
                    //Ÿ�� ����� �޼ҵ� �߰�
                    SpawnTile((TileType_G)map.Mapdata[index], position);

                }
                else if (map.Mapdata[index] >= (int)Item_Type.coin)
                {
                    //������ ����� �޼ҵ� �߰�
                    SpawnItem(position);
                }
            }
        }
        stageUI.UpdateTextCoin(CurrentCoin, MaxCoin);

        //TileBlink blink ��ü �ϳ��ϳ� �����ϴµ� �ε����� �����ϴ� ���� �ƴ�
        //�ڷᱸ���� ��ü���� �����ϴ� ���
        foreach (TileBlink blink in blinkTiles)
        {
            //blink Ÿ�ϵ����� ���� Ÿ�� �˷��ִ� �޼ҵ� ȣ��
            blink.SetupBlinkTile(blinkTiles);
        }
    }

    public void SpawnTile(TileType_G type, Vector3 position)
    {
        //���߿� Ÿ�� Ÿ�Կ� ���� �͵��� �߰��� �Ǹ� ����Ǿ�� �� �κ�
        GameObject Clone = Instantiate(Tile_Prefabs_G[(int)type-1], position, Quaternion.identity);
        Clone.transform.SetParent(transform);
        Clone.transform.name = "Tile";
        Tile_G tile = Clone.GetComponent<Tile_G>();
        tile.Setup(movement);
        if (type.Equals(TileType_G.Blink))
        {
            blinkTiles.Add(Clone.GetComponent<TileBlink>());
        }

    }

    public void SpawnItem(Vector3 position)
    {
        GameObject clone = Instantiate(ItemPrefabs, position, Quaternion.identity);
        clone.transform.SetParent(transform);
        clone.transform.name = "Item";
        MaxCoin++;
    }

    public void GetCoin(GameObject coin)
    {
        CurrentCoin++;

        stageUI.UpdateTextCoin(CurrentCoin, MaxCoin);

        coin.GetComponent<Item>().Exit();
        if (CurrentCoin == MaxCoin)
        {
            //GameClear ���߿� �߰����� 0927
            stagecontroller.Gameclear();
            //���� �س��� ����������...
        }
    }


    #endregion
}
