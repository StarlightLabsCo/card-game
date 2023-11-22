using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using Starlight.Words;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour
{
    public WebSocket websocket;

    public static WebSocketClient Instance { get; private set; }

    // TODO: Eventually move to a card manager class
    private Dictionary<string, CardData> cardDetails;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    async void Start()
    {
        // Set up the card details dictionary - TODO: eventually move to a card manager class
        cardDetails = new Dictionary<string, CardData>();

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

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    // TODO: eventually move to a card manager class
    public void GenerateCardDetails(CardData cardData)
    {
        cardDetails.Add(cardData.Id, cardData);

        var jsonObject = new JObject();
        jsonObject["Type"] = "GenerateCardDetails";
        jsonObject["Data"] = JObject.FromObject(cardData);

        string message = jsonObject.ToString();
        websocket.SendText(message);
    }

    private void UpdateCardDetails(JObject cardData)
    {
        var cardId = cardData["Id"].Value<string>();
        var card = cardDetails[cardId];

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