export class UserModel {
  id: number;
  username: string;

  constructor(username: string) {
    this.id = 0;
    this.username = username; 
  }
}