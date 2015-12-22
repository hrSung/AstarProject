using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarCellGroup : MonoBehaviour
{
    private const int WIDTHCNT = 13;
    private const int HEIGHTCNT = 13;

    public enum eMoveCell
    {
        Ready,
        Start,
        End,
    }

    private AstarCell[][] m_CellList;
    private AstarCellInfo[][] m_CellInfoList;
    private AstarCellInfo m_StartCell;
    private AstarCellInfo m_EndCell;

    private List<AstarCellInfo> m_OpenCellList;

    private KeyValuePair<int, int> m_StartIdx = new KeyValuePair<int, int>(2, 2);
    private KeyValuePair<int, int> m_EndIdx = new KeyValuePair<int, int>(8, 7);

    private bool m_isClick = true;
    private eMoveCell m_eMoveEnd = eMoveCell.Ready;


    void Awake()
    {
        m_OpenCellList = new List<AstarCellInfo>();

        m_CellList = new AstarCell[HEIGHTCNT][];
        m_CellInfoList = new AstarCellInfo[HEIGHTCNT][];
        for (int i = 0; i < HEIGHTCNT; i++)
        {
            AstarCell[] arrCell = new AstarCell[WIDTHCNT];
            AstarCellInfo[] arrCellInfo = new AstarCellInfo[WIDTHCNT];
            for (int j = 0; j < WIDTHCNT; j++)
            {
                arrCell[j] = GameSystem.InstantiateAndAddTo("Prefabs/AStar/", "Cell", gameObject, false).GetComponent<AstarCell>();
                arrCell[j].transform.localPosition = new Vector3((j - (WIDTHCNT / 2)) * AstarCell.CellWidth + AstarCell.CellWidth * 0.5f
                    , (-i + (HEIGHTCNT / 2)) * AstarCell.CellHeight - AstarCell.CellHeight * 0.5f);
                arrCell[j].ActCellClick = CellClickProcess;

                arrCellInfo[j] = new AstarCellInfo();
                arrCellInfo[j].x = j;
                arrCellInfo[j].y = i;

                arrCellInfo[j].cellState = eCellState.None;

                arrCell[j].SetData(arrCellInfo[j]);
            }
            m_CellList[i] = arrCell;
            m_CellInfoList[i] = arrCellInfo;
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

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Debug.Log("Number1 Key!!!");
            m_isClick = false;
            if (m_eMoveEnd.Equals(eMoveCell.End)) return;
            m_eMoveEnd = eMoveCell.Start;

            if (m_StartCell.openState.Equals(eOpenState.None))
            {
                m_StartCell.openState = eOpenState.Close;

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
            }
            else
            {
                int min = -1;

                for (int i = 0; i < m_OpenCellList.Count; i++)
                {
                    if (min.Equals(-1))
                    {
                        if (!m_OpenCellList[i].openState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
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
                            && !m_OpenCellList[i].openState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
                        {
                            min = m_OpenCellList[i].g;
                        }
                    }
                }

                List<AstarCellInfo> list = new List<AstarCellInfo>();
                for (int i = 0; i < m_OpenCellList.Count; i++)
                {
                    if (min.Equals(m_OpenCellList[i].g)
                            && !m_OpenCellList[i].openState.Equals(eOpenState.Close)
                            && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
                    {
                        list.Add(m_OpenCellList[i]);
                    }
                }


                min = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (min.Equals(-1))
                    {
                        min = list[i].h;
                    }
                    else
                    {
                        if (min > list[i].h)
                        {
                            min = list[i].h;
                        }
                    }
                }

                list = list.FindAll(x => x.h.Equals(min));
                AstarCellInfo info = list[0];

                Debug.Log("info.x : " + info.x + ", info.y : " + info.y);

                if (info != null)
                {
                    SetDirection(info);
                    if (info.Equals(m_EndCell))
                    {
                        m_eMoveEnd = eMoveCell.End;
                        int count = 1;
                        AstarCellInfo _info = m_EndCell.preAsterCell;
                        while (_info != null)
                        {
                            _info.cellState = eCellState.Path;
                            _info.f = count;
                            m_CellList[_info.y][_info.x].SetData(_info);
                            _info = _info.preAsterCell;
                            count++;
                            if (_info.Equals(m_StartCell)) return;
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Debug.Log("Number2 Key!!!");

            AstarCellInfo info;
            for (int i = 0; i < m_CellInfoList.Length; i++)
            {
                for (int j = 0; j < m_CellInfoList[i].Length; j++)
                {
                    info = m_CellInfoList[i][j];
                    info.SetInit();
                    m_CellList[i][j].SetData(info);
                }
            }

            m_StartCell = null;
            m_EndCell = null;
            m_OpenCellList = new List<AstarCellInfo>();
            m_isClick = true;
            m_eMoveEnd = eMoveCell.Ready;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            while (!m_eMoveEnd.Equals(eMoveCell.End))
            {
                m_isClick = false;
                if (m_eMoveEnd.Equals(eMoveCell.End)) return;
                m_eMoveEnd = eMoveCell.Start;

                if (m_StartCell.openState.Equals(eOpenState.None))
                {
                    m_StartCell.openState = eOpenState.Close;

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
                }
                else
                {
                    int min = -1;

                    for (int i = 0; i < m_OpenCellList.Count; i++)
                    {
                        if (min.Equals(-1))
                        {
                            if (!m_OpenCellList[i].openState.Equals(eOpenState.Close)
                                && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
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
                                && !m_OpenCellList[i].openState.Equals(eOpenState.Close)
                                && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
                            {
                                min = m_OpenCellList[i].g;
                            }
                        }
                    }

                    List<AstarCellInfo> list = new List<AstarCellInfo>();
                    for (int i = 0; i < m_OpenCellList.Count; i++)
                    {
                        if (min.Equals(m_OpenCellList[i].g)
                                && !m_OpenCellList[i].openState.Equals(eOpenState.Close)
                                && !m_OpenCellList[i].cellState.Equals(eCellState.Obstacle))
                        {
                            list.Add(m_OpenCellList[i]);
                        }
                    }


                    min = -1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (min.Equals(-1))
                        {
                            min = list[i].h;
                        }
                        else
                        {
                            if (min > list[i].h)
                            {
                                min = list[i].h;
                            }
                        }
                    }

                    list = list.FindAll(x => x.h.Equals(min));
                    AstarCellInfo info = list[0];

                    Debug.Log("info.x : " + info.x + ", info.y : " + info.y);

                    if (info != null)
                    {
                        SetDirection(info);
                        if (info.Equals(m_EndCell))
                        {
                            m_eMoveEnd = eMoveCell.End;
                        }
                    }
                }
            }

            int count = 1;
            AstarCellInfo _info = m_EndCell.preAsterCell;
            while (_info != null)
            {
                _info.cellState = eCellState.Path;
                _info.f = count;
                m_CellList[_info.y][_info.x].SetData(_info);
                _info = _info.preAsterCell;
                count++;
                if (_info.Equals(m_StartCell)) return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            AstarCellInfo info;
            for (int i = 0; i < m_CellInfoList.Length; i++)
            {
                for (int j = 0; j < m_CellInfoList[i].Length; j++)
                {
                    info = m_CellInfoList[i][j];
                    info.SetDataExceptCell();
                    m_CellList[i][j].SetData(info);
                }
            }

            m_OpenCellList = new List<AstarCellInfo>();
            m_isClick = true;
            m_eMoveEnd = eMoveCell.Ready;
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

        AstarCellInfo cell = m_CellInfoList[_y][_x];
        switch (cell.cellState)
        {
            case eCellState.None:
                if (m_StartCell == null)
                {
                    cell.cellState = eCellState.Start;
                    m_StartCell = cell;
                }
                else if (m_EndCell == null)
                {
                    cell.cellState = eCellState.End;
                    m_EndCell = cell;
                }
                else
                {
                    cell.cellState = eCellState.Obstacle;
                }
                break;
            case eCellState.Start:
                cell.cellState = eCellState.None;
                break;
            case eCellState.Obstacle:
                cell.cellState = eCellState.None;
                break;
            case eCellState.End:
                cell.cellState = eCellState.None;
                break;
        }

        m_CellList[_y][_x].SetData(cell);
    }

    public void SetDirection(AstarCellInfo _info)
    {
        int x = _info.x;
        int y = _info.y;
        int sX = m_StartCell.x;
        int sY = m_StartCell.y;
        int eX = m_EndCell.x;
        int eY = m_EndCell.y;
        int count = 0;
        bool isDirectionChange = false;

        _info.openState = eOpenState.Close;

        int tempY = y - 1;
        AstarCellInfo[] arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }

            tempX = x;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }
        }

        tempY = y;
        arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }
        }

        tempY = y + 1;
        arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }

            tempX = x;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (info != null && !info.cellState.Equals(eCellState.Obstacle))
            {
                count++;
            }
        }

        if (count.Equals(8)) isDirectionChange = true;

        tempY = y - 1;
        arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info) && !(info.openState.Equals(eOpenState.Open) && !isDirectionChange))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.NorthWest;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }

            tempX = x;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.North;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info) && !(info.openState.Equals(eOpenState.Open) && !isDirectionChange))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.NorthEast;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }
        }

        tempY = y;
        arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.West;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.East;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }
        }

        tempY = y + 1;
        arrInfo = tempY < 0 || tempY > WIDTHCNT - 1 ? null : m_CellInfoList[tempY];
        if (arrInfo != null)
        {
            int tempX = x - 1;
            AstarCellInfo info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info) && !(info.openState.Equals(eOpenState.Open) && !isDirectionChange))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.SouthWest;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }

            tempX = x;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.South;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }

            tempX = x + 1;
            info = tempX < 0 || tempX > HEIGHTCNT - 1 ? null : arrInfo[tempX];
            if (IsChange(_info, info) && !(info.openState.Equals(eOpenState.Open) && !isDirectionChange))
            {
                if (info.h.Equals(0) || info.h > _info.h + 1)
                {
                    info.h = _info.h + 1;
                    info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
                    info.f = info.h + info.g;
                }
                info.openState = eOpenState.Open;
                info.direction = eDirection.SouthEast;
                info.preAsterCell = _info;
                m_OpenCellList.Add(info);
                m_CellList[tempY][tempX].SetDataView(info);
            }
        }

        #region 이전 코드
        //int tempY = y - 1;
        //AstarCellInfo[] arrInfo = tempY < 0 || tempY > widthCnt - 1 ? null : m_CellInfoList[tempY];
        //if (arrInfo != null)
        //{
        //    int tempX = x - 1;
        //    AstarCellInfo info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.NorthWest;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }

        //    tempX = x;
        //    info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.North;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }

        //    tempX = x + 1;
        //    info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.NorthEast;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }
        //}

        //tempY = y;
        //arrInfo = tempY < 0 || tempY > widthCnt - 1 ? null : m_CellInfoList[tempY];
        //if (arrInfo != null)
        //{
        //    int tempX = x - 1;
        //    AstarCellInfo info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.West;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }

        //    tempX = x + 1;
        //    info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.East;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }
        //}

        //tempY = y + 1;
        //arrInfo = tempY < 0 || tempY > widthCnt - 1 ? null : m_CellInfoList[tempY];
        //if (arrInfo != null)
        //{
        //    int tempX = x - 1;
        //    AstarCellInfo info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.SouthWest;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }

        //    tempX = x;
        //    info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y);
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.South;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }

        //    tempX = x + 1;
        //    info = tempX < 0 || tempX > heightCnt - 1 ? null : arrInfo[tempX];
        //    if (info != null && !info.eCellState.Equals(eCellState.Obstacle)
        //        && !info.eOpenState.Equals(eOpenState.Close))
        //    {
        //        info.h = Mathf.Abs(sX - info.x) + Mathf.Abs(sY - info.y) - 1;
        //        info.g = Mathf.Abs(info.x - eX) + Mathf.Abs(info.y - eY);
        //        info.f = info.h + info.g;

        //        info.eOpenState = eOpenState.Open;
        //        info.eDirection = eDirection.SouthEast;
        //        info.preAsterCell = _info;
        //        m_OpenCellList.Add(info);
        //        m_CellList[tempY][tempX].SetDataView(info);
        //    }
        //}
        #endregion

        m_OpenCellList.Remove(_info);
        m_CellList[y][x].SetDataView(_info);
    }

    private bool IsChange(AstarCellInfo _infoA, AstarCellInfo _infoB)
    {
        if (_infoB != null && !_infoB.cellState.Equals(eCellState.Obstacle)
                && !_infoB.openState.Equals(eOpenState.Close))
        {
            if (_infoB.preAsterCell == null)
            {
                return true;
            }

            if (_infoB.preAsterCell.Equals(m_StartCell))
            {
                return false;
            }

            if (_infoA.preAsterCell != null && _infoA.preAsterCell.Equals(m_StartCell) && _infoB.preAsterCell.h > _infoA.h)
            {
                return true;
            }

            if (_infoB.preAsterCell.h > _infoA.h)
            {
                return true;
            }

        }

        return false;
    }
}
