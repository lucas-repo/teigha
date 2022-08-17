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
using System.Reflection;


namespace DevGuideExamples
{
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window
  {
    string dirName = "C:\\";

    int btnInRow = 0;
    int btnHeight = 23;
    int btnWidth = 75;
    int btnBorder = 3;
    int countDefEll = 0;
    Assembly curAsm;

    public Window1()
    {
      InitializeComponent();

      countDefEll = GridPanel.Children.Count;
      strPath.Content = dirName;
      radioButtonC.IsChecked = true;
    }

    void BtnsUpdate(Assembly asm)
    {
      curAsm = asm;
      if (GridPanel.Children.Count > countDefEll)
        GridPanel.Children.RemoveRange(countDefEll+1, GridPanel.Children.Count - countDefEll);
      Type[] allTypes = curAsm.GetExportedTypes();
      int i = 0;
      btnInRow = (int)GridPanel.Width / (btnWidth + btnBorder);
      foreach (Type type in allTypes)
      {
        AddNewTestButton(type.Name.ToString(), i++);
      }
    }

    void AddNewTestButton(String text, int i)
    {
      Button btn = new Button();
      btn.Height = btnHeight;
      btn.Width = btnWidth;
      btn.Content = text;
      int row = i / btnInRow;
      int pos = i - row * btnInRow;
      btn.Margin = new System.Windows.Thickness(btnWidth * pos + pos * btnBorder, row * btn.Height + row * btnBorder, 0, 0);
      btn.HorizontalAlignment = HorizontalAlignment.Left;
      btn.VerticalAlignment = VerticalAlignment.Top;
      btn.Click += new RoutedEventHandler(button_Click);
      GridPanel.Children.Add(btn);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
    }

    private void button_Click(object sender, RoutedEventArgs e)
    {
      Button btn = (Button)sender;
      Type[] allTypes = curAsm.GetTypes();
      foreach (Type type in allTypes)
      {
        if (btn.Content.ToString() == type.Name)
        {
          object[] args = new object[1];
          args[0] = dirName;
          object tp = type.InvokeMember(null, BindingFlags.CreateInstance, null, type, args);
        }
      }
    }

    private void buttonPath_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Forms.FolderBrowserDialog myDialog = new System.Windows.Forms.FolderBrowserDialog();
      if (myDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        dirName = myDialog.SelectedPath + "\\";
        strPath.Content = dirName;
      }
    }

    private void radioButtonC_Checked(object sender, RoutedEventArgs e)
    {
      BtnsUpdate(typeof(CDevGuideExamplesProject.MTextEx).Assembly);
    }

    private void radioButtonVBNET_Checked(object sender, RoutedEventArgs e)
    {
      BtnsUpdate(typeof(VBNETDevGuideExamplesProject.MTextEx).Assembly);
    }
  }
}
