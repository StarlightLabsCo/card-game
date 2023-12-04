using Newtonsoft.Json.Linq;
using Starlight.UI;
using Starlight.Words;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Starlight.Managers
{
    public class CardManager : PersistentSingletonBehaviour<CardManager>
    {
	   private Dictionary<string, CardData> cardDetails;

	   private void Start()
	   {
		  // Set up the card details dictionary
		  cardDetails = new Dictionary<string, CardData>();
	   }

	   public CardData GetCardDetails(string uuid)
	   {
		  if(!cardDetails.ContainsKey(uuid))
		  {
			 Debug.LogErrorFormat("Invalid card UUID: {0}", uuid);
		  }
		  return cardDetails[uuid];
	   }

	   public void GenerateCardDetails(CardData cardData)
	   {
		  cardDetails.Add(cardData.Id, cardData);

		  var jsonObject = new JObject();
		  jsonObject["Type"] = "GenerateCardDetails";
		  jsonObject["Data"] = JObject.FromObject(cardData);

		  string message = jsonObject.ToString();
		  WebSocketClient.instance.websocket.SendText(message);
	   }

#if !UNITY_WEBGL || UNITY_EDITOR
	   void Update()
	   {
		  WebSocketClient.instance.websocket.DispatchMessageQueue();
	   }
#endif
    }
}