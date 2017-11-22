using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WASdk.Unity;
using System.Text;

public class MainScript : BaseScript {
	private Text txtTitle;
	private Button btnClose;
	private Button btnPay;
	private Button btnTracking;
	private Button btnLogout;
	private Button btnLoginUserDetail;

	// Use this for initialization
	void Start () {
		// 标题
		txtTitle = GameObject.Find ("TxtTitle").GetComponent<Text>();
		txtTitle.text = "WADemo 3.6.5.1";

		// 关闭程序按钮
		btnClose = GameObject.Find ("BtnClose").GetComponent<Button>();
		btnClose.onClick.AddListener (delegate {
			buttonClick(btnClose);
		});

		// 支付
		btnPay = GameObject.Find ("BtnPay").GetComponent<Button>();
		btnPay.onClick.AddListener (delegate {
			buttonClick(btnPay);
		});

		// 数据统计
		btnTracking = GameObject.Find ("BtnTracking").GetComponent<Button>();
		btnTracking.onClick.AddListener (delegate {
			buttonClick(btnTracking);
		});

		// 登出
		btnLogout = GameObject.Find ("BtnLogout").GetComponent<Button>();
		btnLogout.onClick.AddListener (delegate {
			buttonClick(btnLogout);
		});

		// 登录用户信息
		btnLoginUserDetail = GameObject.Find ("BtnLoginUserDetail").GetComponent<Button>();
		btnLoginUserDetail.onClick.AddListener (delegate {
			buttonClick(btnLoginUserDetail);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 点击事件
	void buttonClick(Button button) {

		if (button == btnClose) { // 关闭程序
			showDialog("提示", "确定要退出程序吗？", "确定", delegate(bool isClickOk) {
				Application.Quit();
			});
		} else if (button == btnPay) { // 支付
			AsyncOperation loginOperation = SceneManager.LoadSceneAsync ("PayScene", LoadSceneMode.Additive);
			loginOperation.allowSceneActivation = true;
		} else if (button == btnTracking) {  // 数据统计
			AsyncOperation loginOperation = SceneManager.LoadSceneAsync ("TrackingScene", LoadSceneMode.Additive);
			loginOperation.allowSceneActivation = true;
		} else if (button == btnLogout) { // 登出
			showDialog("提示", "确定要退出登录吗？", "确定", delegate(bool isClickOk) {
				if (isClickOk) {
					WAUserProxy.LogOut ();

					SceneManager.LoadScene (0);
				}
			});
		} else if (button == btnLoginUserDetail) { // 登录用户信息
			StringBuilder info = new StringBuilder ();
			info.AppendFormat ("ClientId:{0}", WACoreProxy.GetClientId());
			info.AppendFormat ("\n{0}", PlayerPrefs.GetString("WALoginUserDetail"));

			showDialog("提示", info.ToString (), "确定", null);
		}
	}
}
