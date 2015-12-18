using UnityEngine;
using System.Collections;

public class BaseCamara : MonoBehaviour
{
    protected float m_pixelGain = 1.0f;	//MaximumHeight(ex:720) / CameraPixelHeight(depend on device)
    protected Rect m_touchArea;


    protected void SetTouchArea(Rect _area)
    {
        m_touchArea = _area;

        Debug.Log(name + "TOUCH AREA :::::::::: " + m_touchArea);
    }

    //STANDARD 해상도에 따라서 othographicSize를 변경.
    public void SetCameraOthSize()
    {
        float w;
        float h;
        
        w = Screen.width > Screen.height ? Screen.width : Screen.height;
        h = Screen.width < Screen.height ? Screen.width : Screen.height;

        //float size = w / (GlobalSetting.STANDARD_WIDTH * (h / GlobalSetting.STANDARD_HEIGHT));
        //camera.orthographicSize = size;
        GetComponent<Camera>().orthographicSize = 1;
        Debug.Log(name + "OTHOGRAPHIC SIZE :::::::::: " + GetComponent<Camera>().orthographicSize);

        GlobalSetting.CONTENT_HEIGHT = GlobalSetting.STANDARD_SIZE;
        GlobalSetting.CONTENT_WIDTH = ((float)w / (float)h) * GlobalSetting.CONTENT_HEIGHT;

        //GlobalSetting.FOOTER_SIZE = (GlobalSetting.CONTENT_WIDTH - 960.0f)/ 2.0f;
        GlobalSetting.FOOTER_SIZE = (GlobalSetting.CONTENT_WIDTH - (GlobalSetting.STANDARD_SIZE / 2 * 3)) / 2.0f;

        Debug.Log("CONTENT_WIDTH ::: " + GlobalSetting.CONTENT_WIDTH + ", HEIGHT ::: " + GlobalSetting.CONTENT_HEIGHT);

        Debug.Log("Screen w : " + w + ", h : " + h);

        UIRoot uiRoot = GameObject.Find("UI Root").GetComponent<UIRoot>();
        uiRoot.scalingStyle = UIRoot.Scaling.Flexible;
        uiRoot.minimumHeight = uiRoot.maximumHeight = (int)GlobalSetting.CONTENT_HEIGHT;
    }

    //STANDARD 해상도와 othgraphicSize에 따라서 mouse좌표에 대한 보정값(gain)을 결정.
    public void SetPixelGain()
    {
        float w;
        float h;

        w = GetComponent<Camera>().pixelWidth > GetComponent<Camera>().pixelHeight ? GetComponent<Camera>().pixelWidth : GetComponent<Camera>().pixelHeight;
        h = GetComponent<Camera>().pixelWidth < GetComponent<Camera>().pixelHeight ? GetComponent<Camera>().pixelWidth : GetComponent<Camera>().pixelHeight;
        m_pixelGain = GlobalSetting.STANDARD_SIZE / h * GetComponent<Camera>().orthographicSize;

        GlobalSetting.PIXEL_GAIN = m_pixelGain;

        //m_touchRegion = new Rect(120f * (1 / m_pixelGain)
        //    , 0f + (camera.pixelHeight - (camera.pixelWidth / 16f * 9f)) * 0.5f
        //    , 1030f * (1 / m_pixelGain)
        //    , 600f * (1 / m_pixelGain));

        m_touchArea = new Rect(0, 0, w, h);
    }
}
