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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsInterface;
using Teigha.GraphicsSystem;
using Teigha.Runtime;


namespace WpfView
{
  public partial class DrawControl : UserControl
  {
    Graphics graphics;
    LayoutHelperDevice helperDevice;

    public DrawControl()
    {
      InitializeComponent();
     }

    public void init(Database database)
    {      
      try
      {
        graphics = Graphics.FromHwnd(this.Handle);
        // load some predefined rendering module (may be also "WinDirectX" or "WinOpenGL")
        using (GsModule gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("WinDirectX.txv", false, true))
        {
          // create graphics device
          using (Teigha.GraphicsSystem.Device graphichsDevice = gsModule.CreateDevice())
          {
            // setup device properties
            using (Dictionary props = graphichsDevice.Properties)
            {
              if (props.Contains("WindowHWND")) // Check if property is supported
                props.AtPut("WindowHWND", new RxVariant(this.Handle)); // hWnd necessary for DirectX device
              if (props.Contains("WindowHDC")) // Check if property is supported
                props.AtPut("WindowHDC", new RxVariant(graphics.GetHdc())); // hWindowDC necessary for Bitmap device
              if (props.Contains("DoubleBufferEnabled")) // Check if property is supported
                props.AtPut("DoubleBufferEnabled", new RxVariant(true));
              if (props.Contains("EnableSoftwareHLR")) // Check if property is supported
                props.AtPut("EnableSoftwareHLR", new RxVariant(true));
              if (props.Contains("DiscardBackFaces")) // Check if property is supported
                props.AtPut("DiscardBackFaces", new RxVariant(true));
            }
            // setup paperspace viewports or tiles
            ContextForDbDatabase ctx = new ContextForDbDatabase(database);
            ctx.UseGsModel = true;

            helperDevice = LayoutHelperDevice.SetupActiveLayoutViews(graphichsDevice, ctx);
          }
        }
        // set palette
        helperDevice.SetLogicalPalette(Device.DarkPalette);
        // set output extents
        resize();
        //changeActiveViewMode(Teigha.GraphicsSystem.RenderMode.Optimized2D);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    public void reinit(Database database)
    {
      if (graphics != null)
        graphics.Dispose();
      if (helperDevice != null)
        helperDevice.Dispose();
      init(database);
    }

    public void resize()
    {
      if (helperDevice != null)
      {
        Rectangle r = this.Bounds;
        r.Offset(-this.Location.X, -this.Location.Y);
        // HDC assigned to the device corresponds to the whole client area of the panel
        helperDevice.OnSize(r);
        Invalidate();
      }
    }

    private void DrawControl_Paint(object sender, PaintEventArgs e)
    {
      if (helperDevice != null)
      {
        try
        {
          helperDevice.Update();
        }
        catch (System.Exception ex)
        {
          graphics.DrawString(ex.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), new PointF(150.0F, 150.0F));
        }
      }
    }
  }
}
