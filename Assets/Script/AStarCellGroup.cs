using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarCellGroup : MonoBehaviour
{
    private List<AstarCell[]> m_CellList;
    private List<AstarCellInfo[]> m_CellInfoList;
    private AstarCellInfo m_StartCell;
    private AstarCellInfo m_EndCell;

    private KeyValuePair<int, int> m_StartIdx = new KeyValuePair<int, int>(2, 2);
    private KeyValuePair<int, int> m_EndIdx = new KeyValuePair<int, int>(8, 7);
    private int widthCnt = 10;
    private int heightCnt = 10;

    void Awake()
    {
        m_CellList = new List<AstarCell[]>();
        m_CellInfoList = new List<AstarCellInfo[]>();
        for (int i = 0; i < heightCnt; i++)
        {
            AstarCell[] arrCell = new AstarCell[widthCnt];
            AstarCellInfo[] arrCellInfo = new AstarCellInfo[widthCnt];
            for (int j = 0; j < widthCnt; j++)
            {
                arrCell[j] = GameSystem.InstantiateAndAddTo("Prefabs/AStar/", "Cell", gameObject, false).GetComponent<AstarCell>();
                arrCell[j].transform.localPosition = new Vector3((j - 5) * AstarCell.CellWidth + AstarCell.CellWidth * 0.5f
                    , (-i + 5) * AstarCell.CellHeight + AstarCell.CellHeight * 0.5f);
                arrCell[j].ActCellClick = CellClickProcess;

                arrCellInfo[j] = new AstarCellInfo();
                arrCellInfo[j].x = i;
                arrCellInfo[j].y = j;

                if (m_StartIdx.Key.Equals(i) && m_StartIdx.Value.Equals(j))
                {
                    arrCellInfo[j].eCellState = eCellState.Start;
                    m_StartCell = arrCellInfo[j].Clone();
                }
                else if (m_EndIdx.Key.Equals(i) && m_EndIdx.Value.Equals(j))
                {
                    arrCellInfo[j].eCellState = eCellState.End;
                    m_EndCell = arrCellInfo[j].Clone();
                }
                else
                {
                    arrCellInfo[j].eCellState = eCellState.None;
                }

                arrCell[j].SetData(arrCellInfo[j]);
            }
            m_CellList.Add(arrCell);
            m_CellInfoList.Add(arrCellInfo);
        }
        Debug.Log("Success!!");
    }

    public void SetUI()
    {

    }

    public void SetData()
    {

    }

    private void CellClickProcess(int _x, int _y)
    {
        if (m_StartCell != null && m_StartCell.x.Equals(_x) && m_StartCell.y.Equals(_y))
        {
            m_StartCell = null;
        }
        else if (m_EndCell != null && m_EndCell.x.Equals(_x) && m_EndCell.y.Equals(_y))
        {
            m_EndCell = null;
        }

        AstarCellInfo cell = m_CellInfoList[_x][_y];
        switch (cell.eCellState)
        {
            case eCellState.None:
                if (m_StartCell == null)
                {
                    cell.eCellState = eCellState.Start;
                    m_StartCell = cell.Clone();
                }
                else if (m_EndCell == null)
                {
                    cell.eCellState = eCellState.End;
                    m_EndCell = cell.Clone();
                }
                else
                {
                    cell.eCellState = eCellState.Obstacle;
                }
                break;
            case eCellState.Start:
                cell.eCellState = eCellState.None;
                break;
            case eCellState.Obstacle:
                cell.eCellState = eCellState.None;
                break;
            case eCellState.End:
                cell.eCellState = eCellState.None;
                break;
        }

        m_CellList[_x][_y].SetData(cell);
    }
}
