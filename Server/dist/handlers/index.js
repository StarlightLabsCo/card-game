"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.handlers = void 0;
const generate_card_1 = require("./generate-card");
exports.handlers = {
    ["GenerateCardDetails"]: generate_card_1.generateCardDetails,
};
