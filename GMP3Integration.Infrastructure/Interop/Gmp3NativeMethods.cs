using GMP3Integration.Application.DTOs.DepertmenConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP3Integration.Infrastructure.Interop
{
    internal class Gmp3NativeMethods
    {
        // TODO: gerçek gmp3.dll geldiğinde uncomment edip doğru DLL adını kullanacağız.
        // [DllImport("gmp3.dll", EntryPoint = "FP3_Start", CharSet = CharSet.Ansi)]
        // private static extern int FP3_Start_Native(string interfaceName, out ulong transactionHandle);
        internal static int FP3_SetDepartments_Native(
            ulong transactionHandle,
            DepartmentConfigItem[] departments,
            int count)
        {
            // Gerçek DLL geldiğinde DllImport + struct marshaling'e çevireceğiz.
            throw new NotImplementedException("FP3_SetDepartments native method not integrated yet.");
        }
        internal static int FP3_Start_Native(string interfaceName, out ulong transactionHandle)
        {
            // Stub: DLL yok, geçici handle değeri dönüyoruz.
            transactionHandle = 0;
            throw new NotImplementedException("GMP3 native DLL henüz entegre edilmedi.");
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
        }
        internal static int FP3_Close_Native(ulong transactionHandle)
        {
            throw new NotImplementedException("FP3_Close native method not integrated yet.");
        }
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
