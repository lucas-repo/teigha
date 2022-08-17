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
using System.Windows.Forms;
using Teigha;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.DatabaseServices;
using Teigha.Export_Import;
using System.IO;

namespace OdViewExMgd
{
  class Aux
  {
    static bool bWindowPrint = false;

    public static ObjectId active_viewport_id(Database database)
    {
      if (database.TileMode)
      {
        return database.CurrentViewportTableRecordId;
      }
      else
      {
        using(BlockTableRecord paperBTR = (BlockTableRecord)database.CurrentSpaceId.GetObject(OpenMode.ForRead))
        {
          Layout l = (Layout)paperBTR.LayoutId.GetObject(OpenMode.ForRead);
          return l.CurrentViewportId;
        }
      }
    }

    public static void preparePlotstyles(Database database, ContextForDbDatabase ctx)
    {
      using (BlockTableRecord paperBTR = (BlockTableRecord)database.CurrentSpaceId.GetObject(OpenMode.ForRead))
      {
        using (Layout pLayout = (Layout)paperBTR.LayoutId.GetObject(OpenMode.ForRead))
        {
          if (ctx.IsPlotGeneration ? pLayout.PlotPlotStyles : pLayout.ShowPlotStyles)
          {
            String pssFile = pLayout.CurrentStyleSheet;
            if (pssFile.Length > 0)
            {
              String testpath = ((HostAppServ)HostApplicationServices.Current).FindFile(pssFile, database, FindFileHint.Default);
              if (testpath.Length > 0)
              {
                using (FileStreamBuf pFileBuf = new FileStreamBuf(testpath))
                {
                  ctx.LoadPlotStyleTable(pFileBuf);
                }
              }
            }
          }
        }
      }
    }

    public static Teigha.GraphicsSystem.View activeTopView(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice)
    {
      Teigha.GraphicsSystem.View pView = helperDevice.ActiveView;
      if (!database.TileMode)
      {
        using (DBObject pVpObj = Aux.active_viewport_id(database).GetObject(OpenMode.ForWrite))
        {
          AbstractViewportData pAVD = new AbstractViewportData(pVpObj);
          if (null != pAVD)
          {
            Teigha.GraphicsSystem.View pGsView = pAVD.GsView;
            if (null != pGsView)
              pView = pGsView;
          }
        }
      }
      return pView;
    }

    public static bool SelectWindowPrint
    {
      get { return bWindowPrint; }
      set { bWindowPrint = value; }
    }
  }

  class CustomAuditInfo : AuditInfo
  {
    StreamWriter m_ReportFile;
    bool m_isSomething;
    string m_ReportFileName;

    public CustomAuditInfo(string reportFileName)
    {
      m_isSomething = false;
      m_ReportFileName = reportFileName;

      /********************************************************************/
      /* Create a report file                                             */
      /********************************************************************/
      m_ReportFile = new StreamWriter(new FileStream(m_ReportFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
    }

    protected override void Dispose(bool b)
    {
      /********************************************************************/
      /* Refer user to TD_T("Audit Report.txt") if error(s) have been output.   */
      /********************************************************************/
      if (m_isSomething)
      {
        Console.WriteLine("\n\nAudit error : Check \"{0}\" for errors.", m_ReportFileName);
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
    public override void PrintError(string name, string value, string validation, string defaultValue)
    {
      m_isSomething = true;
      m_ReportFile.WriteLine("{0} {1} {2} {3}\n", name, value, validation, defaultValue);
    }
  }
}
