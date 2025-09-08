namespace GMP3Integration.Infrastructure.Interop.Native.Enums
{
    /// <summary>
    /// Payment types for GMP3 transactions
    /// </summary>
    public enum EPaymentTypes
    {
        CASH = 0,
        CREDIT_CARD = 1,
        DEBIT_CARD = 2,
        CHECK = 3,
        GIFT_CARD = 4,
        LOYALTY = 5,
        FOOD_STAMP = 6,
        VOUCHER = 7,
        OTHER = 8
    }

    /// <summary>
    /// Item unit types for GMP3 transactions
    /// </summary>
    public enum EItemUnitTypes
    {
        PIECE = 0,
        KILOGRAM = 1,
        GRAM = 2,
        LITER = 3,
        METER = 4,
        SQUARE_METER = 5,
        HOUR = 6,
        MINUTE = 7,
        OTHER = 8
    }

    /// <summary>
    /// Ticket types for GMP3 transactions - CORRECT GMP3 DLL ENUM VALUES
    /// </summary>
    public enum TTicketType
    {
        TTasnifDisi = 0,
        TProcessSale = 1,       //Fiscal Ticket           
        TZReport = 2,
        TXReport = 3,
        TEJReport = 4,
        TFiscal2Z = 5,
        TFiscal2T = 6,
        TFiscalCumm = 7,
        TAvans = 8,             //Non_Fiscal Ticket
        TPayment = 9,           //Non_Fiscal Ticket
        TZDonemReport = 10,
        TXDonemReport = 11,
        TXPluSale = 12,
        TInvoice = 13,          //Non_Fiscal Ticket
        TVoidSale = 14,         //Non_Fiscal Ticket
        TRefund = 15,           //Non_Fiscal Ticket
        TYemekceki = 16,        //Non_Fiscal Ticket
        TOtopark = 17,          //Non_Fiscal Ticket 
        TZReportForce = 18,
        TInfo = 19,             //Non_Fiscal Ticket
        TTaxFree = 20,          //Fiscal Ticket
        TDailyMemory = 21,
        TKasaAvans = 22,        //Non_Fiscal Ticket
        TCariHesap,             //Non_Fiscal Ticket
        TDailyReport,
        TMonthlyReport,
        TDaily_X_Report,
        TMonthly_X_Report,
        TMaliFatura,
        TSerbestMeslekMakbuzu,
        TGiderPusulasi,
        TMustahsilMakbuzu,
        TBilet,
        TSerbestMEslekMakbuzuBilgi,
        T_E_BiletBilgi,
        T_E_IrsaliyeBilgi,
        TUniqueId = 127,
        TLAST                    // Bu satir hep sonda kalmali
    }
}
