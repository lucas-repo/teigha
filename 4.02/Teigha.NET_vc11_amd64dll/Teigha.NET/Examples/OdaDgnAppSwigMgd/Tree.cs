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
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Teigha.Core;
using Teigha.TG;

namespace OdaDgnAppMgd
{
  public partial class Tree : Form
  {
    OdDgDatabase _db;
    public OdDgDatabase Database
    {
      get { return _db; }
    }
    public Tree(OdDgDatabase db)
    {
      _db = db;
      InitializeComponent();
      treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
    }

    void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Nodes.Count == 0)
        TreeFiller.explandItem(e.Node);
      listView.Items.Clear();
      listView.SuspendLayout();
      listView.UseWaitCursor = true;
      try
      {
        OdRxObject pObj = TreeFiller.getObject(e.Node);
        if (pObj != null)
        {
          ListFiller.dump(pObj);
        }
      }
      catch (OdError ex)
      {
        ListViewItem item = listView.Items.Add("OdError:");
        item.SubItems.Add(ex.description());
      }
      catch (Exception ex1)
      {
        ListViewItem item = listView.Items.Add("OdError:");
        item.SubItems.Add(ex1.Message);
      }
      listView.ResumeLayout(true);
      listView.UseWaitCursor = false;
      listView.Update();
    }

    DgnListFiller m_pListFiller = null;
    DgnListFiller ListFiller
    {
      get
      {
        if (m_pListFiller == null)
          m_pListFiller = new DgnListFiller(_db, listView);
        return m_pListFiller;
      }
    }
    DgnTreeFiller m_pTreeFiller = null;
    DgnTreeFiller TreeFiller
    {
      get
      {
        if (m_pTreeFiller == null)
          m_pTreeFiller = new DgnTreeFiller(_db, treeView);
        return m_pTreeFiller;
      }
    }
    public bool Find(TreeNode item, OdDgElementId id)
    {
      for (; item != null; item = item.NextNode)
      {
        OdDgElementId data = item.Tag as OdDgElementId;
        OdDbHandle h1 = id.getHandle();
        OdDbHandle h2 = data.getHandle();
        if ((UInt64)h1 == (UInt64)h2)
        {
          treeView.SelectedNode = item;
          return true;
        }
        if (item.Nodes.Count > 0)
        {
          item.Expand();
          if (Find(item.Nodes[0], id))
            return true;
          item.Collapse(true);
        }
      }
      return false;
    }
    void Find(OdDgElementId targetId)
    {
      Find(treeView.Nodes[0], targetId);
    }

    private void findToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FindObjectDlg fd = new FindObjectDlg();
      if (fd.ShowDialog() == DialogResult.OK)
      {
        // Calculating handle
        OdDbHandle han = new OdDbHandle(fd.textBox.Text);
        // Requesting ObjectId
        OdDgElementId targetId = _db.getElementId(han);
        if (targetId.isNull())
          MessageBox.Show(this, "Incorrect handle", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        else
          Find(targetId);
      }
    }
    public void FillTree()
    {
			treeView.Nodes.Clear();
      try
      {
        // Tables
        TreeFiller.addElement(_db.getLevelTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getLevelFilterTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getFontTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getTextStyleTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getDimStyleTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getMaterialTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getMultilineStyleTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getLineStyleTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getLineStyleDefTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getRegAppTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getViewGroupTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getNamedViewTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getModelTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getSharedCellDefinitionTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getTagDefinitionSetTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getColorTable(), TreeFiller.DbTreeItem);
        TreeFiller.addElement(_db.getElementTemplateTable(), TreeFiller.DbTreeItem);
				//TreeFiller.addElement(_db.getDisplayStyleTable(), TreeFiller.DbTreeItem);
				//TreeFiller.addElement(_db.getLinkSetTable(), TreeFiller.DbTreeItem);
				//TreeFiller.addElement(_db.getColorBookTable(), TreeFiller.DbTreeItem);
				TreeFiller.addElement(_db.getNonModelElementCollection(), TreeFiller.DbTreeItem);

        TreeFiller.DbTreeItem.Expand();
      }
      catch (OdError e)
      {
        MessageBox.Show(this, e.Message, "Error reading DB...");
        //theApp.reportError(_T(), e);
      }
    }
    List<VectorizeForm> _vectorizers = new List<VectorizeForm>();
    OdDgElementId _vectorizedModelId = null;
    private void vectorizeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_vectorizers.Count == 0)
      {
        OdDgViewGroup pViewGroup = _db.getActiveViewGroupId().openObject() as OdDgViewGroup;

        _vectorizedModelId = _db.getActiveModelId();
        if (_vectorizedModelId.isNull())
        {
          _vectorizedModelId = _db.getDefaultModelId();
          _db.setActiveModelId(_vectorizedModelId);
        }

        if (pViewGroup == null)
        {
          pViewGroup = _db.recommendActiveViewGroupId().openObject() as OdDgViewGroup;
          if (pViewGroup == null)
            return;
        }

        OdDgElementIterator pIt = pViewGroup.createIterator();
        for (; !pIt.done(); pIt.step())
        {
          OdDgView pView = OdDgView.cast(pIt.item().openObject());
          if (pView != null && pView.getVisibleFlag())
          {
            OdDgElementId _vectorizedViewId = pIt.item();
            var _vectorizer = ((Form1)Parent.Parent).createVectorizationWindow(this, _vectorizedViewId, _vectorizedModelId);
            //set its position
            OdGsDCRect rectangle = pView.getViewRectangle();
            OdGsDCPoint low = rectangle.m_min,
                        hi = rectangle.m_max;
            _vectorizer.Bounds = new Rectangle(low.x, hi.y, hi.x - low.x, low.y - hi.y);
            _vectorizers.Add(_vectorizer);
          }
        }
      }
      else
        _vectorizers[0].Activate();
    }

    private void zoomExtentsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_vectorizers.Count == 0)
        return;
      _vectorizers[0].ZoomExtents();
    }

    private void Tree_Activated(object sender, EventArgs e)
    {
      ((Form1)Parent.Parent).UpdateZoomExtFlag(false);
    }

  }
}