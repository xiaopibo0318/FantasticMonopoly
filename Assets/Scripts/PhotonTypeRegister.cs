using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PlayerManager;
using ExitGames.Client.Photon;
using System;
using UnityEditor;
using System.Text;
using System.Linq;

public class PhotonTypeRegister
{
    public int myNumber = -1;
    public string myString = string.Empty;

    public static byte[] Serialize(object obj)
    {
        PhotonTypeRegister data = (PhotonTypeRegister)obj;
        byte[] myNumberBytes = BitConverter.GetBytes(data.myNumber);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(myNumberBytes);
        }
        byte[] myStringBytes = Encoding.ASCII.GetBytes(data.myString);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(myStringBytes);
        }
        return JoinBytes(myNumberBytes, myStringBytes);
    }


    private static byte[] JoinBytes(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }
        return rv;
    }

}
