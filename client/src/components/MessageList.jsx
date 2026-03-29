function formatTime(isoDate) {
  return new Intl.DateTimeFormat("ru-RU", {
    hour: "2-digit",
    minute: "2-digit",
  }).format(new Date(isoDate));
}

export function MessageList({ messages }) {
  if (messages.length === 0) {
    return (
      <div className="empty-state">
        <p>No messages yet.</p>
        <span>Start the conversation from this desktop client.</span>
      </div>
    );
  }

  return (
    <>
      {messages.map((message) => (
        <article className="message-item" key={message.id}>
          <div className="message-meta">
            <strong>Local user</strong>
            <time>{formatTime(message.createdAt)}</time>
          </div>
          <p>{message.text}</p>
        </article>
      ))}
    </>
  );
}
