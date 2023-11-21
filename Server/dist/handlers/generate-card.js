"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.generateCardDetails = void 0;
const openai_1 = require("../services/openai");
async function generateCardDetails(data) {
    const cardResponse = await openai_1.openai.chat.completions.create({
        messages: [
            {
                role: "system",
                content: "You are a master of trading card games and an expert game designer. Your job is to take an incomplete card design via a JSON object, and return valid JSON with interesting and new values for description, health, and damage. Be creative! Make thngs fun and vibrant. Keep in mind that flavor text should be short and sweet.",
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
    console.log(cardResponse);
    console.log("-------------------");
    console.log(cardResponse.choices[0].message.content);
    const cardData = JSON.parse(cardResponse.choices[0].message.content);
    const cardName = cardData.Words.map((word) => word.word).join(" ");
    const imageResponse = await openai_1.openai.images.generate({
        model: "dall-e-3",
        prompt: `A vibrant & epic pixel art image of a ${cardName}, described as "${cardData.Description}". No text.`,
        quality: "hd",
        size: "1024x1024",
    });
    console.log(imageResponse);
    console.log("-------------------");
    console.log(imageResponse.data);
}
exports.generateCardDetails = generateCardDetails;
