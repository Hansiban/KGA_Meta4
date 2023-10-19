using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 8;
    private Rigidbody bullet_r;
    private void Start()
    {
        if (TryGetComponent(out bullet_r))
        {
            bullet_r.velocity = transform.forward * speed;
        }
        Destroy(gameObject, 3f);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.TryGetComponent(out PlayerController controller))
            {
                controller.Die();
            }
        }
    }

}
