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

namespace OdaDwgAppMgd
{
  partial class Export2PdfForm
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
      this.PdfFilePathString = new System.Windows.Forms.TextBox();
      this.Browse = new System.Windows.Forms.Button();
      this.Properties = new System.Windows.Forms.GroupBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.ExportHyperlinks1 = new System.Windows.Forms.CheckBox();
      this.CreatorString = new System.Windows.Forms.TextBox();
      this.ProducerString = new System.Windows.Forms.TextBox();
      this.SubjectString = new System.Windows.Forms.TextBox();
      this.AuthorString = new System.Windows.Forms.TextBox();
      this.TitleString = new System.Windows.Forms.TextBox();
      this.KeywordsString = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.All = new System.Windows.Forms.RadioButton();
      this.Active = new System.Windows.Forms.RadioButton();
      this.PaperHeightString = new System.Windows.Forms.TextBox();
      this.PaperWidthString = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.Encoded_small = new System.Windows.Forms.CheckBox();
      this.Off_layers = new System.Windows.Forms.CheckBox();
      this.Layer_support = new System.Windows.Forms.CheckBox();
      this.Hidden_line = new System.Windows.Forms.CheckBox();
      this.Zoom_extents = new System.Windows.Forms.CheckBox();
      this.Enable_optimization = new System.Windows.Forms.CheckBox();
      this.SHX_text = new System.Windows.Forms.CheckBox();
      this.True_type = new System.Windows.Forms.CheckBox();
      this.Embedded_fonts = new System.Windows.Forms.CheckBox();
      this.Export = new System.Windows.Forms.Button();
      this.Cancel = new System.Windows.Forms.Button();
      this.Properties.SuspendLayout();
      this.SuspendLayout();
      // 
      // PdfFilePathString
      // 
      this.PdfFilePathString.Location = new System.Drawing.Point(16, 30);
      this.PdfFilePathString.Margin = new System.Windows.Forms.Padding(4);
      this.PdfFilePathString.Name = "PdfFilePathString";
      this.PdfFilePathString.ReadOnly = true;
      this.PdfFilePathString.Size = new System.Drawing.Size(677, 22);
      this.PdfFilePathString.TabIndex = 0;
      // 
      // Browse
      // 
      this.Browse.Location = new System.Drawing.Point(703, 27);
      this.Browse.Margin = new System.Windows.Forms.Padding(4);
      this.Browse.Name = "Browse";
      this.Browse.Size = new System.Drawing.Size(100, 28);
      this.Browse.TabIndex = 1;
      this.Browse.Text = "Browse";
      this.Browse.UseVisualStyleBackColor = true;
      this.Browse.Click += new System.EventHandler(this.Browse_Click);
      // 
      // Properties
      // 
      this.Properties.Controls.Add(this.textBox2);
      this.Properties.Controls.Add(this.label11);
      this.Properties.Controls.Add(this.textBox1);
      this.Properties.Controls.Add(this.label10);
      this.Properties.Controls.Add(this.ExportHyperlinks1);
      this.Properties.Controls.Add(this.CreatorString);
      this.Properties.Controls.Add(this.ProducerString);
      this.Properties.Controls.Add(this.SubjectString);
      this.Properties.Controls.Add(this.AuthorString);
      this.Properties.Controls.Add(this.TitleString);
      this.Properties.Controls.Add(this.KeywordsString);
      this.Properties.Controls.Add(this.label9);
      this.Properties.Controls.Add(this.label8);
      this.Properties.Controls.Add(this.label7);
      this.Properties.Controls.Add(this.label6);
      this.Properties.Controls.Add(this.label5);
      this.Properties.Controls.Add(this.label4);
      this.Properties.Controls.Add(this.All);
      this.Properties.Controls.Add(this.Active);
      this.Properties.Controls.Add(this.PaperHeightString);
      this.Properties.Controls.Add(this.PaperWidthString);
      this.Properties.Controls.Add(this.label3);
      this.Properties.Controls.Add(this.label2);
      this.Properties.Controls.Add(this.label1);
      this.Properties.Controls.Add(this.Encoded_small);
      this.Properties.Controls.Add(this.Off_layers);
      this.Properties.Controls.Add(this.Layer_support);
      this.Properties.Controls.Add(this.Hidden_line);
      this.Properties.Controls.Add(this.Zoom_extents);
      this.Properties.Controls.Add(this.Enable_optimization);
      this.Properties.Controls.Add(this.SHX_text);
      this.Properties.Controls.Add(this.True_type);
      this.Properties.Controls.Add(this.Embedded_fonts);
      this.Properties.Location = new System.Drawing.Point(16, 62);
      this.Properties.Margin = new System.Windows.Forms.Padding(4);
      this.Properties.Name = "Properties";
      this.Properties.Padding = new System.Windows.Forms.Padding(4);
      this.Properties.Size = new System.Drawing.Size(787, 522);
      this.Properties.TabIndex = 2;
      this.Properties.TabStop = false;
      this.Properties.Text = "Properties";
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(536, 141);
      this.textBox2.Margin = new System.Windows.Forms.Padding(4);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(132, 22);
      this.textBox2.TabIndex = 32;
      this.textBox2.Text = "600";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(395, 144);
      this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(116, 17);
      this.label11.TabIndex = 31;
      this.label11.Text = "Paper width, mm:";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(536, 110);
      this.textBox1.Margin = new System.Windows.Forms.Padding(4);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(132, 22);
      this.textBox1.TabIndex = 30;
      this.textBox1.Text = "150";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(395, 117);
      this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(125, 17);
      this.label10.TabIndex = 29;
      this.label10.Text = "Hatch to BMP, dpi:";
      // 
      // ExportHyperlinks1
      // 
      this.ExportHyperlinks1.AutoSize = true;
      this.ExportHyperlinks1.Location = new System.Drawing.Point(8, 283);
      this.ExportHyperlinks1.Margin = new System.Windows.Forms.Padding(4);
      this.ExportHyperlinks1.Name = "ExportHyperlinks1";
      this.ExportHyperlinks1.Size = new System.Drawing.Size(140, 21);
      this.ExportHyperlinks1.TabIndex = 28;
      this.ExportHyperlinks1.Text = "Export Hyperlinks";
      this.ExportHyperlinks1.UseVisualStyleBackColor = true;
      // 
      // CreatorString
      // 
      this.CreatorString.Location = new System.Drawing.Point(87, 452);
      this.CreatorString.Margin = new System.Windows.Forms.Padding(4);
      this.CreatorString.Name = "CreatorString";
      this.CreatorString.Size = new System.Drawing.Size(691, 22);
      this.CreatorString.TabIndex = 27;
      // 
      // ProducerString
      // 
      this.ProducerString.Location = new System.Drawing.Point(87, 482);
      this.ProducerString.Margin = new System.Windows.Forms.Padding(4);
      this.ProducerString.Name = "ProducerString";
      this.ProducerString.Size = new System.Drawing.Size(691, 22);
      this.ProducerString.TabIndex = 26;
      // 
      // SubjectString
      // 
      this.SubjectString.Location = new System.Drawing.Point(87, 390);
      this.SubjectString.Margin = new System.Windows.Forms.Padding(4);
      this.SubjectString.Name = "SubjectString";
      this.SubjectString.Size = new System.Drawing.Size(691, 22);
      this.SubjectString.TabIndex = 25;
      // 
      // AuthorString
      // 
      this.AuthorString.Location = new System.Drawing.Point(87, 359);
      this.AuthorString.Margin = new System.Windows.Forms.Padding(4);
      this.AuthorString.Name = "AuthorString";
      this.AuthorString.Size = new System.Drawing.Size(691, 22);
      this.AuthorString.TabIndex = 24;
      // 
      // TitleString
      // 
      this.TitleString.Location = new System.Drawing.Point(87, 329);
      this.TitleString.Margin = new System.Windows.Forms.Padding(4);
      this.TitleString.Name = "TitleString";
      this.TitleString.Size = new System.Drawing.Size(691, 22);
      this.TitleString.TabIndex = 23;
      // 
      // KeywordsString
      // 
      this.KeywordsString.Location = new System.Drawing.Point(87, 421);
      this.KeywordsString.Margin = new System.Windows.Forms.Padding(4);
      this.KeywordsString.Name = "KeywordsString";
      this.KeywordsString.Size = new System.Drawing.Size(691, 22);
      this.KeywordsString.TabIndex = 22;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(4, 486);
      this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(70, 17);
      this.label9.TabIndex = 21;
      this.label9.Text = "Producer:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(4, 455);
      this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(59, 17);
      this.label8.TabIndex = 20;
      this.label8.Text = "Creator:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(4, 425);
      this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(73, 17);
      this.label7.TabIndex = 19;
      this.label7.Text = "Keywords:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(4, 394);
      this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(59, 17);
      this.label6.TabIndex = 18;
      this.label6.Text = "Subject:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(4, 363);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(54, 17);
      this.label5.TabIndex = 17;
      this.label5.Text = "Author:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(4, 332);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(39, 17);
      this.label4.TabIndex = 16;
      this.label4.Text = "Title:";
      // 
      // All
      // 
      this.All.AutoSize = true;
      this.All.Location = new System.Drawing.Point(520, 227);
      this.All.Margin = new System.Windows.Forms.Padding(4);
      this.All.Name = "All";
      this.All.Size = new System.Drawing.Size(44, 21);
      this.All.TabIndex = 15;
      this.All.Text = "All";
      this.All.UseVisualStyleBackColor = true;
      // 
      // Active
      // 
      this.Active.AutoSize = true;
      this.Active.Checked = true;
      this.Active.Location = new System.Drawing.Point(520, 196);
      this.Active.Margin = new System.Windows.Forms.Padding(4);
      this.Active.Name = "Active";
      this.Active.Size = new System.Drawing.Size(67, 21);
      this.Active.TabIndex = 14;
      this.Active.TabStop = true;
      this.Active.Text = "Active";
      this.Active.UseVisualStyleBackColor = true;
      // 
      // PaperHeightString
      // 
      this.PaperHeightString.Location = new System.Drawing.Point(537, 84);
      this.PaperHeightString.Margin = new System.Windows.Forms.Padding(4);
      this.PaperHeightString.Name = "PaperHeightString";
      this.PaperHeightString.Size = new System.Drawing.Size(132, 22);
      this.PaperHeightString.TabIndex = 13;
      this.PaperHeightString.Text = "297";
      // 
      // PaperWidthString
      // 
      this.PaperWidthString.Location = new System.Drawing.Point(537, 52);
      this.PaperWidthString.Margin = new System.Windows.Forms.Padding(4);
      this.PaperWidthString.Name = "PaperWidthString";
      this.PaperWidthString.Size = new System.Drawing.Size(132, 22);
      this.PaperWidthString.TabIndex = 12;
      this.PaperWidthString.Text = "210";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(395, 198);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(121, 17);
      this.label3.TabIndex = 11;
      this.label3.Text = "Layouts to export:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(396, 89);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(123, 17);
      this.label2.TabIndex = 10;
      this.label2.Text = "Paper height, mm:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(396, 55);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(116, 17);
      this.label1.TabIndex = 9;
      this.label1.Text = "Paper width, mm:";
      // 
      // Encoded_small
      // 
      this.Encoded_small.AutoSize = true;
      this.Encoded_small.Checked = true;
      this.Encoded_small.CheckState = System.Windows.Forms.CheckState.Checked;
      this.Encoded_small.Location = new System.Drawing.Point(399, 283);
      this.Encoded_small.Margin = new System.Windows.Forms.Padding(4);
      this.Encoded_small.Name = "Encoded_small";
      this.Encoded_small.Size = new System.Drawing.Size(161, 21);
      this.Encoded_small.TabIndex = 8;
      this.Encoded_small.Text = "Encoded (small size)";
      this.Encoded_small.UseVisualStyleBackColor = true;
      // 
      // Off_layers
      // 
      this.Off_layers.AutoSize = true;
      this.Off_layers.Enabled = false;
      this.Off_layers.Location = new System.Drawing.Point(21, 254);
      this.Off_layers.Margin = new System.Windows.Forms.Padding(4);
      this.Off_layers.Name = "Off_layers";
      this.Off_layers.Size = new System.Drawing.Size(159, 21);
      this.Off_layers.TabIndex = 7;
      this.Off_layers.Text = "Export Off layers too";
      this.Off_layers.UseVisualStyleBackColor = true;
      // 
      // Layer_support
      // 
      this.Layer_support.AutoSize = true;
      this.Layer_support.Location = new System.Drawing.Point(8, 225);
      this.Layer_support.Margin = new System.Windows.Forms.Padding(4);
      this.Layer_support.Name = "Layer_support";
      this.Layer_support.Size = new System.Drawing.Size(230, 21);
      this.Layer_support.TabIndex = 6;
      this.Layer_support.Text = "Enable layer support (pdf v 1.5)";
      this.Layer_support.UseVisualStyleBackColor = true;
      // 
      // Hidden_line
      // 
      this.Hidden_line.AutoSize = true;
      this.Hidden_line.Checked = true;
      this.Hidden_line.CheckState = System.Windows.Forms.CheckState.Checked;
      this.Hidden_line.Location = new System.Drawing.Point(8, 197);
      this.Hidden_line.Margin = new System.Windows.Forms.Padding(4);
      this.Hidden_line.Name = "Hidden_line";
      this.Hidden_line.Size = new System.Drawing.Size(244, 21);
      this.Hidden_line.TabIndex = 5;
      this.Hidden_line.Text = "Use hidden line removal algorithm";
      this.Hidden_line.UseVisualStyleBackColor = true;
      // 
      // Zoom_extents
      // 
      this.Zoom_extents.AutoSize = true;
      this.Zoom_extents.Checked = true;
      this.Zoom_extents.CheckState = System.Windows.Forms.CheckState.Checked;
      this.Zoom_extents.Location = new System.Drawing.Point(8, 169);
      this.Zoom_extents.Margin = new System.Windows.Forms.Padding(4);
      this.Zoom_extents.Name = "Zoom_extents";
      this.Zoom_extents.Size = new System.Drawing.Size(170, 21);
      this.Zoom_extents.TabIndex = 4;
      this.Zoom_extents.Text = "Zoom to extents mode";
      this.Zoom_extents.UseVisualStyleBackColor = true;
      // 
      // Enable_optimization
      // 
      this.Enable_optimization.AutoSize = true;
      this.Enable_optimization.Location = new System.Drawing.Point(8, 140);
      this.Enable_optimization.Margin = new System.Windows.Forms.Padding(4);
      this.Enable_optimization.Name = "Enable_optimization";
      this.Enable_optimization.Size = new System.Drawing.Size(260, 21);
      this.Enable_optimization.TabIndex = 3;
      this.Enable_optimization.Text = "Enable simple geometry optimization";
      this.Enable_optimization.UseVisualStyleBackColor = true;
      // 
      // SHX_text
      // 
      this.SHX_text.AutoSize = true;
      this.SHX_text.Checked = true;
      this.SHX_text.CheckState = System.Windows.Forms.CheckState.Checked;
      this.SHX_text.Location = new System.Drawing.Point(8, 112);
      this.SHX_text.Margin = new System.Windows.Forms.Padding(4);
      this.SHX_text.Name = "SHX_text";
      this.SHX_text.Size = new System.Drawing.Size(166, 21);
      this.SHX_text.TabIndex = 2;
      this.SHX_text.Text = "SHX text as geometry";
      this.SHX_text.UseVisualStyleBackColor = true;
      // 
      // True_type
      // 
      this.True_type.AutoSize = true;
      this.True_type.Checked = true;
      this.True_type.CheckState = System.Windows.Forms.CheckState.Checked;
      this.True_type.Location = new System.Drawing.Point(8, 84);
      this.True_type.Margin = new System.Windows.Forms.Padding(4);
      this.True_type.Name = "True_type";
      this.True_type.Size = new System.Drawing.Size(173, 21);
      this.True_type.TabIndex = 1;
      this.True_type.Text = "True type as geometry";
      this.True_type.UseVisualStyleBackColor = true;
      // 
      // Embedded_fonts
      // 
      this.Embedded_fonts.AutoSize = true;
      this.Embedded_fonts.Location = new System.Drawing.Point(8, 55);
      this.Embedded_fonts.Margin = new System.Windows.Forms.Padding(4);
      this.Embedded_fonts.Name = "Embedded_fonts";
      this.Embedded_fonts.Size = new System.Drawing.Size(133, 21);
      this.Embedded_fonts.TabIndex = 0;
      this.Embedded_fonts.Text = "Embedded fonts";
      this.Embedded_fonts.UseVisualStyleBackColor = true;
      // 
      // Export
      // 
      this.Export.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.Export.Enabled = false;
      this.Export.Location = new System.Drawing.Point(169, 598);
      this.Export.Margin = new System.Windows.Forms.Padding(4);
      this.Export.Name = "Export";
      this.Export.Size = new System.Drawing.Size(100, 28);
      this.Export.TabIndex = 3;
      this.Export.Text = "Export";
      this.Export.UseVisualStyleBackColor = true;
      this.Export.Click += new System.EventHandler(this.Export_Click);
      // 
      // Cancel
      // 
      this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Cancel.Location = new System.Drawing.Point(479, 598);
      this.Cancel.Margin = new System.Windows.Forms.Padding(4);
      this.Cancel.Name = "Cancel";
      this.Cancel.Size = new System.Drawing.Size(100, 28);
      this.Cancel.TabIndex = 4;
      this.Cancel.Text = "Cancel";
      this.Cancel.UseVisualStyleBackColor = true;
      // 
      // Export2PdfForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(825, 641);
      this.ControlBox = false;
      this.Controls.Add(this.Cancel);
      this.Controls.Add(this.Export);
      this.Controls.Add(this.Properties);
      this.Controls.Add(this.Browse);
      this.Controls.Add(this.PdfFilePathString);
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "Export2PdfForm";
      this.Text = "Export2PdfForm";
      this.Shown += new System.EventHandler(this.Export2PdfForm_Shown);
      this.Properties.ResumeLayout(false);
      this.Properties.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox PdfFilePathString;
    private System.Windows.Forms.Button Browse;
    private System.Windows.Forms.GroupBox Properties;
    private System.Windows.Forms.CheckBox Off_layers;
    private System.Windows.Forms.CheckBox Layer_support;
    private System.Windows.Forms.CheckBox Hidden_line;
    private System.Windows.Forms.CheckBox Zoom_extents;
    private System.Windows.Forms.CheckBox Enable_optimization;
    private System.Windows.Forms.CheckBox SHX_text;
    private System.Windows.Forms.CheckBox True_type;
    private System.Windows.Forms.CheckBox Embedded_fonts;
    private System.Windows.Forms.TextBox PaperHeightString;
    private System.Windows.Forms.TextBox PaperWidthString;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox Encoded_small;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.RadioButton All;
    private System.Windows.Forms.RadioButton Active;
    private System.Windows.Forms.TextBox CreatorString;
    private System.Windows.Forms.TextBox ProducerString;
    private System.Windows.Forms.TextBox SubjectString;
    private System.Windows.Forms.TextBox AuthorString;
    private System.Windows.Forms.TextBox TitleString;
    private System.Windows.Forms.TextBox KeywordsString;
    private System.Windows.Forms.Button Export;
    private System.Windows.Forms.Button Cancel;
    private System.Windows.Forms.CheckBox ExportHyperlinks1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label10;
  }
}