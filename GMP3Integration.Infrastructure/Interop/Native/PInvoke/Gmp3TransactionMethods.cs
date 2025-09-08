using System;
using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Constants;
using GMP3Integration.Infrastructure.Interop.Native.Enums;
using GMP3Integration.Infrastructure.Interop.Native.Structs;

namespace GMP3Integration.Infrastructure.Interop.Native.PInvoke
{
    /// <summary>
    /// GMP3 Transaction-related P/Invoke methods
    /// </summary>
    public static class Gmp3TransactionMethods
    {
        // Core Transaction Methods
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FP3_Start")]
        public static extern int FP3_Start(string iface, ref ulong tranHandle, byte[] uniqueId, int timeout);

        // EMULATOR PATTERN: Exact signature like emulator!
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FP3_Start")]
        public static extern int FP3_Start_Handle(uint interfaceHandle, ref ulong tranHandle, byte isBackground, 
            byte[] uniqueId, int lengthOfUniqueId, byte[] uniqueIdSign, int lengthOfUniqueIdSign, 
            byte[] userData, int lengthOfUserData, int timeout);

        // CORRECT SIGNATURE from User's emulator code
        [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Close", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 FP3_Close(UInt32 hInt, UInt64 hTrx, int TimeoutInMiliseconds);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetTicket(string iface, ulong tranHandle, ST_TICKET ticket, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_OptionFlags(string iface, ulong tranHandle, int flags, int timeout);

        // CORRECT SIGNATURE from User - Simple TicketType only!
        [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_TicketHeader", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 FP3_TicketHeader(UInt32 hInt, UInt64 hTrx, TTicketType TicketType, int TimeoutInMiliseconds);

        // JSON-based GetTicket (for reading results)
        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_GetTicket", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 Json_FP3_GetTicket(UInt32 hInt, UInt64 hTrx, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_ItemSale(string iface, ulong tranHandle, ref ST_ITEM item, int timeout);

        // CORRECT Payment from Documentation (Page 18) - Uses ST_PAYMENT_REQUEST & ST_TICKET
        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int FP3_Payment(string iface, ulong tranHandle, ref ST_PAYMENT_REQUEST paymentRequest, ref ST_TICKET ticket, int timeout);

        // Handle-based Payment (like TicketHeader) - Use same entry point but different signature
        [DllImport("GMPSmartDLL.dll", EntryPoint = "FP3_Payment", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint FP3_Payment_Handle(uint hInt, ulong hTrx, ST_PAYMENT_REQUEST paymentRequest, ST_TICKET ticket, int timeout);

        // JSON-based Payment (Emulator style) - Takes byte[] for JSON serialization
        [DllImport("GMPSmartDLL.dll", EntryPoint = "Json_FP3_Payment", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint Json_FP3_Payment(uint hInt, ulong hTrx, byte[] stPaymentRequest, byte[] Out_stPaymentRequest, int Out_stPaymentRequestLen, byte[] szJsonTicket_Out, int JsonTicketLen_Out, int TimeoutInMiliseconds);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_PrintTotalsAndPayments(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_PrintUserMessage(string iface, ulong tranHandle, ref ST_USER_MESSAGE message, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_PrintBeforeMF(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_PrintMF(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetDepartments(string iface, ulong tranHandle, ref ST_DEPARTMENT[] departments, ref int count, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SetDepartments(string iface, ulong tranHandle, ref ST_DEPARTMENT[] departments, int count, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetTaxRates(string iface, ulong tranHandle, ref ST_TAX_RATE[] taxRates, ref int count, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetPaymentApplicationInfo(string iface, ulong tranHandle, ref ST_PAYMENT_APPLICATION_INFO info, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetCurrentHandle(uint interfaceHandle, ref ulong tranHandle, byte[] uniqueId, int maxLengthOfUniqueId, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_PrintUserMessage_Ex(string iface, ulong tranHandle, ref ST_USER_MESSAGE message, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_ReversePayment(string iface, ulong tranHandle, ref ST_PAYMENT payment, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SetInvoice(string iface, ulong tranHandle, ref ST_INVOICE_INFO invoice, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SetParkingTicket(string iface, ulong tranHandle, ref ST_TICKET ticket, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_KasaAvans(string iface, ulong tranHandle, decimal amount, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_CustomerAvans(string iface, ulong tranHandle, decimal amount, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_KasaPayment(string iface, ulong tranHandle, ref ST_PAYMENT_REQUEST request, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_FunctionBankingRefundExt(string iface, ulong tranHandle, ref ST_PAYMENT payment, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_VoidItem(string iface, ulong tranHandle, ref ST_ITEM item, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_VoidPayment(string iface, ulong tranHandle, ref ST_PAYMENT payment, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_VoidAll(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Plus(string iface, ulong tranHandle, decimal amount, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Minus(string iface, ulong tranHandle, decimal amount, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Inc(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Dec(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SetOnlineInvoice(string iface, ulong tranHandle, ref ST_ONLINE_INVOICE_INFO invoice, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Pretotal(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_DisplayPaymentSummary(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetPLU(string iface, ulong tranHandle, ref ST_PLU plu, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SendFrontStationPrint(string iface, ulong tranHandle, string message, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetVasApplicationInfo(string iface, ulong tranHandle, ref ST_PAYMENT_APPLICATION_INFO info, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_FunctionCashierLogin_WE(string iface, ulong tranHandle, string cashierId, string password, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_SetCurrencyProfile(string iface, ulong tranHandle, ref ST_EXCHANGE_PROFILE profile, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Ping(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Busy(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_Echo(string iface, ulong tranHandle, ref ST_ECHO echo, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_ReloadTransaction(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_OnBnClickedButtonVoidAll(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetTransactionHandle(string iface, ref ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_AddTrxHandles(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_DeleteTrxHandles(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_ClearTransactionUniqueId(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetCurrency(string iface, ulong tranHandle, ref ST_EXCHANGE exchange, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_HandleErrorCode(string iface, ulong tranHandle, int errorCode, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_setFunctionCallLog(string iface, ulong tranHandle, bool enable, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_DisplayEcrStatus(string iface, ulong tranHandle, int timeout);

        [DllImport("GMPSmartDLL.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FP3_GetPayment(string iface, ulong tranHandle, ref ST_PAYMENT payment, int timeout);
    }
}
