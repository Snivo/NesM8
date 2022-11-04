namespace NesM8;

public class Bus
{
    private byte temp = 0;
    public ref byte ReadByte(ushort address) => ref temp;

    public void WriteByte(ushort address, byte value)
    {
    }
}