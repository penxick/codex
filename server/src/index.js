import { createServer } from "node:http";
import { Server } from "socket.io";
import { createApp } from "./app/createApp.js";
import { config } from "./config.js";
import { MessageStore } from "./services/messageStore.js";
import { registerChatHandlers } from "./socket/registerChatHandlers.js";

async function startServer() {
  const app = createApp(config.clientUrl);
  const httpServer = createServer(app);
  const io = new Server(httpServer, {
    cors: {
      origin: config.clientUrl,
      methods: ["GET", "POST"],
    },
  });
  const messageStore = new MessageStore(config.messagesFilePath);

  await messageStore.init();
  registerChatHandlers(io, messageStore);

  httpServer.listen(config.port, () => {
    console.log(`Testcord server listening on http://127.0.0.1:${config.port}`);
  });
}

startServer().catch((error) => {
  console.error("Failed to start server");
  console.error(error);
  process.exit(1);
});
