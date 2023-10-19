using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBroken : Tile_G
{
    [SerializeField] private GameObject TileBrokenEffect;
    public override void collsition(collisionDirection direction)
    {
        //Ÿ���� �μ����� ȿ�� ���
        Instantiate(TileBrokenEffect, transform.position, Quaternion.identity);
        if (direction == collisionDirection.down)
        {
            movement.JumpTo();
        }
        Destroy(gameObject);
    }
}
