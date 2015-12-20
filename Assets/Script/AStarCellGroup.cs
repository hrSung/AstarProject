using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarCellGroup : MonoBehaviour
{
    public enum eMoveCell
    {
        Ready,
        Start,
        End,
    }

    private List<AstarCell[]> m_CellList;
    private List<AstarCellInfo[]> m_CellInfoList;
    private AstarCellInfo m_StartCell;
    private AstarCellInfo m_EndCell;

    //private AstarCellInfo m_CurMoveCell;
    private List<AstarCellInfo> m_OpenCellList;

    private KeyValuePair<int, int> m_StartIdx = new KeyValuePair<int, int>(2, 2);
    private KeyValuePair<int, int> m_EndIdx = new KeyValuePair<int, int>(8, 7);
    private int widthCnt = 10;
    private int heightCnt = 10;

    private bool m_isClick = true;
    private eMoveCell m_eMoveEnd = eMoveCell.Ready;


    void Awake()
    {
        m_OpenCellList = new List<AstarCellInfo>();

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
                    , (-i + 5) * AstarCell.CellHeight - AstarCell.CellHeight * 0.5f);
                arrCell[j].ActCellClick = CellClickProcess;

                arrCellInfo[j] = new AstarCellInfo();
                arrCellInfo[j].x = i;
                arrCellInfo[j].y = j;

                arrCellInfo[j].eCellState = eCellState.None;

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

    void Update()
    {
        if (m_StartCell == null || m_EndCell == null)
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log("Number1 Key!!!");
            m_isClick = false;
            if (m_eMoveEnd.Equals(eMoveCell.End)) return;
            m_eMoveEnd = eMoveCell.Start;

            if (m_StartCell.eOpenState.Equals(eOpenState.None))
            {
                m_StartCell.eOpenState = eOpenState.Close;

                int sX = m_StartCell.x;
                int sY = m_StartCell.y;
                int eX = m_EndCell.x;
                int eY = m_EndCell.y;

                m_StartCell.h = 0;
                m_StartCell.g = Mathf.Abs(sX - eX) + Mathf.Abs(sY - eY);
                m_StartCell.f = m_StartCell.h + m_StartCell.g;

                m_EndCell.h = Mathf.Abs(sX - eX) + Mathf.Abs(sY - eY);
                m_EndCell.g = 0;
                m_EndCell.f = m_EndCell.h + m_EndCell.g;

                SetDirection(m_StartCell);
                //m_CurMoveCell = m_StartCell;
            }
            else
            {
                int min = -1;

                for (int i = 0; i < m_OpenCellList.Count; i++)
                {
                    if (min.Equals(-1))
                    {
                        if (!m_OpenCellList[i].eOpenState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].eCellState.Equals(eCellState.Obstacle))
                        {
                            min = m_OpenCellList[i].g;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (min > m_OpenCellList[i].g
                            && !m_OpenCellList[i].eOpenState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].eCellState.Equals(eCellState.Obstacle))
                        {
                            min = m_OpenCellList[i].g;
                        }
                    }
                }

                List<AstarCellInfo> list = new List<AstarCellInfo>();
                for (int i = 0; i < m_OpenCellList.Count; i++)
                {
                    if (min.Equals(m_OpenCellList[i].g)
                            && !m_OpenCellList[i].eOpenState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].eCellState.Equals(eCellState.Obstacle))
                    {
                        list.Add(m_OpenCellList[i]);
                    }
                }


                min = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (min.Equals(-1))
                    {
                        min = list[i].f;
                    }
                    else
                    {
                        if (min > list[i].f)
                        {
                            min = list[i].f;
                        }
                    }
                }

                list = list.FindAll(x => x.f.Equals(min));
                AstarCellInfo info = list[0];

                Debug.Log("info.x : " + info.x + ", info.y : " + info.y);

                if (info != null)
                {
                    info.eOpenState = eOpenState.Close;
                    SetDirection(info);
                    if(info.Equals(m_EndCell))
                    {
                        m_eMoveEnd = eMoveCell.End;
                    }
                    //m_CurMoveCell = info;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Debug.Log("Number2 Key!!!");
        }
    }

    private void CellClickProcess(int _x, int _y)
    {
        if (!m_isClick) return;

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
                    m_StartCell = cell;
                }
                else if (m_EndCell == null)
                {
                    cell.eCellState = eCellState.End;
                    m_EndCell = cell;
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

    public void SetDirection(AstarCellInfo _info)
    {
        int x = _info.x;
        int y = _info.y;
        int sX = m_StartCell.x;
        int sY = m_StartCell.y;
        int eX = m_EndCell.x;
        int eY = m_EndCell.y;

        int tempX = x - 1;
        AstarCellInfo[] arrInfo = tempX < 0 || tempX > widthCnt - 1 ? null : m_CellInfoList[tempX];
        if (arrInfo != null)
        {
            int tempY = y - 1;
            AstarCellInfo info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.SouthWest;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }

            tempY = y;
            info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.West;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }

            tempY = y + 1;
            info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.NorthWest;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }
        }

        tempX = x;
        arrInfo = tempX < 0 || tempX > widthCnt - 1 ? null : m_CellInfoList[tempX];
        if (arrInfo != null)
        {
            int tempY = y - 1;
            AstarCellInfo info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.South;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }

            tempY = y + 1;
            info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.North;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }
        }

        tempX = x + 1;
        arrInfo = tempX < 0 || tempX > widthCnt - 1 ? null : m_CellInfoList[tempX];
        if (arrInfo != null)
        {
            int tempY = y - 1;
            AstarCellInfo info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.SouthEast;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }

            tempY = y;
            info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.East;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }

            tempY = y + 1;
            info = tempY < 0 || tempY > heightCnt - 1 ? null : arrInfo[tempY];
            if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
                && info.eOpenState.Equals(eOpenState.None))
            {
                info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
                info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                info.f = info.h + info.g;

                info.eOpenState = eOpenState.Open;
                info.eDirection = eDirection.NorthEast;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempX][tempY].SetDataView(info);
            }
        }

        m_CellList[x][y].SetDataView(_info);
    }
}
