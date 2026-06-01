export interface ResetPasswordRequest {
  emailOrPhone: string;
  verificationCode: string;
  newPassword: string;
  confirmPassword: string;
}