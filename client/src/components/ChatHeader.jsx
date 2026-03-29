export function ChatHeader({ isConnected }) {
  return (
    <header className="chat-header">
      <div>
        <p className="eyebrow">Testcord Desktop MVP</p>
        <h1>Realtime chat</h1>
      </div>
      <span className={isConnected ? "status online" : "status offline"}>
        {isConnected ? "Connected" : "Disconnected"}
      </span>
    </header>
  );
}
