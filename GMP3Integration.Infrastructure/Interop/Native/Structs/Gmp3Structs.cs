using System;
using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Enums;

namespace GMP3Integration.Infrastructure.Interop.Native.Structs
{
    /// <summary>
    /// GMP3 Constants and Defines
    /// </summary>
    public static class Defines
    {
        public const int MAX_LOYALITY_TRANS_NUMBER = 32;
    }

    // Placeholder structs for ST_TICKET fields
    public class ST_SALEINFO { }
    public class ST_VATDetail { }
    public class ST_printerDataForOneLine { }
    public class ST_LOYALTY_SERVICE_INFO { }
    /// <summary>
    /// GMP3 Pairing structure (Emulator Style - No StructLayout)
    /// </summary>
    public class ST_GMP_PAIR
    {
        public string szProcOrderNumber { get; set; }
        public string szProcDate { get; set; }
        public string szProcTime { get; set; }
        public string szExternalDeviceBrand { get; set; }
        public string szExternalDeviceModel { get; set; }
        public string szExternalDeviceSerialNumber { get; set; }
        public string szEcrSerialNumber { get; set; }

        public ST_GMP_PAIR()
        {
            szProcOrderNumber = "";
            szProcDate = "";
            szProcTime = "";
            szExternalDeviceBrand = "";
            szExternalDeviceModel = "";
            szExternalDeviceSerialNumber = "";
            szEcrSerialNumber = "";
        }
    }

    /// <summary>
    /// GMP3 Pairing Response structure (Emulator Style - No StructLayout)
    /// </summary>
    public class ST_GMP_PAIR_RESP
    {
        public uint ErrorCode { get; set; }
        public string szVersionNumber { get; set; }
        public string szNewVersionNumber { get; set; }
        public string szHashFirstDate { get; set; }
        public string szHashLastDate { get; set; }
        public string szHashExpireDate { get; set; }

        public ST_GMP_PAIR_RESP()
        {
            ErrorCode = 0;
            szVersionNumber = "";
            szNewVersionNumber = "";
            szHashFirstDate = "";
            szHashLastDate = "";
            szHashExpireDate = "";
        }
    }

    /// <summary>
    /// GMP3 Ticket structure
    /// </summary>
    /// <summary>
    /// GMP3 Transaction Ticket structure - CORRECT EMULATOR FORMAT
    /// </summary>
    public class ST_TICKET
    {
        public UInt32 TransactionFlags;
        public UInt32 OptionFlags;
        public UInt16 ZNo;
        public UInt16 FNo;
        public UInt16 EJNo;
        public UInt32 TotalReceiptAmount;
        public UInt32 TotalReceiptTax;
        public UInt32 TotalReceiptDiscount;
        public UInt32 TotalReceiptIncrement;
        public UInt32 CashBackAmount;
        public UInt32 TotalReceiptItemCancel;
        public UInt32 TotalReceiptPayment;
        public UInt32 TotalReceiptReversedPayment;
        public UInt32 KasaAvansAmount;
        public UInt32 KasaPaymentAmount;
        public UInt32 invoiceAmount;
        public UInt32 invoiceAmountCurrency;
        public UInt32 KatkiPayiAmount;
        public UInt32 TaxFreeRefund;
        public UInt32 TaxFreeCalculated;
        public string szTicketDate;
        public string szTicketTime;
        public UInt16 SourceVasAppID;
        public UInt16 PaymentVasAppID;
        public UInt16 BankVasAppID;
        public byte ticketType;  // This is the TTicketType!
        public UInt16 totalNumberOfItems;
        public UInt16 numberOfItemsInThis;
        public UInt16 totalNumberOfPayments;
        public UInt16 numberOfPaymentsInThis;
        public UInt16 numberOfLoyaltyInThis;
        public string TckNo;
        public string invoiceNo;
        public UInt32 invoiceDate;
        public byte invoiceType;
        public int totalNumberOfPrinterLines;
        public int numberOfPrinterLinesInThis;
        public byte[] uniqueId;
        public byte[] rawData;
        public UInt16 rawDataLen;
        public string LastPaymentErrorCode;        // bank error code
        public string LastPaymentErrorMsg;         // bank error message
        public string BankPaymentUniqueId;
        public ST_SALEINFO[] SaleInfo;
        public ST_PAYMENT[] stPayment;
        public ST_VATDetail[] stTaxDetails;
        public ST_printerDataForOneLine[] stPrinterCopy;
        public byte[] UserData;
        public ST_LOYALTY_SERVICE_INFO[] stLoyaltyService;
        public int CurrencyProfileIndex;

        public ST_TICKET()
        {
            TckNo = "";
            invoiceNo = "";
            szTicketDate = "";
            szTicketTime = "";
            uniqueId = new byte[24];
            rawData = new byte[512];
            SaleInfo = new ST_SALEINFO[512];
            stPayment = new ST_PAYMENT[24];
            stTaxDetails = new ST_VATDetail[8];
            stPrinterCopy = new ST_printerDataForOneLine[1024];
            stLoyaltyService = new ST_LOYALTY_SERVICE_INFO[Defines.MAX_LOYALITY_TRANS_NUMBER];
            
            // Initialize other fields
            TransactionFlags = 0;
            OptionFlags = 0;
            ZNo = 0;
            FNo = 0;
            EJNo = 0;
            TotalReceiptAmount = 0;
            TotalReceiptTax = 0;
            TotalReceiptDiscount = 0;
            TotalReceiptIncrement = 0;
            CashBackAmount = 0;
            TotalReceiptItemCancel = 0;
            TotalReceiptPayment = 0;
            TotalReceiptReversedPayment = 0;
            KasaAvansAmount = 0;
            KasaPaymentAmount = 0;
            invoiceAmount = 0;
            invoiceAmountCurrency = 0;
            KatkiPayiAmount = 0;
            TaxFreeRefund = 0;
            TaxFreeCalculated = 0;
            SourceVasAppID = 0;
            PaymentVasAppID = 0;
            BankVasAppID = 0;
            ticketType = 1; // TProcessSale by default
            totalNumberOfItems = 0;
            numberOfItemsInThis = 0;
            totalNumberOfPayments = 0;
            numberOfPaymentsInThis = 0;
            numberOfLoyaltyInThis = 0;
            invoiceDate = 0;
            invoiceType = 0;
            totalNumberOfPrinterLines = 0;
            numberOfPrinterLinesInThis = 0;
            rawDataLen = 0;
            LastPaymentErrorCode = "";
            LastPaymentErrorMsg = "";
            BankPaymentUniqueId = "";
            CurrencyProfileIndex = 0;
        }

        public void Checkelements()
        {
            if (TckNo == null)
                TckNo = "";
            if (invoiceNo == null)
                invoiceNo = "";
            if (szTicketDate == null)
                szTicketDate = "";
            if (szTicketTime == null)
                szTicketTime = "";
            if (uniqueId == null)
                uniqueId = new byte[24];
            if (rawData == null)
                rawData = new byte[512];
            if (SaleInfo == null)
                SaleInfo = new ST_SALEINFO[512];
            if (stPayment == null)
                stPayment = new ST_PAYMENT[24];
            if (stTaxDetails == null)
                stTaxDetails = new ST_VATDetail[8];
            if (stPrinterCopy == null)
                stPrinterCopy = new ST_printerDataForOneLine[1024];
            if (stLoyaltyService == null)
                stLoyaltyService = new ST_LOYALTY_SERVICE_INFO[Defines.MAX_LOYALITY_TRANS_NUMBER];
        }
    }

    /// <summary>
    /// GMP3 Payment structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_PAYMENT
    {
        public EPaymentTypes PaymentType;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PaymentMethod;
        
        public decimal Amount;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ReferenceNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CardNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ApprovalCode;
    }

    /// <summary>
    /// GMP3 Item structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_ITEM
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ItemCode;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ItemName;
        
        public decimal UnitPrice;
        public decimal Quantity;
        public EItemUnitTypes UnitType;
        public decimal TaxRate;
        public decimal DiscountRate;
        public decimal TotalAmount;
    }

    /// <summary>
    /// GMP3 Payment Request structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_PAYMENT_REQUEST
    {
        public EPaymentTypes PaymentType;
        public decimal Amount;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Currency;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Reference;
    }

    /// <summary>
    /// GMP3 Department structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_DEPARTMENT
    {
        public int DepartmentId;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DepartmentName;
        
        public decimal TaxRate;
        public bool IsActive;
    }

    /// <summary>
    /// GMP3 Tax Rate structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_TAX_RATE
    {
        public int TaxRateId;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string TaxRateName;
        
        public decimal Rate;
        public bool IsActive;
    }

    /// <summary>
    /// GMP3 Payment Application Info structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_PAYMENT_APPLICATION_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ApplicationName;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Version;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Vendor;
        
        public bool IsSupported;
    }

    /// <summary>
    /// GMP3 User Message structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_USER_MESSAGE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Message;
        
        public int MessageType;
        public bool IsDisplayed;
    }

    /// <summary>
    /// GMP3 Invoice Info structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_INVOICE_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InvoiceNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string TaxNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CompanyName;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Address;
        
        public bool IsValid;
    }

    /// <summary>
    /// GMP3 Online Invoice Info structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_ONLINE_INVOICE_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InvoiceNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string QRCode;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string URL;
        
        public bool IsApproved;
    }

    /// <summary>
    /// GMP3 PLU structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_PLU
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PLUCode;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string PLUName;
        
        public decimal Price;
        public int DepartmentId;
        public decimal TaxRate;
        public bool IsActive;
    }

    /// <summary>
    /// GMP3 Exchange structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_EXCHANGE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FromCurrency;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ToCurrency;
        
        public decimal ExchangeRate;
        public bool IsValid;
    }

    /// <summary>
    /// GMP3 Exchange Profile structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_EXCHANGE_PROFILE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ProfileName;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string BaseCurrency;
        
        public ST_EXCHANGE[] ExchangeRates;
        public int ExchangeRateCount;
        public bool IsActive;
    }

    /// <summary>
    /// GMP3 Date structure (Emulator'dan alındı!)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_DATE
    {
        public byte day;      // Day
        public byte month;    // Month
        public ushort year;   // Year (UInt16)
    }

    /// <summary>
    /// GMP3 Time structure (Emulator'dan alındı!)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_TIME
    {
        public byte hour;     // Hour
        public byte minute;   // Minute
        public byte second;   // Second
    }

    /// <summary>
    /// GMP3 Cashier structure (Emulator'dan alındı!)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_CASHIER
    {
        public uint cashierId;       // Cashier ID (UInt32)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string cashierName;   // Cashier name (32 chars)
        public byte accessLevel;     // Access level
    }

    /// <summary>
    /// GMP3 Echo structure (Emulator Style - No StructLayout)
    /// </summary>
    public class ST_ECHO
    {
        public uint retcode;
        public uint status;
        public byte[] kvc;
        public byte ecrMode;
        public ushort mtuSize;
        public byte[] ecrVersion;
        public byte[] ecrNewVersion;
        public ST_DATE date;
        public ST_TIME time;
        public ST_CASHIER activeCashier;

        public ST_ECHO()
        {
            kvc = new byte[8];
            ecrMode = 0;
            ecrVersion = new byte[16];
            ecrNewVersion = new byte[16];
            activeCashier = new ST_CASHIER();
            date = new ST_DATE();
            time = new ST_TIME();
        }
    }

    /// <summary>
    /// GMP3 Interface structure for CreateInterface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_INTERFACE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InterfaceString;
        
        public int Timeout;
        
        public int RetryCount;
    }
}
