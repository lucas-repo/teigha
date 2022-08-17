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
using Teigha.DatabaseServices;


namespace OdViewExMgd
{
  public partial class Layers : Form
  {
    Database databaseInternal;
    Teigha.GraphicsSystem.LayoutHelperDevice hDevice;
    public Layers(Database database, Teigha.GraphicsSystem.LayoutHelperDevice helperDevice)
    {
      InitializeComponent();

      databaseInternal = database;
      hDevice = helperDevice;
      using (Transaction ta = database.TransactionManager.StartTransaction())
      {
        using (LayerTable layerTable = (LayerTable)ta.GetObject(database.LayerTableId, OpenMode.ForWrite, false))
        {
          foreach (ObjectId id in layerTable)
          {
            using (LayerTableRecord ltRecord = (LayerTableRecord)ta.GetObject(id, OpenMode.ForRead))
            {
              ListViewItem lvItem = new ListViewItem(ltRecord.Name);
              lvItem.SubItems.Add((!ltRecord.IsOff).ToString());
              lvItem.SubItems.Add(ltRecord.IsFrozen.ToString());
              lvItem.SubItems.Add(ltRecord.IsLocked.ToString());
              lvItem.SubItems.Add(ltRecord.Color.ToString());

              listView1.Items.Add(lvItem);
            }
          }//foreach (ObjectId id in layerTable)
        }//using (LayerTable layerTable = (LayerTable)ta.GetObject(database.LayerTableId, OpenMode.ForWrite, false))
        ta.Abort();
      }//using (Transaction ta = database.TransactionManager.StartTransaction())
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      int iCounter = 0;
      using (Transaction ta = databaseInternal.TransactionManager.StartTransaction())
      {
        using (LayerTable layerTable = (LayerTable)ta.GetObject(databaseInternal.LayerTableId, OpenMode.ForWrite, false))
        {
          foreach (ObjectId id in layerTable)
          {
            using (LayerTableRecord ltRecord = (LayerTableRecord)ta.GetObject(id, OpenMode.ForWrite))
            {
              ListViewItem item = listView1.Items[iCounter];
              ltRecord.IsOff    = !bool.Parse(item.SubItems[1].Text);
              ltRecord.IsFrozen = bool.Parse(item.SubItems[2].Text);
              ltRecord.IsLocked = bool.Parse(item.SubItems[3].Text);
              iCounter++;
            }
          }//foreach (ObjectId id in layerTable)
        }//using (LayerTable layerTable = (LayerTable)ta.GetObject(database.LayerTableId, OpenMode.ForWrite, false))
        ta.Commit();
      }//using (Transaction ta = database.TransactionManager.StartTransaction())
      databaseInternal = null;
      hDevice.Invalidate();
      hDevice.Update();
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      databaseInternal = null;
      Close();
    }

    ListViewItem.ListViewSubItem SelectedLSI;
    private void listView1_MouseUp(object sender, MouseEventArgs e)
    {
      ListViewHitTestInfo hitTest = listView1.HitTest(e.X, e.Y);
      SelectedLSI = hitTest.SubItem;
      int iIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);

      if (iIndex > 0 && iIndex < 4)
      {
        bool bTmp = bool.Parse(SelectedLSI.Text);
        bTmp = !bTmp;
        SelectedLSI.Text = bTmp.ToString();
      }
    }
  }
}
