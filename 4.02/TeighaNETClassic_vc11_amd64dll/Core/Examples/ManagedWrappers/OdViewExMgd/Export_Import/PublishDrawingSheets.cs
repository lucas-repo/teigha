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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Teigha;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.DatabaseServices;
using Teigha.Export_Import;

namespace OdViewExMgd
{
  public partial class PublishDrawingSheets : Form
  {
    DwfPageDataCollection pdCollection = new DwfPageDataCollection();
    mDwfExportParams param;
    String fileName;
    public PublishDrawingSheets(mDwfExportParams par)
    {
      InitializeComponent();
      param = par;
      fileName = param.Database.Filename;
      textBoxMultySheetDWFFileName.Text = fileName;

      InitPagaDate();
      FillListSheets();

      btSetting.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
      btRemove.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
      textBoxPassDWF.Enabled = false;

      Update();
    }

    void InitPagaDate()
    {
      String strSeed = param.Database.Filename;

      int first = strSeed.LastIndexOf("\\") + 1;
      int last  = strSeed.LastIndexOf(".");
      strSeed = strSeed.Substring(first, last - first);

      ObjectId idBlockModelSpaceId = SymbolUtilityServices.GetBlockModelSpaceId(param.Database);
      ObjectId idLayout;
      using (BlockTableRecord objBlockModelSpaceId = (BlockTableRecord)idBlockModelSpaceId.GetObject(OpenMode.ForRead))
      {
        idLayout = objBlockModelSpaceId.LayoutId;
        using (Layout objLayout = (Layout)idLayout.GetObject(OpenMode.ForRead))
        {
          DWFPageData pageData = new DWFPageData();
          pageData.Layout = objLayout.LayoutName;

          // init default sheet name
          pageData.PageTitle = strSeed + '-' + pageData.Layout;
          pdCollection.Add(pageData);
        }
      }

      // To get the access paper layouts in this database.
      using (DBDictionary layouts = (DBDictionary)param.Database.LayoutDictionaryId.GetObject(OpenMode.ForRead))
      {
        foreach (DBDictionaryEntry entry in layouts)
        {
          if (entry.Value == idLayout)
            continue;

          DWFPageData pageData = new DWFPageData();
          pageData.Layout = entry.Key;
          pageData.PageTitle = strSeed + '-' + entry.Key;
          pdCollection.Add(pageData);
        }
      } 
    }

    void FillListSheets()
    {
      listView1.Items.Clear();
      foreach (DWFPageData pd in pdCollection)
      {
        ListViewItem lvItem = new ListViewItem(pd.Layout);
        lvItem.SubItems.Add(pd.PageTitle);
        lvItem.SubItems.Add(param.Database.Filename);
        lvItem.SubItems.Add(pd.PageAuthor);
        lvItem.SubItems.Add(pd.PageSubject);
        lvItem.SubItems.Add(pd.Preview.FileName);
        lvItem.SubItems.Add(pd.PageComments);
        listView1.Items.Add(lvItem);
      }
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btPublish_Click(object sender, EventArgs e)
    {
      Update();

      String strFileName = fileName;
      int find = strFileName.LastIndexOf(".dwf");
      if (find == -1)
      {
        strFileName += String.Format(".dwf");
      }
      
      param.FileName = strFileName;
      if (radioButtonBinaryDWF.Checked)
      {
        param.Format  = DwfFormat.UNCOMPRESSED_BINARY;
        param.Version = DwfVersion.Dwf_v60;
      }
      else
      {
        param.Format  = DwfFormat.ASCII;
        param.Version = DwfVersion.Dwf_v60;
      }
      param.PageData = pdCollection;
      Close();
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      btSetting.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
      btRemove.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
    }

    private void btBrowse_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == openFileDialog1.ShowDialog())
      {
        String strFileName = openFileDialog1.FileName;
        int find = strFileName.LastIndexOf(".dwf");
        if (find == -1)
        {
          strFileName += String.Format(".dwf");
        }
        textBoxMultySheetDWFFileName.Text = strFileName;
        Update();
      }
    }

    private void btRemove_Click(object sender, EventArgs e)
    {
      if (listView1.SelectedIndices.Count > 0)
      {
        int index = 0;
        foreach (ListViewItem lm in listView1.SelectedItems)
        {
          index = lm.Index;
          pdCollection.RemoveAt(lm.Index);
        }
        listView1.Items.RemoveAt(index);
      }  
      btSetting.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
      btRemove.Enabled = (bool)(listView1.SelectedIndices.Count > 0);
    }

    private void btSetting_Click(object sender, EventArgs e)
    {
      if (listView1.SelectedIndices.Count > 0)
      {
        int index = 0;
        ListViewItem lvItem;
        foreach (ListViewItem lm in listView1.SelectedItems)
        {
          DWFPageData pageData = pdCollection[lm.Index];
          publishSetting pSet = new publishSetting(pageData, fileName);
          if(DialogResult.OK == pSet.ShowDialog())
          {
            index = lm.Index;
            pdCollection[index] = pageData;
            lvItem = new ListViewItem(pageData.Layout);
            lvItem.SubItems.Add(pageData.PageTitle);
            lvItem.SubItems.Add(fileName);
            lvItem.SubItems.Add(pageData.PageAuthor);
            lvItem.SubItems.Add(pageData.PageSubject);
            lvItem.SubItems.Add(pageData.Preview.FileName);
            lvItem.SubItems.Add(pageData.PageComments);
            listView1.Items.RemoveAt(index);
            listView1.Items.Insert(index, lvItem);
            Update();
          }
        }
      }  
    }
  }
}