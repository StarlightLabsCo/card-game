import { WebSocket } from "ws";
import sharp from "sharp";
import { openai } from "../services/openai";

export async function generateCardDetails(ws: WebSocket, data: Object) {
    // -- Generate Card Details --
    const cardResponse = await openai.chat.completions.create({
        messages: [
            {
                role: "system",
                content:
                    "You are a master of trading card games and an expert game designer. Your job is to take an incomplete card design via a JSON object, and return valid JSON with interesting and new values for description, health, and damage. Be creative! Make thngs fun and vibrant. Keep in mind that flavor text should be short and sweet.",
            },
            {
                role: "user",
                content: JSON.stringify(data),
            },
        ],
        model: "gpt-4-1106-preview",
        response_format: {
            type: "json_object",
        },
    });

    const cardData = JSON.parse(
        cardResponse.choices[0].message.content as string
    );
    const cardName = cardData.Words.map(
        (word: any) => word.word as string
    ).join(" ");

    // -- Generate Card Image --
    const imageResponse = await openai.images.generate({
        model: "dall-e-3",
        prompt: `A vibrant pixel art potrait of a ${cardName}, described as "${cardData.Description}". No text!`,
        quality: "hd",
        size: "1024x1024",
        response_format: "b64_json",
    });

    if (!imageResponse) {
        console.error(imageResponse);
        throw new Error("OpenAI image generation failed");
    }

    // Use Sharp to convert the image to JPG
    const imageBuffer = Buffer.from(
        imageResponse.data[0].b64_json as string,
        "base64"
    );
    const convertedImageBuffer = await sharp(imageBuffer)
        .toFormat("jpeg", { quality: 100 }) // Set highest quality for minimal loss
        .toBuffer();

    cardData.Icon = convertedImageBuffer.toString("base64");

    // -- Send back to client --
    ws.send(JSON.stringify({ Type: "UpdateCardDetails", Data: cardData }));
}
