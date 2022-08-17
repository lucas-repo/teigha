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
/* This console application creates and populates DWG/DXF/DXB file of   */
/* the specified version.                                               */
/*                                                                      */
/* Calling sequence:                                                    */
/*                                                                      */
/* OdWriteExMgd <filename> [OutVer] [OutType] [-NoWait]                 */
/*                                                                      */
/*    OutVer can be any of ACAD12, ACAD13, ACAD14, ACAD2000, ACAD2004,  */
/*    ACAD2007, ACAD2010, ACAD2013                                      */
/*                                                                      */
/*    OutType can be any of DWG, DXF, DXB                               */
/*                                                                      */
/*    -NoWait disables prompt of pressing ENTER at the end of execution */
/*                                                                      */
/* The following files from the Examples\ManagedWrappers\OdWriteExMgd   */
/* folder are referenced by <filename> and must either be               */
/* in the same folder as <filename> or in a folder in the support path  */
/*                                                                      */
/*   OdWriteEx.jpg, OdWriteEx.sat, OdWriteEx XRef.dwg                   */
/*                                                                      */
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Teigha.Runtime;
using Teigha.DatabaseServices;
using System.IO;

namespace OdWriteExMgd
{
  /************************************************************************/
  /* Define an a Custom OdDbAuditInfo class                               */
  /************************************************************************/
  class CustomAuditInfo : AuditInfo
  {
    StreamWriter m_ReportFile;
    bool m_isSomething;
    string m_ReportFileName;
    
    public CustomAuditInfo( string reportFileName )
    {
      m_isSomething = false;
      m_ReportFileName = reportFileName;

      /********************************************************************/
      /* Create a report file                                             */
      /********************************************************************/
      m_ReportFile = new StreamWriter(new FileStream(m_ReportFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
    }

    protected override void  Dispose(bool b)
    {
      /********************************************************************/
      /* Refer user to TD_T("Audit Report.txt") if error(s) have been output.   */
      /********************************************************************/
      if (m_isSomething)
      {
        Console.WriteLine("\n\nAudit error : Check \"{0}\" for errors.", m_ReportFileName );
      }
      else
      {
        Console.WriteLine("\n\nAudit : ok.\n");
      }

      if (b)
        m_ReportFile.Dispose();

      base.Dispose(b);
    }

    /**********************************************************************/
    /* Print error to ReportFile                                          */
    /**********************************************************************/
    public override void  PrintError(string name, string value, string validation, string defaultValue)
    {
      m_isSomething = true;
      m_ReportFile.WriteLine( "{0} {1} {2} {3}\n", name, value, validation, defaultValue );
    }
  }

  class Program
  {
    [STAThread]
    static int Main(string[] args)
    {
      int nRes = 0; // Return value for main

      /**********************************************************************/
      /* Set command line defaults                                          */
      /**********************************************************************/
      SaveType fileType = SaveType.Save12;
      DwgVersion outVer = DwgVersion.AC1021;
      bool disableWait = false;

      /**********************************************************************/
      /* Initialize Teigha                                                  */
      /**********************************************************************/
      using (Services svcs = new Services())
      {
        HostApplicationServices.Current = new OdaMgdMViewApp.HostAppServ();
        /**********************************************************************/
        /* Display the Product and Version that created the executable        */
        /**********************************************************************/
        Console.WriteLine("\nWriteExMgd developed using {0} ver {1}", HostApplicationServices.Current.Product, HostApplicationServices.Current.VersionString);

        /**********************************************************************/
        /* Parse Command Line inputs                                          */
        /**********************************************************************/
        bool bInvalidArgs = (args.Length < 2);
        if (bInvalidArgs)
        {
          bInvalidArgs = true;
          nRes = 1;
        }

        /**********************************************************************/
        /* Set file version                                                   */
        /**********************************************************************/
        if (args.Length >= 2)
        {
          string argv2 = args[1];
          switch (argv2)
          {
            case "ACAD12":
              outVer = DwgVersion.vAC12;
              break;
            case "ACAD13":
              outVer = DwgVersion.vAC13;
              break;
            case "ACAD14":
              outVer = DwgVersion.vAC14;
              break;
            case "ACAD2000":
              outVer = DwgVersion.vAC15;
              break;
            case "ACAD2004":
              outVer = DwgVersion.vAC18;
              break;
            case "ACAD2007":
              outVer = DwgVersion.vAC21;
              break;
            case "ACAD2010":
              outVer = DwgVersion.vAC24;
              break;
            case "ACAD2013":
              outVer = DwgVersion.vAC27;
              break;
            default:
              bInvalidArgs = true;
              nRes = 1;
              break;
          }
        }

        /**********************************************************************/
        /* Set file type                                                      */
        /**********************************************************************/
        if (args.Length >= 3)
        {
          string argv3 = args[2];
          switch (argv3)
          {
            case "DWG":
              fileType = SaveType.Save12;
              break;
            case "DXF":
              fileType = SaveType.Save13;
              break;
            case "DXB":
              fileType = SaveType.Save14;
              break;
            default:
              bInvalidArgs = true;
              nRes = 1;
              break;
          }
        }

        /**********************************************************************/
        /* Disable prompt of pressing ENTER                                   */
        /**********************************************************************/
        if (args.Length >= 4)
        {
          string argv4 = args[3];
          if (argv4 == "-NoWait")
            disableWait = true;
        }

        if (bInvalidArgs)
        {
          Console.WriteLine("\n\n\tusage: OdWriteExMgd <filename> [OutVer] [OutType] [-NoWait]");
          Console.WriteLine("\n\tOutVer can be any of ACAD12, ACAD13, ACAD14, ACAD2000, ACAD2004, ACAD2007, ACAD2010, ACAD2013\n");
          Console.WriteLine("\n\tOutType can be any of DWG, DXF DXB\n");
          Console.WriteLine("\n\t-NoWait disables prompt of pressing ENTER at the end of execution.\n");
          return nRes;
        }

        /**********************************************************************/
        /* Load and Release RecomputeDimBlock                                 */
        /**********************************************************************/
        //::odrxDynamicLinker()->loadApp(TD_T("RecomputeDimBlock")).release();
        //::odrxDynamicLinker()->loadApp(TD_T("ExFieldEvaluator")).release();

        /**********************************************************************/
        /* Find the folder of the output file                                 */
        /**********************************************************************/
        string outputFolder = args[0];
        outputFolder = Path.GetDirectoryName(outputFolder);
        FileStreamBuf fileBuf = new FileStreamBuf(args[0], false, FileShareMode.DenyNo, FileCreationDisposition.CreateAlways);

        try
        {
          /********************************************************************/
          /* Create a default OdDbDatabase object                             */
          /********************************************************************/
          using (Database pDb = new Database())
          {
            DatabaseSummaryInfo pSumInfo = pDb.SummaryInfo;
            DatabaseSummaryInfoBuilder builder = new DatabaseSummaryInfoBuilder(pSumInfo);
            builder.Title = "Title";
            builder.Author = "Author";
            builder.Comments = "Comments";
            builder.Comments = "File was created by OdWriteExMgd";
            pDb.SummaryInfo = builder.ToDatabaseSummaryInfo();
            
            /********************************************************************/
            /* Fill the database                                                */
            /********************************************************************/
            HostApplicationServices.WorkingDatabase = pDb;
            DbFiller filler = new DbFiller();
            filler.fillDatabase(pDb);

            /********************************************************************/
            /* Audit the database                                               */
            /********************************************************************/
            using (AuditInfo aiAppAudit = new CustomAuditInfo(outputFolder + "\\AuditReport.txt"))
            {
              aiAppAudit.FixErrors = true;
              Console.WriteLine("\n\n");
              pDb.Audit(aiAppAudit);
            }
            #region TODO block
            //{
            //  OdSecurityParams securityParams;
            //  securityParams.password = "Teigha";
            //  securityParams.nFlags = 1;
            //  securityParams.nProvType = 0xd;

            //  // L"Microsoft Base DSS and Diffie-Hellman Cryptographic Provider"
            //  static wchar_t pn[] =
            //  {
            //    0x4D, 0x69, 0x63, 0x72, 0x6F,
            //    0x73, 0x6F, 0x66, 0x74, 0x20,
            //    0x42, 0x61, 0x73, 0x65, 0x20,
            //    0x44, 0x53, 0x53, 0x20, 0x61,
            //    0x6E, 0x64, 0x20, 0x44, 0x69,
            //    0x66, 0x66, 0x69, 0x65, 0x2D,
            //    0x48, 0x65, 0x6C, 0x6C, 0x6D,
            //    0x61, 0x6E, 0x20, 0x43, 0x72,
            //    0x79, 0x70, 0x74, 0x6F, 0x67,
            //    0x72, 0x61, 0x70, 0x68, 0x69,
            //    0x63, 0x20, 0x50, 0x72, 0x6F,
            //    0x76, 0x69, 0x64, 0x65, 0x72,
            //    0x00
            //  };
            //  securityParams.provName = pn;

            //  /************************************************************************/
            //  /* Uncomment the following line to add the password TD_T("Teigha") to the  */
            //  /* R18+ file to be created                                              */
            //  /************************************************************************/
            //  //pDb->setSecurityParams(securityParams);
            //}
            #endregion
            /********************************************************************/
            /* Write the database                                               */
            /********************************************************************/
            pDb.SaveAs(fileBuf, fileType, outVer, true, 16);
            /*if (SaveType.Save12 == fileType)
            {
              pDb.SaveAs(args[0], outVer);
            }
            else
              pDb.DxfOut(args[0], 16, outVer);//16 - default*/
            Console.WriteLine("\nDatabase has been saved as {0}", pDb.Filename);
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("\n\nError: {0}", e.Message);
          nRes = -1;
        }
      }

      if (nRes != 0)
      {
        Console.WriteLine("\n\nPress ENTER to continue...");
        if (!disableWait)
        {
          Console.ReadLine();
        }
      }
      Console.WriteLine("\n\n");
      return nRes;
    }
  }
}
