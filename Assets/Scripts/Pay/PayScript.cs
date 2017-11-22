using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WASdk.Unity;
using System.Text;

public class PayScript : BaseScript {
	private Text txtTitle;
	private Button btnBack;
	private RectTransform svContent;
	private List<WAProduct> products = null;

	// Use this for initialization
	void Start () {
		txtTitle = GameObject.Find ("TxtTitle").GetComponent<Text>();
		txtTitle.text = "支付";

		btnBack = GameObject.Find ("BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener (delegate {
			buttonClick(btnBack);
		});

		svContent = GameObject.Find ("SvContent").GetComponent<RectTransform>();

		// 初始化支付
		WAPayProxy.Initialize (delegate(WAResultBase result) {
			if (result.Code == WAStatusCode.CODE_SUCCESS) { // 支付初始化成功
				// 查询商品列表
				WAPayProxy.QueryInventory (delegate(WAProductResult productResult) {

					if (productResult.Code == WAStatusCode.CODE_SUCCESS) { // 查询商品列表成功
						products = productResult.Products;
						initData ();
					} else {											   // 查询商品列表失败
						showDialog("提示", productResult.Msg, "确定", delegate(bool isClickOk) {
							buttonClick(btnBack);
						});
					}
				});
			} else {										// 支付初始化失败
				showDialog("提示", result.Msg, "确定", null);
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initData() {

		RectTransform content = svContent.Find ("Viewport/Content").GetComponent<RectTransform>();

		// 样子item
		RectTransform item = content.Find("Item").GetComponent<RectTransform>();
		// 移除item
		item.transform.SetParent (null);

		for (int i = 0; i < products.Count; i++) {
			// 克隆Item
			RectTransform itemCell = Instantiate (item);
			// 设置商品文字
			itemCell.Find ("TxtTitle").GetComponent<Text>().text = products [i].ProductName;
			// 购买按钮
			Button btnBuy = itemCell.Find ("BtnBuy").GetComponent<Button> ();
			btnBuy.name = "" + (i + 1);
			btnBuy.onClick.AddListener(  
				delegate() {  
					buttonClick(btnBuy);  
				}  
			);  

			// 添加Button
			itemCell.transform.SetParent(content);  
		}
		int cellMargin = 10;
		// 修改可滑动区域
		content.sizeDelta = new Vector2(content.sizeDelta.x, products.Count * (item.sizeDelta.y + cellMargin));
	}

	void buttonClick(Button button) {

		if (button == btnBack) { // 返回按钮
			SceneManager.LoadScene (1);
		} else {				 // 商品购买按钮
			int buyProductIndex = 0;
			if (int.TryParse (button.name, out buyProductIndex)) {
				WAProduct product = products [buyProductIndex - 1];
				string extInfo = "";
				WAPayProxy.Pay (product.ProductId, extInfo, delegate(WAPayResult result) {
					if (result.Code == WAStatusCode.CODE_SUCCESS) {	// 支付成功
						StringBuilder paySB = new StringBuilder();
						paySB.AppendFormat("Platform:{0}", result.Platform);
						paySB.AppendFormat("WAProductId:{0}", result.WAProductId);
						paySB.AppendFormat("ProductName:{0}", result.ProductName);
						paySB.AppendFormat("OrderId:{0}", result.OrderId);
						paySB.AppendFormat("PriceCurrencyCode:{0}", result.PriceCurrencyCode);
						paySB.AppendFormat("PriceAmountMicros:{0}", result.PriceAmountMicros);
						paySB.AppendFormat("DefaultCurrency:{0}", result.DefaultCurrency);
						paySB.AppendFormat("DefaultAmountMicro:{0}", result.DefaultAmountMicro);
						paySB.AppendFormat("VirtualCoinAmount:{0}", result.VirtualCoinAmount);
						paySB.AppendFormat("VirtualCurrency:{0}", result.VirtualCurrency);
						paySB.AppendFormat("ExtInfo:{0}", result.ExtInfo);

						showDialog("支付成功", paySB.ToString(), "确定", null);
					} else {
						showDialog("提示", result.Msg, "确定", null);
					}

				});
			} else {												// 支付失败
				showDialog("提示", "购买商品失败，商品序列错误! ", "确定", null);
			}
		}
	}
}
