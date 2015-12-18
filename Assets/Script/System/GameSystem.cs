using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum eSceneType
{
    Non,
    Intro,
    Title,
    VillageLoading,
    Village,
    WorldMap,
    ActMap,
    Battle
    //ArenaTown
}

public class GameSystem : MonoBehaviour
{
    private static GameSystem s_instance;
    static public GameSystem Instance
    {
        get
        {
            if (null == s_instance)
            {
                Debug.LogError("Failed To Find GameSystem Instance");
            }
            return s_instance;
        }
    }

    //*ForTest 

    [HideInInspector] public bool RunClearPanels = false;

    private eSceneType m_currentSceneType;
    public eSceneType CurrentScene { get { return m_currentSceneType; } }

    // blocker - 씬 전환 시 Fade In, Out 연출.
    private GameObject blocker;
    private float m_hideBlockerDuration;
    public delegate void FinishShowBlocker();
    public FinishShowBlocker OnFinishShowBlocker;
    public delegate void FinishHideBlocker();
    public FinishHideBlocker OnFinishHideBlocker;

    public Action actOnSceneChanged = null;

    private Transform m_tfUIRoot = null;
    public Transform tfUIRoot { get { return m_tfUIRoot; } }

    public static bool isPreWorldMapLoad = true;
    public static bool isQAMode = true;
    public static bool isTest = true;

    private static List<string> m_listPrefabKey = new List<string>();
    private static Dictionary<string, UnityEngine.Object> m_dicPrefab = new Dictionary<string, UnityEngine.Object>();

    static public GameObject InstantiateAndAddTo(string _resPath, string _resName, GameObject _parent, bool _hasPanelinfo = true)
    {
        //UnityEngine.Object res = Resources.Load(_resPath + _resName);

        // Prefab Asset Bundle 활용시 삭제될 코드 by KCWON [Start] 
        if (m_dicPrefab.ContainsKey(_resName) == false)
        {
            //Debug.Log("[GameSystem] InstantiateAndAddTo() ------------------- Add " + _resName + ".Prefab");
            m_dicPrefab.Add(_resName, Resources.Load(_resPath + _resName));
            m_listPrefabKey.Add(_resName);
            if (m_dicPrefab.Keys.Count > 50)
            {
                m_dicPrefab.Remove(m_listPrefabKey[0]);
                m_listPrefabKey.RemoveAt(0);
            }
        }
        UnityEngine.Object res = (UnityEngine.Object)m_dicPrefab[_resName] as UnityEngine.Object;
        // Prefab Asset Bundle 활용시 삭제될 코드 by KCWON [End]

        if (null == res)
        {
            return null;
        }

        GameObject obj = Instantiate(res) as GameObject;

        if (_hasPanelinfo)
        {
        }

        Vector3 localPos = obj.transform.localPosition;
        Vector3 localScale = obj.transform.localScale;

        if (_parent!= null)
            obj.transform.parent = _parent.transform;

        obj.transform.localPosition = localPos;
        obj.transform.localScale = localScale;
        
        //if (null != TutorialManager.Instance)
        //{
        //    TutorialManager.Instance.CheckAndStartLoadPanel(_resName);
        //}

        //Debug.Log("INSTANTIATE : " + obj.name + " " + obj.transform.localPosition + " " + obj.transform.localScale);
        return obj;
    }
}
