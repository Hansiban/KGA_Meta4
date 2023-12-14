using UnityEngine;

public enum color
{
    none = -1,
    red = 0,
    blue = 1
}
public class CubeControll : MonoBehaviour
{
    [Range(1f, 100f)]
    public float Speed = 20f;
    public color color = color.none;

    private void Update()
    {
        transform.position += Time.deltaTime * -transform.forward * Speed;
    }
}
