using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStriaght : Tile_G
{
    [SerializeField] private MoveType movetype;
    private BoxCollider2D collider;
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }
    public override void collsition(collisionDirection direction)
    {
        //플레이어의 위치를 현재 타일 위치에서 이동 방향 1만큼 움직임
        Vector3 position = collider.bounds.center + Vector3.right * (int)movetype;
        //플레이어가 왼쪽 오른쪽 이동할 수 있도록 메소드 호출
        movement.SetupStrightMove(movetype, position);

    }
}
