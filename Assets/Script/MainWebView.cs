/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.Collections;
using UnityEngine;

public class MainWebView : MonoBehaviour
{
	public string Url;
	public string SameDomainUrl;
	public GUIText status;
	WebViewObject webViewObject;

    private UISprite m_sprCenter;
    private ButtonListener m_btnBack;
    private ButtonListener m_btnClose;


    void Awake()
    {
        m_sprCenter = transform.FindChild("sprCenter").GetComponent<UISprite>();
        m_btnBack = transform.FindChild("btnBack").GetComponent<ButtonListener>();
        m_btnClose = transform.FindChild("btnClose").GetComponent<ButtonListener>();
        m_btnBack.index = 0;
        m_btnClose.index = 1;
        m_btnBack.OnClicked = OnClickBtn;
        m_btnClose.OnClicked = OnClickBtn;

        m_btnBack.transform.localPosition = new Vector3(-(Screen.width / 2 - 50), Screen.height / 2 - 50, 0);
        m_btnClose.transform.localPosition = new Vector3(Screen.width / 2 - 51, Screen.height / 2 - 52, 0);
        m_sprCenter.transform.localPosition = Vector3.up * (Screen.height / 2 - 52);
        m_sprCenter.width = Screen.width;

        webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) =>
        {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
            status.text = msg;
            status.GetComponent<Animation>().Play();
        });

        webViewObject.SetMargins(0, 100, 0, 0);
        webViewObject.SetVisibility(true);


        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                //webViewObject.LoadURL(" https://chro-dra.marv.jp/game_news/");
                webViewObject.LoadURL(" http://www.naver.com/");
                if (Application.platform != RuntimePlatform.Android)
                {
                    webViewObject.EvaluateJS(
                        "window.addEventListener('load', function() {" +
                        "	window.Unity = {" +
                        "		call:function(msg) {" +
                        "			var iframe = document.createElement('IFRAME');" +
                        "			iframe.setAttribute('src', 'unity:' + msg);" +
                        "			document.documentElement.appendChild(iframe);" +
                        "			iframe.parentNode.removeChild(iframe);" +
                        "			iframe = null;" +
                        "		}" +
                        "	}" +
                        "}, false);");
                }
                break;
            case RuntimePlatform.OSXWebPlayer:
            case RuntimePlatform.WindowsWebPlayer:
                webViewObject.LoadURL(" https://chro-dra.marv.jp/game_news/");
                webViewObject.EvaluateJS(
                    "parent.$(function() {" +
                    "	window.Unity = {" +
                    "		call:function(msg) {" +
                    "			parent.unityWebView.sendMessage('WebViewObject', msg)" +
                    "		}" +
                    "	};" +
                    "});");
                break;
        }
    }

    private void OnClickBtn(ButtonListener _btn)
    {
        switch(_btn.index)
        {
            case 0:
                webViewObject.PageBack();
                break;
            case 1:
                Destroy(webViewObject);
                Destroy(gameObject);
                break;
        }
    }
}
