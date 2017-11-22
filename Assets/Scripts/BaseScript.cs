using UnityEngine;
using System.Collections;
using System.Text;

public class BaseScript : MonoBehaviour
{
	private string title = "";
	private StringBuilder message = new StringBuilder();
	private string ok = "";
	private string cancel = "";
	private bool isShowDialog = false;

	private const float CONTENT_MARGIN_WIDTH = 20.0f;
	private const float CONTENT_MARGIN_HEIGHT = 30.0f;
	private Vector2 BUTTON_SIZE = new Vector2 (80, 30);
	private const float BUTTON_MARGIN = 40.0f;

	public delegate void DialogClick(bool isClickOk);
	private DialogClick dialogClick = null;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI() {
		if (isShowDialog) {
			Rect dialogRect = new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);

			GUIStyle style = GUI.skin.box;
//			style.normal.background.alphaIsTransparency = false;
			style.normal.textColor = Color.white;
			style.fontSize = 18;

			GUI.Box(dialogRect, title);

			style= GUI.skin.label;
			style.normal.textColor = Color.white;
			style.fontSize = 14;
			GUI.Label(new Rect(dialogRect.x + CONTENT_MARGIN_WIDTH, dialogRect.y + CONTENT_MARGIN_HEIGHT, 
				dialogRect.width - CONTENT_MARGIN_WIDTH * 2, dialogRect.height - CONTENT_MARGIN_HEIGHT * 2 - BUTTON_SIZE.y), message.ToString(), style);

			float x = dialogRect.x + (dialogRect.width - BUTTON_SIZE.x) / 2;
			if (! string.IsNullOrEmpty (cancel)) {
				x = dialogRect.x + (dialogRect.width - BUTTON_SIZE.x * 2 - BUTTON_MARGIN) / 2;
			}

			if (GUI.Button(new Rect(new Vector2(x, dialogRect.y + dialogRect.height - BUTTON_MARGIN),  BUTTON_SIZE), ok)) {
				message = new StringBuilder ();
				isShowDialog = false;
				if (dialogClick != null)
					dialogClick (true);
			}

			if (! string.IsNullOrEmpty (cancel)) {
				Vector2 buttonPosition = new Vector2 (x + BUTTON_SIZE.x + BUTTON_MARGIN, dialogRect.y + dialogRect.height - BUTTON_MARGIN);
				if (GUI.Button(new Rect(buttonPosition,  BUTTON_SIZE), cancel)) {
					message = new StringBuilder ();
					isShowDialog = false;
					if (dialogClick != null)
						dialogClick (false);
				}
			}
		}
	}

	public void showDialog(string title, string message, string ok, DialogClick dialogClick = null) {
		this.title = title;
		this.message.AppendFormat("{0}\n", message);
		this.ok= ok;
		this.dialogClick = dialogClick;
		isShowDialog = true;
	}

	public void showDialog(string title, string message, string ok, string cancel, DialogClick dialogClick) {
		this.cancel= cancel;
		showDialog(title, message, ok, dialogClick);
	}
}

