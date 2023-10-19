using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControll : MonoBehaviour
{
    [SerializeField] private Camera main;
    [SerializeField] private CameraControll cameraControll;

    private Vector2 previousMousePosition;
    private Vector2 currentMousePosition;

    private Tile_Type current_Type = Tile_Type.Empty;

    private Tile PlayerTile;

    private void Update()
    {
        //���� ���콺�� UI Canvas Object ���� ������ true
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        UpdateCamera();
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            /*
             RayCast
            ��� ������ ������ ���� �´� ������Ʈ�� ������ �ҷ��´� 
            //ray debug ��¹�
            //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity);
             */
            // ī�޶�� ���� ȭ���� ���콺 ������ ��ġ�� �����ϴ� ������ ����
            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                if (tile != null)
                {
                    if (current_Type.Equals(Tile_Type.Player))
                    {
                        if (PlayerTile != null)
                        {
                            PlayerTile.Tiletype = Tile_Type.Empty;
                        }
                        PlayerTile = tile;
                    }
                    tile.Tiletype = current_Type;
                }
            }
        }
    }

    public void SetTileType(int tiletype)
    {
        current_Type = (Tile_Type)tiletype;
    }

    public void UpdateCamera()
    {
        //Ű���� �Է����� ī�޶� �̵�
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        cameraControll.SetPosition(x, y);

        //���콺 �� ��ư�� �̿��Ͽ� ī�޶� �̵�
        if (Input.GetMouseButtonDown(2))
        {
            currentMousePosition = previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            currentMousePosition = Input.mousePosition;
            if (previousMousePosition != currentMousePosition)
            {
                Vector2 move = (previousMousePosition - currentMousePosition) * 0.5f;
                cameraControll.SetPosition(move.x, move.y);
            }
        }
        previousMousePosition = currentMousePosition;

        //���� �ܾƿ� Mouse ScrollWheel
        float distance = Input.GetAxisRaw("Mouse ScrollWheel");
        cameraControll.SetOrthographicSize(-distance);
    }


}
