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

#ifndef __ODGIEXTENTS2DSPACETREE_H__
#define __ODGIEXTENTS2DSPACETREE_H__

#include "Ge/GeExtents2d.h"
#include "GiExtents3DSpaceTree.h"
#define STL_USING_SET
#include "OdaSTL.h"
#include "OdList.h"

#include "TD_PackPush.h"

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents2dSpaceNode
{
  friend class OdGiExtents2dSpaceTree;

  // left child node (only link, not need to delete)
  OdGiExtents2dSpaceNode*   m_pLeftChild;
  //right child node (only link, not need to delete)
  OdGiExtents2dSpaceNode*   m_pRightChild;
  //parent node (only link, not need to delete)
  OdGiExtents2dSpaceNode*   m_pParent;

  // extents of the node
  OdGeExtents2d     m_extents;
  
  // array of lists with IDs of the objects which are intersect with the current space
  OdList<OdGiExtentsSpaceObject*>*   m_pObjectPointers;
  int                 m_iObjectsTypes;

  // special (the power of 8 ) depth of the node in the tree
  int                 m_iDepth;
public:

  //constructor
  OdGiExtents2dSpaceNode(OdGiExtents2dSpaceNode* parent, OdGeExtents2d& extents, int nDepth, int nTypeOfObjects)
  {
    m_pParent = parent;
    m_extents = extents;

    m_pLeftChild = NULL;
    m_pRightChild = NULL;

    m_iObjectsTypes = 0;
    if ( nTypeOfObjects > 0 )
    {
      m_pObjectPointers = new OdList<OdGiExtentsSpaceObject*>[nTypeOfObjects];
      m_iObjectsTypes = nTypeOfObjects;
    }
    else
    {
      m_pObjectPointers = NULL;
    }

    m_iDepth = nDepth;
  }

  //destructor
  ~OdGiExtents2dSpaceNode()
  {
    if (m_pObjectPointers)
      delete[]m_pObjectPointers;
  }

  // methods
  // check that the node is leave
  bool isLeave()
  {
    return (m_pLeftChild == NULL && m_pRightChild == NULL);
  }

  //get the depth of the ndoe
  int getDepth() {return m_iDepth;}

  // retrieve the pointer the stores objects list
  OdList<OdGiExtentsSpaceObject*>* getObjectPointersPtr(int iType)
  {
    if ( iType < m_iObjectsTypes )
      return &m_pObjectPointers[iType];

    return NULL;
  }
};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents2dSpaceTree
{
  // maximal value of the depth
  static const OdInt MAX_DEPTH = 10;

  //root node
  OdGiExtents2dSpaceNode*  m_pRootNode;

  // list of all nodes
  OdList<OdGiExtents2dSpaceNode*> m_Nodes;

  // list of all leaves
  OdList<OdGiExtents2dSpaceNode*> m_Leaves;

  //Is addaptive - means that started additioanl splitting when the number of objects starts to be greater than 'm_iMaxNodeObjects'
  bool              m_bIsAdaptive;
  OdUInt64          m_iMaxNodeObjects;

  // callback for getting object by ID
  OdGiExtentsSpaceObjectCallback* m_pGetObjectByIDCallback;

  // for storing the same objects during processing
  OdList<OdGiExtentsSpaceObject*>     m_theSameObjects;

   // for storing the intersected leaves during processing
  OdList<OdGiExtents2dSpaceNode*>     m_theIntersectedLeaves;

public:
  
  //constructor
  OdGiExtents2dSpaceTree()
  {
    m_pRootNode = NULL;
    m_bIsAdaptive = false;
    m_iMaxNodeObjects = 10;
    m_pGetObjectByIDCallback = NULL;
  }

  //destructor
  ~OdGiExtents2dSpaceTree(){reset();}

  //build the tree (the correctness of the incoming extents should check the caller)
   void buildTree(OdGeExtents2d& extents, int nTypeOfObjects, OdInt depth = 2)
   {   
     if ( depth > MAX_DEPTH )
      depth = MAX_DEPTH;
     else if ( depth < 0 )
      depth = 0;

     if ( m_pRootNode != NULL )
      reset();

     //1. Create root node
     m_pRootNode = new OdGiExtents2dSpaceNode(NULL, extents, 0, nTypeOfObjects);
     m_Nodes.push_back(m_pRootNode);

     //2. Call recursive method for making the tree
     constructChilds(m_pRootNode, 2, depth, nTypeOfObjects); 
  }

  // reset the tree
  void reset()
  {
    m_pRootNode = NULL;

    OdList<OdGiExtents2dSpaceNode*>::iterator it = m_Nodes.begin();
    OdList<OdGiExtents2dSpaceNode*>::iterator itEnd = m_Nodes.end();
    while ( it != itEnd )
    {
      OdGiExtents2dSpaceNode* pNode = *it;
      delete pNode;
      ++it;
    }

    m_Leaves.clear();
    m_Nodes.clear();

    m_theSameObjects.clear();
    m_theIntersectedLeaves.clear();
  }

  // set that the tree should be adaptive or not
  void setAdaptive(bool bAdaptive) {m_bIsAdaptive = bAdaptive;}

  // set the maximal node objects (used only if 'm_bIsAdaptive' = true)
  void setMaxNodeObjects(OdUInt64 nObjects) { m_iMaxNodeObjects = nObjects <= 0 ? m_iMaxNodeObjects : nObjects; }

  // set object by ID callback
  void setGetObjectByIDCallback(OdGiExtentsSpaceObjectCallback* pCallback) { m_pGetObjectByIDCallback = pCallback; }

  //process object
  OdGiExtentsSpaceObject* processObject(OdGiExtentsSpaceObject* pObject, int iObjectType, bool bCheckTheSame = false)
  {
    m_theSameObjects.clear();

    if ( m_pRootNode == NULL )
      return NULL;

    nodeProcessObject(m_pRootNode, pObject, iObjectType, bCheckTheSame);
  
    if ( m_theSameObjects.size() == 0 )
      return NULL;

    return m_theSameObjects.front();
  }

  // retrieve the leaves nodes
  OdList<OdGiExtents2dSpaceNode*>* retrieveLeaves(){ return &m_Leaves;}

   //retrieve the extents of the root node
  bool getRootExtents(OdGeExtents2d& exts)
  {
    if ( m_pRootNode == NULL )
      return false;

    if(m_pRootNode->m_extents.isValidExtents())
    {
      exts = m_pRootNode->m_extents;
      return true;
    }
    return false;
  }

  OdList<OdGiExtents2dSpaceNode*>* getExtentsLeaves(OdGeExtents2d& ext)
  {
    m_theIntersectedLeaves.clear();

    if ( m_pRootNode == NULL )
      return NULL;

    nodeProcessExtent(m_pRootNode, ext);
  
    if ( m_theIntersectedLeaves.size() == 0 )
      return NULL;

    return &m_theIntersectedLeaves;
  }

  OdList<OdGiExtents2dSpaceNode*>* getPointLeaves(const OdGePoint2d& pt)
  {
    m_theIntersectedLeaves.clear();

    if ( m_pRootNode == NULL )
      return NULL;

    nodeProcessPoint(m_pRootNode, pt);
  
    if ( m_theIntersectedLeaves.size() == 0 )
      return NULL;

    return &m_theIntersectedLeaves;
  }

private:

  // internal recursive method for building the tree
  void constructChilds(OdGiExtents2dSpaceNode* pParentNode, int axislevel, int curDepth, int nTypeOfObjects)
  {
    if ( pParentNode == NULL )
      return;

    if ( curDepth == 0 )
    {
      // means we have a leave
      m_Leaves.push_back(pParentNode);
      return;
    }

    //define the left part extents
    OdGeExtents2d     leftChildExtents;

    OdGePoint2d leftBounPoint = pParentNode->m_extents.maxPoint();
    switch ( axislevel )
    {
    case 2:
      leftBounPoint.x = (pParentNode->m_extents.maxPoint().x + pParentNode->m_extents.minPoint().x)/2.;
      break;
    case 1:
      leftBounPoint.y = (pParentNode->m_extents.maxPoint().y + pParentNode->m_extents.minPoint().y)/2.;
      break;
    }
  
    leftChildExtents.set(pParentNode->m_extents.minPoint(), leftBounPoint);

    // create left child
    pParentNode->m_pLeftChild = new OdGiExtents2dSpaceNode(pParentNode, leftChildExtents, axislevel > 1 ? pParentNode->m_iDepth : pParentNode->m_iDepth + 1, nTypeOfObjects);
    m_Nodes.push_back(pParentNode->m_pLeftChild);

    // go deeper
    if ( axislevel > 1 )
      constructChilds(pParentNode->m_pLeftChild, axislevel-1, curDepth, nTypeOfObjects);
    else
      constructChilds(pParentNode->m_pLeftChild, 2, curDepth-1, nTypeOfObjects);

    //define the right part extents
    OdGeExtents2d     rightChildExtents;

    OdGePoint2d rightBounPoint = pParentNode->m_extents.minPoint();
    switch ( axislevel )
    {
    case 2:
      rightBounPoint.x = (pParentNode->m_extents.maxPoint().x + pParentNode->m_extents.minPoint().x)/2.;
      break;
    case 1:
      rightBounPoint.y = (pParentNode->m_extents.maxPoint().y + pParentNode->m_extents.minPoint().y)/2.;
      break;
    }
  
    rightChildExtents.set(rightBounPoint, pParentNode->m_extents.maxPoint());

    // create right child
    pParentNode->m_pRightChild = new OdGiExtents2dSpaceNode(pParentNode, rightChildExtents, axislevel > 1 ? pParentNode->m_iDepth : pParentNode->m_iDepth + 1, nTypeOfObjects);
    m_Nodes.push_back(pParentNode->m_pRightChild);

    // go deeper
    if ( axislevel > 1 )
      constructChilds(pParentNode->m_pRightChild, axislevel-1, curDepth, nTypeOfObjects);
    else
      constructChilds(pParentNode->m_pRightChild, 2, curDepth-1, nTypeOfObjects);

    return;
  }

  // internal recursive method for process objects
  void nodeProcessObject(OdGiExtents2dSpaceNode* pNode, OdGiExtentsSpaceObject* pObject, int iObjectType, bool bCheckTheSame)
  {
    if ( pNode == NULL || pObject == NULL )
      return;

    bool bIntersect = pObject->isInExtents(pNode->m_extents);

    if ( bIntersect )
    {
      if ( pNode->isLeave() )
      {
        OdList<OdGiExtentsSpaceObject*>* pNodeObjects = pNode->getObjectPointersPtr(iObjectType);
        if ( pNodeObjects )
        {
          if ( bCheckTheSame && m_pGetObjectByIDCallback ) // check that we already have the same object
          {
            OdList<OdGiExtentsSpaceObject*>::iterator itNodes = pNodeObjects->begin();
            OdList<OdGiExtentsSpaceObject*>::iterator itNodesEnd = pNodeObjects->end();
            while ( itNodes != itNodesEnd )
            {
              OdGiExtentsSpaceObject* pObjectInList = *itNodes;
              if ( pObjectInList )
              {
                if ( pObjectInList->isEqual(pObject) )
                {
                  m_theSameObjects.push_back(pObjectInList);
                  return;
                }
              }
              ++itNodes;
            }
            pNodeObjects->push_back( pObject );
          }
          else // simply add
          {
            pNodeObjects->push_back( pObject );
          }
        }

        //adaptive case
        if ( m_bIsAdaptive && pNodeObjects->size() > m_iMaxNodeObjects && pNode->m_iDepth <= MAX_DEPTH && m_pGetObjectByIDCallback )
        {
          // remove current node from leaves
          m_Leaves.remove(pNode);

          //try to make additional split
          constructChilds(pNode, 2, 1/*one step deeper*/, pNode->m_iObjectsTypes);

          //put down the objects
          for ( int i = 0; i < pNode->m_iObjectsTypes; i++)
          {
            pNodeObjects = pNode->getObjectPointersPtr(i);
            if ( pNodeObjects )
            {
              OdList<OdGiExtentsSpaceObject*>::iterator it = pNodeObjects->begin();
              OdList<OdGiExtentsSpaceObject*>::iterator itEnd = pNodeObjects->end();
              while ( it != itEnd )
              {
                OdGiExtentsSpaceObject* pObjectInList = *it;

                if ( pObjectInList )
                {
                  nodeProcessObject(pNode->m_pLeftChild, pObjectInList, i, false);
                  nodeProcessObject(pNode->m_pRightChild, pObjectInList, i, false);
                }
            
                ++it;
              }
              //remove objects from leave
              pNodeObjects->clear();
            }
          } //eo for...
        }
      }
      else
      {
        nodeProcessObject(pNode->m_pLeftChild, pObject, iObjectType, bCheckTheSame);
        nodeProcessObject(pNode->m_pRightChild, pObject, iObjectType, bCheckTheSame);
      }
    }
  
    return;
   }

  // internal recursive method for process extents
  void nodeProcessExtent(OdGiExtents2dSpaceNode* pNode, OdGeExtents2d& ext)
  {
    if ( pNode == NULL || !ext.isValidExtents() )
      return;

    OdGeExtents2d isect;
    ext.intersectWith(pNode->m_extents, &isect);

    if ( isect.isValidExtents() )
    {
      if ( pNode->isLeave() )
      {
        m_theIntersectedLeaves.push_back(pNode);
      }
      else
      {
        nodeProcessExtent(pNode->m_pLeftChild, ext);
        nodeProcessExtent(pNode->m_pRightChild, ext);
      }
    }
  
    return;
  }

  // internal recursive method for process point
  void nodeProcessPoint(OdGiExtents2dSpaceNode* pNode, const OdGePoint2d& pt)
  {
    if ( pNode == NULL )
      return;

    if ( pNode->m_extents.contains(pt) )
    {
      if ( pNode->isLeave() )
      {
        m_theIntersectedLeaves.push_back(pNode);
      }
      else
      {
        nodeProcessPoint(pNode->m_pLeftChild, pt);
        nodeProcessPoint(pNode->m_pRightChild, pt);
      }
    }
  
    return;
  }
};

#include "TD_PackPop.h"

#endif //#ifndef __ODGIEXTENTS2DSPACETREE_H__
