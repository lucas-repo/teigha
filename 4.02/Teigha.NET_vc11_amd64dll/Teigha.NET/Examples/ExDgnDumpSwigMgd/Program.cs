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
using System.Collections;
//using System.Linq;
using System.Text;
using System.IO;
using Teigha.Core;
using Teigha.TG;
using System.Runtime.Remoting;
using System.Reflection;


namespace ExDgnDumpSwigMgd
{
    class Program
    {
        public static StreamWriter DumpStream = null;
        public static OdRxObject_Dumper GetProperType(OdRxObject pObject)
        {
            String CurType = pObject.GetType().ToString();
            CurType = CurType.Replace("Teigha.TG", "ExDgnDumpSwigMgd");
            CurType = string.Format("{0}_Dumper", CurType);
            try
            {
              ObjectHandle obj = AppDomain.CurrentDomain.CreateInstance("ExDgnDumpSwigMgd", CurType);
              return (OdRxObject_Dumper)obj.Unwrap();
            }
            catch
            {
              return null;
            }
        }

        class MyStream : OdStreamBuf
        {
            System.IO.FileStream _fs;
            public MyStream(System.IO.FileStream fs) { _fs = fs; }
            /*public override byte[] getBytes(int size)
            {
                byte[] data = new byte[size];
                _fs.Read(data, 0, size);
                return data;
            }*/
            //public override byte[] getBytesByNum(uint numBytes)
            public override uint  getBytesByNum(OdUInt8Array data, ulong startPos, uint numBytes)
            {
                byte[] dat = new byte[numBytes];
                _fs.Read(dat, 0, (int)numBytes);
                data = new OdUInt8Array(dat);
                return numBytes;
            }

        }
        class Services : OdExDgnSystemServices
        {
            public override OdStreamBuf createFile(String filename, FileAccessMode accessMode)
            {
                if (accessMode == FileAccessMode.kFileRead)
                {
                    return new MyStream(System.IO.File.OpenRead(filename));
                }
                return null;
            }
        }
        class Srv : OdExDgnHostAppServices
        {
          public override bool ttfFileNameByDescriptor(OdTtfDescriptor description, ref string filename)
            {
              filename = "test";
              return false;
            }
        }
        static void Main(string[] args)
        {
            MemoryManager mMan = MemoryManager.GetMemoryManager();
            MemoryTransaction mStartTrans = mMan.StartTransaction();
            ///////////
            if (args.Length < 2)
            {
              Console.WriteLine("\n\tUsage: ExDgnDumpSwigMgd <srcfilename> <dstfilename> [-Multirun] ");
                Console.WriteLine("\tfor multirun usage:  <dstfilename> - short name");
                Console.WriteLine("\t                     -Multirun = 1");
                Console.WriteLine("\t                     and /output/ subfolder should be existed ");
                return;
            }
            String szSource = args[0];
            if (!File.Exists(szSource))
            {
              Console.WriteLine(string.Format("File {0} does not exist", szSource));
              return;
            }
            String szDump = args[1];
            bool bMultirun = false;
            if (args.Length > 2)
            {
              bMultirun = true;
            }
            else
            {
              DumpStream = new StreamWriter(szDump);
              DumpStream.WriteLine("Dump of the file " + szDump);
            }

            Services Serv = new Services();
            Srv HostApp = new Srv();            
            Globals.odrxInitialize(Serv);
            Globals.odgsInitialize();
            int j = (bMultirun) ? 0 : 19;
            for (; j < 20; j++)
            {
                if (bMultirun)
                {
                  DumpStream = new StreamWriter(Directory.GetCurrentDirectory() + "//output//" + j.ToString() + szDump);
                }
                MemoryTransaction mTrans = mMan.StartTransaction();
                try
                {
                    if (bMultirun)
                    {
                      DumpStream.WriteLine("Dump of the file " + j.ToString() + szDump);
                    }
                    Globals.odrxDynamicLinker().loadModule("TG_Db", false);
                    OdDgDatabase pDb = null;
                    ArrayList arrayRscList = new ArrayList();
                    String sRscPath = "C:\\Documents and Settings\\All Users\\Application Data\\Bentley\\WorkSpace\\System\\Symb\\font.rsc";
                    arrayRscList.Add(sRscPath);
                    if (String.Empty != HostApp.findFile(szSource))
                    {
                        pDb = HostApp.readFile(szSource);
                    }
                    OdDgDumper dumper = new OdDgDumper();
                    dumper.rootDump(pDb);
                    //dumper.Dispose();
                    //pDb.Dispose();

                    //DumpStream.Close();
                    //DumpStream.Dispose();
                }
                catch (OdError Err)
                {
                    Console.WriteLine(string.Format("OdError - {0}", Err.description()));
                }
                catch (Exception Err)
                {
                    Console.WriteLine(string.Format("Other error {0}", Err.Message));
                }
                finally
                {
                    DumpStream.Close();
                    mMan.StopTransaction(mTrans);
                    //DumpStream.Dispose();
                }
            }
            //DumpStream.Close();
            mMan.StopTransaction(mStartTrans);
            mMan.StopAll();
            Globals.odgsUninitialize();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            try
            {
                Globals.odrxUninitialize();
            }
            catch (Exception err)
            {
                Console.WriteLine(string.Format("Uninitialize error {0}", err.Message));
            }
        }
    }
}
