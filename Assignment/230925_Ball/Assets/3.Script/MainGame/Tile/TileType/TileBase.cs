using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : Tile_G
{
    public override void collsition(collisionDirection direction)
    {
        if (direction == collisionDirection.down)
        {
            movement.JumpTo();
        }
    }
}
