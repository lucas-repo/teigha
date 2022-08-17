'/////////////////////////////////////////////////////////////////////////////// 
'// Copyright (C) 2002-2016, Open Design Alliance (the "Alliance"). 
'// All rights reserved. 
'// 
'// This software and its documentation and related materials are owned by 
'// the Alliance. The software may only be incorporated into application 
'// programs owned by members of the Alliance, subject to a signed 
'// Membership Agreement and Supplemental Software License Agreement with the
'// Alliance. The structure and organization of this software are the valuable  
'// trade secrets of the Alliance and its suppliers. The software is also 
'// protected by copyright law and international treaty provisions. Application  
'// programs incorporating this software must include the following statement 
'// with their copyright notices:
'//   
'//   This application incorporates Teigha(R) software pursuant to a license 
'//   agreement with Open Design Alliance.
'//   Teigha(R) Copyright (C) 2002-2016 by Open Design Alliance. 
'//   All rights reserved.
'//
'// By use of this software, its documentation or related materials, you 
'// acknowledge and accept the above terms.
'///////////////////////////////////////////////////////////////////////////////
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
    Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.EportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.InsertBlockToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.openFileDialog = New System.Windows.Forms.OpenFileDialog
    Me.DrawingPanel = New System.Windows.Forms.Panel
    Me.saveAsFileDialog = New System.Windows.Forms.SaveFileDialog
    Me.RegenerateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
    Me.MenuStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'MenuStrip1
    '
    Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EportToolStripMenuItem, Me.RegenerateToolStripMenuItem})
    Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
    Me.MenuStrip1.Name = "MenuStrip1"
    Me.MenuStrip1.Size = New System.Drawing.Size(564, 24)
    Me.MenuStrip1.TabIndex = 0
    Me.MenuStrip1.Text = "MenuStrip1"
    '
    'FileToolStripMenuItem
    '
    Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.ExportToolStripMenuItem, Me.SaveAsToolStripMenuItem})
    Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
    Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
    Me.FileToolStripMenuItem.Text = "File"
    '
    'OpenToolStripMenuItem
    '
    Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
    Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
    Me.OpenToolStripMenuItem.Text = "Open"
    '
    'ExportToolStripMenuItem
    '
    Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
    Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
    Me.ExportToolStripMenuItem.Text = "Export"
    '
    'SaveAsToolStripMenuItem
    '
    Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
    Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
    Me.SaveAsToolStripMenuItem.Text = "Save As"
    '
    'EportToolStripMenuItem
    '
    Me.EportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InsertBlockToolStripMenuItem})
    Me.EportToolStripMenuItem.Name = "EportToolStripMenuItem"
    Me.EportToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
    Me.EportToolStripMenuItem.Text = "Edit"
    '
    'InsertBlockToolStripMenuItem
    '
    Me.InsertBlockToolStripMenuItem.Name = "InsertBlockToolStripMenuItem"
    Me.InsertBlockToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
    Me.InsertBlockToolStripMenuItem.Text = "Insert Block"
    '
    'openFileDialog
    '
    Me.openFileDialog.DefaultExt = "DWG"
    Me.openFileDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf"
    '
    'DrawingPanel
    '
    Me.DrawingPanel.Location = New System.Drawing.Point(0, 27)
    Me.DrawingPanel.Name = "DrawingPanel"
    Me.DrawingPanel.Size = New System.Drawing.Size(564, 345)
    Me.DrawingPanel.TabIndex = 1
    '
    'saveAsFileDialog
    '
    Me.saveAsFileDialog.DefaultExt = "DWG"
    Me.saveAsFileDialog.Filter = "DWG files|*.dwg|DXF files|*.dxf"
    '
    'RegenerateToolStripMenuItem
    '
    Me.RegenerateToolStripMenuItem.Name = "RegenerateToolStripMenuItem"
    Me.RegenerateToolStripMenuItem.Size = New System.Drawing.Size(78, 20)
    Me.RegenerateToolStripMenuItem.Text = "Regenerate"
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(564, 372)
    Me.Controls.Add(Me.DrawingPanel)
    Me.Controls.Add(Me.MenuStrip1)
    Me.MainMenuStrip = Me.MenuStrip1
    Me.Name = "Form1"
    Me.Text = "VBView"
    Me.MenuStrip1.ResumeLayout(False)
    Me.MenuStrip1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
  Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Private WithEvents openFileDialog As System.Windows.Forms.OpenFileDialog
  Friend WithEvents EportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents DrawingPanel As System.Windows.Forms.Panel
  Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents InsertBlockToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Private WithEvents saveAsFileDialog As System.Windows.Forms.SaveFileDialog
  Friend WithEvents RegenerateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
