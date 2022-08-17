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
using Teigha.TD;
using Teigha.TG;

namespace DwgViewer
{
  public class Orbit : OdGiDrawable
  {
    private OdGsCache pCache;
    public Orbit()
    {
      pCache = null;
    }
    public override bool isPersistent()
    {
      return false;
    }
    public override OdDbStub id()
    {
      return null;
    }
    public override void setGsNode(OdGsCache pGsNode)
    {
      pCache = pGsNode;
    }
    public override OdGsCache gsNode()
    {
      return pCache;
    }

    protected override void subViewportDraw(OdGiViewportDraw vd)
    {
      OdGiViewport vp = vd.viewport();
      OdGiGeometry geom = vd.geometry();

      OdGiContext context = vd.context();
      OdGiGeometry rawGiGeom = vd.rawGeometry();
      OdGiSubEntityTraits traits = vd.subEntityTraits();

      vd.subEntityTraits().setColor((ushort)OdCmEntityColor.ACIcolorMethod.kACIGreen);
      vd.subEntityTraits().setTrueColor(new OdCmEntityColor(192, 32, 255));
      vd.subEntityTraits().setFillType(OdGiFillType.kOdGiFillNever);
//
      //OdGiModelTransformSaver mt = new OdGiModelTransformSaver(geom, vp.getEyeToModelTransform());

      OdGePoint2d pt1_1 = new OdGePoint2d();      
      OdGePoint2d pt2 = new OdGePoint2d();
      vp.getViewportDcCorners(pt1_1, pt2);
      pt2.x += pt1_1.x;
      pt2.y += pt1_1.y;
      OdGePoint3d pt1 = new OdGePoint3d(pt2.x/2, pt2.y/2, 0);
      /*double r = odmin(pt2.x, pt2.y) / 9. * 7. / 2.;
      ((OdGePoint2d&)pt1) += (pt2.asVector() / 2.);
      geom.circle(pt1, r, OdGeVector3d::kZAxis);

      geom.circle(pt1 + OdGeVector3d(0., r, 0.), r / 20., OdGeVector3d::kZAxis);
      geom.circle(pt1 + OdGeVector3d(0.,-r, 0.), r / 20., OdGeVector3d::kZAxis);
      geom.circle(pt1 + OdGeVector3d( r, 0.,0.), r / 20., OdGeVector3d::kZAxis);
      geom.circle(pt1 + OdGeVector3d(-r, 0.,0.), r / 20., OdGeVector3d::kZAxis);    */
      geom.circle(pt1, (pt2.y - pt1_1.y)/4, OdGeVector3d.kZAxis);
    }
    protected override bool subWorldDraw(OdGiWorldDraw wd)
    {
      return false;
    }

    protected override uint subSetAttributes(OdGiDrawableTraits traits)
    {
      return (uint)SetAttributesFlags.kDrawableIsAnEntity | (uint)SetAttributesFlags.kDrawableRegenDraw; //kDrawableIsAnEntity | kDrawableRegenDraw;
    }

    public override Teigha.Core.OdRxClass isA()
    {
      return base.isA();
    }
  }
}
