using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrackingScript : BaseScript {
	private Text txtTitle;
	private Button btnBack;

	private RectTransform svContent = null;

	private ArrayList eventNameList;

	// Use this for initialization
	void Start () {
		// 标题
		txtTitle = GameObject.Find ("TxtTitle").GetComponent<Text>();
		txtTitle.text = "数据统计";

		// 返回按钮
		btnBack = GameObject.Find ("BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener (delegate {
			buttonClick(btnBack);
		});

		svContent = GameObject.Find ("SvContent").GetComponent<RectTransform>();

		eventNameList = Event.getEventNames ();

		initData ();
	}

	void initData() {
		RectTransform content = svContent.Find ("Viewport/Content").GetComponent<RectTransform> ();

		DefaultControls.Resources res = new DefaultControls.Resources ();
//		res.standard = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
//		res.standard = Resources.GetBuiltinResource(typeof(Sprite), "Resources/unity_builtin_extra/UISprite.psd") as Sprite;
		Vector2 btnSize = new Vector2 (200, 40);
		foreach (string eventName in eventNameList) {
			Button button = DefaultControls.CreateButton (res).GetComponent<Button> ();
//			button.image.sprite = res.standard;
			button.GetComponent<RectTransform> ().sizeDelta = btnSize;

			Text txtTitle = button.transform.Find ("Text").GetComponent<Text> ();
			txtTitle.fontSize = 16;
			txtTitle.text = eventName;

			button.onClick.AddListener (delegate {
				buttonClick(button);
			});
			button.transform.SetParent (content);
		}

		int margin = 10;

		content.sizeDelta = new Vector2(content.sizeDelta.x, 
			eventNameList.Count * (btnSize.y + margin));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void buttonClick(Button button) {
		if (button == btnBack) {
			SceneManager.LoadScene (1);
		} else {
			string eventName = button.transform.Find ("Text").GetComponent<Text> ().text;

			PlayerPrefs.SetString("tracking_event_name", eventName);
			PlayerPrefs.Save ();

			SceneManager.LoadScene (4);
		}
	}
}
