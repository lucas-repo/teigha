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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSample2
{
  /// <summary>
  /// Interaction logic for CustomImage1.xaml
  /// </summary>
  public partial class CustomImage1 : UserControl
  {
    private bool mDown = false;
    private Point startPoint = new Point(0, 0);
    private Point curPoint = new Point(0, 0);
    private TeighaD3DImage mTeighaD3DImage = null;
    public String FilePath
    {
      get 
      {
        if (null == mTeighaD3DImage)
        {
          return pFilePath;
        }
        return mTeighaD3DImage.FilePath; 
      }
      set 
      {
        if (null != mTeighaD3DImage)
        {
          // first time setting a dwg
          if (String.Empty == pFilePath)
          {
            renderImage.Source = mTeighaD3DImage;
          }
          mTeighaD3DImage.FilePath = value;
        }
        pFilePath = value;
      }
    }
    private String pFilePath = String.Empty;
    public CustomImage1()
    {
      InitializeComponent();
      mTeighaD3DImage = new TeighaD3DImage();
    }
    public void ControlSizeChanged(Size size)
    {
      mTeighaD3DImage.OnRenderSizeChanged(size);
    }
    public override void EndInit()
    {
      base.EndInit();
      if (String.Empty != pFilePath)
      {
        mTeighaD3DImage.FilePath = pFilePath;
        renderImage.Source = mTeighaD3DImage;
      }
    }
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      mTeighaD3DImage.Zoom(e.Delta > 0);      
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (mDown)
      {
        curPoint = e.GetPosition(renderImage);
        mTeighaD3DImage.Dolly(startPoint.X - curPoint.X, curPoint.Y - startPoint.Y, 0);
        startPoint = curPoint;
      }
      base.OnMouseMove(e);
    }
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      mDown = true;
      startPoint = e.GetPosition(renderImage);
      base.OnMouseDown(e);
    }
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      mDown = false;
      base.OnMouseUp(e);
    }

    public void DrawLine()
    {
      mTeighaD3DImage.DrawLine();
    }
    public void RunTest(int number)
    {
      mTeighaD3DImage.RunTest(number);
    }
  }
}
