import { UserModel } from "./user.model";

export class ChatModel {
  id: number;
  title: string;
  participants: UserModel[]

  constructor(title: string, participants: UserModel[]) {
    this.id = 0; 
    this.title = title; 
    this.participants = participants; 
  }
}