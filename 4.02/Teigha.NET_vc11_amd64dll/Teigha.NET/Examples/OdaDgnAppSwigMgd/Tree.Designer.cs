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

namespace OdaDgnAppMgd
{
  partial class Tree
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
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.treeView = new System.Windows.Forms.TreeView();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.vectorizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.zoomExtentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.listView = new System.Windows.Forms.ListView();
      this.Field = new System.Windows.Forms.ColumnHeader();
      this.Value = new System.Windows.Forms.ColumnHeader();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.treeView);
      this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.listView);
      this.splitContainer1.Size = new System.Drawing.Size(548, 401);
      this.splitContainer1.SplitterDistance = 247;
      this.splitContainer1.TabIndex = 0;
      // 
      // treeView
      // 
      this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView.Location = new System.Drawing.Point(0, 0);
      this.treeView.Name = "treeView";
      this.treeView.Size = new System.Drawing.Size(247, 401);
      this.treeView.TabIndex = 0;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(56, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vectorizeToolStripMenuItem,
            this.zoomExtentsToolStripMenuItem});
      this.toolsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // vectorizeToolStripMenuItem
      // 
      this.vectorizeToolStripMenuItem.Name = "vectorizeToolStripMenuItem";
      this.vectorizeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.vectorizeToolStripMenuItem.Text = "Vectorize";
      this.vectorizeToolStripMenuItem.Click += new System.EventHandler(this.vectorizeToolStripMenuItem_Click);
      // 
      // zoomExtentsToolStripMenuItem
      // 
      this.zoomExtentsToolStripMenuItem.Name = "zoomExtentsToolStripMenuItem";
      this.zoomExtentsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.zoomExtentsToolStripMenuItem.Text = "ZoomExtents";
      this.zoomExtentsToolStripMenuItem.Visible = false;
      this.zoomExtentsToolStripMenuItem.Click += new System.EventHandler(this.zoomExtentsToolStripMenuItem_Click);
      // 
      // listView
      // 
      this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Field,
            this.Value});
      this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listView.FullRowSelect = true;
      this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listView.HideSelection = false;
      this.listView.Location = new System.Drawing.Point(0, 0);
      this.listView.MultiSelect = false;
      this.listView.Name = "listView";
      this.listView.ShowGroups = false;
      this.listView.Size = new System.Drawing.Size(297, 401);
      this.listView.TabIndex = 0;
      this.listView.UseCompatibleStateImageBehavior = false;
      this.listView.View = System.Windows.Forms.View.Details;
      // 
      // Field
      // 
      this.Field.Text = "Field";
      this.Field.Width = 240;
      // 
      // Value
      // 
      this.Value.Text = "Value";
      this.Value.Width = 240;
      // 
      // Tree
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(548, 401);
      this.Controls.Add(this.splitContainer1);
      this.Name = "Tree";
      this.Text = "Tree view";
      this.Activated += new System.EventHandler(this.Tree_Activated);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.TreeView treeView;
    private System.Windows.Forms.ListView listView;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ColumnHeader Field;
    private System.Windows.Forms.ColumnHeader Value;
    private System.Windows.Forms.ToolStripMenuItem vectorizeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem zoomExtentsToolStripMenuItem;
  }
}