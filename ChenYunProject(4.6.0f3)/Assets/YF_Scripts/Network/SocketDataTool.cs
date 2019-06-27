using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;



public class SocketDataTool
{

    /// <summary>
    /// 获取数组数目
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <param name="numPostion"></param>
    /// <returns></returns>
    public static int GetNum<T>(byte[] bytes, int numPostion)
    {
        Type nType = typeof(T);
        int nSize = Marshal.SizeOf(nType);

        int num = 0;
        if (nSize == 2)
        {
            num = BitConverter.ToInt16(bytes, numPostion);
        }
        else if (nSize == 4)
        {
            num = BitConverter.ToInt32(bytes, numPostion);
        }

        return num;
    }

    /// <summary>
    /// 得到data
    /// </summary>
    /// <typeparam name="N">表示数组数目的值的类型</typeparam>
    /// <typeparam name="T">数组元素的类型</typeparam>
    /// <param name="bytes">数据缓冲区</param>
    /// <param name="arrayPosition">data的在缓冲区的位置</param>
    /// <param name="arraySize"></param>
    /// <returns></returns>
    public static T[] GetArray<T>(byte[] bytes, int arrayPosition, int arrayCount)
    {
        Type tType = typeof(T);
        int tSize = Marshal.SizeOf(tType);

        T[] array = null;

        // 数组占用内存总大小
        int totalSize = arrayCount * tSize;
        // 如果数据长度有误
        if (arrayPosition + totalSize > bytes.Length)
        {
            Debug.LogError("如果数据长度有误");
            return null;
        }

        array = new T[arrayCount];

        // 开辟非托管内存
        IntPtr ptr = Marshal.AllocHGlobal(totalSize);

        // 将流拷贝到非托管内存
        Marshal.Copy(bytes, arrayPosition, ptr, totalSize);

        // 转化数组成员
        for (int i = 0; i < arrayCount; i++)
        {
            IntPtr iPtr = (IntPtr)(ptr.ToInt32() + i * tSize);
            array[i] = (T)Marshal.PtrToStructure(iPtr, tType);
        }

        Marshal.FreeHGlobal(ptr);

        return array;
    }


    /// <summary>
    /// 得到data
    /// </summary>
    /// <typeparam name="N">表示数组数目的值的类型</typeparam>
    /// <typeparam name="T">数组元素的类型</typeparam>
    /// <param name="bytes">数据缓冲区</param>
    /// <param name="position">data的在缓冲区的位置</param>
    /// <returns></returns>
    public static T[] GetArray<N, T>(byte[] bytes, int position)
    {
        return GetArray<T>(bytes, position + Marshal.SizeOf(typeof(N)), GetNum<N>(bytes, position));
    }

    /// <summary>
    /// 从指定流中解析出一组T对象数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static T[] GetArray<T>(byte[] bytes)
    {
        return GetArray<T>(bytes, 0);
    }

    /// <summary>
    /// 从指定流中解析出一组T对象数组, 加上偏移参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <param name="offset">真实数据的偏移</param>
    /// <returns></returns>
    public static T[] GetArray<T>(byte[] bytes, int offset)
    {
        int num = bytes.Length / Marshal.SizeOf(typeof(T));
        return GetArray<T>(bytes, offset, num);
    }

    /// <summary>
    /// 从指定流中解析出一个T结构体对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <param name="offset">真实数据在流中的偏移</param>
    /// <returns></returns>
    public static T GetStruct<T>(byte[] bytes, int offset) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));

        if (bytes == null || size + offset > bytes.Length)
        {
            return default(T);
        }

        T obj = default(T);

        // 开辟非托管内存
        IntPtr ptr = Marshal.AllocHGlobal(size);

        try
        {
            // 将流拷贝到非托管内存
            Marshal.Copy(bytes, offset, ptr, size);

            obj = (T)Marshal.PtrToStructure(ptr, typeof(T));

        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return obj;
    }

    /// <summary>
    /// 将结构体对象转化为字节数组
    /// </summary>
    /// <param name="structObj"></param>
    /// <returns></returns>
    public static byte[] GetBytes(object structObj)
    {
        int structSize = Marshal.SizeOf(structObj);

        byte[] bytes = new byte[structSize];
        IntPtr structPtr = Marshal.AllocHGlobal(structSize);

        try
        {
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, structSize);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(structPtr);
        }
    }

    /// <summary>
    /// 从指定流中解析出一个继承自ISocketData接口的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    static public T GetSocketData<T>(byte[] bytes, int offset) where T : ISocketData, new()
    {
        if (bytes == null || bytes.Length == 0)
        {
            return default(T);
        }

        T data = new T();
        data.Deserialize(bytes, offset);
        return data;
    }

    public static byte[] Mergebtye(byte[] data1, byte[] data2)
    {
        var all = new byte[data1.Length + data2.Length];
        Buffer.BlockCopy(data1, 0, all, 0, data1.Length);
        Buffer.BlockCopy(data2, 0, all, data1.Length * sizeof(byte), data2.Length);
        return all;
    }

}

