using UnityEngine;


public class AndroidManager : MonoBehaviour
{
    private static AndroidManager s_instance;
#if UNITY_EDITOR
#elif UNITY_ANDROID
    private AndroidJavaObject AndroidPlugin = null;
#endif

    public static AndroidManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameObject("AndroidManager").AddComponent<AndroidManager>();
            }
            return s_instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidJavaClass androidClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        if (androidClass != null)
        {
            AndroidPlugin = androidClass.GetStatic<AndroidJavaObject>("currentActivity");
            androidClass.Dispose();
        }
#endif
        GetGCMRegisterId();
        NotificationTest();
    }

    private void CallAndroid(string _methodName, params object[] _args)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (AndroidPlugin != null)
        {
            AndroidPlugin.Call(_methodName, _args);
        }
#endif
    }
    
    //Notification
    public void NotificationTest()
    {
        CallAndroid("notificationTest");
    }

    //Version
    public void CheckVersion()
    {
        CallAndroid("getVersionName", name, "CheckVersionCallBack");
    }

    public void CheckVersionCallBack(string _str)
    {
        Debug.Log("CheckVersionCallBack : " + _str);
    }

    //Network
    public void CheckNetwork()
    {
        CallAndroid("getNetworkInfo", name, "CheckNetworkCallBack");
    }

    public void CheckNetworkCallBack(string _str)
    {
        Debug.Log("CheckNetworkCallBack : " + _str + " (0 3g, 1 wifi, 2 null)");

        //phoneNetwork = _str;
        //if ("2" == _str)
        //{
        //    GameSystem.Instance.LoadErrorExitNetworkPopup();
        //}
    }

    //GCM
    public void GetGCMRegisterId()
    {
        CallAndroid("getGCMRegisterId", name, "GetGCMRegisterIdCallBack");
    }

    public void GetGCMRegisterIdCallBack(string _str)
    {
        Debug.Log("gcmRegisterId ::::: " + _str);
    }

    //token
    public void GetCurrentDeviceId()
    {
        CallAndroid("getCurrentDeviceId", name, "GetCurrentDeviceIdCallBack");
    }

    public void GetCurrentDeviceIdCallBack(string _str)
    {
        Debug.Log("currentDeviceId ::::: " + _str);
    }
}