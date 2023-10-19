using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    /*
     �� ��尡 ������
    �θ���
    x,y ��ǥ��
    f = h+g
    h = ������(��ֹ��� ������ ��ǥ������ �Ÿ�)
    g = �������κ��� �̵��ߴ� �Ÿ�
    
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

    //�밢���� �̿��� ������?
    public bool AllowDigonal = true;
    //�ڳʸ� �������� ���� ���� ��� �̵� �� ���� ���� ��ֹ��� �ִ��� �Ǵ�
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

        //��� ������ ��� ����
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                bool iswall = false;

                //������ Ȯ��
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                {
                    if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                    {
                        iswall = true;
                    }
                }
                //��� ���
                nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        //���۰� �� ���, ���� ����Ʈ, ���� ����Ʈ, ���� ��� ����Ʈ �ʱ�ȭ
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
                //���� ����Ʈ �� ���� F�� �۰� F�� ���ٸ�
                //H�� ���� ���� ���� ���� ����
                if (OpenList[i].F <= Curnode.F && OpenList[i].H <= Curnode.H)
                {
                    Curnode = OpenList[i];
                }
                //���� ����Ʈ���� ���� ����Ʈ�� �ű��
                OpenList.Remove(Curnode);  
                CloseList.Add(Curnode);

                //curnode�� ���������
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
                    //�밢������ �����̴� cost ���
                    // �֢آע�
                    OpenListAdd(Curnode.x + 1, Curnode.y - 1);
                    OpenListAdd(Curnode.x - 1, Curnode.y + 1);
                    OpenListAdd(Curnode.x + 1, Curnode.y + 1);
                    OpenListAdd(Curnode.x - 1, Curnode.y - 1);

                }
                //�������� �����̴� cost ���
                // �����
                OpenListAdd(Curnode.x + 1, Curnode.y);
                OpenListAdd(Curnode.x - 1, Curnode.y);
                OpenListAdd(Curnode.x, Curnode.y - 1);
                OpenListAdd(Curnode.x, Curnode.y + 1);
            }

        }

    }
    public void OpenListAdd(int checkX, int checkY)
    {

        //����
        /*
        �����¿� ������ ����� �ʰ�,
        ���� �ƴϸ鼭
        ���� ����Ʈ�� ����� �Ѵ�.
         */

        if (
            checkX >= bottomLeft.x && checkX < topRight.x + 1//x��bottomLeft�� topRight�ȿ� �ְ�
           && checkY >= bottomLeft.y && checkY < topRight.y + 1
           && !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall
           && !CloseList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y])
           )
        {
            //�밢�� ��� �� (�� ���̷δ� ����� ���� ����)
            if (AllowDigonal)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, checkY - bottomLeft.y].isWall &&
                    nodeArray[checkX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            //�ڳʸ� �������� ���� ���� �� (�̵� �� ���� ���� ��ֹ��� ������ �ȵ�)
            if (DontCrossCorner)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, checkY - bottomLeft.y].isWall ||
                nodeArray[checkX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }

            //check�ϴ� ��带 �̿� ��忡 �ְ� ������ 10 �밢���� 14

            Node neighdorNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int movecost = Curnode.G + (Curnode.x - checkX == 0 || Curnode.y - checkY == 0 ? 10 : 14);

            //�̵� ����� �̿� ��� G���� �۰ų�, �Ǵ� ���� ����Ʈ�� �̿� ��尡 ���ٸ�
            if (movecost < neighdorNode.G || !OpenList.Contains(neighdorNode))
            {
                //G H parentnode�� ���� �� ���� ����Ʈ�� �߰�
                neighdorNode.G = movecost;
                neighdorNode.H = (Mathf.Abs(neighdorNode.x - Endnode.x) + Mathf.Abs(neighdorNode.y - Endnode.y)) * 10;

                neighdorNode.Parentnode = Curnode;

                OpenList.Add(neighdorNode);
            }
        }
    }
    private void OnDrawGizmos()
    {
        //�� ���� Debug �뵵�� �׸��� �׸��� ���
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
            Debug.Log("��ã����� ���� ��. ������ Ʋ���ݾ�.");
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
