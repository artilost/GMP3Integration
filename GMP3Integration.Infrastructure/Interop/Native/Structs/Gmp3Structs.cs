using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Enums;

namespace GMP3Integration.Infrastructure.Interop.Native.Structs
{
    /// <summary>
    /// GMP3 Pairing structure (Emulator Style - No StructLayout)
    /// </summary>
    public class ST_GMP_PAIR
    {
        public string szProcOrderNumber;
        public string szProcDate;
        public string szProcTime;
        public string szExternalDeviceBrand;
        public string szExternalDeviceModel;
        public string szExternalDeviceSerialNumber;
        public string szEcrSerialNumber;

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
        public uint ErrorCode;
        public string szVersionNumber;
        public string szNewVersionNumber;
        public string szHashFirstDate;
        public string szHashLastDate;
        public string szHashExpireDate;

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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_TICKET
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string TicketNumber;
        
        public TTicketType TicketType;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CashierId;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CustomerId;
        
        public decimal TotalAmount;
        public decimal TaxAmount;
        public decimal DiscountAmount;
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
