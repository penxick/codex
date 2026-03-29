import crypto from "node:crypto";
import { createMessage, SOCKET_EVENTS } from "../../../shared/src/chat.js";

export function registerChatHandlers(io, store) {
  io.on("connection", (socket) => {
    store.getAll().forEach((message) => {
      socket.emit(SOCKET_EVENTS.RECEIVE_MESSAGE, message);
    });

    socket.on(SOCKET_EVENTS.SEND_MESSAGE, async (payload) => {
      const text = typeof payload?.text === "string" ? payload.text.trim() : "";

      if (!text) {
        return;
      }

      const message = createMessage({
        id: crypto.randomUUID(),
        text,
        createdAt: new Date().toISOString(),
      });

      await store.add(message);
      io.emit(SOCKET_EVENTS.RECEIVE_MESSAGE, message);
    });
  });
}
