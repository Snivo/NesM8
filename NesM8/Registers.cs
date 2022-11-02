namespace NesM8;

public struct Registers
{
    // Accumulator Register //
    public byte A;
    // Indexer Registers //
    public byte X;
    public byte Y;
    // Program Counter //
    public ushort PC;
    // Stack Pointer //
    public ushort SP;
    // Status Register //
    public byte S;

    public bool ReadFlag(StatusFlag flag) => flag switch
    {
        StatusFlag.Carry => (S & 0x01) != 0,
        StatusFlag.Zero => (S & 0x02) != 0,
        StatusFlag.InterruptDisable => (S & 0x04) != 0,
        StatusFlag.Decimal => (S & 0x08) != 0,
        StatusFlag.Overflow => (S & 0x40) != 0,
        StatusFlag.Negative => (S & 0x80) != 0,
        _ => false
    };

    public void WriteFlag(StatusFlag flag, bool value) => S = flag switch
    {
        StatusFlag.Carry => (byte)((S & ~0x01) | (value ? 0x01 : 0)),
        StatusFlag.Zero => (byte)((S & ~0x02) | (value ? 0x02 : 0)),
        StatusFlag.InterruptDisable => (byte)((S & ~0x04) | (value ? 0x04 : 0)),
        StatusFlag.Decimal => (byte)((S & ~0x08) | (value ? 0x08 : 0)),
        StatusFlag.Overflow => (byte)((S & ~0x10) | (value ? 0x10 : 0)),
        StatusFlag.Negative => (byte)((S & ~0x20) | (value ? 0x20 : 0)),
        _ => S
    };

    public byte BreakTrue() => (byte)((S & 0xCF) | 0x01);
    public byte BreakFalse() => (byte)(S & 0xCF);
}