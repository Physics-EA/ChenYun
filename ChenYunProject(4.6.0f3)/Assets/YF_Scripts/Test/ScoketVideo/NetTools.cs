using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

public class NetTools : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

        public static IntPtr GetIntPtr(object structObj)
        {
            int structSize = Marshal.SizeOf(structObj);

            IntPtr structPtr = Marshal.AllocHGlobal(structSize);

            try
            {
                Marshal.StructureToPtr(structObj, structPtr, false);
                return structPtr;
            }
            finally
            {
                Marshal.FreeHGlobal(structPtr);
            }
        }

        private static byte _fillChar = 0;      //the fill character
        public static byte[] CodeBytes(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = string.Empty;
            }

            byte[] result = new byte[len];
            byte[] strBytes = Encoding.Default.GetBytes(str);

            //copy the array converted into result, and fill the remaining bytes with 0
            for (int i = 0; i < len; i++)
                result[i] = ((i < strBytes.Length) ? strBytes[i] : _fillChar);

            return result;
        }

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
        public static object BytesToStuct(byte[] bytes, Type type)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object bj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return bj;
        }

        public static bool PasswordEquals(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length) return false;
            if (b1 == null || b2 == null) return false;
            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;
            return true;
        }
}
