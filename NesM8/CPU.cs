namespace NesM8;

public class CPU
{
    private int param;
    private ushort addr;
    private bool accMode = false;
    
    public Registers Registers;
    public Bus Bus { get; }

    private byte ReadByte(ushort address) => Bus.ReadByte(address);
    private byte Immediate() => ReadByte(Registers.PC++);
    private ushort Immediate16() => (ushort)(Immediate() | (Immediate() << 8));
    private void WriteByte(ushort address, byte value) => Bus.WriteByte(address, value);

    #region Addressing Modes
    
    private int ModeA()
    {
        accMode = true;
        param = Registers.A;
        return 0;
    }
    private int ModeAbs()
    {
        accMode = false;
        addr = Immediate16();
        param = ReadByte(addr);
        return 0;
    }
    private int ModeAbsX()
    {
        int imm = Immediate16();
        accMode = false;
        addr = (ushort)(imm + Registers.X);
        param = ReadByte(addr);
        return (imm & 0xFF) + Registers.X > 0xFF ? 1 : 0;
    }
    private int ModeAbsY()
    {
        int imm = Immediate16();
        accMode = false;
        addr = (ushort)(imm + Registers.Y);
        param = ReadByte(addr);
        return (imm & 0xFF) + Registers.Y > 0xFF ? 1 : 0;
    }
    private int ModeImm()
    {
        accMode = false;
        addr = Registers.PC;
        param = Immediate();
        return 0;
    }
    private int ModeImp()
    {
        accMode = false;
        addr = 0;
        param = 0;
        return 0;
    }
    private int ModeInd()
    {
        accMode = false;
        addr = Immediate16();
        param = ReadByte(addr);
        return 0;
    }
    private int ModeXInd()
    {
        int imm = Immediate();
        accMode = false;
        addr = (ushort)(ReadByte((ushort)((imm + Registers.X) % 256)) | (ReadByte((ushort)((imm + Registers.X + 1) % 256)) << 8));
        param = ReadByte(addr);
        return 0;
    }
    private int ModeIndY()
    {
        int imm = Immediate();
        accMode = false;
        addr = (ushort)(ReadByte((ushort)imm) | (ReadByte((ushort)((imm + 1) % 256)) << 8) + Registers.Y);
        param = ReadByte(addr);
        return imm + 1 > 0xFF ? 1 : 0;
    }
    private int ModeRel()
    {
        sbyte imm = (sbyte)Immediate();
        accMode = false;
        addr = (ushort)(Registers.PC + imm);
        param = 0;

        return (Registers.PC & 0xFF) + imm > 0xFF ? 1 : 0;
    }
    private int ModeZpg()
    {
        accMode = false;
        addr = Immediate();
        param = ReadByte(addr);
        return 0;
    }
    private int ModeZpgX()
    {
        int imm = Immediate();
        accMode = false;
        addr = (ushort)((imm + Registers.X) % 0x100);
        param = ReadByte(addr);

        return imm + Registers.X > 0xFF ? 1 : 0;
    }
    private int ModeZpgY()
    {
        int imm = Immediate();
        addr = (ushort)((imm + Registers.Y) % 0x100);
        param = ReadByte(addr);

        return imm + Registers.Y > 0xFF ? 1 : 0;
    }
    
    #endregion

    #region Operations

    private void Adc()
    {
        // Flags: NZCV
        bool flagV;
        bool flagC;
        bool flagZ;
        bool flagN;

        int temp = Registers.A;
        int carry = Registers.ReadFlag(StatusFlag.Carry) ? 1 : 0;
        
        //   Check if sign is same between both operands            if so, overflow has occured if the sign changes 
        flagV = ((Registers.A ^ param) & 0x80) == 0 && (Registers.A & 0x80) == ((Registers.A + param + carry) & 0x80);

        temp += param + carry;

        flagC = temp > 0xFF;

        temp = temp;

        flagZ = temp == 0;
        flagN = (temp & 0x80) != 0;
        
        Registers.WriteFlag(StatusFlag.Carry, flagC);
        Registers.WriteFlag(StatusFlag.Zero, flagZ);
        Registers.WriteFlag(StatusFlag.Overflow, flagV);
        Registers.WriteFlag(StatusFlag.Negative, flagN);

        Registers.A = (byte)temp;
    }

    private void And()
    {
        // Flags: NZ
        bool flagN;
        bool flagZ;

        Registers.A &= (byte)param;

        flagN = (Registers.A & 0x80) != 0;
        flagZ = Registers.A == 0;
        
        Registers.WriteFlag(StatusFlag.Negative, flagN);
        Registers.WriteFlag(StatusFlag.Zero, flagZ);
    }

    private void Asl()
    {
        // Flags: NZC
        bool flagN;
        bool flagZ;
        bool flagC;

        param <<= 1;
        
        flagN = (param & 0x80) != 0;
        flagC = (param & 0x100) != 0;

        param &= 0xFF;

        flagZ = param == 0;

        if (accMode)
            Registers.A = (byte)param;
        else
            WriteByte(addr, (byte)param);
    }

    #endregion
    
    public CPU(Bus bus)
    {
        Bus = bus;
    }
}