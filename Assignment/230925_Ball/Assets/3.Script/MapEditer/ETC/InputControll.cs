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
        //현재 마우스가 UI Canvas Object 위에 있으면 true
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        UpdateCamera();
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            /*
             RayCast
            어떠한 기준의 광선을 쏴서 맞는 오브젝트의 정보를 불러온다 
            //ray debug 찍는법
            //Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity);
             */
            // 카메라로 부터 화면의 마우스 포지션 위치를 관통하는 광선을 생성
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
        //키보드 입력으로 카메라 이동
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        cameraControll.SetPosition(x, y);

        //마우스 휠 버튼을 이용하여 카메라 이동
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

        //줌인 줌아웃 Mouse ScrollWheel
        float distance = Input.GetAxisRaw("Mouse ScrollWheel");
        cameraControll.SetOrthographicSize(-distance);
    }


}
