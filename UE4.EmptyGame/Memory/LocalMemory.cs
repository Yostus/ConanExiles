using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;

namespace UE4.EmptyGame.Memory
{
    public class LocalMemory : IMemoryUtility
    {
        public byte[] ReadMem(IntPtr address, int length)
        {
            byte[] buffer = new byte[length];
            Marshal.Copy(address,buffer,0,buffer.Length);
            return buffer;
        }

       
        public void Initialize()
        {
            Process = Process.GetCurrentProcess();
            ProcessBaseAddress = Process.MainModule.BaseAddress;
        }

        public string ReadAnsiString(IntPtr address, int offset)
        {
            byte[] buffer = ReadMem(address + offset, 1024);
            string res = "";
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                    return res;
                res += (char)buffer[i];
            }
            return res;
        }

        public byte ReadByte(IntPtr address, int offset)
        {
            return ReadMem(address + offset, 1)[0];
        }

        public byte[] ReadBytes(IntPtr address, int offset, int length)
        {
            return ReadMem(address + offset, length);
        }

        public double ReadDouble(IntPtr address, int offset)
        {
            return BitConverter.ToDouble(ReadMem(address + offset, 4), 0);
        }

        public int ReadInt32(IntPtr address, int offset)
        {
            return BitConverter.ToInt32(ReadMem(address + offset, 4), 0);
        }

        public long ReadInt64(IntPtr address, int offset)
        {
            return BitConverter.ToInt64(ReadMem(address + offset, 8), 0);
        }

        public IntPtr ReadIntPtr(IntPtr address, int offset)
        {
            if (IntPtr.Size == 8)
                return new IntPtr(BitConverter.ToInt64(ReadMem(address + offset, 8), 0));
            return new IntPtr(BitConverter.ToInt32(ReadMem(address + offset, 4), 0));
        }

        public Matrix ReadMatrix(IntPtr address, int offset)
        {
            Matrix m = new Matrix();
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int i2 = 0; i2 < 4; i2++)
                {
                    m[i, i2] = ReadSingle(address, offset + counter);
                    counter += 4;
                }
            }
            return m;
        }

        public float ReadSingle(IntPtr address, int offset)
        {
            return BitConverter.ToSingle(ReadMem(address + offset, 4), 0);
        }

        public T ReadStruct<T>(IntPtr address, int offset) where T : MemoryObject, new()
        {
            if (address == IntPtr.Zero)
                return default(T);
            return new T { BaseAddress = address + offset };
        }

        public ushort ReadUInt16(IntPtr address, int offset)
        {
            return BitConverter.ToUInt16(ReadMem(address + offset, 2), 0);
        }

        public uint ReadUInt32(IntPtr address, int offset)
        {
            return BitConverter.ToUInt32(ReadMem(address + offset, 4), 0);
        }

        public ulong ReadUInt64(IntPtr address, int offset)
        {
            return BitConverter.ToUInt64(ReadMem(address + offset, 8), 0);
        }

        public T ReadUObject<T>(IntPtr address, int offset) where T : MemoryObject, new()
        {
            return SafeGet<T>(address, offset);
        }

        public Vector2 ReadVector2(IntPtr address, int offset)
        {
            return new Vector2(ReadSingle(address + offset, 0), ReadSingle(address + offset, 4));
        }

        public Vector3 ReadVector3(IntPtr address, int offset)
        {
            return new Vector3(ReadSingle(address + offset, 0), ReadSingle(address + offset, 4), ReadSingle(address + offset, 8));
        }

        public Vector4 ReadVector4(IntPtr address, int offset)
        {
            return new Vector4(ReadSingle(address + offset, 0), ReadSingle(address + offset, 4), ReadSingle(address + offset, 8), ReadSingle(address + offset, 12));
        }

        public T SafeGet<T>(IntPtr address, int offset) where T : MemoryObject, new()
        {
            var ptr = ReadIntPtr(address, offset);
            return ptr == IntPtr.Zero ? default(T) : new T() { BaseAddress = ptr };
        }

        public IntPtr ProcessBaseAddress { get; set; }
        public Process Process { get; set; }

        void WriteMem(IntPtr address, byte[] data)
        {
            Marshal.Copy(data,0,address,data.Length);
        }
        public void WriteBool(IntPtr address, int offset, bool value)
        {
            WriteMem(address + offset, new byte[] { value ? (byte)1 : (byte)0 });
        }

        public void WriteInt64(IntPtr address, int offset, long value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteInt32(IntPtr address, int offset, int value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteInt16(IntPtr address, int offset, short value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteUInt64(IntPtr address, int offset, ulong value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteUInt32(IntPtr address, int offset, uint value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteUInt16(IntPtr address, int offset, ushort value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteByte(IntPtr address, int offset, byte value)
        {
            WriteMem(address + offset, new[] { value });
        }

        public void WriteSingle(IntPtr address, int offset, float value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public void WriteDouble(IntPtr address, int offset, double value)
        {
            WriteMem(address + offset, BitConverter.GetBytes(value));
        }

        public short ReadInt16(IntPtr address, int offset)
        {
            return BitConverter.ToInt16(ReadMem(address + offset, 2), 0);
        }

        public bool ReadBool(IntPtr address, int offset)
        {
            return BitConverter.ToBoolean(ReadMem(address + offset, 1), 0);
        }
    }
}