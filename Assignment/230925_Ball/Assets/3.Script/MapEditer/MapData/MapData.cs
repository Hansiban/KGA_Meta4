using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
��ü ����ȭ
����ȭ��?
��ü�� ���¸� �޸𸮳� ���� ���� ��ġ�� ���尡���� 0�� 1�� ������ �ٲٴ� ��

����ȭ�� ������?
�⺻ ������ ����(int, float �� new ���� ���Ǵ� �͵�)�� ���� ������� ����
Ŭ���� ����ü ���� ������ �����̳� ���, ���� ������ �� ������ ���� ������� ���� ��� ������ �����ϴ� ������� �����Ͽ��� �Ѵ�
 
 */
[System.Serializable] //��ü ����ȭ
public class MapData
{
    public Vector2Int Mapsize;
    public int[] Mapdata;
    public Vector2Int PlayerPosition;
}
