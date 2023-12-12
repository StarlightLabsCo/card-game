using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using Starlight.Words;
using System.Collections.Generic;
using System;
using Starlight.Managers;

public class WebSocketClient : PersistentSingletonBehaviour<WebSocketClient>
{
    public WebSocket websocket;

    public static WebSocketClient Instance { get; private set; }

    // Start is called before the first frame update
    async void Start()
    {
        // Set up the websocket
        websocket = new WebSocket("ws://localhost:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);

            Debug.Log("OnMessage! " + message);

            var jsonObject = JsonConvert.DeserializeObject<JObject>(message);
            var eventType = jsonObject["Type"].Value<string>();
            var eventData = jsonObject["Data"].Value<JObject>();

            switch (eventType)
            {
                case "UpdateCardDetails":
                    UpdateCardDetails(eventData);
                    break;
                default:
                    Debug.Log("Unknown event type: " + eventType);
                    break;
            }
        };

        await websocket.Connect();
    }

    private void UpdateCardDetails(JObject cardData)
    {
	   var cardId = cardData["Id"].Value<string>();

        // if card has been removed from play (e.g. killed by an opponent),
        // then there's no point in gathering and setting the received data
        if (!CardManager.instance.CardExists(cardId)) return;


	   var card = CardManager.instance.GetCardDetails(cardId);

	   var icon = ConvertBase64ToSprite(cardData["Icon"].Value<string>());

	   card.SetIcon(icon);
	   card.SetDescription(cardData["Description"].Value<string>());
	   card.SetHealth(cardData["Health"].Value<int>());
	   card.SetDamage(cardData["Damage"].Value<int>());
    }

    private Sprite ConvertBase64ToSprite(string base64String)
    {
        Debug.Log("Converting base64 string to sprite: " + base64String.Substring(0, 10) + "...");

        byte[] imageBytes = System.Convert.FromBase64String(base64String);

        Debug.Log("Image bytes length: " + imageBytes.Length);

        Texture2D tex = new Texture2D(1, 1);
        if (tex.LoadImage(imageBytes))
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            Debug.Log("Successfully loaded texture from base64 string.");
            return sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from base64 string.");
            return null;
        }
    }
}