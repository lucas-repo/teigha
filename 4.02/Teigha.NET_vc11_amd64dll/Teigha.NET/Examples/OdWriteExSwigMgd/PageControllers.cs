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
using Teigha.Core;
using Teigha.TD;

namespace OdWriteExMgd
{
  /** Description:
    This PageController class implements unloading of objects in partially opened database.
  */
  class ExUnloadController : OdDbPageController
  {
    public ExUnloadController() { }
    public override int pagingType() { return (int)PagingType.kUnload; }
    public override OdStreamBuf read(long key) { return null; }
    public override bool write(out uint key, OdStreamBuf pStreamBuf) { key = 0; return false; }
    public override void setDatabase(OdDbDatabase pDb) { m_pDb = pDb; }
    public override OdDbDatabase database() { return m_pDb; }
    private OdDbDatabase m_pDb;
  }
  /** Description:
     This class implements unloading and paging to external storage.
     Remarks:
     This class contains a very simple implementation of paging, with consecutive writing in a file.
  */
  class ExPageController : ExUnloadController
  {
    private class MyPageStream : OdStreamBuf
    {
      private byte[] m_data = null;
      private Int32 m_pos = 0;
      public bool init(System.IO.FileStream fp)
      {
        byte[] tmp = new byte[4];
        if (4 != fp.Read(tmp, 0, 4))
          return false;
        Int32 len = BitConverter.ToInt32(tmp, 0);
        m_data = new byte[len];
        if (len != fp.Read(m_data, 0, len))
          return false;
        return true;
      }

      public override byte getByte()
      {
        return m_data[m_pos++];
      }

      /*public override byte[] getBytes(int nLen)
      {
        if ((nLen + m_pos) > m_data.Length)
          throw new Exception("invalid paged data request");
        byte[] res = new byte[nLen];
        Array.Copy(m_data, m_pos, res, 0, nLen);
        return res;
      }*/

      public override UInt64 seek(Int64 offset, FilerSeekType whence)
      {
        int pos = m_pos;
        switch (whence)
        {
          case FilerSeekType.kSeekFromCurrent:
            pos += (Int32)offset;
            break;
          case FilerSeekType.kSeekFromStart:
            pos = (Int32)offset;
            break;
          case FilerSeekType.kSeekFromEnd:
            pos = m_data.Length - 1 - (Int32)offset;
            break;
        }
        if (pos < 0 || pos >= m_data.Length)
          throw new Exception("invalid paged data seek request");
        m_pos = pos;
        return (UInt64)m_pos;
      }

      public override UInt64 tell()
      {
        return (UInt64)m_pos;
      }
    }
    public ExPageController() { }
    public override void Dispose()
    {
      m_fp.Close();
      base.Dispose();
    }
    public override int pagingType() { return (int)PagingType.kPage | (int)PagingType.kUnload; }
    public override OdStreamBuf read(long key)
    {
      if (m_fp == null)
        return null;
      try
      {
        m_fp.Position = key;
      }
      catch (Exception)
      {
        return null;
      }

      MyPageStream pRet = new MyPageStream();
      if (!pRet.init(m_fp))
        return null;
      return pRet;
    }
    public override bool write(out uint key, OdStreamBuf pStreamBuf)
    {
      key = 0;
      if (m_fp == null)
        return false;
      m_fp.Seek(0, System.IO.SeekOrigin.End);
      key = (uint)m_fp.Position;
      Int32 len = (Int32)pStreamBuf.length();
      m_fp.Write(BitConverter.GetBytes(len), 0, 4);
      //m_fp.Write(pStreamBuf.getBytes(len), 0, len);
      return true;
    }
    public override void setDatabase(OdDbDatabase pDb)
    {
      base.setDatabase(pDb);
      m_fp = System.IO.File.Create(System.IO.Path.GetTempFileName());
    }
    System.IO.FileStream m_fp = null;
  };
}