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
using Teigha.TD;
using System.Windows.Forms;

namespace OdaDwgAppMgd
{
  using TableMap = System.Collections.Generic.Dictionary<TreeNode, OdRxObject>;
  class DwgTreeFiller
  {
    TableMap m_nonDBROItems = new TableMap();
    TreeView m_pObjTree;
    OdDbDatabase m_pDb;
    TreeNode m_dbTreeItem;
    Stack<KeyValuePair<TreeNode, OdDbDictionaryIterator> > _itemStack;


    public DwgTreeFiller(OdDbDatabase pDb, TreeView pObjTree)
    {
      m_pObjTree = pObjTree;
      m_pDb = pDb;

      TreeNode rootNode = new TreeNode();

      _itemStack = new Stack<KeyValuePair<TreeNode, OdDbDictionaryIterator>>();
      KeyValuePair<TreeNode, OdDbDictionaryIterator> pairRoot = new KeyValuePair<TreeNode,OdDbDictionaryIterator>(rootNode, null);

      // _itemStack.Push(rootNode, (OdDbDictionary)null);
      _itemStack.Push(pairRoot);

      // Add root database node
      String sName = String.Format("<{0}>", pDb.isA().name());
      m_dbTreeItem = m_pObjTree.Nodes.Add(sName);
    }
    public TreeNode addElement(OdDbObjectId pElm, TreeNode hParent)
    {
        return addElement(pElm, hParent, null);
    }
    public TreeNode addElement(OdDbObjectId objId, TreeNode hParent, TreeNode hInsertAfter)
    {
        string name;
        string strName = "";
        string strDict = "";
        string strPrx ="";
        OdDbObject obj = objId.openObject();
        if (obj != null)
        {
            OdDbSymbolTableRecord rec = OdDbSymbolTableRecord.cast(obj);
            if (rec != null)
            {
                strName = rec.getName();
            }
            else if (_itemStack.Peek().Value != null)
            {
                OdDbDictionaryIterator pIter = _itemStack.Peek().Value;
                if (pIter.setPosition(objId))
                {
                    strDict = String.Format("<{0}>", pIter.name());
                }
            }
            if (String.IsNullOrEmpty(strName))
            {
                OdDbProxyExt pProxyExt = OdDbProxyExt.cast(obj);
                if(pProxyExt != null)
                {
                    strPrx = " <Proxy> : ";
                    strName = pProxyExt.originalClassName(obj);
                }
                else
                {
                    if (obj.isKindOf(OdDbDatabase.desc()) && (!((OdDbDatabase)obj).xrefBlockId().isNull()))
                    {
                        strDict = "XREF:";
                        strPrx = "";
                        strName = obj.isA().name();
                    }
                    else
                    {
                        strPrx = "";
                        strName = obj.isA().name();
                    }   
                }
            }
            name = String.Format("{0}<{1}{2}>", strDict, strPrx, strName);

            TreeNode hItem = (hInsertAfter == null) ? hParent.Nodes.Add(name)
                                                    : hParent.Nodes.Insert(hParent.Nodes.IndexOf(hInsertAfter), name);
            if (!objId.isNull())
                hItem.Tag = objId;

            return hItem;
        }
        return null;
    }

    void addSubElements(OdDbObjectId pElm, TreeNode hParent)
    {
        return;
        /*
        OdDbObjectDumperPE pDumper = OdDbRxObjectDumperPE.getDumper(pElm.isA()) as OdDbObjectDumperPE;
      if ( pDumper != null )
      {
        OdDbObjectIterator pIt = pDumper.createIterator( pElm, true, true );
        for (; pIt != null && !pIt.done(); pIt.step())
        {
          OdDbObjectId childId = pIt.item();
          addElement((OdDbObject)childId.openObject(), hParent, null);
        }
      }
         * */
    }
    public void explandItem(TreeNode hItem)
    {
        return;
        /*
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
        OdDbObjectId pIdStub = (OdDbObjectId)hItem.Tag;
        pElm = (OdDgElement)pIdStub.openObject();
      }

      if (pElm != null)
      {
        addSubElements(pElm, hItem);
      }
         * */
    }

    public TreeNode DbTreeItem
    { 
      get 
      { 
        return m_dbTreeItem; 
      }
    }

    OdDbDatabase database() { return m_pDb; }

    public OdRxObject getObject(TreeNode hItem)
    {
      if (hItem == m_dbTreeItem)
        return database();
      OdDbObjectId elmId = hItem.Tag as OdDbObjectId;
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

  class DwgListFiller : OdExDwgDumper
  {
      public DwgListFiller(OdDbDatabase pDb, ListView pObjList)
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
        OdDbObjectDumperPE pDumper = OdDbRxObjectDumperPE.getDumper(pObj.isA()) as OdDbObjectDumperPE;
      if ( pDumper != null)
        pDumper.dump(pObj, this);
    }
    ListView m_pObjList;
    OdDbDatabase m_pDb;
  };
}
