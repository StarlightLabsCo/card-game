import { WebSocketServer } from "ws";
import { handlers } from "./handlers";

const port = 3000;
const wss = new WebSocketServer({ port });

wss.on("connection", function connection(ws) {
    console.log(`new connection`);

    ws.on("message", function message(data) {
        const parsedData = JSON.parse(data.toString());
        const { Type, Data } = parsedData;

        // TODO: eventually add types / runtime validation to this, as well as heartbeats and other websocket goodies, similar to bonfire

        const handler = handlers[Type as keyof typeof handlers];
        if (handler) {
            handler(ws, Data);
        } else {
            console.log("No handler found for type: %s", Type);
        }
    });

    ws.on("error", console.error);

    ws.on("close", function close(data) {
        console.log("close: %s", data);
    });
});

console.log(`Server started on port ${port}`);
