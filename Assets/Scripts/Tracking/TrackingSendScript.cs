using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using WASdk.Unity;

public class TrackingSendScript : BaseScript {
	private Text txtTitle;
	private Button btnBack;
	private Button btnSend;
	private Button btnWA;
	private Button btnFacebook;

	private RectTransform panelContent;
	private RectTransform svWA;
	private RectTransform svFacebook;
	private Button btnWAAdd;
	private Button btnFacebookAdd;

	private string trackingEventName;

	// Use this for initialization
	void Start () {
		// 标题
		txtTitle = GameObject.Find ("PanelTitle/TxtTitle").GetComponent<Text>();
		txtTitle.text = "数据统计";

		// 返回按钮
		btnBack = GameObject.Find ("PanelTitle/BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener (delegate {
			buttonClick(btnBack);
		});

		// 发送按钮
		btnSend = GameObject.Find ("PanelTitle/BtnSend").GetComponent<Button>();
		btnSend.onClick.AddListener (delegate {
			buttonClick(btnSend);
		});

		// WA
		btnWA = GameObject.Find ("PanelTab/BtnWA").GetComponent<Button>();
		btnWA.interactable = false;
		btnWA.onClick.AddListener (delegate {
			buttonClick(btnWA);
		});

		// Facebook
		btnFacebook = GameObject.Find ("PanelTab/BtnFacebook").GetComponent<Button>();
		btnFacebook.onClick.AddListener (delegate {
			buttonClick(btnFacebook);
		});

		panelContent = GameObject.Find ("PanelContent").GetComponent<RectTransform>();

		RectTransform svContent = panelContent.Find ("SvContent").GetComponent<RectTransform>();
		svContent.SetParent (null);

		svWA = Instantiate (svContent);
		svWA.SetParent (panelContent);
		svWA.gameObject.SetActive (true);

		svFacebook = Instantiate (svContent);
		svFacebook.SetParent (panelContent);
		svFacebook.gameObject.SetActive (false);

		// 事件名称
		trackingEventName = PlayerPrefs.GetString ("tracking_event_name");
		PlayerPrefs.DeleteKey ("tracking_event_name");
		PlayerPrefs.Save ();

		initWAData ();
		initFacebookData ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**
	  * WA 预定义事件
	  */
	void initWAData() {
		bool isCustomEvent = WAEventType.CUSTOM_EVENT_PREFIX.Equals (trackingEventName);

		RectTransform waContent = svWA.Find ("Viewport/Content").GetComponent<RectTransform> ();

		// 事件名称
		waContent.Find ("TxtEventName").GetComponent<Text> ().text = "事件名称: " + trackingEventName;

		// value
		waContent.Find ("PanelValue/IFValue").GetComponent<InputField> ().text = "0.0";

		// 事件参数 模板
		RectTransform waItem = waContent.Find ("PanelItem").GetComponent<RectTransform> ();
		waItem.SetParent (null);

		// 添加按钮
		RectTransform panelAdd = waContent.Find ("PanelAdd").GetComponent<RectTransform>();
		// 移除添加按钮到界面
		panelAdd.transform.SetParent (null);

		btnWAAdd = panelAdd.Find ("BtnAdd").GetComponent<Button>();
		// 如果是WA自定义事件隐藏添加按钮
		btnWAAdd.gameObject.SetActive (isCustomEvent);
		btnWAAdd.onClick.AddListener (delegate {
			buttonClick(btnWAAdd);
		});

		Dictionary<string, object> eventParamNames = Event.getEventParamNames (trackingEventName);

		RectTransform waItemCell;
		InputField waIfParamTitle;
		InputField waIfParamValue;
		foreach (string eventParamName in eventParamNames.Keys) {
			waItemCell = Instantiate (waItem);
			waItemCell.transform.SetParent (waContent);

			waIfParamTitle = waItemCell.Find ("PanelParam/IfParamNameTitle").GetComponent<InputField> ();
			waIfParamTitle.text = eventParamName;
			waIfParamTitle.interactable = isCustomEvent;

			waIfParamValue = waItemCell.Find ("PanelParam/IfParamValue").GetComponent<InputField> ();

			object value = eventParamNames [eventParamName];
			if (typeof(int).IsInstanceOfType (value) || typeof(long).IsInstanceOfType (value)) {
				waIfParamValue.contentType = InputField.ContentType.IntegerNumber;
			} else if (typeof(float).IsInstanceOfType (value)) {
				waIfParamValue.contentType = InputField.ContentType.DecimalNumber;
			}
			waIfParamValue.text = "" + value;

			Button btnDel = waItemCell.Find ("BtnDel").GetComponent<Button> ();
			btnDel.gameObject.SetActive (isCustomEvent);
			btnDel.onClick.AddListener(
				delegate {
					buttonClick(btnDel);
				}
			);
		}

		// 添加添加按钮图层到界面
		panelAdd.transform.SetParent (waContent);

		// 修改可滑动区域
		waContent.sizeDelta = new Vector2 (waContent.sizeDelta.x, 
			(eventParamNames.Keys.Count + (isCustomEvent ? 4 : 3)) * waItem.sizeDelta.y);
	}

	/** 
	 * Facebook事件
	 */
	void initFacebookData() {
		RectTransform faceBookContent = svFacebook.Find ("Viewport/Content").GetComponent<RectTransform>();

		// 事件名称
		faceBookContent.Find ("TxtEventName").GetComponent<Text>().text = "事件名称: " + trackingEventName;

		// value
		faceBookContent.Find ("PanelValue/IFValue").GetComponent<InputField> ().text = "0.0";

		// 添加删除事件
		Button btnDel = faceBookContent.Find ("PanelItem/BtnDel").GetComponent<Button> ();
		btnDel.onClick.AddListener(
			delegate {
				buttonClick(btnDel);
			}
		);

		// 添加按钮
		btnFacebookAdd = faceBookContent.Find ("PanelAdd/BtnAdd").GetComponent<Button>();
		btnFacebookAdd.onClick.AddListener (delegate {
			buttonClick(btnFacebookAdd);
		});
	}

	/** 
	 * 添加文本框
	 */
	void addParamField(RectTransform content) {
		// 事件参数模板
		RectTransform item = content.GetChild (3).GetComponent<RectTransform> ();

		// 添加按钮
		RectTransform panelAdd = content.Find ("PanelAdd").GetComponent<RectTransform>();
		panelAdd.transform.SetParent (null);

		RectTransform itemCell = Instantiate (item);
		itemCell.transform.SetParent(content);

		itemCell.Find ("PanelParam/IfParamNameTitle").GetComponent<InputField> ().text = "";
		itemCell.Find ("PanelParam/IfParamValue").GetComponent<InputField> ().text = "";

		Button btnDel = itemCell.Find ("BtnDel").GetComponent<Button> ();
		btnDel.onClick.AddListener(
			delegate {
				buttonClick(btnDel);
			}
		);

		panelAdd.transform.SetParent (content);

		content.sizeDelta = new Vector2(content.sizeDelta.x, 
			content.sizeDelta.y + item.sizeDelta.y);
	}

	void buttonClick(Button button) {
		if (button == btnBack) {		// 返回按钮
			SceneManager.LoadScene (3);
		} else if (button == btnSend) { // 发送按钮
			WAEvent.Builder builder = new WAEvent.Builder();

			// 数据组装
			RectTransform waContent = svWA.Find ("Viewport/Content").GetComponent<RectTransform>();

			float waValue = 0.0f;
			float.TryParse(waContent.Find ("PanelValue/IFValue").GetComponent<InputField> ().text, out waValue);

			string paramName;
			string paramValue;
			RectTransform itemCell;
			Dictionary<string, object> waParamDict = new Dictionary<string, object> ();
			// WA
			for (int i = 3; i < waContent.childCount - 1; i++) {
				itemCell = waContent.GetChild (i).GetComponent<RectTransform> ();
				paramName = itemCell.Find ("PanelParam/IfParamNameTitle").GetComponent<InputField> ().text;
				paramValue = itemCell.Find ("PanelParam/IfParamValue").GetComponent<InputField> ().text;

				if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(paramValue))
					waParamDict.Add (paramName, paramValue);
			}
			// 添加默认事件
			builder.SetDefaultEventName(trackingEventName);
			builder.SetDefaultValue (waValue);
			builder.SetDefaultEventValues (waParamDict);

			// 添加WA事件
			builder.SetChannelEventName(WAChannelType.CHANNEL_WA, trackingEventName);
			builder.SetChannelValue (WAChannelType.CHANNEL_WA, waValue);
			builder.SetChannelEventValues(WAChannelType.CHANNEL_WA, waParamDict);

			// Facebook
			Dictionary<string, object> fbParamDict = new Dictionary<string, object> ();

			RectTransform faceBookContent = svFacebook.Find ("Viewport/Content").GetComponent<RectTransform>();

			for (int i = 3; i < faceBookContent.childCount - 1; i++) {
				itemCell = faceBookContent.GetChild (i).GetComponent<RectTransform> ();
				paramName = itemCell.Find ("PanelParam/IfParamNameTitle").GetComponent<InputField> ().text;
				paramValue = itemCell.Find ("PanelParam/IfParamValue").GetComponent<InputField> ().text;

				if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(paramValue))
					fbParamDict.Add (paramName, paramValue);
			}

			// 添加Facebook事件
			float fbValue = 0.0f;
			float.TryParse(faceBookContent.Find ("PanelValue/IFValue").GetComponent<InputField> ().text, out fbValue);

			builder.SetChannelEventName(WAChannelType.CHANNEL_FACEBOOK, trackingEventName);
			builder.SetChannelValue(WAChannelType.CHANNEL_FACEBOOK, fbValue);
			if (fbParamDict.Count > 0) {
				builder.SetChannelEventValues(WAChannelType.CHANNEL_FACEBOOK, fbParamDict);
			}

			// 发送事件
			WATrackProxy.TrackEvent(builder.Build());
		} else if (button == btnWA) { 			// Tab WA按钮
			btnWA.interactable = false;
			btnFacebook.interactable = true;

			svWA.gameObject.SetActive (true);
			svFacebook.gameObject.SetActive (false);
		} else if (button == btnFacebook) {		// Tab Facebook按钮
			btnWA.interactable = true;
			btnFacebook.interactable = false;

			svWA.gameObject.SetActive (false);
			svFacebook.gameObject.SetActive (true);
		} else if (button == btnWAAdd) { 		// WA新增按钮
			RectTransform waContent = svWA.Find ("Viewport/Content").GetComponent<RectTransform>();
			addParamField (waContent);
		} else if (button == btnFacebookAdd) { 	// facebook新增按钮
			RectTransform faceBookContent = svFacebook.Find ("Viewport/Content").GetComponent<RectTransform>();
			addParamField (faceBookContent);
		} else { 								// 删除按钮
			int count = button.transform.parent.parent.childCount;
			if (count > 5) {
				button.transform.parent.transform.SetParent (null);
			} else {
				Debug.Log ("至少要有1个参数");
			}
		}
	}
}
