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
namespace OdaDwgAppMgd
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
        Teigha.Core.Globals.odgsUninitialize();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Teigha.Core.Globals.odrxUninitialize();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
        this.ExportSVG = new System.Windows.Forms.ToolStripMenuItem();
        this.ExportPDF = new System.Windows.Forms.ToolStripMenuItem();
        this.ExportDWF = new System.Windows.Forms.ToolStripMenuItem();
        this.ImportDWF = new System.Windows.Forms.ToolStripMenuItem();
        this.ImportDGN = new System.Windows.Forms.ToolStripMenuItem();
        this.importDWF2 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.renderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.snapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.zoomExtents = new System.Windows.Forms.ToolStripMenuItem();
        this.undoTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.appendLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.undoLineAddingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.undovariant2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.openDwgDialog = new System.Windows.Forms.OpenFileDialog();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.menuStrip1.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem1,
            this.undoTestToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(431, 24);
        this.menuStrip1.TabIndex = 1;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem1
        // 
        this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.toolStripSeparator4,
            this.ExportSVG,
            this.ExportPDF,
            this.ExportDWF,
            this.ImportDWF,
            this.ImportDGN,
            this.importDWF2,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem1});
        this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
        this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
        this.fileToolStripMenuItem1.Text = "&File";
        // 
        // openToolStripMenuItem1
        // 
        this.openToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem1.Image")));
        this.openToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
        this.openToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
        this.openToolStripMenuItem1.Text = "&Open";
        this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
        this.toolStripSeparator1.Visible = false;
        // 
        // printToolStripMenuItem
        // 
        this.printToolStripMenuItem.Enabled = false;
        this.printToolStripMenuItem.Name = "printToolStripMenuItem";
        this.printToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
        this.printToolStripMenuItem.Text = "Print";
        this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
        // 
        // toolStripSeparator4
        // 
        this.toolStripSeparator4.Name = "toolStripSeparator4";
        this.toolStripSeparator4.Size = new System.Drawing.Size(146, 6);
        // 
        // ExportSVG
        // 
        this.ExportSVG.Name = "ExportSVG";
        this.ExportSVG.Size = new System.Drawing.Size(149, 22);
        this.ExportSVG.Text = "Export to SVG";
        this.ExportSVG.Visible = false;
        this.ExportSVG.Click += new System.EventHandler(this.ExportSVG_Click);
        // 
        // ExportPDF
        // 
        this.ExportPDF.Name = "ExportPDF";
        this.ExportPDF.Size = new System.Drawing.Size(149, 22);
        this.ExportPDF.Text = "Export to PDF";
        this.ExportPDF.Visible = false;
        this.ExportPDF.Click += new System.EventHandler(this.ExportPDF_Click);
        // 
        // ExportDWF
        // 
        this.ExportDWF.Name = "ExportDWF";
        this.ExportDWF.Size = new System.Drawing.Size(149, 22);
        this.ExportDWF.Text = "Export to DWF";
        this.ExportDWF.Visible = false;
        this.ExportDWF.Click += new System.EventHandler(this.ExportDWF_Click);
        // 
        // ImportDWF
        // 
        this.ImportDWF.Name = "ImportDWF";
        this.ImportDWF.Size = new System.Drawing.Size(149, 22);
        this.ImportDWF.Text = "Import DWF";
        this.ImportDWF.Visible = false;
        this.ImportDWF.Click += new System.EventHandler(this.ImportDWF_Click);
        // 
        // ImportDGN
        // 
        this.ImportDGN.Name = "ImportDGN";
        this.ImportDGN.Size = new System.Drawing.Size(149, 22);
        this.ImportDGN.Text = "Import DGN";
        this.ImportDGN.Click += new System.EventHandler(this.ImportDGN_Click);
        // 
        // importDWF2
        // 
        this.importDWF2.Name = "importDWF2";
        this.importDWF2.Size = new System.Drawing.Size(149, 22);
        this.importDWF2.Text = "Import DWF 2";
        this.importDWF2.Visible = false;
        this.importDWF2.Click += new System.EventHandler(this.importDWF2_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(146, 6);
        this.toolStripSeparator3.Click += new System.EventHandler(this.toolStripSeparator3_Click);
        // 
        // exitToolStripMenuItem1
        // 
        this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
        this.exitToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
        this.exitToolStripMenuItem1.Text = "E&xit";
        // 
        // viewToolStripMenuItem
        // 
        this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderToolStripMenuItem});
        this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
        this.viewToolStripMenuItem.Text = "View";
        this.viewToolStripMenuItem.Visible = false;
        // 
        // renderToolStripMenuItem
        // 
        this.renderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapshotToolStripMenuItem});
        this.renderToolStripMenuItem.Name = "renderToolStripMenuItem";
        this.renderToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
        this.renderToolStripMenuItem.Text = "Render";
        // 
        // snapshotToolStripMenuItem
        // 
        this.snapshotToolStripMenuItem.Enabled = false;
        this.snapshotToolStripMenuItem.Name = "snapshotToolStripMenuItem";
        this.snapshotToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
        this.snapshotToolStripMenuItem.Text = "Snapshot";
        this.snapshotToolStripMenuItem.Click += new System.EventHandler(this.snapshotToolStripMenuItem_Click);
        // 
        // toolsToolStripMenuItem1
        // 
        this.toolsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomExtents});
        this.toolsToolStripMenuItem1.Name = "toolsToolStripMenuItem1";
        this.toolsToolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
        this.toolsToolStripMenuItem1.Text = "&Tools";
        // 
        // zoomExtents
        // 
        this.zoomExtents.Name = "zoomExtents";
        this.zoomExtents.Size = new System.Drawing.Size(146, 22);
        this.zoomExtents.Text = "Zoom extents";
        this.zoomExtents.Visible = false;
        this.zoomExtents.Click += new System.EventHandler(this.zoomExtents_Click);
        // 
        // undoTestToolStripMenuItem
        // 
        this.undoTestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appendLinesToolStripMenuItem,
            this.undoLineAddingToolStripMenuItem,
            this.undovariant2ToolStripMenuItem,
            this.redoToolStripMenuItem});
        this.undoTestToolStripMenuItem.Name = "undoTestToolStripMenuItem";
        this.undoTestToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
        this.undoTestToolStripMenuItem.Text = "Undo test";
        // 
        // appendLinesToolStripMenuItem
        // 
        this.appendLinesToolStripMenuItem.Name = "appendLinesToolStripMenuItem";
        this.appendLinesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
        this.appendLinesToolStripMenuItem.Text = "Add 2 lines";
        this.appendLinesToolStripMenuItem.Click += new System.EventHandler(this.appendLinesToolStripMenuItem_Click);
        // 
        // undoLineAddingToolStripMenuItem
        // 
        this.undoLineAddingToolStripMenuItem.Name = "undoLineAddingToolStripMenuItem";
        this.undoLineAddingToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
        this.undoLineAddingToolStripMenuItem.Text = "Undo (variant 1)";
        this.undoLineAddingToolStripMenuItem.Click += new System.EventHandler(this.undovariant1ToolStripMenuItem_Click);
        // 
        // undovariant2ToolStripMenuItem
        // 
        this.undovariant2ToolStripMenuItem.Name = "undovariant2ToolStripMenuItem";
        this.undovariant2ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
        this.undovariant2ToolStripMenuItem.Text = "Undo (variant 2)";
        this.undovariant2ToolStripMenuItem.Click += new System.EventHandler(this.undovariant2ToolStripMenuItem_Click);
        // 
        // openDwgDialog
        // 
        this.openDwgDialog.FileName = "openDwnDialog";
        this.openDwgDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf";
        // 
        // statusStrip1
        // 
        this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
        this.statusStrip1.Location = new System.Drawing.Point(0, 298);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Size = new System.Drawing.Size(431, 22);
        this.statusStrip1.TabIndex = 3;
        this.statusStrip1.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
        this.toolStripStatusLabel1.Text = "Ready";
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
        // 
        // redoToolStripMenuItem
        // 
        this.redoToolStripMenuItem.Enabled = false;
        this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
        this.redoToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
        this.redoToolStripMenuItem.Text = "Redo";
        this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(431, 320);
        this.Controls.Add(this.statusStrip1);
        this.Controls.Add(this.menuStrip1);
        this.IsMdiContainer = true;
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "Form1";
        this.Text = "Form1";
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.statusStrip1.ResumeLayout(false);
        this.statusStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.OpenFileDialog openDwgDialog;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem ExportSVG;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem ExportPDF;
    private System.Windows.Forms.ToolStripMenuItem zoomExtents;
    private System.Windows.Forms.ToolStripMenuItem ExportDWF;
    private System.Windows.Forms.ToolStripMenuItem ImportDWF;
    private System.Windows.Forms.ToolStripMenuItem ImportDGN;
    private System.Windows.Forms.ToolStripMenuItem importDWF2;
    //private System.Windows.Forms.ToolStripMenuItem Print;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem renderToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem snapshotToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoTestToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem appendLinesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoLineAddingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undovariant2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
  }
}

