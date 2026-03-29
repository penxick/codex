import { CLIENT_DEV_URL, SERVER_PORT } from "../../shared/src/chat.js";

export const config = {
  port: SERVER_PORT,
  clientUrl: CLIENT_DEV_URL,
  messagesFilePath: new URL("../data/messages.json", import.meta.url),
};
