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

namespace OdBrExSwigMgd
{
  class MyServices : ExSystemServices { }
  class HostAppServices : ExHostAppServices
  {
    public override String product()
    {
      return String.Format("Teigha.NET");
    }
  }

  class Program
  {
    static Process process = new Process();

    static void Main(string[] args)
    {
      MemoryManager mMan = MemoryManager.GetMemoryManager();
      MemoryTransaction mStartTrans = mMan.StartTransaction();

      HostAppServices hostServices = new HostAppServices();
      MyServices svcs = new MyServices();
      TD_Db.odInitialize(svcs);
      hostServices.disableOutput(true);

      Console.WriteLine("OdBrExSwigMgd developed using {0} ver {1}",
        hostServices.product(), hostServices.versionString());

      if (args.Length != 1)
      {
        Console.WriteLine("usage: OdBrExSwigMgd <filename>\n");
        return;
      }

      try
      {
        OdDbDatabase pDb = null;

        string f = args[0];
        pDb = hostServices.readFile(f);

        string strOut = "";
        if (pDb != null)
        {
          dumpAcis(pDb, strOut);
        }
      }
      catch (OdError Err)
      {
        Console.WriteLine(string.Format("OdError - {0}", Err.description()));
      }
      catch (Exception Err)
      { 
        Console.WriteLine(string.Format("Other error {0}", Err.Message));
      }

      mMan.StopTransaction(mStartTrans);
      mMan.StopAll();
      TD_Db.odUninitialize();
    }

    static void dumpAcis(OdDbDatabase pDb, string os)
    {
      OdDbBlockTable pBlocks = (OdDbBlockTable)pDb.getBlockTableId().safeOpenObject();
      OdDbSymbolTableIterator pBlkIter = pBlocks.newIterator();
      for (pBlkIter.start(); !pBlkIter.done(); pBlkIter.step())
      {
        OdDbBlockTableRecord pBlock = (OdDbBlockTableRecord)pBlkIter.getRecordId().safeOpenObject();
        OdDbObjectIterator pEntIter = pBlock.newIterator();
        for (; !pEntIter.done(); pEntIter.step())
        {
          OdDbEntity pEnt = (OdDbEntity)pEntIter.objectId().safeOpenObject();
          string entityName = pEnt.ToString();
          entityName = entityName.Remove(0, 10);
          Console.WriteLine("\nEntity: " + entityName + " <" + pEnt.getDbHandle().ascii() + ">");
          if ((pEnt.isKindOf(OdDb3dSolid.desc())) ||
              (pEnt.isKindOf(OdDbBody.desc())) ||
              (pEnt.isKindOf(OdDbRegion.desc())))
          {
            Console.WriteLine("Process (y/n)? ");
            string choice = Console.ReadLine();
            if (choice == "Y" || choice == "y")
            {
              OdBrBrep br = new OdBrBrep();

              if (pEnt.isKindOf(OdDb3dSolid.desc()))
              {
                OdDb3dSolid sol = (OdDb3dSolid)pEnt;
                sol.brep(br);
              }
              else if (pEnt.isKindOf(OdDbBody.desc()))
              {
                OdDbBody body = (OdDbBody)pEnt;
                body.brep(br);
              }
              else if (pEnt.isKindOf(OdDbRegion.desc()))
              {
                OdDbRegion region = (OdDbRegion)pEnt;
                region.brep(br);
              }
              int testToRun = process.menu();
              while (testToRun > 0)
              {
                process.processOption(br, pEnt, testToRun);
                testToRun = process.menu();
              }
            }
          }
        }
      }
    }


  }
}
