using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBroken : Tile_G
{
    [SerializeField] private GameObject TileBrokenEffect;
    public override void collsition(collisionDirection direction)
    {
        //타일이 부서지는 효과 재생
        Instantiate(TileBrokenEffect, transform.position, Quaternion.identity);
        if (direction == collisionDirection.down)
        {
            movement.JumpTo();
        }
        Destroy(gameObject);
    }
}
