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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Printing;

using Teigha.Core;
using Teigha.TD;


namespace OdViewExMgd.Print
{
  class Printing
  {
    OdGsLayoutHelper m_pPrinterDevice;
    OdDbDatabase database;
    OdGsDevice _gsDevice;
    OdGsView pViewDr;

    public void Swap<T>(T a, T b)
    {
      T temp;
      temp = a;
      a = b;
      b = temp;
    }

    public static OdDbObjectId active_viewport_id(OdDbDatabase database)
    {
      if (database.getTILEMODE())
      {
        OdDbViewportTable pVPT = (OdDbViewportTable)database.getViewportTableId().safeOpenObject();
        OdDbObjectId pActiveVPId = pVPT.getActiveViewportId();

        return pActiveVPId;
      }
      else
      {
        using (OdDbBlockTableRecord paperBTR = (OdDbBlockTableRecord)database.getActiveLayoutBTRId().openObject(OpenMode.kForRead))
        {
          OdDbLayout l = (OdDbLayout)paperBTR.getLayoutId().openObject(OpenMode.kForRead);
          return l.activeViewportId();
        }
      }
   }

    void PrintPage1(Object sender, PrintPageEventArgs ev)
    {
      //    //private void PrintDocument_PrintPage(Object sender, PrintPageEventArgs e)
      //      #region Source posted on forum from sample
      //      //OdGsDevice printDevice;
      ////      using (OdGsModule gsModule = (OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false))
      //      {
      ////          OdGsDevice printDevice = gsModule.createDevice();
      //          //using (OdGsDevice printDevice = gsModule.createDevice())
      //          {
      ////              using (OdGiContextForDbDatabase ctx = OdGiContextForDbDatabase.createObject())
      //              {
      ////                  ctx.enableGsModel(true);
      ////                  ctx.setPlotGeneration(true);
      ////                  ctx.setDatabase(_tdDatabase);
      ////                  printDevice.setUserGiContext(ctx);
      ////                  printDevice = (OdGsDevice)TD_Db.setupActiveLayoutViews(printDevice, ctx);
      //
      ////                  Boolean modelSpace = _tdDatabase.getTILEMODE();
      ////                  SetViewportBorderProperties(_gsDevice, !modelSpace);
      //
      //
      //
      //
      //
      //
      //
      //
      //
      //                  printDevice.setBackgroundColor(_monoPalette[0]);
      //                  printDevice.setLogicalPalette(_monoPalette, 256);
      //                if (m_pPrinterDevice != null)
      //                  {
      //                      Double factorX = ev.Graphics.DpiX / 100;
      //                      Double factorY = ev.Graphics.DpiY / 100;
      //
      //                      OdGsView pView = m_pPrinterDevice.viewAt(0);
      //                      OdAbstractViewPE view = OdAbstractViewPE.cast(pView);
      //                      view.zoomExtents(pView);
      //
      //                      OdGsDCRect rect = new OdGsDCRect((Int32)(ev.MarginBounds.X * factorX), 
      //                                                       (Int32)(ev.MarginBounds.Y * factorY), 
      //                                                       (Int32)(ev.MarginBounds.Width * factorX), 
      //                                                       (Int32)(ev.MarginBounds.Height * factorY));
      //                      //OdGsDCRect rect = new OdGsDCRect(0, 0, (Int32)(e.MarginBounds.Width * factorX) - (Int32)(e.MarginBounds.X * factorX), (Int32)(e.MarginBounds.Height * factorY) - (Int32)(e.MarginBounds.Y * factorY));
      //                      m_pPrinterDevice.onSize(rect);
      //
      //                      if (m_pPrinterDevice.properties().has("WindowHDC"))
      //                      {
      //                        m_pPrinterDevice.properties().putAt("WindowHDC", new OdRxVariantValue(ev.Graphics.GetHdc().ToInt32()));
      //                      }
      //
      //                      m_pPrinterDevice.update();
      //                  }
      //              }
      //          }
      //      }
      //      #endregion
      //      return;

      try
      {
        //        OdGsLayoutHelper printerDevice = null;
        //        using (OdGiContextForDbDatabase databaseContext = OdGiContextForDbDatabase.createObject())
        //        OdGiContextForDbDatabase databaseContext = OdGiContextForDbDatabase.createObject();
        {
          //          databaseContext.setDatabase(database);
          //          if (_gsDevice != null)
          //          {
          //            ctx.enableGsModel(true);
          //            ctx.setPlotGeneration(true);
          //            ctx.setDatabase(_tdDatabase);
          //            printDevice.setUserGiContext(ctx);

          //            databaseContext.setPlotGeneration(true);
          ////            databaseContext.enableGsModel(false);
          //            databaseContext.enableGsModel(true);
          //             printerDevice = TD_Db.setupActiveLayoutViews(_gsDevice, databaseContext);
          //            databaseContext.setPlotGeneration(false);
          //          }

          if (m_pPrinterDevice != null)
          {
            OdGiContextForDbDatabase databaseContext = (OdGiContextForDbDatabase)m_pPrinterDevice.userGiContext();

            Double printerLeftMargin = ev.PageSettings.HardMarginX;
            Double printerTopMargin = ev.PageSettings.HardMarginY;
            Double printerRightMargin = ev.PageSettings.PaperSize.Width - ev.PageSettings.Margins.Left - printerLeftMargin;
            Double printerBottomMargin = ev.PageSettings.PaperSize.Height - ev.PageSettings.Margins.Top - printerTopMargin;
            //Double koeffX = ev.PageSettings.PrinterResolution.X / AC.Shared.Utility.Length.InchToMillimeters;
            //Double koeffY = ev.PageSettings.PrinterResolution.Y / AC.Shared.Utility.Length.InchToMillimeters;
            Double dPrinterWidth = ev.PageBounds.Width;
            Double dPrinterHeight = ev.PageBounds.Height;
            Double dLogPixelX = ev.Graphics.DpiX; //dot per inch
            Double dLogPixelY = ev.Graphics.DpiY; //dot per inch
            Double kMmPerInch = 25.4;
            Double kMmPerHInch = 0.254;
            Double koeffX = dLogPixelX / kMmPerInch;
            Double koeffY = dLogPixelY / kMmPerInch;


            //            if (_printColor)
            //            {
            //              printerDevice.setLogicalPalette(Teigha.Core.AllPalettes.getLightPalette(), 256);
            //            }
            //            else
            //            {
            //              printerDevice.setLogicalPalette(_monoPalette, 256);
            //            }
            //            printerDevice.setBackgroundColor(_monoPalette[0]);

            // Get Layout info
            OdDbLayout layout = (OdDbLayout)m_pPrinterDevice.layoutId().safeOpenObject();
            Boolean scaledToFit = layout.useStandardScale() && (OdDbPlotSettings.StdScaleType.kScaleToFit == layout.stdScaleType());
            Boolean centered = layout.plotCentered();
            Boolean metric = (layout.plotPaperUnits() != OdDbPlotSettings.PlotPaperUnits.kInches) ? true : false;
            Boolean printLW = layout.printLineweights() || layout.showPlotStyles();

            Double offsetX, offsetY;
            layout.getPlotOrigin(out offsetX, out offsetY); // in mm
            OdGePoint2d pio = layout.getPaperImageOrigin(); // in mm
            Double leftMargin = layout.getLeftMargin();  // in mm
            Double rightMargin = layout.getRightMargin(); // in mm
            Double topMargin = layout.getTopMargin();   // in mm
            Double bottomMargin = layout.getBottomMargin();// in mm
            //            Double leftMargin, rightMargin, bottomMargin, topMargin;
            //            layout.getPlotPaperMargins(out dLeftMargin, out dBottomMargin, out dRightMargin, out dTopMargin);

            OdDbPlotSettings.PlotType plotType = layout.plotType();
            // Force the plot type to "extents" and switch-off lineweights
            scaledToFit = centered = true;
            plotType = OdDbPlotSettings.PlotType.kExtents;
            printLW = false;

            Boolean model = m_pPrinterDevice.isKindOf(OdGsModelLayoutHelper.desc());
            // Set LineWeight scale factor for model space
            //            if (printLW && model)
            //            {
            //              OdGsView pTo = printerDevice.viewAt(0);
            //              pTo.setLineweightToDcScale(Math.Max(e.PageSettings.PrinterResolution.X, e.PageSettings.PrinterResolution.Y) / AC.Shared.Utility.Length.InchToMillimeters * 0.01);
            //            }

            Boolean print0 = false;
            Boolean print90 = false;
            Boolean print180 = false;
            Boolean print270 = false;
            switch (layout.plotRotation())
            {
              default:
              case OdDbPlotSettings.PlotRotation.k0degrees:
                {
                  print0 = true;
                }
                break;

              case OdDbPlotSettings.PlotRotation.k90degrees:
                print90 = true;
                Swap<Double>(topMargin, rightMargin);
                Swap<Double>(bottomMargin, leftMargin);
                Swap<Double>(bottomMargin, topMargin);
                Swap<Double>(offsetX, offsetY);
                offsetY = -offsetY;
                offsetX = -offsetX;
                break;

              case OdDbPlotSettings.PlotRotation.k180degrees:
                print180 = true;
                Swap<Double>(rightMargin, leftMargin);
                offsetY = -offsetY;
                offsetX = -offsetX;
                break;

              case OdDbPlotSettings.PlotRotation.k270degrees:
                print270 = true;
                Swap<Double>(topMargin, rightMargin);
                Swap<Double>(bottomMargin, leftMargin);
                Swap<Double>(offsetX, offsetY);
                break;
            }

            // Get scale factor
            Double factor;
            if (layout.useStandardScale())
            {
              layout.getStdScale(out factor);
            }
            else
            {
              Double numerator, denominator;
              layout.getCustomPrintScale(out numerator, out denominator);
              factor = numerator / denominator;
            }

            if (leftMargin < printerLeftMargin / koeffX)
            {
              leftMargin = printerLeftMargin / koeffX;
            }
            if (topMargin < printerTopMargin / koeffY)
            {
              topMargin = printerTopMargin / koeffY;
            }

            // Also adjust Right and Bottom margins
            if (rightMargin < printerRightMargin / koeffX)
            {
              rightMargin = printerRightMargin / koeffX;
            }
            if (bottomMargin < printerBottomMargin / koeffY)
            {
              bottomMargin = printerBottomMargin / koeffY;
            }

            // Calculate paper drawable area using margins from layout (in mm).
            Double rx1 = (-printerLeftMargin / koeffX + leftMargin);                // in mm
            Double rx2 = (rx1 + ev.PageSettings.PaperSize.Width / koeffX - leftMargin - rightMargin); // in mm
            Double ry1 = (-printerTopMargin / koeffY + topMargin);                // in mm
            Double ry2 = (ry1 + ev.PageSettings.PaperSize.Height / koeffY - topMargin - bottomMargin); // in mm

            // Margin clip box calculation
            topMargin *= koeffY; // in printer units
            rightMargin *= koeffX;
            bottomMargin *= koeffY;
            leftMargin *= koeffX;

            Double dScreenFactorH, dScreenFactorW;
            Double x = leftMargin - printerLeftMargin;
            Double y = topMargin - printerTopMargin;
            Rectangle marginsClipBox = new Rectangle((Int32)x, (Int32)y,
                                                        (Int32)(x + ev.PageSettings.PaperSize.Width - leftMargin - rightMargin),
                                                        (Int32)(y + ev.PageSettings.PaperSize.Height - topMargin - bottomMargin));
            dScreenFactorH = 1.0;
            dScreenFactorW = 1.0;

            // MarginsClipBox is calculated
            Rectangle clipBox = marginsClipBox;

            // Get view and viewport position, direction ...
            OdGsView pOverallView;

            OdGePoint3d viewportCenter;
            OdGePoint3d viewTarget;
            OdGeVector3d upV, viewDir;
            Boolean isPerspective;
            Double viewportH, viewportW;
            OdGeMatrix3d eyeToWorld, WorldToeye;
            Boolean SkipClipping = false;

            OdRxObject pVObject;
            OdAbstractViewPE pAbstractViewPE = null;
            pOverallView = model ? OdGsModelLayoutHelper.cast(m_pPrinterDevice).activeView()
                                 : OdGsPaperLayoutHelper.cast(m_pPrinterDevice).overallView();
            if (plotType == OdDbPlotSettings.PlotType.kView)
            {
              String sPlotViewName = layout.getPlotViewName();
              OdDbViewTableRecord pVtr = (OdDbViewTableRecord)((OdDbViewTable)(databaseContext.getDatabase().getViewTableId().safeOpenObject())).getAt(sPlotViewName).safeOpenObject();
              viewTarget = pVtr.target();     // in plotPaperUnits
              pVObject = pVtr;
            }
            else if (model)
            {
              OdDbViewportTable pVPT = (OdDbViewportTable)databaseContext.getDatabase().getViewportTableId().safeOpenObject();
              OdDbViewportTableRecord pActiveVP = (OdDbViewportTableRecord)pVPT.getActiveViewportId().safeOpenObject();
              viewTarget = pActiveVP.target();     // in plotPaperUnits
              pVObject = pActiveVP;
            }
            else
            {
              OdDbObjectId overallVpId = layout.overallVportId();
              OdDbViewport pActiveVP = (OdDbViewport)overallVpId.safeOpenObject();
              viewTarget = pActiveVP.viewTarget();   // in plotPaperUnits
              pVObject = pActiveVP;
            }
            pAbstractViewPE = OdAbstractViewPE.cast(pVObject);

            // get info from view, viewport .... etc
            viewportCenter = pAbstractViewPE.target(pVObject);       // in plotPaperUnits
            isPerspective = pAbstractViewPE.isPerspective(pVObject);
            viewportH = pAbstractViewPE.fieldHeight(pVObject);  // in plotPaperUnits
            viewportW = pAbstractViewPE.fieldWidth(pVObject);   // in plotPaperUnits
            viewDir = pAbstractViewPE.direction(pVObject);    // in plotPaperUnits
            upV = pAbstractViewPE.upVector(pVObject);     // in plotPaperUnits
            eyeToWorld = pAbstractViewPE.eyeToWorld(pVObject);
            WorldToeye = pAbstractViewPE.worldToEye(pVObject);

            Boolean isPlanView = viewTarget.isEqualTo(new OdGePoint3d(0, 0, 0)) && viewDir.normal().isEqualTo(OdGeVector3d.kZAxis);

            // To set OverAll View using default settings
            // get rect of drawing to view (in plotPaperUnits)
            Double fieldWidth = viewportW;
            Double fieldHeight = viewportH;
            if (plotType == OdDbPlotSettings.PlotType.kWindow || (plotType == OdDbPlotSettings.PlotType.kLimits && isPlanView))
            {
              Double xmin, ymin, xmax, ymax;
              if (plotType == OdDbPlotSettings.PlotType.kWindow)
              {
                layout.getPlotWindowArea(out xmin, out ymin, out xmax, out ymax); // in plotPaperUnits
              }
              else
              {
                xmin = databaseContext.getDatabase().getLIMMIN().x;
                ymin = databaseContext.getDatabase().getLIMMIN().y;
                xmax = databaseContext.getDatabase().getLIMMAX().x;
                ymax = databaseContext.getDatabase().getLIMMAX().y;
              }

              fieldWidth = xmax - xmin;
              fieldHeight = ymax - ymin;

              OdGeVector3d tmp = viewportCenter - viewTarget;
              viewTarget.set((xmin + xmax) / 2.0, (ymin + ymax) / 2.0, 0);
              viewTarget.transformBy(eyeToWorld);
              viewTarget -= tmp;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kDisplay)
            {
              viewTarget = viewportCenter;
              fieldWidth = viewportW;
              fieldHeight = viewportH;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kExtents || (plotType == OdDbPlotSettings.PlotType.kLimits && !isPlanView))
            {
              OdGeBoundBlock3d extents = new OdGeBoundBlock3d();
              OdGsView pView = m_pPrinterDevice.viewAt(0);
              OdAbstractViewPE abstractView = OdAbstractViewPE.cast(pView);
              if (abstractView.viewExtents(pView, extents))
              {
                fieldWidth = Math.Abs(extents.maxPoint().x - extents.minPoint().x);
                fieldHeight = Math.Abs(extents.maxPoint().y - extents.minPoint().y);

                extents.transformBy(abstractView.eyeToWorld(pView));

                viewTarget = (extents.minPoint() + extents.maxPoint().asVector()).Div(2.0);
              }
            }
            else if (plotType == OdDbPlotSettings.PlotType.kView)
            {
              viewTarget = viewportCenter;

              fieldWidth = viewportW;
              fieldHeight = viewportH;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kLayout)
            {
              SkipClipping = true; // used full paper drawing area.

              fieldWidth = (rx2 - rx1) / factor; // drx in mm -> fieldWidth in mm
              fieldHeight = (ry2 - ry1) / factor;

              viewTarget.set(fieldWidth / 2.0 - pio.x - offsetX / factor, fieldHeight / 2.0 - pio.y - offsetY / factor, 0); // in mm
              if (!metric)
              {
                //                viewTarget = viewTarget.Div(AC.Shared.Utility.Length.InchToMillimeters); // must be in plotpaper units
                //                fieldWidth /= AC.Shared.Utility.Length.InchToMillimeters;
                //                fieldHeight /= AC.Shared.Utility.Length.InchToMillimeters;
              }

              offsetX = 0.0;
              offsetY = 0.0;
              pio.x = 0.0;
              pio.y = 0.0; // it was applied to viewTarget, reset it.
              centered = scaledToFit = false;       // kLayout doesn't support it.
            }

            // in plotpaper units
            pOverallView.setView(viewTarget, viewTarget + viewDir, upV, fieldWidth, fieldHeight, isPerspective ? OdGsView.Projection.kPerspective : OdGsView.Projection.kParallel);

            if (!metric)
            {
              //              fieldWidth *= AC.Shared.Utility.Length.InchToMillimeters;
              //              fieldHeight *= AC.Shared.Utility.Length.InchToMillimeters;
            }

            if (scaledToFit) // Scale factor can be stored in layout, but preview recalculate it if scaledToFit = true.
            {                 // Some times stored factor isn't correct, due to autocad isn't update it before saving.
              factor = Math.Min((rx2 - rx1) / fieldWidth, (ry2 - ry1) / fieldHeight);
            }

            if (centered)    // Offset also can be incorectly saved.
            {
              offsetX = ((rx2 - rx1) - fieldWidth * factor) / 2.0;
              offsetY = ((ry2 - ry1) - fieldHeight * factor) / 2.0;

              if (print90 || print180)
              {
                offsetY = -offsetY;
                offsetX = -offsetX;
              }
            }

            if (print180 || print90)
            {
              rx1 = rx2 - fieldWidth * factor;
              ry2 = ry1 + fieldHeight * factor;
            }
            else if (print0 || print270)
            {
              rx2 = rx1 + fieldWidth * factor;
              ry1 = ry2 - fieldHeight * factor;
            }
            else // preview
            {
              rx2 = rx1 + fieldWidth * factor;
              ry1 = ry2 - fieldHeight * factor;
            }

            if (!SkipClipping)
            {
              if (print180 || print90)
              {
                clipBox.X = (Int32)(clipBox.Right - (rx2 - rx1) * koeffX * dScreenFactorW);
                clipBox.Height = (Int32)(clipBox.Top + (ry2 - ry1) * koeffY * dScreenFactorH);
              }
              else if (print0 || print270)
              {
                clipBox.Width = (Int32)(clipBox.Left + (rx2 - rx1) * koeffX * dScreenFactorW);
                clipBox.Y = (Int32)(clipBox.Bottom - (ry2 - ry1) * koeffY * dScreenFactorH);
              }
              else // preview
              {
                clipBox.Width = (Int32)(clipBox.Left + (rx2 - rx1) * koeffX * dScreenFactorW);
                clipBox.Y = (Int32)(clipBox.Bottom - (ry2 - ry1) * koeffY * dScreenFactorH);
              }
              clipBox.Offset((Int32)(offsetX * koeffX * dScreenFactorW), (Int32)(-offsetY * koeffY * dScreenFactorH));
            }

            pOverallView.setViewport(new OdGePoint2d(0, 0), new OdGePoint2d(1, 1));

            Rectangle resultClipBox = Rectangle.Intersect(marginsClipBox, clipBox);
            // Apply clip region to screen
            ev.Graphics.SetClip(resultClipBox);
            //Rectangle newClip;
            //newClip.CreateRectRgnIndirect(&ResultClipBox);
            //e.Graphics.SetClip(newClip);

            // Calculate viewport rect in printer units
            Int32 x1 = (Int32)((offsetX + rx1) * koeffX);
            Int32 x2 = (Int32)((offsetX + rx2) * koeffX);
            Int32 y1 = (Int32)((-offsetY + ry1) * koeffY);
            Int32 y2 = (Int32)((-offsetY + ry2) * koeffY);

            OdGsDCRect viewportRect;
            if (print180 || print90)
            {
              viewportRect = new OdGsDCRect(x1, x2, y1, y2);
            }
            else if (print0 || print270)
            {
              viewportRect = new OdGsDCRect(x2, x1, y2, y1);
            }
            else // preview
            {
              viewportRect = new OdGsDCRect(x2, x1, y2, y1);
            }

            m_pPrinterDevice.onSize(viewportRect);
            m_pPrinterDevice.properties().putAt("WindowHDC", new OdRxVariantValue((Int32)ev.Graphics.GetHdc()));

            //            databaseContext.setPlotGeneration(true);
            m_pPrinterDevice.update();
            //            databaseContext.setPlotGeneration(false);

            // Release the printer device
            m_pPrinterDevice.Dispose();
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    void PrintPage(Object sender, PrintPageEventArgs ev)
    {
      if (m_pPrinterDevice != null)
      {
        //mku OdGsView pView = m_pPrinterDevice.viewAt(0);
      
        using (OdDbObjectId pVpObj = active_viewport_id(database))
        {
          //using (OdDbAbstractViewportData pAV = Teigha.TD.OdDbAbstractViewportData.createObject())
          //OdDbAbstractViewportData.gsView((pVpObj)
//          using(OdAbstractViewPE pAV = OdAbstractViewPE.cast(pView))
          using (OdDbAbstractViewportData pAV = OdDbAbstractViewportData.cast(pVpObj.openObject()))
          {
            OdGsView pGSView = pAV.gsView(pVpObj.openObject(OpenMode.kForRead)); // ???kForWrite
      
            PrintDocument prDoc = (PrintDocument)sender;
      
            // Get printer paper info
            Double dPrinterWidth = ev.PageBounds.Width;
            Double dPrinterHeight = ev.PageBounds.Height;
            Double dLogPixelX = ev.Graphics.DpiX; //dot per inch
            Double dLogPixelY = ev.Graphics.DpiY; //dot per inch
            Double kMmPerInch = 25.4;
            Double kMmPerHInch = 0.254;
            Double koeffX = dLogPixelX / kMmPerInch;
            Double koeffY = dLogPixelY / kMmPerInch;
      
            OdDbLayout pLayout = (OdDbLayout)m_pPrinterDevice.layoutId().openObject(OpenMode.kForRead);
            Boolean bScaledToFit = pLayout.useStandardScale(); // && (StdScaleType.ScaleToFit == pLayout.StdScaleType);
            Boolean bCentered = pLayout.plotCentered();
            Boolean bMetric = (pLayout.plotPaperUnits() != OdDbPlotSettings.PlotPaperUnits.kInches);
            Boolean bPrintLW = pLayout.printLineweights() || pLayout.showPlotStyles();
      
            Double offsetX, offsetY;
            pLayout.getPlotOrigin(out offsetX, out offsetY); // in mm
      
            Double dLeftMargin, dRightMargin, dBottomMargin, dTopMargin;
            pLayout.getPlotPaperMargins(out dLeftMargin, out dBottomMargin, out dRightMargin, out dTopMargin);
            OdDbPlotSettings.PlotType plotType = pLayout.plotType();
      
            OdDbPlotSettings.PlotRotation plotRotation = pLayout.plotRotation();
            if (plotRotation == OdDbPlotSettings.PlotRotation.k90degrees || plotRotation == OdDbPlotSettings.PlotRotation.k270degrees)
            {
              plotRotation = (plotRotation == OdDbPlotSettings.PlotRotation.k90degrees) ? OdDbPlotSettings.PlotRotation.k270degrees : OdDbPlotSettings.PlotRotation.k90degrees;
            }
      
            OdGePoint2d offsets = new OdGePoint2d(offsetX, offsetY);
      
            switch (plotRotation)
            {
              case OdDbPlotSettings.PlotRotation.k90degrees:
                Swap<Double>(dTopMargin, dRightMargin);
                Swap<Double>(dBottomMargin, dLeftMargin);
                Swap<Double>(dBottomMargin, dTopMargin);
                Swap<Double>(dTopMargin, dRightMargin);
                offsets = new OdGePoint2d(-offsets.x, -offsets.y);
                break;
              case OdDbPlotSettings.PlotRotation.k180degrees:
                Swap<Double>(dRightMargin, dLeftMargin);
                offsets = new OdGePoint2d(-offsets.x, -offsets.y);
                break;
              case OdDbPlotSettings.PlotRotation.k270degrees:
                Swap<Double>(dTopMargin, dRightMargin);
                Swap<Double>(dBottomMargin, dLeftMargin);
                Swap<Double>(dBottomMargin, dTopMargin);
                offsets = new OdGePoint2d(offsets.x, offsets.y);
                break;
            }
      
            // Get scale factor
            double dFactor;
            if (pLayout.useStandardScale())
            {
              pLayout.getStdScale(out dFactor);
            }
            else
            {
              double numerator, denominator;
              pLayout.getCustomPrintScale(out numerator, out denominator);
              dFactor = numerator / denominator;
            }
      
            // Calculate paper drawable area using margins from layout (in mm).
            Double drx1 = (ev.MarginBounds.Left * kMmPerHInch + dLeftMargin);                // in mm
            Double drx2 = (ev.MarginBounds.Width * kMmPerHInch - dLeftMargin - dRightMargin); // in mm
            Double dry1 = (ev.MarginBounds.Top * kMmPerHInch + dTopMargin);                // in mm
            Double dry2 = (ev.MarginBounds.Height * kMmPerHInch - dTopMargin - dBottomMargin); // in mm
      
            Boolean bType = (plotType == OdDbPlotSettings.PlotType.kDisplay || plotType == OdDbPlotSettings.PlotType.kLayout);
            OdAbstractViewPE pAbstractViewPE = (bType) ? OdAbstractViewPE.cast(pViewDr)
                                                       : OdAbstractViewPE.cast(pGSView);
      
            // set LineWeight scale factor for model space
            if (bPrintLW && database.getTILEMODE())
            {
              OdGsView pTo = m_pPrinterDevice.viewAt(0);
              pTo.setLineweightToDcScale( Math.Max(dLogPixelX, dLogPixelY) / kMmPerInch * 0.01 );
            }

            OdRxObject pVObject = (bType) ? pViewDr : pGSView;

            OdGePoint3d viewTarget = pAbstractViewPE.target(pVObject);
            OdGePoint3d viewportCenter = pAbstractViewPE.target(pVObject) ;       // in plotPaperUnits
            Boolean isPerspective = pAbstractViewPE.isPerspective(pVObject);
            Double viewportH = pAbstractViewPE.fieldHeight(pVObject);  // in plotPaperUnits
            Double viewportW = pAbstractViewPE.fieldWidth(pVObject);   // in plotPaperUnits
            OdGeVector3d viewDir = pAbstractViewPE.direction(pVObject);    // in plotPaperUnits
            OdGeVector3d upV = pAbstractViewPE.upVector(pVObject);     // in plotPaperUnits
            OdGeMatrix3d eyeToWorld = pAbstractViewPE.eyeToWorld(pVObject);
            OdGeMatrix3d WorldToeye = pAbstractViewPE.worldToEye(pVObject);
      
            Boolean isPlanView = viewDir.normal().Equals(OdGeVector3d.kZAxis);
            OdGePoint3d oldTarget = viewTarget;
      
            Double fieldWidth = viewportW, fieldHeight = viewportH;
      
            if (plotType == OdDbPlotSettings.PlotType.kWindow || (plotType == OdDbPlotSettings.PlotType.kLimits && isPlanView))
            {
              //OdGeExtents2d ext;
              Double xmin, ymin, xmax, ymax;
              if (plotType == OdDbPlotSettings.PlotType.kWindow)
              {
                pLayout.getPlotWindowArea(out xmin, out ymin, out xmax, out ymax);
              }
              else
              {
                xmin = database.getLIMMIN().x;
                ymin = database.getLIMMIN().y;
                xmax = database.getLIMMAX().x;
                ymax = database.getLIMMAX().y;
              }
      
              fieldWidth = xmax - xmin;
              fieldHeight = ymax - ymin;
      
              OdGeVector3d tmp = viewportCenter - viewTarget;
              viewTarget.set((xmin + xmax) / 2, (ymin + ymax) / 2, 0);
              viewTarget.transformBy(eyeToWorld);
              viewTarget -= tmp;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kDisplay)
            {
              viewTarget = viewportCenter;
              fieldWidth = viewportW;
              fieldHeight = viewportH;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kExtents || (plotType == OdDbPlotSettings.PlotType.kLimits && !isPlanView))
            {
              OdGeBoundBlock3d extents = new OdGeBoundBlock3d();
              //OdAbstractViewPE abstractView = OdAbstractViewPE.cast(pView);
              if (pAbstractViewPE.plotExtents(pVObject, extents)) // pIter also skip 'off layers'
              {
                extents.transformBy(eyeToWorld);
                viewTarget = (extents.minPoint() + extents.maxPoint().asVector()).Div(2.0);
                extents.transformBy(WorldToeye);
      
                fieldWidth = Math.Abs(extents.maxPoint().x - extents.minPoint().x);
                fieldHeight = Math.Abs(extents.maxPoint().y - extents.minPoint().y);
              }
            }
            else if (plotType == OdDbPlotSettings.PlotType.kView)
            {
              viewTarget = viewportCenter;
              fieldWidth = viewportW;
              fieldHeight = viewportH;
            }
            else if (plotType == OdDbPlotSettings.PlotType.kLimits)
            {
              fieldWidth = (drx2 - drx1) / dFactor; // drx in mm -> fieldWidth in mm
              fieldHeight = (dry2 - dry1) / dFactor;
      
              viewTarget = new OdGePoint3d(fieldWidth / 2.0 - offsets.x / dFactor, fieldHeight / 2.0 - offsets.y / dFactor, 0); // in mm
              if (!bMetric)
              {
                viewTarget = viewTarget.Div(kMmPerInch); // must be in plotpaper units
                fieldWidth /= kMmPerInch;
                fieldHeight /= kMmPerInch;
              }
      
              bCentered = bScaledToFit = false;       // kLayout doesn't support pIter.
            }
      
            if (plotType != OdDbPlotSettings.PlotType.kView)
            {
              viewTarget = viewTarget.orthoProject(new OdGePlane(oldTarget, viewDir));
            }
      
            pGSView.setView(viewTarget + viewDir, 
                            viewTarget, upV, 
                            fieldWidth, fieldHeight, 
                            isPerspective ? OdGsView.Projection.kPerspective : OdGsView.Projection.kParallel);
      
            if (!bMetric)
            {
              fieldWidth *= kMmPerInch;
              fieldHeight *= kMmPerInch;
            }
      
            if (bScaledToFit)
            {
              dFactor = Math.Min((drx2 - drx1) / fieldWidth, (dry2 - dry1) / fieldHeight);
            }
      
            if (bCentered)    // Offset also can be incorectly saved.
            {
              offsets = new OdGePoint2d(((drx2 - drx1) - fieldWidth * dFactor) / 2.0,
                                    ((dry2 - dry1) - fieldHeight * dFactor) / 2.0);
      
              if (plotRotation == OdDbPlotSettings.PlotRotation.k90degrees || plotRotation == OdDbPlotSettings.PlotRotation.k180degrees)
              {
                offsets = new OdGePoint2d(-offsets.x, -offsets.y);
              }
            }
      
            switch (plotRotation)
            {
              case OdDbPlotSettings.PlotRotation.k90degrees:
              case OdDbPlotSettings.PlotRotation.k180degrees:
                drx1 = drx2 - fieldWidth * dFactor;
                dry2 = dry1 + fieldHeight * dFactor;
                break;
              case OdDbPlotSettings.PlotRotation.k270degrees:
              case OdDbPlotSettings.PlotRotation.k0degrees:
              default:
                drx2 = drx1 + fieldWidth  * dFactor;
                dry1 = dry2 - fieldHeight * dFactor;
                break;
            }
      
            pGSView.setViewport(new OdGePoint2d(0, 0), new OdGePoint2d(1, 1));
      
            //Calculate viewport rect in printer units
            int x1 = (int)((offsets.x + drx1)  * koeffX);
            int x2 = (int)((offsets.x + drx2)  * koeffX);
            int y1 = (int)((-offsets.y + dry1) * koeffY);
            int y2 = (int)((-offsets.y + dry2) * koeffY);
            /*int x1 = (int)(drx1 * koeffX);
            int x2 = (int)(drx2 * koeffX);
            int y1 = (int)(dry1 * koeffY);
            int y2 = (int)(dry2 * koeffY);*/
      
            OdGsDCRect viewportRect = new OdGsDCRect(x1, y1, x2, y2);
            m_pPrinterDevice.onSize(viewportRect);
            //if (m_pPrinterDevice.underlyingDevice().properties().Contains("WindowHDC", OdRxObject??))
              m_pPrinterDevice.underlyingDevice().properties().putAt("WindowHDC", new OdRxVariantValue((Int32)ev.Graphics.GetHdc()));
              //m_pPrinterDevice.properties().putAt("WindowHDC", new OdRxVariantValue((Int32)ev.Graphics.GetHdc()));
            m_pPrinterDevice.update();
      
            pAbstractViewPE.Dispose();
            pLayout.Dispose();
          }
        }
      }
    }




    public void Print(OdDbDatabase db, OdGsView pView, bool bPreview)
    {
      database = db;
      pViewDr = pView;
      //OdDbObjectId idLayout = database.currentLayoutId();
      using (OdDbBlockTableRecord btr = (OdDbBlockTableRecord)database.getActiveLayoutBTRId().safeOpenObject()) //.CurrentSpaceId.GetObject(OpenMode.ForRead))
      {
        using (OdDbLayout pLayout = (OdDbLayout)btr.getLayoutId().safeOpenObject()) //. LayoutId.GetObject(OpenMode.ForRead))
        {
          PrintDocument prDoc = new PrintDocument();
          prDoc.PrintPage += new PrintPageEventHandler(this.PrintPage);
//          prDoc.PrinterSettings.PrinterName = pLayout.getPlotCfgName();

          PageSettings pageSetting = prDoc.DefaultPageSettings;
          OdDbPlotSettings.PlotRotation rotation = pLayout.plotRotation();
          if (rotation == OdDbPlotSettings.PlotRotation.k90degrees || rotation == OdDbPlotSettings.PlotRotation.k270degrees)
            pageSetting.Landscape = true;
          else
            pageSetting.Landscape = false;

          Double kMmPerInch = 10 / 2.54;
          Double dPaperWidth = 0;
          Double dPaperHeight = 0;
          pLayout.getPlotPaperSize(out dPaperWidth, out dPaperHeight);
          String sCanonicalMediaName = pLayout.getCanonicalMediaName();
          PaperSize ps = new PaperSize(sCanonicalMediaName, (int)(dPaperWidth * kMmPerInch), (int)(dPaperHeight * kMmPerInch));
          pageSetting.PaperSize = ps;

          //default as OdaMfc
          pageSetting.Margins.Left = 0;
          pageSetting.Margins.Right = 0;
          pageSetting.Margins.Top = 0;
          pageSetting.Margins.Bottom = 0;

          prDoc.DefaultPageSettings = pageSetting;
          if (prDoc.PrinterSettings.IsValid)
          {
            try
            {                                                                             //WinGDI, WinOpenGL
              using (OdGsModule gsModule = (OdGsModule)(OdGsModule)Globals.odrxDynamicLinker().loadModule("WinGDI.txv", false))
              {
                using (OdGsDevice graphichsDevice = gsModule.createDevice())
                {
                  _gsDevice = graphichsDevice;
                  using (OdGiContextForDbDatabase ctx = OdGiContextForDbDatabase.createObject())
                  {
                    //using (Dictionary props = graphichsDevice.Properties)
                    //{
                    //  if (props.Contains("EnableSoftwareHLR")) // Check if property is supported
                    //    props.AtPut("EnableSoftwareHLR", new RxVariant(true));
                    //}

                    ctx.setPlotGeneration(true);
                    ctx.enableGsModel(true);
                    ctx.setDatabase(database);
                    graphichsDevice.setUserGiContext(ctx);

                    m_pPrinterDevice = TD_Db.setupActiveLayoutViews(graphichsDevice, ctx);
                    
                    //OdCmColor color = new OdCmColor();
                    //color.setRGB(255, 255, 255);
                    //m_pPrinterDevice.setBackgroundColor(color); //.BackgroundColor = Color.FromArgb(0, 255, 255, 255);
                    //ctx.PaletteBackground = m_pPrinterDevice.BackgroundColor;
                    //m_pPrinterDevice.SetLogicalPalette(Device.LightPalette);
                    uint[] CurPalette;
                    CurPalette = Teigha.Core.AllPalettes.getLightPalette();
                    m_pPrinterDevice.setBackgroundColor(CurPalette[0]);
                    m_pPrinterDevice.setLogicalPalette(CurPalette, 256);

                    //m_pPrinterDevice.ActiveView.Mode = Teigha.GraphicsSystem.RenderMode.HiddenLine;
//                    Aux.preparePlotstyles(database, ctx);
                  }
                }
              }
            }
            catch (System.Exception ex)
            {
              MessageBox.Show(ex.ToString());
            }

            if (bPreview)
            {
              PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
              printPreviewDialog.Document = prDoc;
              printPreviewDialog.ShowDialog();
            }
            else
            {
              prDoc.Print();
            }

            if (m_pPrinterDevice != null)
              m_pPrinterDevice.Dispose();
            database = null;
            pViewDr = null;
          }
        }
      }
    } 
  };
}
