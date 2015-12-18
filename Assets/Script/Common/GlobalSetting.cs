using UnityEngine;
using System.Collections;
using System.Text;

public class GlobalSetting
{
    public static Font FONT_KOR_GODOB;
    public static Font FONT_JPN_MTLMR3M;

    //iphone 4, 4s: 960x640
    //iphone 5, 5s : 1136x640
    //iPhone 6 : 1334x750
    //iphone 6 plus : 1920x1080

    public static float STANDARD_SIZE = 640.0f;

    public static float CONTENT_WIDTH;
    public static float CONTENT_HEIGHT;

    public static float FOOTER_SIZE;

    public static float PIXEL_GAIN;

    public static float LONG_PRESS_TIME = 0.5f;

    public static int MAX_FRIEND_COUNT = 50;

    public static int MAX_RARITY = 5;

    public static int MAX_DOCK_COUNT = 9;
    public static int MAX_TILE_COUNT = 38;      //타일의 전체 크기 (38X38)
    public static int TILE_BORDER_AREA = 2;     //로비에서 건설 불가능한 테두리 영역의 두께

    public static bool TouchDown(int _touchIndex)
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonDown(_touchIndex);
#elif UNITY_ANDROID || UNITY_IPHONE
        return Input.touchCount > _touchIndex && Input.GetTouch(_touchIndex).phase == TouchPhase.Began;
#else
        return Input.GetMouseButtonDown(_touchIndex);
#endif
    }

    public static bool TouchUp(int _touchIndex)
    {
#if UNITY_EDITOR
        return Input.GetMouseButtonUp(_touchIndex);
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount > _touchIndex)
        {
            Touch touch = Input.GetTouch(_touchIndex);
            return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        else
        {
            return false;
        }
#else
        return Input.GetMouseButtonUp(_touchIndex);
#endif
    }

    public static Vector3 TouchPosition(int _touchIndex)
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IPHONE
        return Input.GetTouch(_touchIndex).position;
#else
        return Input.mousePosition;
#endif
    }

    //c8c8c8
    public static Color ColorGray = new Color(200f / 255f, 200f / 255f, 200f / 255f);

    public static Color ColorHit = Color.white;
    public static Color ColorFreeze = new Color(0f / 255f, 200f / 255f, 255f / 255f);

    //use Village
    public static Color ColorApricot    = new Color(238f / 255f, 230f / 255f, 165f / 255f);
    public static Color ColorYellow     = new Color(251f / 255f, 236f / 255f,  49f / 255f);
    public static Color ColorOrange     = new Color(253f / 255f, 156f / 255f,   0f / 255f);
    public static Color ColorBlueGreen  = new Color( 52f / 255f, 244f / 255f, 158f / 255f);
    public static Color ColorBrown      = new Color( 89f / 255f,  57f / 255f,   8f / 255f);
    public static Color ColorRed        = new Color(255f / 255f,  48f / 255f,  38f / 255f);
    public static Color ColorDarkGray   = new Color(77f / 255f, 77f / 255f, 77f / 255f);

    //2015/08/11
    public static Color ColorGold = new Color(255f / 255f, 195f / 255f, 97f / 255f);
    public static Color ColorGoldInActivate = new Color(255f / 255f, 195f / 255f, 97f / 255f, 0.5f);


    private static StringBuilder m_sb;
    // 6C3D13
    public static string BrownColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[6C3D13]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // 00FCAB
    public static string TintColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[00FCAB]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // FFEE2F
    public static string YellowColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[FFEE2F]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // 266D31
    public static string GreenColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[266D31]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // FFE9A5
    public static string FleshColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[FFE9A5]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // E1AE01
    public static string OrangeColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[E1AE01]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // ff0000
    public static string RedColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[ff0000]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // 8F5706
    public static string LightBrownColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[8F5706]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // B1B5BE
    public static string BlueGrayColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[B1B5BE]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }

    // FF85B0
    public static string PinkColor(string _str)
    {
        m_sb = new StringBuilder();
        m_sb.Append("[FF85B0]");
        m_sb.Append(_str);
        m_sb.Append("[-]");
        return m_sb.ToString();
    }


    //Use Battle
    public static readonly Color C_Fire = new Color(248f / 255f, 46f / 255f, 20f / 255f); // 공격. 붉은색
    public static readonly Color C_Water = new Color(35f / 255f, 111f / 255f, 237f / 255f); // 방어. 파란색
    public static readonly Color C_Wind = new Color(5f / 255f, 200f / 255f, 87f / 255f); // 회복. 녹색
    public static readonly Color C_Light = new Color(255f / 255f, 198f / 255f, 0f / 255f); // 버프. 노란색
    public static readonly Color C_Dark = new Color(156f / 255f, 44f / 255f, 255f / 255f); // 디버프. 보라색
    public static readonly Color ColorTrajectoryWhite = new Color(1f, 1f, 1f, 0.5f);
    public static readonly Color ColorTrajectoryRed = new Color(1f, 0f, 0f, 0.5f);





    //Village Depth Info
    //추가되는 사항 작성바랍니다.
    public static int DEPTH_TOWN_EDIFICE_STATE_VIEW = 70;
    public static int DEPTH_TOWN_BUTTON_UI = 80; 
    public static int DEPTH_TOWN_SUBPAGE = 100; //조선소, 강화소
    public static int DEPTH_TOWN_MAIN_MENU = 200; //사용자 프로필이나 골드 보석 등등(뒤로가기 버튼 포함)
    public static int DEPTH_LOADING = 299;
    public static int DEPTH_TOWN_SUBPAGE_POPUP = 300; //조선소나 강화소에서 팝업형태의 View Depth

    public static int DEPTH_PROLOGUE = 700;
    public static int DEPTH_TOWN_POPUP = 900; // 시스템 팝업

}
