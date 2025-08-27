using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    internal class Gmp3NativeMethods
    {
        // === RETCODES ===
        public const int TRAN_RESULT_OK = 0x0000;
        public const int DLL_RETCODE_INVALID_INTERFACE = unchecked((int)0xF034);
        public const int DLL_RETCODE_UNKNOWN_ECHO = unchecked((int)0xF035);
        public const int DLL_RETCODE_PAIRING_REQUIRED = unchecked((int)0xF020);
        public const int DLL_RETCODE_PORT_NOT_OPEN = unchecked((int)0xF000);
        public const int DLL_RETCODE_TIMEOUT = unchecked((int)0xF00B);
        public const int DLL_RETCODE_ACK_NOT_RECEIVED = unchecked((int)0xF00A);
        public const int DLL_RETCODE_RECV_BUSY = unchecked((int)0xF00E);
        public const int DLL_RETCODE_FUNC_NOT_FOUND = unchecked((int)0xF0FE);
        public const int DLL_RETCODE_HANDSHAKE = 0xF035;

        public const int APP_ERR_ALREADY_DONE = 2080;
        public const int APP_ERR_GMP3_INVALID_HANDLE = 2317;
        public const int APP_ERR_CASHIER_ENTRY_REQUIRED = 2053;
        public const int APP_ERR_GMP3_NO_HANDLE = 2341;
        public const int APP_ERR_GMP3_APP_CHECKSUM_MISMATCH = 2338;


        // ===================== 64-bit Unicode StdCall =====================
        // (Opsiyonel) bazı yerlerde referansım varsa kalsın; çağırmıyoruz.
        [DllImport("GMPSmartDLL", EntryPoint = "FP3_Close",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        internal static extern ushort FP3_Close(int handle);

        // --- Aşağıdakiler daha önce projede vardı (scan/uyumluluk için) ---
        // Kullanılmıyorlar ama geri ekliyoruz ki eski yapı birebir geri gelsin.

        internal static class Iface_AnsiCdecl_x86
        {
            [DllImport("GMPSmartDLL", EntryPoint = "FP3_InterfaceClose",
                CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            internal static extern ushort InterfaceClose(int ifaceHandle);

            [DllImport("GMPSmartDLL", EntryPoint = "FP3_INTERFACE_CLOSE",
                CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            internal static extern ushort INTERFACE_CLOSE(int ifaceHandle);

            [DllImport("GMPSmartDLL", EntryPoint = "FP3_Close",
                CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            internal static extern ushort Close(int handle);
        }


        internal static class Iface_AnsiCdecl_x64
        {

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo",
                CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int Echo(string iface);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_ECHO",
                CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int ECHO(string iface);
        }

        internal static class Iface_AnsiStd_x64
        {

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo",
                CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int Echo(string iface);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_ECHO",
                CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, ExactSpelling = true)]
            internal static extern int ECHO(string iface);
        }

        internal static class Iface_UniStd_x64
        {

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Ping", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, SetLastError = true)]
            internal static extern int FP3_Ping([MarshalAs(UnmanagedType.LPWStr)] string iface);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_StartPairingApprove", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int StartPairingApprove(string iface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_PAIRING_APPROVE", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int PAIRING_APPROVE(string iface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_PAIRING_FINALIZE", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int PAIRING_FINALIZE(string iface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_InterfaceClose")]
            internal static extern int InterfaceClose(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_INTERFACE_CLOSE")]
            internal static extern int INTERFACE_CLOSE(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_Close")]
            internal static extern int Close(string currentInterface, int timeoutMs);


            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Echo",
                CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            internal static extern int Echo(string iface);

            [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_ECHO",
                CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true)]
            internal static extern int ECHO(string iface);
        }


        private static class UniStd_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int FP3_Echo(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int FP3_Ping(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int FP3_Busy(string currentInterface, out byte isBusy, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int FP3_Start(string currentInterface, ref ulong tranHandle, byte[] uniqueId, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
            internal static extern int FP3_GetTicket(string currentInterface, ulong tranHandle, IntPtr pTicket, int timeoutMs);
        }

        // ===================== 64-bit Unicode Cdecl =====================
        private static class UniCdecl_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int FP3_Echo(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_ECHO")]
            internal static extern int FP3_ECHO(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int FP3_Ping(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int FP3_Busy(string currentInterface, out byte isBusy, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int FP3_Start(string currentInterface, ref ulong tranHandle, byte[] uniqueId, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int FP3_GetTicket(string currentInterface, ulong tranHandle, IntPtr pTicket, int timeoutMs);
        }     

        // ===================== Pairing (çeşitli entrypoint isimleri) =====================
        private static class Pairing_UniStd_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_StartPairingInit")]
            internal static extern int StartPairingInit(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_PairingInit")]
            internal static extern int PairingInit(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_PAIRING_INIT")]
            internal static extern int PAIRING_INIT(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_StartPairing")]
            internal static extern int StartPairing(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "FP3_PairingStart")]
            internal static extern int PairingStart(string currentInterface, int timeoutMs);
        }
        private static class Pairing_UniCdecl_x64
        {
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_StartPairingInit")]
            internal static extern int StartPairingInit(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_PairingInit")]
            internal static extern int PairingInit(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_PAIRING_INIT")]
            internal static extern int PAIRING_INIT(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_StartPairing")]
            internal static extern int StartPairing(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_PairingStart")]
            internal static extern int PairingStart(string currentInterface, int timeoutMs);
        }

        // ===================== Interface Open/Close (çeşitli isimler) =====================
        
        
        private static class Iface_UniCdecl_x64
        {
            [DllImport("GMPSmartDLL", EntryPoint = "FP3_StartPairingApprove", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int StartPairingApprove(string iface, int to);


            [DllImport("GMPSmartDLL", EntryPoint = "FP3_PAIRING_APPROVE", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int PAIRING_APPROVE(string iface, int to);


            [DllImport("GMPSmartDLL", EntryPoint = "FP3_StartPairingFinalize", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int StartPairingFinalize(string iface, int to);


            [DllImport("GMPSmartDLL", EntryPoint = "FP3_PAIRING_FINALIZE", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int PAIRING_FINALIZE(string iface, int to);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_INTERFACE_OPEN")]
            internal static extern int INTERFACE_OPEN(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_Open")]
            internal static extern int Open(string currentInterface, int timeoutMs);

            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_InterfaceClose")]
            internal static extern int InterfaceClose(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_INTERFACE_CLOSE")]
            internal static extern int INTERFACE_CLOSE(string currentInterface, int timeoutMs);
            [DllImport("GMPSmartDLL.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FP3_Close")]
            internal static extern int Close(string currentInterface, int timeoutMs);
        }



        // ===================== SARMALAYICI METOTLAR =====================


        internal static int Echo(string iface, int to)
        {
            // 1) Eski çalışan yol: ANSI + StdCall (logda "Echo_Ansi_Std" diye gördüğümüz)
            try { return Iface_AnsiStd_x64.Echo(iface); } catch { }
            try { return Iface_AnsiStd_x64.ECHO(iface); } catch { }

            // 2) Alternatif: ANSI + Cdecl
            try { return Iface_AnsiCdecl_x64.Echo(iface); } catch { }
            try { return Iface_AnsiCdecl_x64.ECHO(iface); } catch { }

            // 3) En sona Unicode imzaları (timeout alanlar)
            try { return UniStd_x64.FP3_Echo(iface, to); } catch { }
            try { return UniCdecl_x64.FP3_Echo(iface, to); } catch { }
            try { return UniCdecl_x64.FP3_ECHO(iface, to); } catch { }

            return DLL_RETCODE_PORT_NOT_OPEN;
        }

        internal static int Ping(string iface, int to)
        {
            try { return UniStd_x64.FP3_Ping(iface, to); } catch { }
            try { return UniCdecl_x64.FP3_Ping(iface, to); } catch { }
            return DLL_RETCODE_PORT_NOT_OPEN;
        }

        internal static int Busy(string iface, out byte busy, int to)
        {
            busy = 1;
            try { return UniStd_x64.FP3_Busy(iface, out busy, to); } catch { }
            try { return UniCdecl_x64.FP3_Busy(iface, out busy, to); } catch { }
            return DLL_RETCODE_PORT_NOT_OPEN;
        }

        internal static int Start(string iface, ref ulong h, byte[] unique16, int to)
        {
            try { ulong t = 0; var rc = UniStd_x64.FP3_Start(iface, ref t, unique16, to); if (rc == TRAN_RESULT_OK || rc > 0) { h = t; return rc; } } catch { }
            try { ulong t = 0; var rc = UniCdecl_x64.FP3_Start(iface, ref t, unique16, to); if (rc == TRAN_RESULT_OK || rc > 0) { h = t; return rc; } } catch { }
            return DLL_RETCODE_PORT_NOT_OPEN;
        }

        public static int StartPairingApprove_All(string iface, int to)
        {
            try { return Iface_UniStd_x64.StartPairingApprove(iface, to); } catch { }
            try { return Iface_UniStd_x64.PAIRING_APPROVE(iface, to); } catch { }
            return DLL_RETCODE_PAIRING_REQUIRED; // 0xF020
        }

        internal static int StartPairingInit_All(string iface, int to)
        {
            try { return Pairing_UniStd_x64.StartPairingInit(iface, to); } catch { }
            try { return Pairing_UniStd_x64.PairingInit(iface, to); } catch { }
            try { return Pairing_UniStd_x64.PAIRING_INIT(iface, to); } catch { }
            try { return Pairing_UniStd_x64.StartPairing(iface, to); } catch { }
            try { return Pairing_UniStd_x64.PairingStart(iface, to); } catch { }

            try { return Pairing_UniCdecl_x64.StartPairingInit(iface, to); } catch { }
            try { return Pairing_UniCdecl_x64.PairingInit(iface, to); } catch { }
            try { return Pairing_UniCdecl_x64.PAIRING_INIT(iface, to); } catch { }
            try { return Pairing_UniCdecl_x64.StartPairing(iface, to); } catch { }
            try { return Pairing_UniCdecl_x64.PairingStart(iface, to); } catch { }

            return DLL_RETCODE_PAIRING_REQUIRED;
        }

        internal static int InterfaceOpen_All(string iface, int to)
        {
            // şu an açık bir Open export'u yoksa port-not-open döndür
            return DLL_RETCODE_PORT_NOT_OPEN;
        }

        internal static int InterfaceClose_All(string iface, int to)
        {
            try { return Iface_UniStd_x64.InterfaceClose(iface, to); } catch { }
            try { return Iface_UniStd_x64.INTERFACE_CLOSE(iface, to); } catch { }
            try { return Iface_UniStd_x64.Close(iface, to); } catch { }
            return DLL_RETCODE_UNKNOWN_ECHO;
        }

        internal static int GetTicketShallow(string iface, ulong h, int to)
        {
            try { return UniStd_x64.FP3_GetTicket(iface, h, IntPtr.Zero, to); } catch { }
            try { return UniCdecl_x64.FP3_GetTicket(iface, h, IntPtr.Zero, to); } catch { }
            return APP_ERR_GMP3_NO_HANDLE;
        }

        //---------------------------------------------------------------------------------
        internal static int FP3_SetDepartments_Native(
            ulong transactionHandle,
            DepartmentConfigItem[] departments,
            int count)
        {
           
            throw new NotImplementedException("FP3_SetDepartments native method not integrated yet.");
        }

        internal static int FP3_OptionFlags_Native(ulong transactionHandle, int activeFlags, int flagsToBeSet)
        {
            // stub: ■ henüz implementasyon yok
            throw new NotImplementedException("FP3_OptionFlags native method not integrated yet.");
        }
        internal static int FP3_TicketHeader_Native(ulong transactionHandle, int ticketType)
        {
            throw new NotImplementedException("FP3_TicketHeader native method not integrated yet.");
        }
       
        internal static int FP3_PrintTotalsAndPayments_Native(ulong transactionHandle)
        {
            throw new NotImplementedException("FP3_PrintTotalsAndPayments native method not integrated yet.");
        }
        internal static int FP3_PrintBeforeMF_Native(ulong transactionHandle)
        {
            throw new NotImplementedException("FP3_PrintBeforeMF native method not integrated yet.");
        }
        internal static int FP3_PrintMF_Native(ulong transactionHandle)
        {
            throw new NotImplementedException("FP3_PrintMF native method not integrated yet.");
        }/*
        internal static int FP3_Close_Native(ulong transactionHandle)
        {
            throw new NotImplementedException("FP3_Close native method not integrated yet.");
        }*/
        internal static int FP3_Refund_Native(ulong transactionHandle, decimal amount)
        {
            throw new NotImplementedException("FP3_Refund native method not integrated yet.");
        }
        internal static int FP3_PrintMessage_Native(ulong transactionHandle, string messageText)
        {
            throw new NotImplementedException("FP3_PrintMessage native method not integrated yet.");
        }
        internal static int FP3_ItemSale_Native(
            ulong transactionHandle, int type, int subType, int deptIndex, int amount, int currencyCode,
            int count, int unitType, string itemCode, string name, string barcode, int flag)
        { throw new NotImplementedException(); }

        internal static int FP3_Payment_Native(
            ulong transactionHandle, string typeOfPayment, string subtypeOfPayment,
            int payAmount, int payAmountCurrencyCode, string bankPaymentUniqueId)
        { throw new NotImplementedException(); }

        internal static int FP3_CancelTransaction_Native(ulong transactionHandle)
        {
            // DLL gelince DllImport ile bağlanacak
            throw new System.NotImplementedException("FP3_CancelTransaction not integrated yet.");
        }

        

    }
}
