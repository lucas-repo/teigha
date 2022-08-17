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
using System.Text;
using Teigha.Core;
using Teigha.TD;
using Microsoft.Win32;

/************************************************************************/
/* This console application creates and populates DWG/DXF/DXB file of   */
/* the specified version.                                               */
/*                                                                      */
/* Calling sequence:                                                    */
/*                                                                      */
/* OdWriteEx <filename> [OutVer] [OutType] [-DO]                        */
/*                                                                      */
/*    OutVer can be any of ACAD12, ACAD13, ACAD14, ACAD2000, ACAD2004,  */
/*    ACAD2007, ACAD2010                                                */
/*                                                                      */
/*    OutType can be any of DWG, DXF, DXB                               */
/*                                                                      */
/*    -DO disables progress meter output                                */
/*                                                                      */
/* The following files from the Examples\OdWriteEx folder are           */
/* referenced by <filename> and must either be in the same folder as    */
/* <filename> or in a folder in the support path                        */
/*                                                                      */
/*   OdWriteEx.jpg, OdWriteEx.sat, OdWriteEx XRef.dwg                   */
/*                                                                      */
/* In addition, the folder containing OdWriteEx.txt must be hard-coded  */
/* in DbFiller::addRText                                                */
/************************************************************************/

namespace OdWriteExMgd
{
  class Srv : ExHostAppServices
  {
    OdGsDevice _bd = null;
    public override OdGsDevice gsBitmapDevice()
    {
      if (_bd == null)
      {
        try
        {
          using(OdGsModule pGsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinBitmap.txv"))
            _bd = pGsModule.createBitmapDevice();
        }
        catch (OdError)
        {
        }
      }
      return _bd;
    }
    public override void Dispose()
    {
      if (null != _bd)
      {
        _bd.Dispose();
        _bd = null;
      }
      base.Dispose();
    }
    // save created page controllers from GC
    List<OdDbPageController> m_pPageControllers = new List<OdDbPageController>();
    public override OdDbPageController newPageController()
    {
      // Simple unloading of objects for partially loaded database.
      //
      // OdDbPageController pPageController = new ExUnloadController();
      // m_pPageControllers.Add(pPageController);
      // return pPageController;

      // Unloading of objects for partially loaded database
      // and paging of objects through ExPageController.
      //
      // OdDbPageController pPageController = new ExPageController();
      // m_pPageControllers.Add(pPageController);
      // return pPageController;
      
      // Paging is not used.
      //
      return null;
    }
    public override OdDbUndoController newUndoController()
    {
      return null;
    }

    int m_myMeterCurrent;
    int m_myMeterOld;
    int m_myMeterLimit;
    //public WeakReference m_myDb = null;
    /*public override OdDbDatabase createDatabase(bool createDefault, MeasurementValue measurement)
    {
      OdDbDatabase ret = base.createDatabase(createDefault, measurement);
      m_myDb = new WeakReference(ret);
      return ret;
    }
    public override void setLimit(int max)
    {
      m_myMeterLimit = max;
      m_myMeterCurrent = 0;
      m_myMeterOld = 0;
      base.setLimit(max);
    }
    public override void meterProgress()
    {
      OdDbDatabase db = m_myDb.Target as OdDbDatabase;
      if (db != null)
      {
        ++m_myMeterCurrent;
        double f1 = (double)m_myMeterCurrent / m_myMeterLimit * 100;
        double f2 = (double)m_myMeterOld / m_myMeterLimit * 100;
        if ((f1 - f2) > 0.7)
        {
          TD_Db.odDbPageObjects(db);
          m_myMeterOld = m_myMeterCurrent;
        }
      }
      base.meterProgress();
    }*/
  }
  
  
  /************************************************************************/
  /* Define an a Custom OdDbAuditInfo class                               */
  /************************************************************************/
  class AuditInfo : OdDbAuditInfo
  {
    OdStreamBuf m_ReportFile;
    bool m_isSomething;
    String m_ReportFileName;

    public AuditInfo(String reportFileName)
    {
      m_isSomething = false;
      m_ReportFileName = reportFileName;
      /********************************************************************/
      /* Create a report file                                             */
      /********************************************************************/
      m_ReportFile = Globals.odrxSystemServices().createFile(m_ReportFileName,
        FileAccessMode.kFileWrite, FileShareMode.kShareDenyNo, FileCreationDisposition.kCreateAlways);
    }
    public override void Dispose()
    {
      /********************************************************************/
      /* Refer user to OD_T("Audit Report.txt") if error(s) have been output.   */
      /********************************************************************/
      if (m_isSomething)
      {
        Console.WriteLine("\n\nAudit error : Check \"{0}\" for errors.", m_ReportFileName);
      }
      else
      {
        Console.WriteLine("\n\nAudit : ok.\n");
      }
      base.Dispose();
    }

    /**********************************************************************/
    /* Print error to ReportFile                                          */
    /**********************************************************************/
    public override void printError(String strName, String strValue, String strValidation, String strDefaultValue)
    {
      m_isSomething = true;
      String str = String.Format("{0} {1} {2} {3}\n",
        strName, strValue, strValidation, strDefaultValue);
      m_ReportFile.putBytes(Encoding.Unicode.GetBytes(str));
    }

    /**********************************************************************/
    /* Print info to ReportFile                                           */
    /**********************************************************************/
    public override void printInfo(String strInfo)
    {
      m_ReportFile.putBytes(Encoding.Unicode.GetBytes(strInfo + "\n"));
    }
  }
  class Program
  {
    /************************************************************************/
    /* Set the ACAD environment variable to the AutoCAD support path        */
    /************************************************************************/
    static bool findAcadInProfile(out string supportPath)
    {
      supportPath = "";
      /********************************************************************/
      /* Open the Registry key Software\Autodesk\AutoCAD                  */
      /********************************************************************/
      RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Autodesk\\AutoCAD");
      if (key == null)
        return false;
      /********************************************************************/
      /* Get the current AutoCAD Version                                  */
      /********************************************************************/
      object val = key.GetValue("CurVer");
      key.Close();
      if (val == null)
        return false;
      string path = "Software\\Autodesk\\AutoCAD\\" + (string)val;
      /******************************************************************/
      /* The key is in the form                                         */
      /* HKEY_CURRENT_USER\Software\Autodesk\AutoCAD\Rxx.x\             */
      /******************************************************************/
      key = Registry.CurrentUser.OpenSubKey(path);
      if (key == null)
        return false;
      val = key.GetValue("CurVer");
      key.Close();
      if (val == null)
        return false;

      /******************************************************************/
      /* The key is in the form                                         */
      /* HKEY_CURRENT_USER\Software\Autodesk\AutoCAD\Rxx.x\             */
      /* ACAD-xxx:xxx\Profiles\<<Unnamed Profile>>\General              */
      /******************************************************************/
      path = path + "\\" + (string)val + "\\Profiles\\<<Unnamed Profile>>\\General";
      key = Registry.CurrentUser.OpenSubKey(path);
      if (key == null)
        return false;
      val = key.GetValue("ACAD");
      key.Close();
      if (val == null)
        return false;
      /**********************************************************************/
      /* We now have the Support Path                                       */
      /**********************************************************************/
      supportPath = (string)val;
      /**********************************************************************/
      /* Set ACAD environment variable                                      */
      /**********************************************************************/
      Environment.SetEnvironmentVariable("ACAD", supportPath);
      return true;
    }
    static bool tryToFindAcad()
    {
      string supportPath = Environment.GetEnvironmentVariable("ACAD");
      if (supportPath == null)
      {
        if (!findAcadInProfile(out supportPath))
        {
          /********************************************************************/
          /* Try the Windows Application Path                                 */
          /********************************************************************/
          RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\acad.exe");
          if (key == null)
            return false;
          object val = key.GetValue("Path");
          if (val == null)
            return false;
          /******************************************************************/
          /* We now have the Support Path                                   */
          /******************************************************************/
          supportPath = (string)val;
          /******************************************************************/
          /* Set ACAD environment variable                                  */
          /******************************************************************/
          Environment.SetEnvironmentVariable("ACAD", supportPath);
        }
      }
      return true;
    }
    static void Main(string[] args)
    {
        MemoryManager mMan = MemoryManager.GetMemoryManager();
        MemoryTransaction mStartTrans = mMan.StartTransaction();
        /**********************************************************************/
        /* Set command line defaults                                          */
        /**********************************************************************/
        SaveType fileType = SaveType.kDwg;
        DwgVersion outVer = DwgVersion.vAC24;
        bool disableOutput = false;
//        ExSystemServices Serv = new ExSystemServices();
//        Srv HostApp = new Srv();
        /**********************************************************************/
        /* Display the Product and Version that created the executable        */
        /**********************************************************************/
//        Console.WriteLine("OdWriteEx developed using {0} ver {1}",
//          HostApp.product(), HostApp.versionString());
        /**********************************************************************/
        /* Parse Command Line inputs                                          */
        /**********************************************************************/
        bool bInvalidArgs = (args.Length < 1);
        if (bInvalidArgs)
        {
            bInvalidArgs = true;
        }

        /**********************************************************************/
        /* Set file version                                                   */
        /**********************************************************************/
        if (args.Length >= 2)
        {
            switch (args[1])
            {
                case "ACAD12": outVer = DwgVersion.vAC12; // R11-12
                    break;
                case "ACAD13": outVer = DwgVersion.vAC13; // R13
                    break;
                case "ACAD14": outVer = DwgVersion.vAC14; // R14
                    break;
                case "ACAD2000": outVer = DwgVersion.vAC15; // 2000-2002
                    break;
                case "ACAD2004": outVer = DwgVersion.vAC18; // 2004-2006
                    break;
                case "ACAD2007": outVer = DwgVersion.vAC21; // 2007
                    break;
                case "ACAD2010": outVer = DwgVersion.vAC24; // 2010
                    break;
                default:
                    bInvalidArgs = true;
                    break;
            }
        }
        /**********************************************************************/
        /* Set file type                                                      */
        /**********************************************************************/
        if (args.Length >= 3)
        {
            switch (args[2])
            {
                case "DWG": fileType = SaveType.kDwg;
                    break;
                case "DXF": fileType = SaveType.kDxf;
                    break;
                case "DXB": fileType = SaveType.kDxb;
                    break;
                default:
                    bInvalidArgs = true;
                    break;
            }
        }
        /**********************************************************************/
        /* Disable progress meter                                             */
        /**********************************************************************/
        if (args.Length >= 4 && args[3] == "-DO")
        {
            disableOutput = true;
        }

        if (bInvalidArgs)
        {
            Console.WriteLine("\n\tusage: OdWriteExSwigMgd <filename> [OutVer] [OutType] [-DO]");
            Console.WriteLine("\tOutVer can be any of ACAD12, ACAD13, ACAD14, ACAD2000, ACAD2004, ACAD2007, ACAD2010\n");
            Console.WriteLine("\tOutType can be any of DWG, DXF, DXB\n");
            Console.WriteLine("\t-DO disables progress meter output.\n");
            return;
        }
        tryToFindAcad();

        String dstFileNameOrigin = args[0];
        bool bDwg = dstFileNameOrigin.EndsWith(".dwg");
        if (!bDwg)
        {
            Console.WriteLine("\n\tOdWriteEx: not .dwg file ");
            return;
        }
        int lastLocation = dstFileNameOrigin.LastIndexOf(".dwg");
        String dstFileName1 = dstFileNameOrigin.Substring(0, lastLocation);

        //for (int i = 0; i < 5; i++)
        {
            MemoryTransaction mTr1 = mMan.StartTransaction();
            ExSystemServices Serv = new ExSystemServices();
            Srv HostApp = new Srv();
                    Console.WriteLine("OdWriteExSwigMgd developed using {0} ver {1}",
                      HostApp.product(), HostApp.versionString());

                    String dstFileName = dstFileName1 + ".dwg";//dstFileName1 + i + ".dwg";

            /************************************************************************/
            /* Disable/Enable progress meter                                       */
            /************************************************************************/
            HostApp.disableOutput(disableOutput);


            /**********************************************************************/
            /* Initialize Teigha                                                  */
            /**********************************************************************/
            TD_Db.odInitialize(Serv);

            OdWinNTCrypt.rxInit();
            Globals.odrxServiceDictionary().putAt("OdCrypt", OdWinNTCrypt.desc());

            /**********************************************************************/
            /* Load and Release RecomputeDimBlock                                 */
            /**********************************************************************/
            Globals.odrxDynamicLinker().loadApp("RecomputeDimBlock").Dispose();
            Globals.odrxDynamicLinker().loadApp("ExFieldEvaluator").Dispose();
            try
            {
                /********************************************************************/
                /* Create a default OdDbDatabase object                             */
                /********************************************************************/
                //using (OdDbDatabase pDb = HostApp.createDatabase(true, MeasurementValue.kEnglish))
                OdDbDatabase pDb = HostApp.createDatabase(true, MeasurementValue.kEnglish);
                {
                    OdDbDatabaseSummaryInfo pSumInfo = TD_Db.oddbGetSummaryInfo(pDb);
                    pSumInfo.setComments("File was created by OdWriteEx");
                    TD_Db.oddbPutSummaryInfo(pSumInfo);

                    /********************************************************************/
                    /* Fill the database                                                */
                    /********************************************************************/
                    MemoryTransaction mTr2 = mMan.StartTransaction();
                    DbFiller filler = new DbFiller();
                    filler.fillDatabase(pDb);

                    // make sure that all the created objects are closed
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();

                    /********************************************************************/
                    /* Audit the database                                               */
                    /********************************************************************/
                    //using (AuditInfo aiAppAudit = new AuditInfo(System.IO.Path.GetDirectoryName(args[0]) + "AuditReport.txt"))
                    AuditInfo aiAppAudit = new AuditInfo(System.IO.Path.GetDirectoryName(args[0]) + "AuditReport.txt");
                    {
                        aiAppAudit.setFixErrors(true);
                        aiAppAudit.setPrintDest(OdAuditInfo.PrintDest.kBoth);
                        Console.WriteLine("\n");
                        pDb.auditDatabase(aiAppAudit);
                    }

                    OdSecurityParams securityParams = new OdSecurityParams();
                    securityParams.password = "Teigha";
                    securityParams.nFlags = 1;
                    securityParams.nProvType = 0xd;
                    securityParams.provName = "Microsoft Base DSS and Diffie-Hellman Cryptographic Provider";
                    /************************************************************************/
                    /* Uncomment the following line to add the password "Teigha" to the     */
                    /* R18+ file to be created                                              */
                    /************************************************************************/
                    //pDb.setSecurityParams(securityParams);

                    /********************************************************************/
                    /* Write the database                                                */
                    /********************************************************************/
                    //pDb.writeFile(args[0], fileType, outVer, true /* save preview */);
                    mMan.StopTransaction(mTr2);
                    //mMan.StopTransaction(mTr1);
                    pDb.writeFile(dstFileName, fileType, outVer, true /* save preview */);
                    Console.WriteLine("Database has been saved as {0}", pDb.getFilename());

                }
            }
            catch (OdError e)
            {
                Console.WriteLine(string.Format("Error: {0}", e.description()));
            }
            catch (Exception err)
            {
                Console.WriteLine("\nUnexpected error.");
                throw;
            }
            finally
            {
                mMan.StopTransaction(mTr1);
            }
            //HostApp.Dispose();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            /**********************************************************************/
            /* Uninitialize Teigha                                                */
            /**********************************************************************/
            Globals.odrxServiceDictionary().remove("OdCrypt");
            OdWinNTCrypt.rxUninit();
            mMan.StopTransaction(mStartTrans);
            mMan.StopAll();
            try
            {
                TD_Db.odUninitialize();
            }
            catch (OdError e)
            {
                Console.WriteLine("Cleanup error: " + e.description());
            }
            //HostApp.Dispose();
            //Serv.Dispose();
            Teigha.Core.Helpers.odUninit();
        }
    }
  }
}
