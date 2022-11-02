namespace NesM8;

public class CPU
{
    private int param;
    
    public Registers Registers;
    public Bus Bus { get; }

    private byte ReadByte(ushort address) => Bus.ReadByte(address);
    private byte Immediate() => ReadByte(Registers.PC++);
    private ushort Immediate16() => (ushort)(Immediate() | (Immediate() << 8));
    private void WriteByte(ushort address, byte value) => Bus.WriteByte(address, value);

    #region Addressing Modes
    
    private int ModeA()
    {
        param = Registers.A;
        return 0;
    }
    private int ModeAbs()
    {
        param = ReadByte(Immediate16());
        return 0;
    }
    private int ModeAbsX()
    {
        int imm = Immediate16();
        param = ReadByte((ushort)(imm + Registers.X));
        return (imm & 0xFF) + Registers.X > 0xFF ? 1 : 0;
    }
    private int ModeAbsY()
    {
        int imm = Immediate16();
        param = ReadByte((ushort)(imm + Registers.Y));
        return (imm & 0xFF) + Registers.Y > 0xFF ? 1 : 0;
    }
    private int ModeImm()
    {
        param = Immediate();
        return 0;
    }
    private int ModeImp() => 0;
    private int ModeInd()
    {
        param = ReadByte(Immediate16());
        return 0;
    }
    private int ModeXInd()
    {
        int imm = Immediate();
        param = ReadByte((ushort)((imm + Registers.X) % 256)) | (ReadByte((ushort)((imm + Registers.X + 1) % 256)) << 8);
        return 0;
    }
    private int ModeIndY()
    {
        int imm = Immediate();
        param = ReadByte((ushort)imm) | (ReadByte((ushort)((imm + 1) % 256)) << 8) + Registers.Y;
        return imm + 1 > 0xFF ? 1 : 0;
    }
    private int ModeRel()
    {
        sbyte imm = (sbyte)Immediate();
        param = Registers.PC + imm;

        return (Registers.PC & 0xFF) + imm > 0xFF ? 1 : 0;
    }
    private int ModeZpg()
    {
        param = ReadByte(Immediate());
        return 0;
    }
    private int ModeZpgX()
    {
        int imm = Immediate();
        param = ReadByte((ushort)((imm + Registers.X) % 0x100));

        return imm + Registers.X > 0xFF ? 1 : 0;
    }
    private int ModeZpgY()
    {
        int imm = Immediate();
        param = ReadByte((ushort)((imm + Registers.Y) % 0x100));

        return imm + Registers.Y > 0xFF ? 1 : 0;
    }
    
    #endregion

    #region Operations

    void Adc()
    {
        
    }

    #endregion
    
    public CPU(Bus bus)
    {
        Bus = bus;
    }
}