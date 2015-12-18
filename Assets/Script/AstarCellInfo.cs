using UnityEngine;
using System.Collections;
using System;

public class AstarCellInfo
{
    public int x;
    public int y;

    public int f;
    public int g;
    public int h;

    public eOpenState eOpenState = eOpenState.None;
    public eCellState eCellState = eCellState.None;

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

        return _info;
    }

    public AstarCellInfo()
    {

    }
}
