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

namespace ExDgnVectorizeMgd
{
  class ExGsSimpleDevice : OdGsBaseVectorizeDevice
  {
    OdGiConveyorGeometryDumper m_pDestGeometry;
    OdGiDumper m_pDumper = new OdGiDumperImpl();
    List<ExSimpleView> views = new List<ExSimpleView>();
    public ExGsSimpleDevice()
    {
      m_pDestGeometry = new OdGiConveyorGeometryDumper(m_pDumper);
    }
    /*public override void Dispose()
    {
      if (views != null)
      {
        foreach (ExSimpleView v in views)
        {
          v.Dispose();
        }
        views = null;
      }
      m_pDumper = null;
      if (m_pDestGeometry != null)
      {
        m_pDestGeometry.Dispose();
        m_pDestGeometry = null;
      }
      base.Dispose();
    }*/
    
    public override OdGsView createView(OdGsClientViewInfo pInfo, bool bEnableLayerVisibilityPerView)
      {
        //ExSimpleView pView = new ExSimpleView(this);
        ExSimpleVectorizer pVect = new ExSimpleVectorizer(this);
        ExSimpleView pView = new ExSimpleView(this);
        pVect._view = pView;
        pVect.setUp(pView);
        // save reference, so that GC would not reclaim it
        views.Add(pView);

        pView.init(this, pInfo, bEnableLayerVisibilityPerView);
    
      /**********************************************************************/
      /* Directs the output geometry for the view to the                    */
      /* destination geometry object                                        */
      /**********************************************************************/
    
      pVect.output().setDestGeometry(m_pDestGeometry);

      return pVect.gsView();
    }
    
    public OdGiConveyorGeometry destGeometry { get { return m_pDestGeometry; } }
    public OdGiDumper dumper { get { return m_pDumper; } }

    /************************************************************************/
    /* Called by the Teigha for .dwg files vectorization framework to update*/
    /* the GUI window for this Device object                                */
    /*                                                                      */
    /* pUpdatedRect specifies a rectangle to receive the region updated     */
    /* by this function.                                                    */
    /*                                                                      */
    /* The override should call the parent version of this function,        */
    /* OdGsBaseVectorizeDevice::update().                                   */
    /************************************************************************/
/*    public override void update(OdGsDCRect pUpdatedRect)
    {
      base.update(pUpdatedRect);
    }*/
    UInt32 LOBYTE(UInt32 w) {return (w & 0xff);}
    UInt32 GetRValue(UInt32 rgb){ return (LOBYTE(rgb));}
    UInt32 GetGValue(UInt32 rgb){ return (LOBYTE(((rgb)) >> 8));}
    UInt32 GetBValue(UInt32 rgb){ return (LOBYTE((rgb)>>16));}

    /************************************************************************/
    /* Called by each associated view object to set the current RGB draw    */
    /* color.                                                               */
    /************************************************************************/  
    public void draw_color(UInt32 color)
    {
      String retVal = "r" + GetRValue(color).ToString() + ":g" 
        + GetGValue(color).ToString() + ":b" + GetBValue(color).ToString();
      dumper.output("draw_color", retVal);
    }
    /************************************************************************/
    /* Called by each associated view object to set the current RGB draw    */
    /* color.                                                               */
    /************************************************************************/
    public void draw_color(OdCmEntityColor color)
    {
      String retVal = "r" + color.red().ToString() + ":g" 
        + color.green().ToString() + ":b" + color.blue().ToString();
      dumper.output("draw_color", retVal);
    }
    
    /************************************************************************/
    /* Called by each associated view object to set the current ACI draw    */
    /* color.                                                               */
    /************************************************************************/  
    public void draw_color_index(UInt16 colorIndex)
    {
      dumper.output("draw_color_index", colorIndex.ToString());
    }

    /************************************************************************/
    /* Called by each associated view object to set the current draw        */
    /* lineweight and pixel width                                           */
    /************************************************************************/  
    public void draw_lineWidth(LineWeight weight, int pixelLineWidth)
    {
      dumper.output("draw_lineWidth");
      dumper.pushIndent();
      dumper.output("weight", weight.ToString());
      dumper.output("pixelLineWidth", pixelLineWidth.ToString());
      dumper.popIndent();
    }
    /************************************************************************/
    /* Called by each associated view object to set the current Fill Mode   */
    /************************************************************************/  
    public void draw_fill_mode(OdGiFillType fillMode)
    {
      switch (fillMode)
      {
        case OdGiFillType.kOdGiFillAlways:
          dumper.output("draw_fill_mode", "FillAlways");
          break;
        case OdGiFillType.kOdGiFillNever:
          dumper.output("draw_fill_mode", "FillNever");
          break;
      }
    }
    public void setupSimplifier(OdGiDeviation pDeviation)
    {
      m_pDestGeometry.setDeviation(pDeviation);
    }
  }

  class ExSimpleVectorizer : OdGsBaseVectorizer
  {
    OdGiClipBoundary m_eyeClip = new OdGiClipBoundary();
    ExGsSimpleDevice _device = null;
    public ExSimpleView _view = null;
    public ExSimpleVectorizer(ExGsSimpleDevice d)
    {
        // pay attention - if Memory Transactions are used, newly created custom vectorizer should be added to current transaction
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().GetCurrentTransaction();
        if (null != mTr)
        {
            mTr.AddObject(this);
        }
      _device = d;
      m_eyeClip.m_bDrawBoundary = false;
    }
    /*public override void Dispose()
    {
      if (m_eyeClip != null)
      {
        m_eyeClip.Dispose();
        m_eyeClip = null;
      }
      base.Dispose();
    }*/
    public override void setUp(OdGsViewImpl view)
    {
      ((ExSimpleView)view)._vect = this;
      base.setUp(view);
    }
    public override void  beginViewVectorization()
    {
 	    base.beginViewVectorization();
      _device.setupSimplifier(drawContext().eyeDeviation());
    }
    OdGsView _gsView = null;
    public OdGsView View
    {
      get 
      {
        if (_gsView == null)
          _gsView = gsView(); 
        return _gsView;
      }
    }
    public override void  draw(OdGiDrawable pDrawable)
    {
      OdDgElement pElm = (OdDgElement)pDrawable;

      _device.dumper.output("");
      String sClassName = "<" + pElm.isA().name() + ">";
      String sHandle = pElm.isDBRO() ? pElm.elementId().getHandle().ascii() : "non-DbResident";
      _device.dumper.output("Start Drawing " + sClassName, sHandle);
      _device.dumper.pushIndent();

      /**********************************************************************/
      /* The parent class version of this function must be called.          */
      /**********************************************************************/
      base.draw(pDrawable);
      //draw(pDrawable);

      _device.dumper.popIndent();
      _device.dumper.output("End Drawing " + sClassName, sHandle);
    }
    public override bool  regenAbort()
    {
 	    return false;
    }
    public override double  deviation(OdGiDeviationType deviationType, OdGePoint3d pointOnCurve)
    {
 	    return 0.8; //0.000001; 
    }
    public override void  onTraitsModified()
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      //base.onTraitsModified();
      OdGiSubEntityTraitsData currTraits = effectiveTraits();
      if (currTraits.trueColor().isByColor())
      {
        _device.draw_color(currTraits.trueColor());
      }
      else
      {
        _device.draw_color_index(currTraits.color());
      }

      LineWeight lw = currTraits.lineWeight();
      _device.draw_lineWidth(lw, view().lineweightToPixels(lw));
      _device.draw_fill_mode(currTraits.fillType());
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    public override void updateViewport()
    {
        MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      /**********************************************************************/
      /* If geometry receiver is a simplifier, we must re-set the draw      */
      /* context for it                                                     */
      /**********************************************************************/
      ((OdGiGeometrySimplifier)(_device.destGeometry)).setDrawContext(drawContext());
      /**********************************************************************/
      /* Comment these functions to get primitives in eye coordinates.      */
      /**********************************************************************/
      OdGeMatrix3d eye2Screen = _view.eyeToScreenMatrix();
      OdGeMatrix3d screen2Eye = eye2Screen.inverse();

      setEyeToOutputTransform(eye2Screen);

      /**********************************************************************/
      /* Perform viewport clipping in eye coordinates.                      */
      /**********************************************************************/
      m_eyeClip.m_bClippingFront = _view.isFrontClipped();
      m_eyeClip.m_bClippingBack = _view.isBackClipped();
      m_eyeClip.m_dFrontClipZ = _view.frontClip();
      m_eyeClip.m_dBackClipZ = _view.backClip();
      m_eyeClip.m_vNormal = viewDir();
      m_eyeClip.m_ptPoint = _view.target();
      m_eyeClip.m_Points.Clear();
      OdGeVector2d halfDiagonal = new OdGeVector2d(_view.fieldWidth() / 2.0, _view.fieldHeight() / 2.0);

      // rectangular clipping
      m_eyeClip.m_Points.Add(OdGePoint2d.kOrigin.Sub(halfDiagonal));
      m_eyeClip.m_Points.Add(OdGePoint2d.kOrigin.Add(halfDiagonal));

      m_eyeClip.m_xToClipSpace = getWorldToEyeTransform();

      pushClipBoundary(m_eyeClip);
      base.updateViewport();
      popClipBoundary();
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
  }
  class ExSimpleView : OdGsBaseVectorizeView
  {
    OdGiClipBoundary m_eyeClip = new OdGiClipBoundary();
    ExGsSimpleDevice _device = null;
    public ExSimpleVectorizer _vect = null;
    /*public override void Dispose()
    {
      _device = null;
      base.Dispose();
    }*/
    public ExSimpleView(ExGsSimpleDevice d)
    {
      _device = d;
    }

    protected override OdGsBaseVectorizer getVectorizer(bool bDisplay)
    {
      //return base.getVectorizer(bDisplay);
      return _vect;
    }
    public override OdGsDevice  device()
    {
 	    return _device;
    }    

    public void ownerDrawDc(OdGePoint3d origin, OdGeVector3d u, OdGeVector3d v, OdGiSelfGdiDrawable pDrawable, bool dcAligned, bool allowClipping)
    {
      MemoryTransaction mTr = MemoryManager.GetMemoryManager().StartTransaction();
      // ownerDrawDc is not conveyor primitive. It means we must take all rendering processings
      // (transforms) by ourselves
      OdGeMatrix3d eyeToOutput = _vect.eyeToOutputTransform();
      OdGePoint3d originXformed = new OdGePoint3d(eyeToOutput * origin);
      OdGeVector3d uXformed = new OdGeVector3d(eyeToOutput * u);
      OdGeVector3d vXformed = new OdGeVector3d(eyeToOutput * v);

      OdGiDumper pDumper = _device.dumper;

      pDumper.output("ownerDrawDc");
      
      // It's shown only in 2d mode.
      if(mode() == RenderMode.k2DOptimized)
      {
        pDumper.pushIndent();
        pDumper.output("origin xformed", originXformed.ToString());
        pDumper.output("u xformed", uXformed.ToString());
        pDumper.output("v xformed", vXformed.ToString());
        pDumper.output("dcAligned", dcAligned.ToString());//
        pDumper.output("allowClipping", allowClipping.ToString());
        pDumper.popIndent();
      }
      MemoryManager.GetMemoryManager().StopTransaction(mTr);
    }
    protected override void releaseVectorizer(OdGsBaseVectorizer pVect)
    {
      // override is needed, otherwise an attempt to call a pure virtual method will be performed
    }
  }
}