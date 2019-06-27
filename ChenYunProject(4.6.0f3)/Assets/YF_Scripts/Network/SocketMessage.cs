using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;


public class SocketMessage
{
    public ALLST_GENERIC realHeader;
    public byte[] realMsg;

    public SocketMessage(byte[] fullMsg, int offset,int size)
    {
        // 获取消息头
        ALLST_GENERIC fHeader = SocketData.Parse<ALLST_GENERIC>(fullMsg, offset);
        realHeader = fHeader;
        realMsg = new byte[fullMsg.Length - offset];
        Buffer.BlockCopy(fullMsg, offset, realMsg, 0, size);
    }
}






