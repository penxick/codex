export function MessageComposer({ value, onChange, onSubmit }) {
  return (
    <form className="composer" onSubmit={onSubmit}>
      <input
        aria-label="Message"
        onChange={(event) => onChange(event.target.value)}
        placeholder="Type a message"
        type="text"
        value={value}
      />
      <button type="submit">Send</button>
    </form>
  );
}
