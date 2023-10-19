using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    public float Move_Speed = 1f;
    [SerializeField] GameManager game_manager;

    private List<Node> path;
    private int currentWaypoint = 0;
    [SerializeField] GameObject StartPos;

    private void Awake()
    {
        ResetMovement();
    }

    public void StartMovement()
    {
        path = game_manager.Final_nodeList;
        if (path != null && path.Count > 0)
        {
            Debug.Log("path != null && path.Count > 0");
            game_manager.PathFinding();
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }
    public void ResetMovement()
    {
        transform.position = StartPos.transform.position;
    }

    IEnumerator FollowPath()
    {

        while (currentWaypoint < path.Count)
        {
            Debug.Log("currentWaypoint < path.Count");
            Vector3 targetPosition = new Vector3(path[currentWaypoint].x, path[currentWaypoint].y, 0);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                Debug.Log("Vector3.Distance(transform.position, targetPosition) > 0.01f");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Move_Speed * Time.deltaTime);
                yield return null;
            }

            currentWaypoint++;
            yield return null;
        }
    }
}