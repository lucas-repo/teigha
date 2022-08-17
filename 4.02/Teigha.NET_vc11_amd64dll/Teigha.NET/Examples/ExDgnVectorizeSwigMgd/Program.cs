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
using Teigha.TG;
using System.IO;

namespace ExDgnVectorizeMgd
{
  #region services
  class Services : OdExDgnSystemServices
  {
  }
  class Srv : OdExDgnHostAppServices
  {
    public override OdDgDatabase readFile(String fileName) 
    {
      if (!File.Exists(fileName))
        return null;
      return base.readFile(fileName);
    }
    public override OdDgDatabase readFile(String fileName, FileShareMode sm)
    {
      if (!File.Exists(fileName))
        return null;
      return base.readFile(fileName, sm);
    }
  }
  #endregion
  class Program
  {
    static void Main(string[] args)
    {
        // get the memory manager
        MemoryManager mMan = MemoryManager.GetMemoryManager();
        // start the first memory transaction
        MemoryTransaction mStartTrans = mMan.StartTransaction();
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: ExDgnVectorizeMgd <filename>");
        return;
      }
      if (!File.Exists(args[0]))
      {
        Console.WriteLine(string.Format("File {0} does not exist", args[0]));
        return;
      }
      Services s = new Services();
      Teigha.Core.Globals.odrxInitialize(s);
      OdTimeStamp ts = new OdTimeStamp();
      ts.getLocalTime();
      string t = "test";
      ts.ctime(ref t);
      Console.WriteLine(t);
      Teigha.Core.Globals.odgsInitialize();
      Teigha.Core.Globals.odrxDynamicLinker().loadModule("TG_Db");
      for (int loop = 0; loop < 10; loop++)
      {
          // we entered the loop, wrap the loop actions in a memory transaction
          MemoryTransaction mTr = mMan.StartTransaction();
          Srv hostApp = new Srv();
          OdDgDatabase pDb = hostApp.readFile(args[0]);
          if (pDb == null)
          {
              Console.WriteLine("Can't open {1}", args[0]);
              return;
          }
          OdGsDevice pDevice = new ExGsSimpleDevice();
          OdDgViewGroup pViewGroup = (OdDgViewGroup)pDb.getActiveViewGroupId().openObject();
          if (pViewGroup != null)
          {
              OdDgElementId vectorizedModelId = pDb.getActiveModelId();
              OdDgElementIterator pIt = pViewGroup.createIterator();
              OdGsDCRect screenRect = new OdGsDCRect(0, 1000, 0, 1000);
              for (; !pIt.done(); pIt.step())
              {
                  //OdDgView pView = pIt.item().openObject() as OdDgView;
                  OdDgView pView = (OdDgView)(pIt.item().openObject());
                  if (pView != null && pView.getVisibleFlag())
                  {
                      //create the context with OdDgView element given (to transmit some properties)
                      OdGiContextForDgDatabase pDgnContext = OdGiContextForDgDatabase.createObject(pDb, pView);
                      OdDgElementId vectorizedViewId = pIt.item();
                      pDevice = OdGsDeviceForDgModel.setupModelView(vectorizedModelId, vectorizedViewId, pDevice, pDgnContext);

                      pDevice.onSize(screenRect);
                      pDevice.update();
                  }
              }
          }
          // stop memory transaction - i.e. dispose all significant objects in a proper order
          mMan.StopTransaction(mTr);
      }
        // the loop is finished - stop the initial (first) transaction
      mMan.StopTransaction(mStartTrans);
        // just to ensure all significant objects are disposed
      mMan.StopAll();
        // no explicit disposes and GC calls needed - memory manager and transactions took care of significant objects
        // all non-significant objects may be collected by GC in any way he likes
      //hostApp.Dispose();
      //GC.Collect();
      //GC.WaitForPendingFinalizers();
        
        // we can unitialize Teigha being sure that all significant objects are closed
      Teigha.Core.Globals.odgsUninitialize();
      Teigha.Core.Globals.odrxUninitialize();
    }
  }
}
