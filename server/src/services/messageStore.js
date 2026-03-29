import { mkdir, readFile, writeFile } from "node:fs/promises";
import { fileURLToPath } from "node:url";
import { dirname } from "node:path";

export class MessageStore {
  constructor(fileUrl) {
    this.fileUrl = fileUrl;
    this.messages = [];
  }

  async init() {
    await mkdir(dirname(fileURLToPath(this.fileUrl)), { recursive: true });

    try {
      const fileContent = await readFile(this.fileUrl, "utf8");
      const parsedMessages = JSON.parse(fileContent);
      this.messages = Array.isArray(parsedMessages) ? parsedMessages : [];
    } catch (error) {
      if (error.code !== "ENOENT") {
        throw error;
      }

      await this.persist();
    }
  }

  getAll() {
    return [...this.messages];
  }

  async add(message) {
    this.messages.push(message);
    await this.persist();
    return message;
  }

  async persist() {
    await writeFile(this.fileUrl, JSON.stringify(this.messages, null, 2), "utf8");
  }
}
