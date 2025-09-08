using System;

namespace GMP3Integration.Infrastructure.Session
{
    /// <summary>
    /// GMP3 cihazı ile olan session state'ini yönetir.
    /// Interface handle, transaction handle ve interface bilgilerini tutar.
    /// </summary>
    public static class Gmp3SessionManager
    {
        /// <summary>
        /// Mevcut interface handle (uint)
        /// </summary>
        public static uint InterfaceHandle { get; set; }

        /// <summary>
        /// Mevcut transaction handle (ulong)
        /// </summary>
        public static ulong TransactionHandle { get; set; }

        /// <summary>
        /// Mevcut interface adı (örn: COM1)
        /// </summary>
        public static string Interface { get; set; }

        /// <summary>
        /// Session'ın aktif olup olmadığını kontrol eder
        /// </summary>
        public static bool IsSessionActive => InterfaceHandle > 0 && !string.IsNullOrEmpty(Interface);

        /// <summary>
        /// Transaction'ın aktif olup olmadığını kontrol eder
        /// </summary>
        public static bool IsTransactionActive => TransactionHandle > 0;

        /// <summary>
        /// Session'ı temizler (yeni session başlatırken kullanılır)
        /// </summary>
        public static void ClearSession()
        {
            InterfaceHandle = 0;
            TransactionHandle = 0;
            Interface = null;
        }

        /// <summary>
        /// Interface handle'ı set eder
        /// </summary>
        public static void SetInterfaceHandle(uint handle, string interfaceName)
        {
            InterfaceHandle = handle;
            Interface = interfaceName;
        }

        /// <summary>
        /// Transaction handle'ı set eder
        /// </summary>
        public static void SetTransactionHandle(ulong handle)
        {
            TransactionHandle = handle;
        }

        /// <summary>
        /// Session bilgilerini string olarak döndürür (debug için)
        /// </summary>
        public static string GetSessionInfo()
        {
            return $"Interface: {Interface}, IHandle: 0x{InterfaceHandle:X}, THandle: 0x{TransactionHandle:X}";
        }
    }
}
