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
using System.IO;
using System.Text;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnReadWrite
{
    class Program
    {
        class MyStream : OdStreamBuf
        {
            System.IO.FileStream _fs;
            public MyStream(System.IO.FileStream fs) { _fs = fs; }
            public override uint  getBytesByNum(OdUInt8Array data, ulong startPos, uint numBytes)
            {
                byte[] dat = new byte[numBytes];
                int res = _fs.Read(dat, (int)startPos, (int)numBytes);
                data.Clear();
                data = new OdUInt8Array(dat);
                return (uint)res;
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
            /*public override bool ttfFileNameByDescriptor(OdTtfDescriptor description, ref string filename)
            {
              filename = "test";
              return false;
            }*/
        }
        static void Main(string[] args)
        {
            // start Memory manager
            // create first memory transaction
            MemoryManager mMan = MemoryManager.GetMemoryManager();
            MemoryTransaction mStartTrans = mMan.StartTransaction();
            Console.WriteLine("\nExDgnReadWrite sample program. Copyright (c) 2016, Open Design Alliance\n");
            /**********************************************************************/
            /* Parse Command Line inputs                                          */
            /**********************************************************************/
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ExReadWriteMgd <srcfilename> <dstfilename>");
                return;
            }
            String srcFileName = args[0];
            String dstFileName = args[1];
            if (!File.Exists(srcFileName))
            {
              Console.WriteLine(string.Format("File {0} does not exist", srcFileName));
              return;
            }
            Services Serv = new Services();
            Srv HostApp = new Srv();
            Teigha.Core.Globals.odrxInitialize(Serv);
            try
            {
                /**********************************************************************/
                /* Initialize Teigha™ for .dgn files                                               */
                /**********************************************************************/
                // store module object and dispose it explicitly later
                OdRxModule mod = Teigha.Core.Globals.odrxDynamicLinker().loadModule("TG_Db", false);

                /********************************************************************/
                /* Read the DGN file into the OdDgDatabase object                   */
                /********************************************************************/
                OdDgDatabase pDb = null;
                try
                {
                    if (String.Empty != HostApp.findFile(srcFileName))
                    {
                        pDb = HostApp.readFile(srcFileName);

                    }
                }
                catch (Exception Err)
                {
                    do
                    {
                        Console.WriteLine(Err.GetType().Name + ": ");
                        Console.WriteLine(Err.Message);
                        Err = Err.InnerException;
                    } while (Err != null);
                }
                /********************************************************************/
                /* Write the OdDgDatabase data into the DGN file                    */
                /********************************************************************/
                if (null != pDb)
                {
                    // make sure that all the temporary created objects are closed
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                    pDb.writeFile(dstFileName);
                }
                else
                {
                    Console.WriteLine("pDb is null");
                }
                // make sure that downloaded TG_Db module and its dependencies are closed
                //mod.Dispose();

                // explicitly dispose manually created objects, otherwise GC will try to dispose them during the above GC.Collect(); call
                //HostApp.Dispose();
                //Serv.Dispose();

                ////////////////////////////////////////////////////////
                // as the sample is not very big a test for bug added //
                ////////////////////////////////////////////////////////
                // for bug swig-340
                OdMemoryStream file = OdMemoryStream.createNew();
                file.putBytes(new byte[100000]);
                // get the entire file length
                UInt64 len = file.length();
                // the length of the block we are going to use
                UInt32 cur_len = 10000;
                // current position in the file - we'll start from the very beginning
                UInt64 cur_pos = 0;
                // array to store the obtained data
                OdUInt8Array res_arr = new OdUInt8Array();
                // while the current position is less than the length
                while (cur_pos < len)
                {
                    // initalize the OdUInt8Array object - in that array we'll get the obtained data
                    OdUInt8Array bytes = new OdUInt8Array();
                    // call getBytesByNum - use the return value (bytes actually read) to increment the current position
                    cur_pos += file.getBytesByNum(bytes, cur_pos, cur_len);
                    // add the obtained data to target array
                    res_arr.AddRange(bytes);
                }
                // just a dumb test - check whether the length of the data obtained is the same as the length of the file
                if (len == (ulong)res_arr.Count)
                {
                    Console.WriteLine("getBytesByNum test: EQUAL");
                }
                else
                {
                    Console.WriteLine("getBytesByNum test: NOT EQUAL");
                }
            }
            /********************************************************************/
            /* Display the error                                                */
            /********************************************************************/
            catch (OdError Err)
            {
                Console.WriteLine("\nTeigha 174 for .DGN Error: \n" + Err.description());
            }
            catch (Exception Err)
            {
                Console.WriteLine("\nUnknown Error.\n" + Err.Message);
            }
            finally
            {
                // stop transaction
                mMan.StopTransaction(mStartTrans);
                mMan.StopAll();
            }
            // pay attention that there are no explicit Dispose calls and GC calls - MemoryManager and MemoryTransaction do the job
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            Teigha.Core.Globals.odrxUninitialize();
        }
    }
}
