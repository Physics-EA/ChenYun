using System.Runtime.InteropServices;
using System;
using UnityEngine;


/// <summary>
/// socket 消息接口
/// </summary>
public interface ISocketData
{
    byte[] Serialize();
    //int Deserialize(byte[] bytes);
    int Deserialize(byte[] bytes, int offset);
}



/// <summary>
/// socket消息的基类
/// 提供序列化和反序列化的接口
/// 如果包含动态（不定长）数组，需要重写Serialize()和Deserialize（）
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public abstract class SocketData : ISocketData
{
    /// <summary>
    /// 序列化结构化数据
    /// 即：将结构化数据转化为byte[]
    /// 如果包含动态（不定长）数组，需要重写
    /// </summary>
    /// <returns></returns>
    public virtual byte[] Serialize()
    {
        return SocketDataTool.GetBytes(this);
    }

    ///// <summary>
    ///// 反序列化
    ///// </summary>
    ///// <param name="bytes"></param>
    ///// <returns>返回反序列化流中新的下标位置</returns>
    //public int Deserialize(byte[] bytes) //where T : SocketData
    //{
    //    return Deserialize(bytes, 0);
    //}

    /// <summary>
    /// 反序列化结构化数据
    /// 即：将byte[]转化为对应的结构化数据
    /// </summary>
    /// <param name="bytes">数据流</param>
    /// <param name="offset">需要结构化的数据在数据流中的起始位置</param>
    public virtual int Deserialize(byte[] bytes, int offset)
    {
        int size = Marshal.SizeOf(this);

        // byte数组长度小于结构的大小 
        if (size > bytes.Length - offset)
        {
			Logger.Instance.WriteLog("反序列化结构化数据:byte数组长度小于结构的大小");
            // 返回空 
            //return default(T);
            return 0;
        }

        // 分配结构大小的内存空间 
        IntPtr structPtr = Marshal.AllocHGlobal(size);

        try
        {
            // 将byte数组拷到分配好的内存空间 
            Marshal.Copy(bytes, offset, structPtr, size);

            // 将内存空间转换为目标结构 
            Marshal.PtrToStructure(structPtr, this);
        }
		catch
		{
			Logger.Instance.WriteLog("反序列化结构化数据:将内存空间转换为目标结构失败");
		}
        finally
        {
            // 释放内存空间 
            Marshal.FreeHGlobal(structPtr);
        }

        return offset + size;
    }

    static public T Parse<T>(byte[] bytes, int offset) where T : ISocketData, new()
    {
        return SocketDataTool.GetSocketData<T>(bytes, offset);
    }
}


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class ALLST_GENERIC : SocketData
{
    public UInt32 dwTimeStamp;          //时间戳
    public UInt16 wType;                //信息类型
    public Int32 iPlayerID;             //玩家ID
    public Int32 iMessageLen;          //信息长度
    public UInt16 nError;               //错误代码

    public ALLST_GENERIC() { }

    public ALLST_GENERIC(UInt16 _cmd,Int32 len)
    {
        dwTimeStamp = 0;
        wType = _cmd;
        iPlayerID = 0;
        iMessageLen = len;
        nError = 0;
    }
}



/// <summary>
/// 简单的（不包含动态结构体数组）socket消息基类
/// 子类不需要再实现Serialize()和Deserialize()
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SocketCmd : SocketData
{
    public ALLST_GENERIC header;
}






