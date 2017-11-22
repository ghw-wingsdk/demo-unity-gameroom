using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WASdk.Unity;
using System.Text;

public class LoginScript : BaseScript {
	private Text txtTitle;
	private Button btnClose;
	private Button btnFacebookLogin;

	void Awake () {
//		WACoreProxy.SetDebugMode (true);
		WACoreProxy.Initialize ();
		WACoreProxy.SetGameUserId ("123456");
		WACoreProxy.SetServerId ("China");
		WACoreProxy.SetLevel (11);
	}

	// Use this for initialization
	void Start () {
////		WACoreProxy.SetDebugMode (true);
//		WACoreProxy.Initialize ();
//		WACoreProxy.SetGameUserId ("123456");
//		WACoreProxy.SetServerId ("China");
//		WACoreProxy.SetLevel (11);

		txtTitle = GameObject.Find ("TxtTitle").GetComponent<Text>();
		txtTitle.text = "登录";

		// 关闭程序按钮
		btnClose = GameObject.Find ("BtnClose").GetComponent<Button>();
		btnClose.onClick.AddListener (delegate {
			buttonClick(btnClose);
		});

		btnFacebookLogin = GameObject.Find ("BtnFacebookLogin").GetComponent<Button>();
		btnFacebookLogin.onClick.AddListener (delegate {
			buttonClick(btnFacebookLogin);
		});

		GameObject.Find ("TxtClientId").GetComponent<Text> ().text = "clientId: " + WACoreProxy.GetClientId();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void buttonClick(Button button) {
		if (button == btnClose) { // 关闭程序
			showDialog("提示", "确定要退出程序吗？", "确定", delegate(bool isClickOk) {
				Application.Quit();
			});
		} else if (button == btnFacebookLogin) { // 登录
//			AsyncOperation loginOperation = SceneManager.LoadSceneAsync ("MainScene", LoadSceneMode.Additive);
//			loginOperation.allowSceneActivation = true;
			string extInfo = "";
            WAUserProxy.Login(WAChannelType.CHANNEL_FACEBOOK, extInfo, delegate (WALoginResult result)
            {
            //    WAUserProxy.Login(WAChannelType.CHANNEL_WA, extInfo, delegate (WALoginResult result)
            //{
                if (result.Code == WAStatusCode.CODE_SUCCESS) {
					StringBuilder loginInfo = new StringBuilder ();
					loginInfo.AppendFormat("Platform:{0}", result.Platform);
					loginInfo.AppendFormat("\nWaUserId:{0}", result.WaUserId);
					loginInfo.AppendFormat("\nWaToken:{0}", result.WaToken);
					loginInfo.AppendFormat("\nPlatformUserId:{0}", result.PlatformUserId);
					loginInfo.AppendFormat("\nPlatformToken:{0}", result.PlatformToken);
					loginInfo.AppendFormat("\nIsBindMobile:{0}", result.IsBindMobile);
					loginInfo.AppendFormat("\nIsFirstLogin:{0}", result.IsFirstLogin);

					showDialog("提示", loginInfo.ToString (), "确定", delegate(bool isClickOk) {
						// 保存登录用户信息
						PlayerPrefs.SetString("WALoginUserDetail", loginInfo.ToString());
						PlayerPrefs.Save();

						SceneManager.LoadScene (1);
					});
				} else {
					string msg = result.Msg;
					if (string.IsNullOrEmpty(msg))
						msg = "登录错误！";


					showDialog("提示", msg, "确定", null);
				}
			});
		}
	}
}
