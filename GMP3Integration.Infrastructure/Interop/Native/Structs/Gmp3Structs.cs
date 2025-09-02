using System.Runtime.InteropServices;
using GMP3Integration.Infrastructure.Interop.Native.Enums;

namespace GMP3Integration.Infrastructure.Interop.Native.Structs
{
    /// <summary>
    /// GMP3 Pairing structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_GMP_PAIR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] UniqueId;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] PairingData;
        
        public int PairingDataLength;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szProcOrderNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szProcDate;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szProcTime;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szExternalDeviceBrand;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szExternalDeviceModel;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szExternalDeviceSerialNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szEcrSerialNumber;
    }

    /// <summary>
    /// GMP3 Pairing Response structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_GMP_PAIR_RESP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] UniqueId;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] PairingResponse;
        
        public int PairingResponseLength;
        
        public int ErrorCode;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szEcrBrand;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szEcrModel;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szEcrSerialNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szVersionNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szNewVersionNumber;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szHashFirstDate;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szHashLastDate;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szHashExpireDate;
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
    /// GMP3 Echo structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ST_ECHO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Message;
        
        public int ResponseCode;
        public bool IsSuccessful;
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
