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
/* Simple application creating new DGN file                             */
/* and filling it with some objects                                     */
/************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Teigha.Core;
using Teigha.TG;

namespace ExDgnSimpleProjectMgd
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
      
      Console.WriteLine("\nExDgnSimpleProjectMgd sample program. Copyright (c) 2016, Open Design Alliance\n");
      /**********************************************************************/
      /* Parse Command Line inputs                                          */
      /**********************************************************************/
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: nExDgnSimpleProjectMgd <filename>");
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
        using (OdDgModel pModel = (OdDgModel)pDb.getActiveModelId().safeOpenObject(OpenMode.kForWrite))
        {
          pModel.setWorkingUnit(OdDgModel.WorkingUnit.kWuMasterUnit);
          pModel.setModelIs3dFlag(true);

          // Create new line
          OdDgLine3d line3d = OdDgLine3d.createObject();

          // Set endpoints
          line3d.setStartPoint(new OdGePoint3d(0, 0, 0));
          line3d.setEndPoint(new OdGePoint3d(10, 10, 10));
          line3d.setColorIndex((uint)OdCmEntityColor.ACIcolorMethod.kACIBlue);

          // Add the line to the model
          pModel.addElement(line3d);

          /********************************************************************/
          /* Set an appropriate set of views                                  */
          /********************************************************************/
          pModel.fitToView();
        }

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
