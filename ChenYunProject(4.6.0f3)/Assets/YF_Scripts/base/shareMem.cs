using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;



public class shareMemOnlyRead  {

	[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);
    
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
    
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr OpenMutex(int dwDesiredAccess,bool bInheritHandle,string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool ReleaseMutex(IntPtr handle);
    
    const int FILE_MAP_READ = 0x0004;
    
    //共享内存打开失败或成功
    bool bSuccess;
    IntPtr hMappingHandle = IntPtr.Zero;
	Mutex hMutex = null;

    //共享内存的大小
    UInt32 uMemSize;
    public shareMemOnlyRead(string name, UInt32 size, ref bool _bSuccess)
    {
        _bSuccess = false;
        hMappingHandle = OpenFileMapping(FILE_MAP_READ, false, name);
        if (hMappingHandle == IntPtr.Zero)
        {
            return;
        }

		//hMutex = OpenMutex(0, true, "mutex_" + name);
		hMutex = Mutex.OpenExisting ("mutex_" + name);
        if(hMutex == null)
        {
			Debug.Log( Marshal.GetLastWin32Error());
            return;
        }
        _bSuccess = true;
        uMemSize = size;
        bSuccess = true;
    }

   public bool read(byte[] buff)
    {
        if (!bSuccess) return false;
        
        IntPtr hVoid = IntPtr.Zero;
        //WaitForSingleObject(hMutex, 0xFFFFFFFF);
		hMutex.WaitOne ();
        hVoid = MapViewOfFile(hMappingHandle, FILE_MAP_READ, 0, 0, uMemSize);
        if (hVoid == IntPtr.Zero)
        {
            //读取内存失败
            return false;
        }
        //读取共享内存中的Buff然后做渲染          
        Marshal.Copy(hVoid, buff, 0, buff.Length);
        UnmapViewOfFile(hVoid);
        //ReleaseMutex(hMutex);
		hMutex.ReleaseMutex ();
        return true;
    }

	public void Realease()
	{
		if (hMappingHandle != IntPtr.Zero)
		{
			CloseHandle(hMappingHandle);
			hMappingHandle = IntPtr.Zero;
		}
	}

    ~shareMemOnlyRead()
    {
        if (hMappingHandle != IntPtr.Zero)
        {
            CloseHandle(hMappingHandle);
            hMappingHandle = IntPtr.Zero;
        }
    }

}
