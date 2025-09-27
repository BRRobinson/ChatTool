export interface ReturnResult<T> {
  isSuccess: boolean;
  message: string;
  value?: T;
}