const SERVER_PORT = 3002;
const SERVER_URL = `http://127.0.0.1:${SERVER_PORT}`;
const CLIENT_DEV_URL = "http://127.0.0.1:5173";

const SOCKET_EVENTS = {
  SEND_MESSAGE: "send_message",
  RECEIVE_MESSAGE: "receive_message",
};

module.exports = {
  CLIENT_DEV_URL,
  SERVER_PORT,
  SERVER_URL,
  SOCKET_EVENTS,
};
