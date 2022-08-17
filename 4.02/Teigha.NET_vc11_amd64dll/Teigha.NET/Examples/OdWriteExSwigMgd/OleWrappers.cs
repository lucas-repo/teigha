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
using Teigha.Core;
using Teigha.TD;
using System.Runtime.InteropServices;

namespace OdWriteExMgd
{
  class OleWrappers
  {
    [ComVisible(false)]
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000A-0000-0000-C000-000000000046")]
    interface ILockBytes
    {
      void ReadAt(long ulOffset, System.IntPtr pv, int cb, out UIntPtr pcbRead);
      void WriteAt(long ulOffset, System.IntPtr pv, int cb, out UIntPtr pcbWritten);
      void Flush();
      void SetSize(long cb);
      void LockRegion(long libOffset, long cb, int dwLockType);
      void UnlockRegion(long libOffset, long cb, int dwLockType);
      void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
    }
    [DllImport("ole32.dll")]
    extern static int CreateILockBytesOnHGlobal(IntPtr hGlobal, [MarshalAs(UnmanagedType.Bool)] bool fDeleteOnRelease, out ILockBytes ppLkbyt);
    [Flags]
    enum StgmConstants
    {
      STGM_READ = 0x0,
      STGM_WRITE = 0x1,
      STGM_READWRITE = 0x2,
      STGM_SHARE_DENY_NONE = 0x40,
      STGM_SHARE_DENY_READ = 0x30,
      STGM_SHARE_DENY_WRITE = 0x20,
      STGM_SHARE_EXCLUSIVE = 0x10,
      STGM_PRIORITY = 0x40000,
      STGM_CREATE = 0x1000,
      STGM_CONVERT = 0x20000,
      STGM_FAILIFTHERE = 0x0,
      STGM_DIRECT = 0x0,
      STGM_TRANSACTED = 0x10000,
      STGM_NOSCRATCH = 0x100000,
      STGM_NOSNAPSHOT = 0x200000,
      STGM_SIMPLE = 0x8000000,
      STGM_DIRECT_SWMR = 0x400000,
      STGM_DELETEONRELEASE = 0x4000000
    }
    [DllImport("ole32.dll")]
    extern static int StgCreateDocfileOnILockBytes(ILockBytes plkbyt, StgmConstants grfMode, int reserved, out IStorage ppstgOpen);

    [ComVisible(false)]
    [ComImport]
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IStorage
    {
      void CreateStream(
        /* [string][in] */ string pwcsName,
        /* [in] */ uint grfMode,
        /* [in] */ uint reserved1,
        /* [in] */ uint reserved2,
        /* [out] */ out IntPtr ppstm);

      void OpenStream(
        /* [string][in] */ string pwcsName,
        /* [unique][in] */ IntPtr reserved1,
        /* [in] */ uint grfMode,
        /* [in] */ uint reserved2,
        /* [out] */ out IntPtr ppstm);

      void CreateStorage(
        /* [string][in] */ string pwcsName,
        /* [in] */ uint grfMode,
        /* [in] */ uint reserved1,
        /* [in] */ uint reserved2,
        /* [out] */ out IStorage ppstg);

      void OpenStorage(
        /* [string][unique][in] */ string pwcsName,
        /* [unique][in] */ IStorage pstgPriority,
        /* [in] */ uint grfMode,
        /* [unique][in] */ IntPtr snbExclude,
        /* [in] */ uint reserved,
        /* [out] */ out IStorage ppstg);

      void CopyTo(
        /* [in] */ uint ciidExclude,
        /* [size_is][unique][in] */ Guid rgiidExclude, // should this be an array?
        /* [unique][in] */ IntPtr snbExclude,
        /* [unique][in] */ IStorage pstgDest);

      void MoveElementTo(
        /* [string][in] */ string pwcsName,
        /* [unique][in] */ IStorage pstgDest,
        /* [string][in] */ string pwcsNewName,
        /* [in] */ uint grfFlags);

      void Commit(
        /* [in] */ uint grfCommitFlags);

      void Revert();

      void EnumElements(
        /* [in] */ uint reserved1,
        /* [size_is][unique][in] */ IntPtr reserved2,
        /* [in] */ uint reserved3,
        /* [out] */ out IntPtr ppenum);

      void DestroyElement(
        /* [string][in] */ string pwcsName);

      void RenameElement(
        /* [string][in] */ string pwcsOldName,
        /* [string][in] */ string pwcsNewName);

      void SetElementTimes(
        /* [string][unique][in] */ string pwcsName,
        /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pctime,
        /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME patime,
        /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pmtime);

      void SetClass(
        /* [in] */ Guid clsid);

      void SetStateBits(
        /* [in] */ uint grfStateBits,
        /* [in] */ uint grfMask);

      void Stat(
        /* [out] */ out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg,
        /* [in] */ uint grfStatFlag);

    }
    
    [StructLayout(LayoutKind.Sequential)]
    struct SIZE
    {
      public int cx;
      public int cy;
    }
    
    [ComVisible(false)]
    [ComImport]
    [Guid("00000112-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IOleObject
    {
      void SetClientSite(IntPtr pClientSite);
      void GetClientSite(ref IntPtr ppClientSite);
      void SetHostNames(object szContainerApp, object szContainerObj);
      void Close(uint dwSaveOption);
      void SetMoniker(uint dwWhichMoniker, object pmk);
      void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
      void InitFromData(IntPtr pDataObject, bool fCreation, uint dwReserved);
      void GetClipboardData(uint dwReserved, ref IntPtr ppDataObject);
      void DoVerb(uint iVerb, uint lpmsg, object pActiveSite, uint lindex, uint hwndParent, uint lprcPosRect);
      void EnumVerbs(ref object ppEnumOleVerb);
      void Update();
      void IsUpToDate();
      void GetUserClassID(uint pClsid);
      void GetUserType(uint dwFormOfType, uint pszUserType);
      void SetExtent(uint dwDrawAspect, uint psizel);
      void GetExtent(uint dwDrawAspect, ref SIZE psizel);
      void Advise(object pAdvSink, uint pdwConnection);
      void Unadvise(uint dwConnection);
      void EnumAdvise(ref object ppenumAdvise);
      void GetMiscStatus(uint dwAspect, uint pdwStatus);
      void SetColorScheme(object pLogpal);
    };
    [DllImport("ole32.dll")]
    static extern int OleCreateFromFile([In] ref Guid rclsid,
       [MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, [In] ref Guid riid,
       uint renderopt, [In] IntPtr pFormatEtc, IntPtr pClientSite,
       IStorage pStg, [MarshalAs(UnmanagedType.IUnknown)] out IOleObject ppvObj);

    [DllImport("ole32.dll")]
    static extern int GetHGlobalFromILockBytes(ILockBytes pLkbyt, out IntPtr phglobal);
    [DllImport("kernel32.dll")]
    static extern UIntPtr GlobalSize(IntPtr hMem);
    [DllImport("kernel32.dll")]
    static extern IntPtr GlobalLock(IntPtr hMem);
    [DllImport("ole32.dll")]
    static extern void OleUninitialize();
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GlobalUnlock(IntPtr hMem);
    [DllImport("ole32.dll")]
    static extern int OleInitialize(IntPtr pvReserved);


    /**********************************************************************/
    /* Minimal code to create OLE compound document from any file.        */
    /**********************************************************************/
    public static OdDbOle2Frame CreateFromFile(string fileName)
    {
      OleInitialize(IntPtr.Zero);
      OdDbOle2Frame pOle2Frame = null;
//      ILockBytes pLockBytes;
//      int hr = CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);
//      IStorage pStrg;
//      hr = StgCreateDocfileOnILockBytes(pLockBytes,
//      StgmConstants.STGM_SHARE_EXCLUSIVE | StgmConstants.STGM_CREATE | StgmConstants.STGM_READWRITE, 0, out pStrg);
//      if (hr == 0)
//      {
//        IOleObject pObj;
//        GuidAttribute g = (GuidAttribute)typeof(IOleObject).GetCustomAttributes(typeof(GuidAttribute), false)[0];
//        Guid IOleObject_Guid = new Guid(g.Value);
//        Guid null_guid = Guid.Empty;
//        hr = OleCreateFromFile(ref null_guid, fileName, ref IOleObject_Guid, 1, IntPtr.Zero, IntPtr.Zero, pStrg, out pObj);
//        if (hr == 0)
//        {
//          SIZE size = new SIZE();
//          size.cx = size.cy = 1000;
//          pObj.GetExtent(1, ref size);
//          Marshal.ReleaseComObject(pObj);
//          pStrg.Commit(0);
//
//          IntPtr hGlobal;
//          hr = GetHGlobalFromILockBytes(pLockBytes, out hGlobal);
//
//
//          UIntPtr dwSize = GlobalSize(hGlobal);
//
//          IntPtr pCompoundDocData = GlobalLock(hGlobal);
//
//          /**********************************************************************/
//          /* Create OdDbOle2Frame and copy OLE object's data into it.           */
//          /**********************************************************************/
//
//          try
//          {
//            pOle2Frame = OdDbOle2Frame.createObject();
//            OdOleItemHandler pHandler = pOle2Frame.getItemHandler();
//            OdFlatMemStream strm = OdFlatMemStream.createNew(pCompoundDocData, dwSize.ToUInt64());
//            pHandler.setCompoundDocument(dwSize.ToUInt32(), strm);
//            pHandler.setDrawAspect(OdOleItemHandler.DvAspect.kContent);
//            pOle2Frame.unhandled_setHimetricSize((UInt16)size.cx, (UInt16)size.cy);
//          }
//          finally
//          {
//            GlobalUnlock(hGlobal);
//            Marshal.ReleaseComObject(pStrg);
//            Marshal.ReleaseComObject(pLockBytes);
//            OleUninitialize();
//          }
//        }
//      }
      return pOle2Frame;
    }
  }
}
