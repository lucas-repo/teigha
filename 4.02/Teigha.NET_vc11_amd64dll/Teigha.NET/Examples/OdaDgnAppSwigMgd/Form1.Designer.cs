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
namespace OdaDgnAppMgd
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
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
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
        this.ExportSTL = new System.Windows.Forms.ToolStripMenuItem();
        this.ExportSVG = new System.Windows.Forms.ToolStripMenuItem();
        this.ExportPDF = new System.Windows.Forms.ToolStripMenuItem();
        this.ExportDWF = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.zoomExtents = new System.Windows.Forms.ToolStripMenuItem();
        this.openDgnDialog = new System.Windows.Forms.OpenFileDialog();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        this.menuStrip1.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.toolsToolStripMenuItem1});
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
            this.ExportSTL,
            this.ExportSVG,
            this.ExportPDF,
            this.ExportDWF,
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
        this.openToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
        this.openToolStripMenuItem1.Text = "&Open";
        this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
        this.toolStripSeparator1.Visible = false;
        // 
        // ExportSTL
        // 
        this.ExportSTL.Name = "ExportSTL";
        this.ExportSTL.Size = new System.Drawing.Size(152, 22);
        this.ExportSTL.Text = "Export to STL";
        this.ExportSTL.Visible = false;
        this.ExportSTL.Click += new System.EventHandler(this.ExportSTL_Click);
        // 
        // ExportSVG
        // 
        this.ExportSVG.Name = "ExportSVG";
        this.ExportSVG.Size = new System.Drawing.Size(152, 22);
        this.ExportSVG.Text = "Export to SVG";
        this.ExportSVG.Visible = false;
        this.ExportSVG.Click += new System.EventHandler(this.ExportSVG_Click);
        // 
        // ExportPDF
        // 
        this.ExportPDF.Name = "ExportPDF";
        this.ExportPDF.Size = new System.Drawing.Size(152, 22);
        this.ExportPDF.Text = "Export to PDF";
        this.ExportPDF.Visible = false;
        this.ExportPDF.Click += new System.EventHandler(this.ExportPDF_Click);
        // 
        // ExportDWF
        // 
        this.ExportDWF.Name = "ExportDWF";
        this.ExportDWF.Size = new System.Drawing.Size(152, 22);
        this.ExportDWF.Text = "Export to DWF";
        this.ExportDWF.Visible = false;
        this.ExportDWF.Click += new System.EventHandler(this.ExportDWF_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
        // 
        // exitToolStripMenuItem1
        // 
        this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
        this.exitToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
        this.exitToolStripMenuItem1.Text = "E&xit";
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
        // openDgnDialog
        // 
        this.openDgnDialog.FileName = "openDgnDialog";
        this.openDgnDialog.Filter = "DGN files|*.dgn";
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
        this.Text = "OdaDgnAppSwig";
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.statusStrip1.ResumeLayout(false);
        this.statusStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.OpenFileDialog openDgnDialog;
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
    private System.Windows.Forms.ToolStripMenuItem ExportSTL;
  }
}

