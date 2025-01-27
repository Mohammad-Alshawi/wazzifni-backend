using System.Text.RegularExpressions;

namespace KeyFinder
{
	public static partial class RegexStore
	{

		[GeneratedRegex(@"^\+\d+$")]
		public static partial Regex DialCodeRegex();

		[GeneratedRegex(@"^\d+$")]
		public static partial Regex SignInLogInPhoneNumberRegex();

		[GeneratedRegex(@"^9639\d{8}$")]
		public static partial Regex SyrianPhonNumberRegex();


		[GeneratedRegex(@"^9639\d{8}$")]
		public static partial Regex MtnPhoneNumberRegex();

		[GeneratedRegex(@"^09\d{8}$")]
		public static partial Regex SyriatelPhoneNumberRegex();

		[GeneratedRegex(@"^\d+$")]
		public static partial Regex TransactionIdRegex();

		[GeneratedRegex(@"^\d{6}$")]
		public static partial Regex OtpRegex();
	}
}
