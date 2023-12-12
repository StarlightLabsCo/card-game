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
	   [SerializeField] CardUI cardPrefab;

	   private void Start()
	   {
		  // Set up the card details dictionary
		  cardDetails = new Dictionary<string, CardData>();
	   }

	   public bool CardExists(string uuid) => cardDetails.ContainsKey(uuid);

	   public CardUI CreateCard(Transform parent, CardData data, Vector2? position = null)
	   {
		  CardUI newCard;
		  if (position == null)
		  {
			 newCard = Instantiate(cardPrefab, parent);
		  } 
		  else
		  {
			 newCard = Instantiate(cardPrefab, position.Value, Quaternion.identity, parent);
		  }
		  newCard.SetCardData(data);
		  return newCard;
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

	   public void RemoveCardDetails(string uuid)
	   {
		  if (!cardDetails.ContainsKey(uuid)) return;
		  cardDetails.Remove(uuid);
	   }

#if !UNITY_WEBGL || UNITY_EDITOR
	   void Update()
	   {
		  WebSocketClient.instance.websocket.DispatchMessageQueue();
	   }
#endif
    }
}