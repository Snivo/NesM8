namespace NesM8;

public class CPU
{
    private int param;
    
    public Registers Registers { get; } = new Registers();
    public Bus Bus { get; }

    private byte ReadByte(ushort address) => Bus.ReadByte(address);
    
    private void WriteByte(ushort address, byte value) => Bus.WriteByte(address, value);

    public CPU(Bus bus)
    {
        Bus = bus;
    }
}