/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Teigha.Core;
using Teigha.TD;

namespace OdWriteExMgd
{
  // This is a rare case when you need to override underlying C++ RTTI
  //
  class OdWinNTCryptClass : OdRxClass
  {
    public OdWinNTCryptClass(){}
    public override OdRxObject create()
    {
      return new OdWinNTCrypt();
    }
    public override String appName() { return "OdWriteEx"; }
    public override String dxfName() { return ""; }
    public override String name() { return "OdWinNTCrypt"; }
    public override DwgVersion getClassVersion(out Teigha.Core.MaintReleaseVer pMaintReleaseVer)
    {
      pMaintReleaseVer = 0;
      return DwgVersion.vAC24;
    }
    public override DwgVersion getClassVersion() { return DwgVersion.vAC24; }
    public override UInt32 proxyFlags() { return 0; }
    public override bool isDerivedFrom(OdRxClass pClass)
    {
      if (pClass == this)
        return true;
      return OdCrypt.desc().isDerivedFrom(pClass);
    }
    public override OdRxClass myParent() { return OdCrypt.desc(); }
  }
  class OdWinNTCrypt : OdCrypt
  {
    static OdWinNTCryptClass g_pDesc;
    public static new OdRxClass desc() { return g_pDesc; }
    public static void rxInit()
    {
      g_pDesc = new OdWinNTCryptClass();
      Globals.odrxClassDictionary().putAt("OdWinNTCrypt", g_pDesc);
    }
    public static void rxUninit()
    {
      Globals.odrxClassDictionary().remove("OdWinNTCrypt");
      g_pDesc = null;
    }
    public override OdRxClass isA() { return g_pDesc; }
    public override OdRxObject queryX(OdRxClass pClass)
    {
      if (desc().isDerivedFrom(pClass))
        return this;
      else
        return null;
    }

    // native CryptoAPI is used because DWG encryption is RC4 by default
    const uint CRYPT_VERIFYCONTEXT = 0xF0000000;
    const uint ALG_CLASS_HASH = (4 << 13);
    const uint ALG_TYPE_ANY = (0);
    const uint ALG_SID_MD5 = 3;
    const uint CALG_MD5 = (ALG_CLASS_HASH | ALG_TYPE_ANY | ALG_SID_MD5);
    const uint CRYPT_NO_SALT = 0x00000010;
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer,
    string pszProvider, uint dwProvType, uint dwFlags);
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool CryptCreateHash(IntPtr hProv, uint algId, IntPtr hKey, uint dwFlags, ref IntPtr phHash);
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptHashData(IntPtr hHash, byte[] pbData, uint dataLen, uint flags);
    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptDeriveKey(IntPtr hProv, uint Algid, IntPtr hBaseData, uint flags, ref IntPtr phKey);
    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, int Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen);
    [DllImport(@"advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, int Final, uint dwFlags, byte[] pbData, ref uint pdwDataLen, uint dwBufLen);
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptDestroyHash(IntPtr hHash);
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptDestroyKey(IntPtr phKey);
    [DllImport("advapi32.dll")]
    static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

    IntPtr m_hCryptProv; // the provider handle
    IntPtr m_hHash;      // the hash object
    IntPtr m_hKey;       // the session key
    public OdWinNTCrypt()
    {
      m_hCryptProv = IntPtr.Zero;
      m_hHash = IntPtr.Zero;
      m_hKey = IntPtr.Zero;
    }
    public override void Dispose()
    {
      clear();
    }
    void clear()
    {
      if (m_hHash != IntPtr.Zero)
        CryptDestroyHash(m_hHash);
      m_hHash = IntPtr.Zero;
      if (m_hKey != IntPtr.Zero)
        CryptDestroyKey(m_hKey);
      m_hKey = IntPtr.Zero;
      if (m_hCryptProv != IntPtr.Zero)
        CryptReleaseContext(m_hCryptProv, 0);
      m_hCryptProv = IntPtr.Zero;
    }
    public override bool initialize(OdSecurityParams secParams)
    {
      clear();
      // Get a handle to the default provider. 
      if (!CryptAcquireContext(ref m_hCryptProv, "", secParams.provName, secParams.nProvType, CRYPT_VERIFYCONTEXT))
        return false;
      // Create a hash object.
      if (!CryptCreateHash(m_hCryptProv, CALG_MD5, IntPtr.Zero, 0, ref m_hHash))
        return false;
      // Hash in the password data.
      byte[] data = Encoding.Unicode.GetBytes(secParams.password);
      if (!CryptHashData(m_hHash, data, (uint)data.Length, 0))
        return false;
      // Derive a session key from the hash object. 
      uint dwFlags = secParams.nKeyLength << 16;
      dwFlags |= CRYPT_NO_SALT;
      return CryptDeriveKey(m_hCryptProv, secParams.nAlgId, m_hHash, dwFlags, ref m_hKey);
    }
    public override bool decryptData(byte[] buffer)//, UInt32 bufferSize)
    {
      UInt32 bufferSize = (UInt32)buffer.Length;
      return CryptDecrypt(m_hKey, IntPtr.Zero, 1, 0, buffer, ref bufferSize);
    }
    public override bool encryptData(byte[] buffer)//, UInt32 bufferSize)
    {
      UInt32 bufferSize = (UInt32)buffer.Length;
      return CryptEncrypt(m_hKey, IntPtr.Zero, 1, 0, buffer, ref bufferSize, bufferSize);
    }
  }
}
