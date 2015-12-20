using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum eCellState
{
    None = 0,   // 아무런 상태도 아님
    Start,      // 시작 지점
    Obstacle,   // 장애물(갈 수 없는 지점)
    End,        // 도착 지점
}

public enum eOpenState
{
    None = 0,   // 세팅이 안된 값
    Open,       // 열린 상태(탐색되지 않음.)
    Close,      // 닫힌 상태(탐색됨.)
}

public enum eDirection
{
    None = 0,   // 세팅이 안된 값
    East,       // 동쪽
    West,       // 서쪽
    South,      // 남쪽
    North,      // 북쪽
    SouthEast,  // 남동쪽
    SouthWest,  // 남서쪽
    NorthEast,  // 북동쪽
    NorthWest,  // 북서쪽
}

public class AstarCellInfo
{
    public int x;
    public int y;

    public int f;
    public int g;
    public int h;

    public eOpenState eOpenState = eOpenState.None;
    public eCellState eCellState = eCellState.None;
    public eDirection eDirection = eDirection.None;

    public AstarCellInfo preAsterCell = null;

    public AstarCellInfo Clone()
    {
        AstarCellInfo _info = new AstarCellInfo();
        _info.x = x;
        _info.y = y;
        _info.f = f;
        _info.g = g;
        _info.h = h;
        _info.eOpenState = eOpenState;
        _info.eCellState = eCellState;
        _info.eDirection = eDirection;
        _info.preAsterCell = preAsterCell;

        return _info;
    }

    public AstarCellInfo()
    {

    }

    public void SetCellState()
    {

    }
}
