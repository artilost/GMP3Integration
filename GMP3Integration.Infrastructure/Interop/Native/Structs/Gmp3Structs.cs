using System;
using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Enums;

namespace GMP3Integration.Infrastructure.Interop.Native.Structs
{
    /// <summary>
    /// GMP3 Constants and Defines - FROM DOCUMENTATION
    /// </summary>
    public static class Defines
    {
        public const int MAX_LOYALITY_TRANS_NUMBER = 32;
        
        // Payment Types (from documentation page 18)
        public const uint PAYMENT_ALL = 0x000FFFFF;
        public const uint PAYMENT_CASH_TL = 0x00000001;
        public const uint PAYMENT_CASH_CURRENCY = 0x00000002;
        public const uint PAYMENT_BANK_CARD = 0x00000004;
        public const uint PAYMENT_YEMEKCEKI = 0x00000008;
        public const uint PAYMENT_MOBILE = 0x00000010;
        public const uint PAYMENT_HEDIYE_CEKI = 0x00000020;
        public const uint PAYMENT_IKRAM = 0x00000040;
        public const uint PAYMENT_ODEMESIZ = 0x00000080;
        public const uint PAYMENT_KAPORA = 0x00000100;
        public const uint PAYMENT_PUAN = 0x00000200;
        public const uint PAYMENT_GIDER_PUSULASI = 0x00000400;
        public const uint PAYMENT_BANKA_TRANSFERI = 0x00000800;
        public const uint PAYMENT_CEK = 0x00001000;
        public const uint PAYMENT_ACIK_HESAP = 0x00002000;
        public const uint PAYMENT_DIGER = 0x00004000;
        
        // Reverse Payment Types (from documentation page 20)
        public const uint REVERSE_PAYMENT_CASH = 0x00100000;
        public const uint REVERSE_PAYMENT_BANK_CARD_VOID = 0x00200000;
        public const uint REVERSE_PAYMENT_BANK_CARD_REFUND = 0x00400000;
        public const uint REVERSE_PAYMENT_YEMEKCEKI = 0x00800000;
        public const uint REVERSE_PAYMENT_MOBILE = 0x01000000;
        public const uint REVERSE_PAYMENT_HEDIYE_CEKI = 0x02000000;
        public const uint REVERSE_PAYMENT_PUAN = 0x04000000;
        public const uint REVERSE_PAYMENT_ACIK_HESAP = 0x08000000;
        public const uint REVERSE_PAYMENT_KAPORA = 0x10000000;
        public const uint REVERSE_PAYMENT_GIDER_PUSULASI = 0x20000000;
        public const uint REVERSE_PAYMENT_BANKA_TRANSFERI = 0x40000000;
        
        // Currency Codes
        public const ushort CURRENCY_TL = 949; // Turkish Lira
        
        // Bank Transaction Flags (from documentation page 45)
        public const uint BANK_TRAN_FLAG_ALL_INPUT_FROM_EXTERNAL_SYSTEM = 0x00000001;
        public const uint BANK_TRAN_FLAG_DO_NOT_ASK_FOR_MISSING_LOYALTY_POINT = 0x00000002;
        public const uint BANK_TRAN_FLAG_ASK_FOR_MISSING_REFUND_INPUTS = 0x00000004;
        public const uint BANK_TRAN_FLAG_LOYALTY_POINT_NOT_SUPPORTED_FOR_TRANS = 0x00000008;
        public const uint BANK_TRAN_FLAG_SALE_WITHOUT_CAMPAIGN = 0x00000010;
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
    /// GMP3 Payment Request structure - CORRECT FROM DOCUMENTATION (Page 18)
    /// </summary>
    public class ST_PAYMENT_REQUEST
    {
        /// <summary>
        /// Main payment method - PAYMENT_CASH_TL, PAYMENT_BANK_CARD, etc.
        /// </summary>
        public uint typeOfPayment { get; set; }
        
        /// <summary>
        /// Subtype - determines different kind of payment (sale, instalment, etc.)
        /// </summary>
        public uint subtypeOfPayment { get; set; }
        
        /// <summary>
        /// Payment amount * 100 (e.g., 150 TL = 15000)
        /// </summary>
        public uint payAmount { get; set; }
        
        /// <summary>
        /// Currency code - 949 for Turkish Lira
        /// </summary>
        public ushort payAmountCurrencyCode { get; set; }
        
        /// <summary>
        /// Bank BKM ID - 0 for automatic selection
        /// </summary>
        public ushort bankBkmId { get; set; }
        
        /// <summary>
        /// Unique ID for payment - must be unique
        /// </summary>
        public string BankPaymentUniqueId { get; set; }
        
        /// <summary>
        /// Bonus amount for loyalty sales
        /// </summary>
        public uint payAmountBonus { get; set; }
        
        /// <summary>
        /// Number of installments
        /// </summary>
        public ushort numberOfinstallments { get; set; }
        
        /// <summary>
        /// Transaction flags for payment options
        /// </summary>
        public uint transactionFlag { get; set; }
        
        /// <summary>
        /// Terminal ID for bank transactions
        /// </summary>
        public byte[] terminalId { get; set; }
        
        /// <summary>
        /// Original transaction data
        /// </summary>
        public _ST_PAYMENT_REQUEST_ORGINAL_DATA OrgTransData { get; set; }
        
        /// <summary>
        /// Batch number
        /// </summary>
        public uint batchNo { get; set; }
        
        /// <summary>
        /// STAN number
        /// </summary>
        public uint stanNo { get; set; }
        
        /// <summary>
        /// Raw data length
        /// </summary>
        public ushort rawDataLen { get; set; }
        
        /// <summary>
        /// Raw data
        /// </summary>
        public byte[] rawData { get; set; }
        
        /// <summary>
        /// Payment name
        /// </summary>
        public string paymentName { get; set; }
        
        /// <summary>
        /// Payment info
        /// </summary>
        public string paymentInfo { get; set; }
        
        /// <summary>
        /// Flags
        /// </summary>
        public uint flags { get; set; }
        
        /// <summary>
        /// Loyalty customer ID
        /// </summary>
        public string LoyaltyCustomerId { get; set; }
        
        /// <summary>
        /// Payment provision ID
        /// </summary>
        public string PaymentProvisionId { get; set; }
        
        /// <summary>
        /// Loyalty service ID
        /// </summary>
        public ushort LoyaltyServiceId { get; set; }
        
        /// <summary>
        /// Allowed input
        /// </summary>
        public byte AllowedInput { get; set; }
        
        public ST_PAYMENT_REQUEST()
        {
            typeOfPayment = 0;
            subtypeOfPayment = 0;
            payAmount = 0;
            payAmountCurrencyCode = 949; // Turkish Lira
            bankBkmId = 0; // Auto select
            BankPaymentUniqueId = "";
            payAmountBonus = 0;
            numberOfinstallments = 0;
            transactionFlag = 0;
            terminalId = new byte[8];
            OrgTransData = new _ST_PAYMENT_REQUEST_ORGINAL_DATA();
            batchNo = 0;
            stanNo = 0;
            rawDataLen = 0;
            rawData = new byte[512];
            paymentName = "";
            paymentInfo = "";
            flags = 0;
            LoyaltyCustomerId = "";
            PaymentProvisionId = "";
            LoyaltyServiceId = 0;
            AllowedInput = 0;
        }
    }

    /// <summary>
    /// Original payment request data structure
    /// </summary>
    public class _ST_PAYMENT_REQUEST_ORGINAL_DATA
    {
        public uint TransactionAmount { get; set; }
        public uint LoyaltyAmount { get; set; }
        public ushort NumberOfinstallments { get; set; }
        public byte[] AuthorizationCode { get; set; }
        public byte[] rrn { get; set; }
        public byte[] TransactionDate { get; set; }
        public byte[] MerchantId { get; set; }
        public byte TransactionType { get; set; }
        public byte[] referenceCodeOfTransaction { get; set; }

        public _ST_PAYMENT_REQUEST_ORGINAL_DATA()
        {
            TransactionAmount = 0;
            LoyaltyAmount = 0;
            NumberOfinstallments = 0;
            AuthorizationCode = new byte[16];
            rrn = new byte[16];
            TransactionDate = new byte[16];
            MerchantId = new byte[16];
            TransactionType = 0;
            referenceCodeOfTransaction = new byte[16];
        }
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
