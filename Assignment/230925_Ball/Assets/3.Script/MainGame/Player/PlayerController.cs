using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement;
    [SerializeField] private TileMap2D tileMap2D;

    private float deathLimitY;
    public void Setup(Vector2Int position, int mapsizeY)
    {
        movement = GetComponent<Movement2D>();
        transform.position = new Vector3(position.x, position.y, 0);

        deathLimitY = -mapsizeY / 2;

    }
    private void Update()
    {
        if (transform.position.y <= deathLimitY)
        {
            //SceneLoad를 만들어주세요...어디서든 쓸 수 있게...
            SceneLoader.LoadScene();
        }
        UpdateMove();
        UpdateCollision();
    }
    private void UpdateCollision()
    {
        if (movement.iscollisionChecker.Up)
        {
            CollisionToTile(collisionDirection.up);
        }
        else if (movement.iscollisionChecker.Down)
        {
            CollisionToTile(collisionDirection.down);
        }
    }
    private void CollisionToTile(collisionDirection direction)
    {
        Tile_G tile = movement.Hittransform.GetComponent<Tile_G>();
        if (tile != null)
        {
            tile.collsition(direction);
        }
    }
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        movement.Moveto(x);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            tileMap2D.GetCoin(col.gameObject);
            //Destroy(col.gameObject);
            //0927
        }

    }
}
