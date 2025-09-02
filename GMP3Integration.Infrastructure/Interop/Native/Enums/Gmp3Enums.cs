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
    /// Ticket types for GMP3 transactions
    /// </summary>
    public enum TTicketType
    {
        SALE = 0,
        REFUND = 1,
        VOID = 2,
        CORRECTION = 3,
        OTHER = 4
    }
}
