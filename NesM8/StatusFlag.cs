namespace NesM8;

public enum StatusFlag
{
    Carry = 0b0000_0001,
    Zero = 0b0000_0010,
    InterruptDisable = 0b0000_0100,
    Decimal = 0b0000_1000,
    Overflow = 0b0100_0000,
    Negative = 0b1000_0000
}