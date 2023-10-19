using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    /*
     이 노드가 벽인지
    부모노드
    x,y 좌표값
    f = h+g
    h = 추적값(장애물을 무시한 목표까지의 거리)
    g = 시작으로부터 이동했던 거리
    
    */
    public bool isWall;
    public Node Parentnode;
    public int x, y;
    public int G;
    public int H;
    public int F
    {
        get
        {
            return G + H;
        }
    }
    public Node(bool iswall, int x, int y)
    {
        this.isWall = iswall;
        this.x = x;
        this.y = y;

    }
}

public class GameManager : MonoBehaviour
{
    public GameObject Start_pos, End_Pos;
    public GameObject BottomLeft, TopRight;

    [SerializeField]
    private Vector2Int bottomLeft, topRight, start_Pos, end_Pos;

    public List<Node> Final_nodeList;

    //대각선을 이용할 것인지?
    public bool AllowDigonal = true;
    //코너를 가로질러 가지 않을 경우 이동 중 수직 수평 장애물이 있는지 판단
    public bool DontCrossCorner = true;

    private int SizeX, SizeY;
    Node[,] nodeArray;

    public Node StartNode, Endnode, Curnode;

    List<Node> OpenList, CloseList;

    public GameObject Player;
    public void Setposition()
    {
        bottomLeft = new Vector2Int((int)BottomLeft.transform.position.x, (int)BottomLeft.transform.position.y);
        topRight = new Vector2Int((int)TopRight.transform.position.x, (int)TopRight.transform.position.y);
        start_Pos = new Vector2Int((int)Start_pos.transform.position.x, (int)Start_pos.transform.position.y);
        end_Pos = new Vector2Int((int)End_Pos.transform.position.x, (int)End_Pos.transform.position.y);
    }
    public void PathFinding()
    {
        Setposition();
        SizeX = topRight.x - bottomLeft.x + 1;
        SizeY = topRight.y - bottomLeft.y + 1;

        nodeArray = new Node[SizeX, SizeY];

        //모든 노드들을 담는 과정
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                bool iswall = false;

                //벽인지 확인
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                    {
                        iswall = true;
                    }
                }
                //노드 담기
                nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        //시작과 끝 노드, 열린 리스트, 닫힌 리스트, 최종 경로 리스트 초기화
        StartNode = nodeArray[start_Pos.x - bottomLeft.x, start_Pos.y - bottomLeft.y];
        Endnode = nodeArray[end_Pos.x - bottomLeft.x, end_Pos.y - bottomLeft.y];

        OpenList = new List<Node>();
        CloseList = new List<Node>();
        Final_nodeList = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Curnode = OpenList[0];

            for (int i = 0; i < OpenList.Count; i++)
            {
                //열린 리스트 중 가장 F가 작고 F가 같다면
                //H가 작은 것을 현재 노드로 설정
                if (OpenList[i].F <= Curnode.F && OpenList[i].H <= Curnode.H)
                {
                    Curnode = OpenList[i];
                }
                //열린 리스트에서 닫힌 리스트로 옮기기
                OpenList.Remove(Curnode);  
                CloseList.Add(Curnode);

                //curnode가 도착지라면
                if (Curnode == Endnode)
                {
                    Node targetnode = Endnode;
                    while (targetnode != StartNode)
                    {
                        Final_nodeList.Add(targetnode);
                        targetnode = targetnode.Parentnode;
                    }
                    Final_nodeList.Add(StartNode);
                    Final_nodeList.Reverse();
                    return;
                }
                if (AllowDigonal)
                {
                    //대각선으로 움직이는 cost 계산
                    // ↗↖↙↘
                    OpenListAdd(Curnode.x + 1, Curnode.y - 1);
                    OpenListAdd(Curnode.x - 1, Curnode.y + 1);
                    OpenListAdd(Curnode.x + 1, Curnode.y + 1);
                    OpenListAdd(Curnode.x - 1, Curnode.y - 1);

                }
                //직선으로 움직이는 cost 계산
                // ↑→↓←
                OpenListAdd(Curnode.x + 1, Curnode.y);
                OpenListAdd(Curnode.x - 1, Curnode.y);
                OpenListAdd(Curnode.x, Curnode.y - 1);
                OpenListAdd(Curnode.x, Curnode.y + 1);
            }

        }

    }
    public void OpenListAdd(int checkX, int checkY)
    {

        //조건
        /*
        상하좌우 범위를 벗어나지 않고,
        벽도 아니면서
        닫힌 리스트에 없어야 한다.
         */

        if (
            checkX >= bottomLeft.x && checkX < topRight.x + 1//x가bottomLeft와 topRight안에 있고
           && checkY >= bottomLeft.y && checkY < topRight.y + 1
           && !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall
           && !CloseList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y])
           )
        {
            //대각선 허용 시 (벽 사이로는 통과가 되지 않음)
            if (AllowDigonal)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, checkY - bottomLeft.y].isWall &&
                    nodeArray[checkX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            //코너를 가로질러 가지 않을 시 (이동 중 수직 수평 장애물이 있으면 안됨)
            if (DontCrossCorner)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, checkY - bottomLeft.y].isWall ||
                nodeArray[checkX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }

            //check하는 노드를 이웃 노드에 넣고 직선은 10 대각선은 14

            Node neighdorNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int movecost = Curnode.G + (Curnode.x - checkX == 0 || Curnode.y - checkY == 0 ? 10 : 14);

            //이동 비용이 이웃 노드 G보다 작거나, 또는 열린 리스트에 이웃 노드가 없다면
            if (movecost < neighdorNode.G || !OpenList.Contains(neighdorNode))
            {
                //G H parentnode를 설정 후 열린 리스트에 추가
                neighdorNode.G = movecost;
                neighdorNode.H = (Mathf.Abs(neighdorNode.x - Endnode.x) + Mathf.Abs(neighdorNode.y - Endnode.y)) * 10;

                neighdorNode.Parentnode = Curnode;

                OpenList.Add(neighdorNode);
            }
        }
    }
    private void OnDrawGizmos()
    {
        //씬 뷰의 Debug 용도로 그림을 그릴때 사용
        if (Final_nodeList != null)
        {
            for (int i = 0; i < Final_nodeList.Count-1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(Final_nodeList[i].x, Final_nodeList[i].y),
                    new Vector2(Final_nodeList[i + 1].x, Final_nodeList[i + 1].y));
            }
        }
    }

    public void Player_Start()
    {
        if (Final_nodeList.Count > 0)
        {
            StartCoroutine(PlayerMove_Co());
        }
        else
        {
            Debug.Log("길찾기부터 먼저 해. 순서가 틀리잖아.");
        }
    }
    public void Player_Reset()
    {
        Vector3 startpos_v3 = new Vector3(start_Pos.x, start_Pos.y, 0);
        Player.transform.position = startpos_v3;
    }
    private IEnumerator PlayerMove_Co()
    {
        for (int i = 0; i < Final_nodeList.Count;)
        {
            Vector3 TargetPost = new Vector3(Final_nodeList[i].x, Final_nodeList[i].y, 0);
            while (Vector3.Distance(Player.transform.position, TargetPost) >= 0.01f)
            {
                Player.transform.position = Vector3.MoveTowards(Player.transform.position, TargetPost, 5f * Time.deltaTime);
                yield return null;
            }
            Player.transform.position = TargetPost;
            i++;
            yield return null;
        }
    }
}
