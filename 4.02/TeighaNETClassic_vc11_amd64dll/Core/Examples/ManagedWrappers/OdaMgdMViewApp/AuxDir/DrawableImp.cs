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
using System.Drawing;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;


namespace OdaMgdMViewApp
{
  public class DrawableImp : Drawable
  {
    public DrawableImp()
    {
    }

    protected override void SubViewportDraw(ViewportDraw vd)
    {
    }

    protected override int SubSetAttributes(DrawableTraits traits)
    {
      return 0;
    }

    protected override int SubViewportDrawLogicalFlags(ViewportDraw vd)
    {
      return 0;
    }

    protected override bool SubWorldDraw(WorldDraw wd)
    {
      return false;
    }

    public override bool IsPersistent
    {
      get
      {
        return false;
      }
    }

    public override ObjectId Id
    {
      get
      {
        return new ObjectId();
      }
    }
  }

}
