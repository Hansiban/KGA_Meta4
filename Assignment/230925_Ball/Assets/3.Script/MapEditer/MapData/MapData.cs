using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
객체 직렬화
직렬화란?
객체의 상태를 메모리나 영구 저장 장치에 저장가능한 0과 1로 순서를 바꾸는 것

직렬화가 없으면?
기본 데이터 형식(int, float 등 new 없이 사용되는 것들)만 파일 입출력이 가능
클래스 구조체 같은 데이터 컨테이너 방식, 복합 데이터 등 형식의 파일 입출력을 위해 모든 변수를 저장하는 방법으로 정의하여야 한다
 
 */
[System.Serializable] //객체 직렬화
public class MapData
{
    public Vector2Int Mapsize;
    public int[] Mapdata;
    public Vector2Int PlayerPosition;
}
