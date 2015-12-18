using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelMain : MonoBehaviour
{
    private static PanelMain m_instance;
    public static PanelMain Instance
    {
        get
        {
            m_instance = FindObjectOfType<PanelMain>();
            if(m_instance == null)
            {
                Debug.LogError("PanelMain Instance NULL!!!");
            }

            return m_instance;
        }
    }

    void Awake()
    {

    }

    private void SetUI()
    {

    }

}
