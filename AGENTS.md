# Testcord Agents

## Current phase

Implement only MVP chat flow for now:

1. backend
2. client connection
3. realtime chat

Do not start accounts, friends, settings, or voice until the MVP chat works end to end.

## Architecture rules

- Keep `client`, `server`, and `shared` separated.
- Keep UI, socket logic, and persistence in different files.
- Prefer small, focused modules over large mixed files.
