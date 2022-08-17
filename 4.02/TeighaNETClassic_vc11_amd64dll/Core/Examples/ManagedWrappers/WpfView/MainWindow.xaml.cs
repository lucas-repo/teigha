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
using System.Windows.Interop;
using System.Drawing;
using Teigha;
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;

namespace WpfView
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Teigha.Runtime.Services tdSrv;
    Database database = null;
    LayoutManager lm;

    public MainWindow()
    {
      InitializeComponent();
      foreach (MenuItem topLevelMenu in MainMenu.Items)
      {
        foreach (MenuItem itemMenu in topLevelMenu.Items)
        {
          itemMenu.Click += new RoutedEventHandler(MenuItem_Click);
        }
      }

      String strPath = Environment.GetEnvironmentVariable("PATH");
      String strPathModules = ""; // System.Environment.CurrentDirectory;
      Environment.SetEnvironmentVariable("PATH", strPathModules + ";" + strPath);

      tdSrv = new Teigha.Runtime.Services();

      DrawControl drawCtrl = new DrawControl();
      winFormsHost.Child = drawCtrl;
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      if (database != null)
        database.Dispose();
      tdSrv.Dispose();
    }


    private void fileOpen(String sFilePath)
    {
      if (lm != null)
      {
        lm.LayoutSwitched -= new Teigha.DatabaseServices.LayoutEventHandler(reinitGraphDevice);
        HostApplicationServices.WorkingDatabase = null;
        lm = null;
      }

      bool bLoaded = true;
      database = new Database(false, false);

      try
      {
        String sExt = sFilePath.Substring(sFilePath.Length - 4);
        sExt = sExt.ToUpper();
        if (sExt.EndsWith(".DWG"))
        {
          database.ReadDwgFile(sFilePath, FileOpenMode.OpenForReadAndAllShare, false, "");
        }
        else if (sExt.EndsWith(".DXF"))
        {
          database.DxfIn(sFilePath, "");
        }
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message);
        bLoaded = false;
      }



      if (bLoaded)
      {
        this.Title = String.Format("WpfViewApp - [{0}]", sFilePath);

        ((DrawControl)winFormsHost.Child).init(database);
       /* userSt.LastOpenedFile = sFilePath;
        UpdateMenuFile();
        userSt.Save();
        HostApplicationServices.WorkingDatabase = database;
        lm = LayoutManager.Current;
        lm.LayoutSwitched += new Teigha.DatabaseServices.LayoutEventHandler(reinitGraphDevice);
        String str = HostApplicationServices.Current.FontMapFileName;

        //menuStrip.
        exportToolStripMenuItem.Enabled = true;
        zoomToExtentsToolStripMenuItem.Enabled = true;
        zoomWindowToolStripMenuItem.Enabled = true;
        setAvtiveLayoutToolStripMenuItem.Enabled = true;
        fileDependencyToolStripMenuItem.Enabled = true;
        panel1.Enabled = true;
        pageSetupToolStripMenuItem.Enabled = true;
        printPreviewToolStripMenuItem.Enabled = true;
        printToolStripMenuItem.Enabled = true;*/
      }
    }

    private void reinitGraphDevice(object sender, Teigha.DatabaseServices.LayoutEventArgs e)
    {
      //disableCommand();
      ((DrawControl)winFormsHost.Child).reinit(database);
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      MenuItem mItem = e.Source as MenuItem;
      if (mItem.IsEnabled)
      {
        String sHeader = mItem.Header as String;
        if ("_Open" == sHeader)
        {
          database = new Database(false, false);
          System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
          openFileDialog.Filter = "dwg files|*.dwg|dxf files|*.dxf";
          openFileDialog.DefaultExt = "dwg";
          openFileDialog.RestoreDirectory = true;

          if (System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog())
          {
            fileOpen(openFileDialog.FileName);
            MenuItem mPar = mItem.Parent as MenuItem;
            MenuItemSave.IsEnabled   = true;
            MenuItemSaveAs.IsEnabled = true;
          }
        }//if ("_Open" == sHeader)
        if ("_Exit" == sHeader)
        {
          this.Close();
        }
        if (database != null)
        {
          if ("_Save" == sHeader)
          {
            if (database != null)
              database.Save();
          }
          if ("_SaveAs" == sHeader)
          {
            System.Windows.Forms.SaveFileDialog saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveAsFileDialog.Filter = "dwg R24 file format(*.dwg)|*.dwg|dwg R24 file format(*.dwg)|*.dwg|dwg R21 file format(*.dwg)|*.dwg|dwg R18 file format(*.dwg)|*.dwg|dwg R15 file format(*.dwg)|*.dwg|dwg R14 file format(*.dwg)|*.dwg|dwg R13 file format(*.dwg)|*.dwg|dwg R12 file format(*.dwg)|*.dwg";
            saveAsFileDialog.DefaultExt = "dwg";
            saveAsFileDialog.RestoreDirectory = true;

            if (System.Windows.Forms.DialogResult.OK == saveAsFileDialog.ShowDialog())
            {
              int version = saveAsFileDialog.FilterIndex % 7;              
              DwgVersion vers = DwgVersion.Current;
              if (0 == version)
                vers = DwgVersion.vAC12;
              else
              {
                if (1 == version)
                  vers = DwgVersion.vAC24;
                else
                {
                  if (2 == version)
                    vers = DwgVersion.vAC21;
                  else
                  {
                    if (3 == version)
                      vers = DwgVersion.vAC18;
                    else
                    {
                      if (4 == version)
                        vers = DwgVersion.vAC15;
                      else if (5 == version)
                        vers = DwgVersion.vAC14;
                    }
                  }
                }
              }//else if (0 == version)
              if (Math.Truncate((double)saveAsFileDialog.FilterIndex / 7) == 0)
                database.SaveAs(saveAsFileDialog.FileName, vers);
              else
                database.DxfOut(saveAsFileDialog.FileName, 16, vers);
            }//if (System.Windows.Forms.DialogResult.OK == saveAsFileDialog.ShowDialog())
          }//if ("_SaveAs" == sHeader)
        }//if (database != null)
      }//if (mItem.IsEnabled)
    }
  }
}//namespace WpfView
