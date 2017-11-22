using System.Collections;
using System.Collections.Generic;
using WASdk.Unity;

public class Event {

	public static ArrayList getEventNames() {
		ArrayList eventNameList = new ArrayList ();
		eventNameList.Add (WAEventType.LOGIN);
		eventNameList.Add (WAEventType.IMPORT_USER);
		eventNameList.Add (WAEventType.USER_CREATED);
		eventNameList.Add (WAEventType.INITIATED_PAYMENT);
		eventNameList.Add (WAEventType.COMPLETE_PAYMENT);
		eventNameList.Add (WAEventType.INITIATED_PURCHASE);
		eventNameList.Add (WAEventType.COMPLETE_PURCHASE);
		eventNameList.Add (WAEventType.LEVEL_ACHIEVED);
		eventNameList.Add (WAEventType.USER_INFO_UPDATE);
		eventNameList.Add (WAEventType.GOLD_UPDATE);
		eventNameList.Add (WAEventType.TASK_UPDATE);
		eventNameList.Add (WAEventType.CUSTOM_EVENT_PREFIX);

		return eventNameList;
	}

	public static Dictionary<string, object> getEventParamNames(string eventName) {
		Dictionary<string, object> paramDict = new Dictionary<string, object> ();

		if (eventName.Equals(WAEventType.LOGIN)) {            // TODO LOGIN

		} else if (eventName.Equals(WAEventType.INITIATED_PAYMENT)) {     // INITIATED_PAYMENT

		} else if (eventName.Equals(WAEventType.COMPLETE_PAYMENT)) {     // COMPLETE_PAYMENT
			paramDict.Add(WAEventParameterName.TRANSACTION_ID, "13241893274981237");
			paramDict.Add(WAEventParameterName.PAYMENT_TYPE, "FACEBOOK");
			paramDict.Add(WAEventParameterName.CURRENCY_TYPE, "USD");
			paramDict.Add(WAEventParameterName.CURRENCY_AMOUNT, 50.23f);
			paramDict.Add(WAEventParameterName.VERTUAL_COIN_AMOUNT, 20000);
			paramDict.Add(WAEventParameterName.VERTUAL_COIN_CURRENCY, "gold");
			paramDict.Add(WAEventParameterName.IAP_ID, "1111111");
			paramDict.Add(WAEventParameterName.IAP_NAME, "GGGGGG");
			paramDict.Add(WAEventParameterName.IAP_AMOUNT, 20);
		} else if (eventName.Equals(WAEventType.INITIATED_PURCHASE)) {     // TODO INITIATED_PURCHASE

		} else if (eventName.Equals(WAEventType.COMPLETE_PURCHASE)) {     // TODO COMPLETE_PURCHASE
			paramDict.Add(WAEventParameterName.ITEM_NAME, "GGGGG");
			paramDict.Add(WAEventParameterName.ITEM_AMOUNT, 20);
			paramDict.Add(WAEventParameterName.PRICE, 50.0f);
		} else if (eventName.Equals(WAEventType.LEVEL_ACHIEVED)) {     // TODO LEVEL_ACHIEVED
			paramDict.Add(WAEventParameterName.SCORE, 3241234);
			paramDict.Add(WAEventParameterName.FIGHTING, 1230020);
		} else if (eventName.Equals(WAEventType.USER_CREATED)) {     // TODO USER_CREATED
			paramDict.Add(WAEventParameterName.ROLE_TYPE, "1");
			paramDict.Add(WAEventParameterName.GENDER, 0);
			paramDict.Add(WAEventParameterName.NICKNAME, "霸气侧漏");
			paramDict.Add(WAEventParameterName.REGISTER_TIME, System.DateTime.Now.Millisecond);
			paramDict.Add(WAEventParameterName.VIP, 8);
			paramDict.Add(WAEventParameterName.BINDED_GAME_GOLD, 100000);
			paramDict.Add(WAEventParameterName.GAME_GOLD, 10000);
			paramDict.Add(WAEventParameterName.FIGHTING, 1230020);
		} else if (eventName.Equals(WAEventType.USER_INFO_UPDATE)) {     // TODO USER_INFO_UPDATE
			paramDict.Add(WAEventParameterName.ROLE_TYPE, 1);
			paramDict.Add(WAEventParameterName.NICKNAME, "霸气侧漏");
			paramDict.Add(WAEventParameterName.VIP, 8);
		} else if (eventName.Equals(WAEventType.TASK_UPDATE)) {     // TODO TASK_UPDATE
			paramDict.Add(WAEventParameterName.TASK_ID, "123");
			paramDict.Add(WAEventParameterName.TASK_NAME, "刺杀希特勒");
			paramDict.Add(WAEventParameterName.TASK_TYPE, "等级任务");
			paramDict.Add(WAEventParameterName.TASK_STATUS, 2);
		} else if (eventName.Equals(WAEventType.GOLD_UPDATE)) {     // TODO GOLD_UPDATE
			paramDict.Add(WAEventParameterName.GOLD_TYPE, 1);
			paramDict.Add(WAEventParameterName.APPROACH, "充值");
			paramDict.Add(WAEventParameterName.AMOUNT, 100000);
			paramDict.Add(WAEventParameterName.CURRENT_AMOUNT, 200000);
		} else if (eventName.Equals(WAEventType.IMPORT_USER)) {    // TODO IMPORT_USER

		} else if (eventName.Equals(WAEventType.CUSTOM_EVENT_PREFIX)) {    // TODO CUSTOM_EVENT_PREFIX
			paramDict.Add("to_level", 141);
			paramDict.Add("fight_force", 1232320);
			paramDict.Add("to_fight_force", 1220020);
			paramDict.Add(WAEventParameterName.SUCCESS, true);
		}

		return paramDict;
	}

}
