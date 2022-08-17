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
using Teigha.Core;
using Teigha.TD;
using System.Windows.Interop;

namespace WpfSample2
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    public Window1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
      // Create OpenFileDialog 
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

      // Set filter for file extension and default file extension 
      dlg.DefaultExt = ".dwg";
      dlg.Filter = "DWG files (.dwg)|*.dwg";

      // Display OpenFileDialog by calling ShowDialog method 
      Nullable<bool> result = dlg.ShowDialog();

      // Get the selected file name and display in a TextBox 
      if (result == true)
      {
        // Open document 
        string filename = dlg.FileName;
        //mTeighaD3DImage.FilePath = filename;
        dwgControl.FilePath = filename;
      }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
      base.OnRenderSizeChanged(sizeInfo);
    }

    private void renderImage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize != e.PreviousSize)
      {
        //mTeighaD3DImage.OnRenderSizeChanged(e.NewSize);
      }
    }

    private void dwgControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize != e.PreviousSize)
      {
        dwgControl.ControlSizeChanged(e.NewSize);
      }
    }

    private void test1_Click(object sender, RoutedEventArgs e)
    {
      dwgControl.DrawLine();
    }

    private void test2_Click(object sender, RoutedEventArgs e)
    {
      dwgControl.RunTest(0);
    }

    private void test3_Click(object sender, RoutedEventArgs e)
    {
      dwgControl.RunTest(1);
    }

    private void test4_Click(object sender, RoutedEventArgs e)
    {
      dwgControl.RunTest(2);
    }
  }
  class Srv : ExHostAppServices
  {
    public override bool getPassword(string dwgName, bool isXref, ref string password)
    {
      Console.WriteLine("Enter password to open drawing: {0}", dwgName);
      password = Console.ReadLine().ToUpper();
      return password != "";
    }

    public override String product()
    {
      return String.Format("Teigha.NET");
    }
  }

}
