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
//using System.Linq;
using System.Text;
using Teigha.Core;
using Teigha.TG;
using System.Windows.Forms;

namespace OdaDgnAppMgd
{
	using TableMap = System.Collections.Generic.Dictionary<TreeNode, OdRxObject>;
	class DgnTreeFiller
	{
		TableMap m_nonDBROItems = new TableMap();
		TreeView m_pObjTree;
		OdDgDatabase m_pDb;
		TreeNode m_dbTreeItem;
		public DgnTreeFiller(OdDgDatabase pDb, TreeView pObjTree)
		{
			m_pObjTree = pObjTree;
			m_pDb = pDb;

			// Add root database node
			String sName = String.Format("<{0}>", pDb.isA().name());
			m_dbTreeItem = m_pObjTree.Nodes.Add(sName);
		}
		TreeNode addTreeItem(String strName, TreeNode hParent, TreeNode hInsertAfter)
		{
			TreeNode hItem = (hInsertAfter == null) ? hParent.Nodes.Add(strName)
					 : hParent.Nodes.Insert(hParent.Nodes.IndexOf(hInsertAfter), strName);

			return hItem;
		}
		public TreeNode addElement(OdDgElement pElm, TreeNode hParent)
		{
			return addElement(pElm, hParent, null);
		}
		public TreeNode addElement(OdDgElement pElm, TreeNode hParent, TreeNode hInsertAfter)
		{
			if (pElm != null)
			{
				String sName = "";
				OdDgElementDumperPE pElmDumper = OdDgRxObjectDumperPE.getDumper(pElm.isA()) as OdDgElementDumperPE;
				if (pElmDumper != null)
					sName = pElmDumper.getName(pElm);
				if (sName == "")
					sName = String.Format("<{0}>", pElm.isA().name());
				TreeNode hItem = (hInsertAfter == null) ? hParent.Nodes.Add(sName)
					: hParent.Nodes.Insert(hParent.Nodes.IndexOf(hInsertAfter), sName);
				if (!pElm.elementId().isNull())
					hItem.Tag = pElm.elementId();
				else // Save NonDBRO collections in separate map
				{
					m_nonDBROItems[hItem] = pElm;
				}
				return hItem;
			}
			return null;
		}
		void addSubElements(OdDgElement pElm, TreeNode hParent)
		{
			OdDgElementDumperPE pDumper = OdDgRxObjectDumperPE.getDumper(pElm.isA()) as OdDgElementDumperPE;
			if (pDumper != null)
			{
				TreeNode hParentItem = hParent;

				OdDgModel pModel = OdDgModel.cast(pElm);
				OdDgReferenceAttachmentHeader pXRef = OdDgReferenceAttachmentHeader.cast(pElm);

				if (pXRef != null)
				{
					OdDgLevelTable pLevelTable = pXRef.getLevelTable(OpenMode.kForRead);
					if (pLevelTable != null)
					{
						addElement(pLevelTable, hParentItem);
					}
				}
				else if (pModel != null)
				{
					OdDgElementIterator pGraphIt = pModel.createGraphicsElementsIterator();
					bool bHasGraphItems = pGraphIt != null && !pGraphIt.done();
					if (bHasGraphItems)
					hParentItem = addTreeItem("Graphics", hParent, null);
				}

				OdDgElementIterator pIt = pDumper.createIterator(pElm, true, true);
				for (; pIt != null && !pIt.done(); pIt.step())
				{
					OdDgElementId childId = pIt.item();
					addElement((OdDgElement)childId.openObject(), hParentItem, null);
				}

				if (pModel != null)
				{
					OdDgModelDumperPE pModelDumper = OdDgRxObjectDumperPE.getDumper(pModel.isA()) as OdDgModelDumperPE;
					OdDgElementIterator pControlIt = pModelDumper.createControlElementsIterator(pElm, true, true);

					bool bHasCtrlItems = pControlIt != null && !pControlIt.done();
					if (bHasCtrlItems)
					hParentItem = addTreeItem("Control", hParent, null);

					for (; !pControlIt.done(); pControlIt.step())
					{
						OdDgElementId childId = pControlIt.item();
						addElement((OdDgElement)childId.openObject(), hParentItem);
					}
				}
			}
		}
		public void explandItem(TreeNode hItem)
		{
			OdDgElement pElm = null;
			if (hItem.Tag == null)
			{
				// Look at map of tables
				OdRxObject value;
				if (m_nonDBROItems.TryGetValue(hItem, out value))
				{
					pElm = OdDgElement.cast(value);
				}
			}
			else
			{
				OdDgElementId pIdStub = (OdDgElementId)hItem.Tag;
				pElm = (OdDgElement)pIdStub.openObject();
			}

			if (pElm != null)
			{
				addSubElements(pElm, hItem);
			}
		}

		public TreeNode DbTreeItem
		{
			get
			{
				return m_dbTreeItem;
			}
		}

		OdDgDatabase database() { return m_pDb; }

		public OdRxObject getObject(TreeNode hItem)
		{
			if (hItem == m_dbTreeItem)
				return database();
			OdDgElementId elmId = hItem.Tag as OdDgElementId;
			if (elmId != null)
				return elmId.openObject();
			else
			{
				OdRxObject res;
				m_nonDBROItems.TryGetValue(hItem, out res);
				return res;
			}
		}
	}

	class DgnListFiller : OdExDgnDumper
	{
		public DgnListFiller(OdDgDatabase pDb, ListView pObjList)
		{
			m_pObjList = pObjList;
			m_pDb = pDb;
		}
		public override void dumpFieldName(string fieldName)
		{
			m_pObjList.Items.Add(fieldName);
		}
		public override void dumpFieldValue(string fieldValue)
		{
			m_pObjList.Items[m_pObjList.Items.Count - 1].SubItems.Add(fieldValue);
		}
		public void dump(OdRxObject pObj)
		{
			OdDgElementDumperPE pDumper = OdDgRxObjectDumperPE.getDumper(pObj.isA()) as OdDgElementDumperPE;
			if (pDumper != null)
				pDumper.dump(pObj, this);
		}
		ListView m_pObjList;
		OdDgDatabase m_pDb;
	};
}
