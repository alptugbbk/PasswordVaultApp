﻿namespace PasswordVaultUI.Models
{
	public class ForgotPasswordViewModel
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
