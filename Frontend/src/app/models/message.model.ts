export class Message {
  from: string;
  to: string;
  body: string;
  sentTime: Date;

  constructor(from: string, to: string, body: string) {
    this.from = from;
    this.to = to;
    this.body = body;    
    this.sentTime = new Date();    
  }
}