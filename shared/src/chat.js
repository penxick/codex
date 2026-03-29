export const SERVER_PORT = 3002;
export const SERVER_URL = `http://127.0.0.1:${SERVER_PORT}`;
export const CLIENT_DEV_URL = "http://127.0.0.1:5173";

export const SOCKET_EVENTS = {
  SEND_MESSAGE: "send_message",
  RECEIVE_MESSAGE: "receive_message",
};

export function createMessage({ id, text, createdAt }) {
  return {
    id,
    text,
    createdAt,
  };
}
