using Wazzifni.Debugging;

namespace Wazzifni
{
    public class WazzifniConsts
    {
        public const string LocalizationSourceName = "Wazzifni";

        public const string AppServerRootAddressKey = "App:ServerRootAddress";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        public const int VerificationTimeCodeInMinutes = 10;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "b4218b40b62647a8985dcfe86e76308b";
    }
}
