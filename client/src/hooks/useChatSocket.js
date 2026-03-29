import { useEffect, useMemo, useState } from "react";
import { io } from "socket.io-client";
import { SERVER_URL, SOCKET_EVENTS } from "../../../shared/src/chat.js";

const socket = io(SERVER_URL, {
  autoConnect: true,
});

export function useChatSocket() {
  const [messageText, setMessageText] = useState("");
  const [messages, setMessages] = useState([]);
  const [isConnected, setIsConnected] = useState(socket.connected);

  useEffect(() => {
    function handleConnect() {
      setIsConnected(true);
    }

    function handleDisconnect() {
      setIsConnected(false);
    }

    function handleReceiveMessage(message) {
      setMessages((currentMessages) => {
        if (currentMessages.some((currentMessage) => currentMessage.id === message.id)) {
          return currentMessages;
        }

        return [...currentMessages, message];
      });
    }

    socket.on("connect", handleConnect);
    socket.on("disconnect", handleDisconnect);
    socket.on(SOCKET_EVENTS.RECEIVE_MESSAGE, handleReceiveMessage);

    return () => {
      socket.off("connect", handleConnect);
      socket.off("disconnect", handleDisconnect);
      socket.off(SOCKET_EVENTS.RECEIVE_MESSAGE, handleReceiveMessage);
    };
  }, []);

  const sortedMessages = useMemo(
    () =>
      [...messages].sort(
        (firstMessage, secondMessage) =>
          new Date(firstMessage.createdAt) - new Date(secondMessage.createdAt),
      ),
    [messages],
  );

  function sendMessage() {
    const trimmedMessage = messageText.trim();

    if (!trimmedMessage) {
      return;
    }

    socket.emit(SOCKET_EVENTS.SEND_MESSAGE, { text: trimmedMessage });
    setMessageText("");
  }

  return {
    isConnected,
    messageText,
    messages: sortedMessages,
    sendMessage,
    setMessageText,
  };
}
