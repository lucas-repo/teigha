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
    partial class Export2STLDlg
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
            this.Browse = new System.Windows.Forms.Button();
            this.StlFilePathString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ElementHandle = new System.Windows.Forms.NumericUpDown();
            this.BinaryFormat = new System.Windows.Forms.CheckBox();
            this.Export = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ElementHandle)).BeginInit();
            this.SuspendLayout();
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(527, 27);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 3;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // StlFilePathString
            // 
            this.StlFilePathString.Location = new System.Drawing.Point(12, 29);
            this.StlFilePathString.Name = "StlFilePathString";
            this.StlFilePathString.ReadOnly = true;
            this.StlFilePathString.Size = new System.Drawing.Size(509, 20);
            this.StlFilePathString.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Element handle (hex) :";
            // 
            // ElementHandle
            // 
            this.ElementHandle.Hexadecimal = true;
            this.ElementHandle.Location = new System.Drawing.Point(129, 60);
            this.ElementHandle.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.ElementHandle.Name = "ElementHandle";
            this.ElementHandle.Size = new System.Drawing.Size(105, 20);
            this.ElementHandle.TabIndex = 7;
            // 
            // BinaryFormat
            // 
            this.BinaryFormat.AutoSize = true;
            this.BinaryFormat.Checked = true;
            this.BinaryFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BinaryFormat.Location = new System.Drawing.Point(333, 62);
            this.BinaryFormat.Name = "BinaryFormat";
            this.BinaryFormat.Size = new System.Drawing.Size(87, 17);
            this.BinaryFormat.TabIndex = 8;
            this.BinaryFormat.Text = "Binary format";
            this.BinaryFormat.UseVisualStyleBackColor = true;
            // 
            // Export
            // 
            this.Export.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Export.Enabled = false;
            this.Export.Location = new System.Drawing.Point(161, 92);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(75, 23);
            this.Export.TabIndex = 9;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(332, 93);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 10;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Export2STLDlg
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 124);
            this.ControlBox = false;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.BinaryFormat);
            this.Controls.Add(this.ElementHandle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.StlFilePathString);
            this.Name = "Export2STLDlg";
            this.Text = "STL export dialog";
            ((System.ComponentModel.ISupportInitialize)(this.ElementHandle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.TextBox StlFilePathString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ElementHandle;
        private System.Windows.Forms.CheckBox BinaryFormat;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Cancel;
    }
}