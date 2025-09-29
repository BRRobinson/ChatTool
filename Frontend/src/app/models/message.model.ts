import { ChatModel } from "./chat.model";
import { UserModel } from "./user.model";

export class MessageModel {
  id: number;
  chat: ChatModel;
  sender: UserModel;
  content: string;
  sentAt: Date;

  constructor(chat: ChatModel, sender: UserModel, content: string) {
    this.id = 0;
    this.chat = chat;
    this.sender = sender;
    this.content = content;
    this.sentAt = new Date();
  }
}