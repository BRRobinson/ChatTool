import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ChatModel } from '../../models/chat.model';
import { FormArray, FormBuilder, FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import { UserModel } from '../../models/user.model';
import { ChatService } from '../../services/chat/chat.service';
import { ReturnResult } from '../../models/return-result.model';
import { ToastService } from '../../services/toast/toast.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';

function minSelectedCheckboxes(min = 1) {
  return (formArray: FormArray) => {
    const totalSelected = formArray.controls
      .map(control => control.value)
      .reduce((prev, next) => next ? prev + 1 : prev, 0);
    return totalSelected >= min ? null : { required: true };
  };
}

@Component({
  selector: 'app-chat-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './chat-form.html',
  styleUrls: ['./chat-form.scss']
})
export class ChatForm {
  private toastService = inject(ToastService);
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private chatService = inject(ChatService);

  @Input() chat: ChatModel | null = null;
  @Output() saved = new EventEmitter<ChatModel>();

  activeUser: UserModel | null = null;
  chatForm!: FormGroup;
  users: UserModel[] = [];

  constructor(private fb: FormBuilder) {
    this.authService.activeUser$.subscribe(user => this.activeUser = user);
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  private loadUsers() {
    this.userService.getUsers().subscribe({
      next: res => {
        if (res.isSuccess && res.value) {
          this.users = res.value.filter(u => u.username !== this.activeUser?.username);
        }
      },
      error: err => console.error(err),
      complete: () => this.initForm()
    });
  }

  private initForm() {
    this.chatForm = this.fb.group({
      title: [this.chat?.title ?? '', Validators.required],
      participants: this.fb.array(
        this.users.map(u => new FormControl(this.chat?.participants?.some(p => p.id === u.id) ?? false))
      )
    });
  }
  
  getParticipantControl(index: number): FormControl {
    return this.selectedUsers.at(index) as FormControl;
  }
  
  get selectedUsers(): FormArray {
    return this.chatForm.get('participants') as FormArray;
  }

  getSelectedUsers(): UserModel[] {
    return this.selectedUsers.controls
      .map((control, i) => control.value ? this.users[i] : null)
      .filter(u => u !== null) as UserModel[];
  }

  save() {
    if (this.chatForm.invalid) return;

    if (!this.chat) {
      this.chat = { id: 0, title: '', participants: [] };
    }

    this.chat.title = this.chatForm.value.title;
    this.chat.participants = this.getSelectedUsers();
    this.chat.participants.push(this.activeUser!)

    const request$ = this.chat.id
      ? this.chatService.updateChat(this.chat)
      : this.chatService.insertChat(this.chat);

    request$.subscribe({
      next: (res: ReturnResult<ChatModel>) => {
        if (res.isSuccess && res.value) {
          this.chat = res.value;
          this.saved.emit(this.chat);
          this.chatForm.reset();
        } else {
          this.toastService.error(res.message);
        }
      },
      error: err => this.toastService.error(err.message)
    });
  }
}
