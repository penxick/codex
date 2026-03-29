import { ChatHeader } from "./components/ChatHeader";
import { MessageComposer } from "./components/MessageComposer";
import { MessageList } from "./components/MessageList";
import { useChatSocket } from "./hooks/useChatSocket";

export default function App() {
  const { isConnected, messageText, messages, sendMessage, setMessageText } =
    useChatSocket();

  function handleSubmit(event) {
    event.preventDefault();
    sendMessage();
  }

  return (
    <main className="app-shell">
      <section className="chat-card">
        <ChatHeader isConnected={isConnected} />

        <section className="message-list">
          <MessageList messages={messages} />
        </section>

        <MessageComposer
          onChange={setMessageText}
          onSubmit={handleSubmit}
          value={messageText}
        />
      </section>
    </main>
  );
}
