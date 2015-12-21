using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AstarCell : MonoBehaviour
{
    private const string NONECELL = "inactivation_tap";
    private const string STARTCELL = "activation_tap";
    private const string OBSTACLECELL = "exit";
    private const string ENDCELL = "advent_blue_name_squre";
    private const string PATHCELL = "quest_reward_button";

    public static int CellWidth { get { return cellWidth; } }
    public static int CellHeight { get { return cellHeight; } }

    private static int cellWidth;
    private static int cellHeight;

    private AstarCellInfo m_AstarCellInfo = new AstarCellInfo();
    public int CellX { get { return m_AstarCellInfo.x; } }
    public int CellY { get { return m_AstarCellInfo.y; } }
    public int Cellf { get { return m_AstarCellInfo.f; } }
    public int Cellg { get { return m_AstarCellInfo.g; } }
    public int Cellh { get { return m_AstarCellInfo.h; } }
    public eOpenState OpenState { get { return m_AstarCellInfo.openState; } }
    public eCellState CellState { get { return m_AstarCellInfo.cellState; } }

    private UISprite m_sprCell;
    private Transform m_Derection;

    private UILabel m_lblF;
    private UILabel m_lblG;
    private UILabel m_lblH;
    private UILabel m_lblOpenState;
    private UILabel m_lblDirection;

    private ButtonListener m_btnCell;

    private System.Action<int, int> m_actCellClick;
    public System.Action<int, int> ActCellClick { set { m_actCellClick = value; } }


    void Awake()
    {
        cellWidth = 50;
        cellHeight = 50;

        m_lblF = transform.FindChild("f").FindChild("label").GetComponent<UILabel>();
        m_lblG = transform.FindChild("g").FindChild("label").GetComponent<UILabel>();
        m_lblH = transform.FindChild("h").FindChild("label").GetComponent<UILabel>();
        m_lblOpenState = transform.FindChild("OpenState").FindChild("label").GetComponent<UILabel>();
        m_lblDirection = transform.FindChild("lblDirection").GetComponent<UILabel>();

        m_sprCell = gameObject.GetComponent<UISprite>();
        m_Derection = transform.FindChild("Direction");

        m_btnCell = gameObject.GetComponent<ButtonListener>();
        m_btnCell.OnClicked = OnClickCell;

        SetUI();
    }

    public void SetUI()
    {
        m_sprCell.width = cellWidth;
        m_sprCell.height = cellHeight;
    }

    public void SetData(AstarCellInfo _info)
    {
        m_AstarCellInfo = _info;
        m_lblF.text = ((m_AstarCellInfo.y * 10) + m_AstarCellInfo.x).ToString();
        m_lblG.text = "";
        m_lblH.text = "";
        m_lblOpenState.text = "";
        m_lblDirection.text = "";
        if (!m_AstarCellInfo.direction.Equals(eDirection.None))
        {
            m_Derection.gameObject.SetActive(true);
        }
        else
        {
            m_Derection.gameObject.SetActive(false);
        }
        SetState();
    }

    public void SetDataInitOnly(AstarCellInfo _info)
    {
        m_AstarCellInfo = _info;
        m_lblF.text = ((m_AstarCellInfo.y * 10) + m_AstarCellInfo.x).ToString();
        m_lblG.text = "";
        m_lblH.text = "";
        m_lblOpenState.text = "";
        m_lblDirection.text = "";
        if (!m_AstarCellInfo.direction.Equals(eDirection.None))
        {
            m_Derection.gameObject.SetActive(true);
        }
        else
        {
            m_Derection.gameObject.SetActive(false);
        }
        SetState();
    }

    public void SetDataView(AstarCellInfo _info)
    {
        m_AstarCellInfo = _info;
        m_lblF.text = m_AstarCellInfo.f.ToString();
        m_lblG.text = m_AstarCellInfo.g.ToString();
        m_lblH.text = m_AstarCellInfo.h.ToString();

        switch (m_AstarCellInfo.openState)
        {
            case eOpenState.Open:
                m_lblOpenState.color = Color.yellow;
                break;
            case eOpenState.Close:
                m_lblOpenState.color = Color.red;
                break;
        }

        m_Derection.gameObject.SetActive(true);
        float angle = 0;
        switch (m_AstarCellInfo.direction)
        {
            case eDirection.None:
                m_Derection.gameObject.SetActive(false);
                break;
            case eDirection.East:
                angle = -90;
                break;
            case eDirection.West:
                angle = 90;
                break;
            case eDirection.South:
                angle = 180;
                break;
            case eDirection.North:
                angle = 0;
                break;
            case eDirection.SouthEast:
                angle = -135;
                break;
            case eDirection.SouthWest:
                angle = 135;
                break;
            case eDirection.NorthEast:
                angle = -45;
                break;
            case eDirection.NorthWest:
                angle = 45;
                break;
        }
        m_Derection.localRotation = Quaternion.Euler(Vector3.forward * angle);
        m_lblDirection.text = m_AstarCellInfo.direction.ToString();

        m_lblOpenState.text = m_AstarCellInfo.openState.ToString();
        SetState();
    }

    private void OnClickCell(ButtonListener _btn)
    {
        if (m_actCellClick != null) m_actCellClick(m_AstarCellInfo.x, m_AstarCellInfo.y);

        SetState();
    }

    private void SetState()
    {
        switch (m_AstarCellInfo.cellState)
        {
            case eCellState.None:
                m_sprCell.spriteName = NONECELL;
                break;
            case eCellState.Start:
                m_sprCell.spriteName = STARTCELL;
                break;
            case eCellState.Obstacle:
                m_sprCell.spriteName = OBSTACLECELL;
                break;
            case eCellState.End:
                m_sprCell.spriteName = ENDCELL;
                break;
            case eCellState.Path:
                m_sprCell.spriteName = PATHCELL;
                break;
        }
    }
}
