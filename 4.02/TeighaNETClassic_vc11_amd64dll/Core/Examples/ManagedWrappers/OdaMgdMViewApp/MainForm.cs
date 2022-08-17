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
using Teigha.DatabaseServices;
using Teigha.GraphicsSystem;
using Teigha.Runtime;
using Teigha.GraphicsInterface;
using Teigha.Geometry;

namespace OdaMgdMViewApp
{
  public partial class MainForm : Form
  {
    Teigha.Runtime.Services TSrv;

    public MainForm()
    {
      TSrv = new Teigha.Runtime.Services();
      SystemObjects.DynamicLinker.LoadApp("GripPoints", false, false);
      SystemObjects.DynamicLinker.LoadApp("PlotSettingsValidator", false, false);
      InitializeComponent();
      menuUpdate(0);

      HostApplicationServices.Current = new HostAppServ();
      Environment.SetEnvironmentVariable("DDPLOTSTYLEPATHS", ((HostAppServ)HostApplicationServices.Current).FindConfigPath(String.Format("PrinterStyleSheetDir")));
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      TSrv.Dispose();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      openFileDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf";
      if (DialogResult.OK == openFileDialog.ShowDialog(this))
      {
        ChildForm newForm = new ChildForm(openFileDialog.FileName, false);
        newForm.MdiParent = this;
        newForm.WindowState = FormWindowState.Maximized;
        newForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChildForm_FormClosed);
        newForm.Show();
        menuUpdate(0);
      }
    }

    private void partialOpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      openFileDialog.Filter = "DWG files|*.dwg";
      if (DialogResult.OK == openFileDialog.ShowDialog(this))
      {
        ChildForm newForm = new ChildForm(openFileDialog.FileName, true);
        newForm.MdiParent = this;
        newForm.WindowState = FormWindowState.Maximized;
        newForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChildForm_FormClosed);
        newForm.Show();
        menuUpdate(0);
      }
    }

    private void menuUpdate(int iCorrecActiveChaild)
    {
      bool bTmp = (this.MdiChildren.Length - iCorrecActiveChaild) > 0;
      toolStripDrawStyle.Enabled = bTmp;
      toolStripDrawStyle.Visible = bTmp;
    }

    private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      menuUpdate(1);
    }

    private void ZoomExtents_Click(object sender, EventArgs e)
    {
      ChildForm cf = (ChildForm)this.ActiveMdiChild;
      if (cf != null)
        cf.ZoomExtents();
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChildForm cf = (ChildForm)this.ActiveMdiChild;
      if (cf != null)
        cf.Close();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}