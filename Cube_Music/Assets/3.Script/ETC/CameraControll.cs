using System.Collections;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 15f;

    private Vector3 playertoDistance = new Vector3();
    private float hitDistance = 0;
    [SerializeField] float zoomDistance = -1.25f;
    private void Start()
    {
        playertoDistance = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 destPos = target.position + playertoDistance + (transform.forward * hitDistance);
        transform.position = Vector3.Lerp(transform.position, destPos, followSpeed * Time.deltaTime);
    }
    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;
        yield return new WaitForSeconds(0.15f);
        hitDistance = 0;
    }
}
