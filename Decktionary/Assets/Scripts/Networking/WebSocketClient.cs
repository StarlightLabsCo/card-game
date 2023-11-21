using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using Starlight.Words;

public class WebSocketClient : MonoBehaviour
{
    public WebSocket websocket;

    public static WebSocketClient Instance { get; private set; }

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
            var eventType = jsonObject["type"].Value<string>();

            switch (eventType)
            {
                default:
                    // Try again
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

    public void GenerateCardDetails(CardData cardData)
    {
        var jsonObject = new JObject();
        jsonObject["Type"] = "GenerateCardDetails";
        jsonObject["Data"] = JObject.FromObject(cardData);

        string message = jsonObject.ToString();
        websocket.SendText(message);
    }
}