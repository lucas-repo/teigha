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

/************************************************************************/
/* This console application creates and populates DGN file.             */
/*                                                                      */
/* Calling sequence:                                                    */
/*                                                                      */
/* ExDgnCreate <filename>                                               */
/*                                                                      */
/************************************************************************/

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnCreate
{
  class Services : OdExDgnSystemServices
  {
  }
  class HostSrv : OdExDgnHostAppServices
  {
  }
  class Program
  {
    static void Main(string[] args)
    {
        // start memory manager
      MemoryManager mMan = MemoryManager.GetMemoryManager();
        // start first memory transaction
      MemoryTransaction mStartTrans = mMan.StartTransaction();
      
      Console.WriteLine("\nExDgnCreateSwigMgd sample program. Copyright (c) 2016, Open Design Alliance\n");
      /**********************************************************************/
      /* Parse Command Line inputs                                          */
      /**********************************************************************/
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: ExDgnCreate <filename>");
        return;
      }
      Services s = new Services();
      Globals.odrxInitialize(s);
      Globals.odgsInitialize();
      HostSrv hostApp = new HostSrv();

      Globals.odrxDynamicLinker().loadModule("TG_Db");
      try
      {
        OdStringArray arrayRscList = new OdStringArray();
        arrayRscList.Add("font.rsc");
        hostApp.setMS_SYMBRSRC(arrayRscList);

        /********************************************************************/
        /* Create a default OdDgDatabase object                             */
        /********************************************************************/
        OdDgDatabase pDb = hostApp.createDatabase();
        /********************************************************************/
        /* Fill the database                                                */
        /********************************************************************/
          // filling the database procedure will create many different objects - wrap it in a memory transaction
          // start a memory transaction
        MemoryTransaction mTrans = mMan.StartTransaction();
        OdExDgnFiller filler = new OdExDgnFiller();
        filler.fillDatabase(pDb);
          // stop a memory transaction
        mMan.StopTransaction(mTrans);
        /********************************************************************/
        /* Write the database                                               */
        /********************************************************************/
          // no additional actions - after stopping the memory transaction we are sure that all objects are closed
        pDb.writeFile(args[0]);
      }
      catch (System.Exception ex)
      {
        Console.WriteLine("Unhandled exception: {0}", ex.Message);
      }
      
        // stop the initial transaction
      mMan.StopTransaction(mStartTrans);
        // just to ensure no transactions were lost
      mMan.StopAll();

        // MemoryManager + MemoryTransactions did all the necessary job, 
        // no GC calls are needed to be sure we can Unitialize Teigha
      //GC.Collect();
      //GC.WaitForPendingFinalizers();
      Globals.odgsUninitialize();
      Globals.odrxUninitialize();
    }
  }
}
