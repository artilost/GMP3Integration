namespace GMP3Integration.Infrastructure.Interop.Native.Constants
{
    /// <summary>
    /// GMP3 Native DLL constants and error codes
    /// </summary>
    public static class Gmp3Constants
    {
        // Error Codes
        public const int TRAN_RESULT_OK = 0x0000;
        public const int DLL_RETCODE_PORT_NOT_OPEN = 0xF001;
        public const int DLL_RETCODE_INVALID_INTERFACE = 0xF002;
        public const int DLL_RETCODE_HANDSHAKE = 0xF035;
        public const int DLL_RETCODE_INVALID_INTERFACE_FORMAT = 0xF034;
        public const int DLL_RETCODE_PAIRING_REQUIRED = 0xF020;
        public const int DLL_RETCODE_JSON_INVALID_INTERFACE = 0xF025;
        public const int DLL_RETCODE_JSON_FUNCTION_ERROR = 0xF032;
        public const int DLL_RETCODE_CREATE_INTERFACE_SUCCESS = 0xF02A;
        public const int DLL_RETCODE_INTERFACE_NOT_SUPPORTED = 0xF037;
        public const int APP_ERR_ALREADY_DONE = 0x1001;

        // Timeout values
        public const int DEFAULT_TIMEOUT = 10000;
        public const int SHORT_TIMEOUT = 5000;
        public const int LONG_TIMEOUT = 30000;

        // Buffer sizes
        public const int UNIQUE_ID_SIZE = 24;
        public const int MAX_STRING_LENGTH = 256;
        public const int MAX_JSON_LENGTH = 4096;

        // Interface types
        public const string INTERFACE_TCP = "TCP";
        public const string INTERFACE_LAN = "LAN";
        public const string INTERFACE_ETHERNET = "ETHERNET";
        public const string INTERFACE_COM = "COM";

        // Function names
        public const string FUNCTION_START = "FP3_Start";
        public const string FUNCTION_CLOSE = "FP3_Close";
        public const string FUNCTION_ECHO = "FP3_Echo";
        public const string FUNCTION_PAIRING_INIT = "FP3_StartPairingInit";
        public const string FUNCTION_PAIRING_APPROVE = "FP3_StartPairingApprove";
    }
}
