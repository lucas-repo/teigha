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

#ifndef __ODGIEXTENTS3DSPACETREE_H__
#define __ODGIEXTENTS3DSPACETREE_H__

#include "Ge/GeExtents3d.h"
#include "Ge/GeExtents2d.h"
#include "OdVector.h"
#define STL_USING_SET
#include "OdaSTL.h"
#include "OdList.h"

#include "TD_PackPush.h"

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtentsSpaceObject
{
  // unique ID of the object
  //IMPORTANT: the user is responsible for the uniqueness of the ID
  OdUInt64    ID;

public:

  // constructor
  OdGiExtentsSpaceObject(OdUInt64 uniqueID)
  {
    ID = uniqueID;
  }

  //set/get ID
  OdUInt64  getID() {return ID;}
  void      setID(OdUInt64 uniqueID){ID = uniqueID;}

  // pure virtual members
  virtual bool isInExtents(OdGeExtents3d& extents) const  = 0;
  virtual bool isInExtents(OdGeExtents2d& extents) const  = 0;
  virtual bool isEqual(OdGiExtentsSpaceObject* pObject) const = 0;
};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/
class OdGiExtentsSpaceObjectCallback
{
  public:
    virtual OdGiExtentsSpaceObject* getObjectByID(OdUInt64 /*requestedID*/, int /*objectType*/) {return NULL;}
};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents3dSpaceNode
{
  friend class OdGiExtents3dSpaceTree;

  // left child node (only link, not need to delete)
  OdGiExtents3dSpaceNode*   m_pLeftChild;
  //right child node (only link, not need to delete)
  OdGiExtents3dSpaceNode*   m_pRightChild;
  //parent node (only link, not need to delete)
  OdGiExtents3dSpaceNode*   m_pParent;

  // extents of the node
  OdGeExtents3d     m_extents;
  
  // array of lists with IDs of the objects which are intersect with the current space
  OdList<OdUInt64>*   m_pObjects;
  int                 m_iObjectsTypes;

  // special (the power of 8 ) depth of the node in the tree
  int                 m_iDepth;
public:

  //constructor
  OdGiExtents3dSpaceNode(OdGiExtents3dSpaceNode* parent, OdGeExtents3d& extents, int nDepth, int nTypeOfObjects)
  {
    m_pParent = parent;
    m_extents = extents;

    m_pLeftChild = NULL;
    m_pRightChild = NULL;

    m_iObjectsTypes = 0;
    if ( nTypeOfObjects > 0 )
    {
      m_pObjects = new OdList<OdUInt64>[nTypeOfObjects];
      m_iObjectsTypes = nTypeOfObjects;
    }
    else
    {
      m_pObjects = NULL;
    }

    m_iDepth = nDepth;
  }

  //destructor
  ~OdGiExtents3dSpaceNode()
  {
    if ( m_pObjects )
      delete[]m_pObjects;
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
  OdList<OdUInt64>* getObjectsPtr(int iType)
  {
    if ( iType < m_iObjectsTypes )
      return &m_pObjects[iType];

    return NULL;
  }
};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents3dSpaceTree
{
  // maximal value of the depth
  static const OdInt MAX_DEPTH = 5;

  //root node
  OdGiExtents3dSpaceNode*  m_pRootNode;

  // list of all nodes
  OdList<OdGiExtents3dSpaceNode*> m_Nodes;

  // list of all leaves
  OdList<OdGiExtents3dSpaceNode*> m_Leaves;

  //Is addaptive - means that started additioanl splitting when the number of objects starts to be greater than 'm_iMaxNodeObjects'
  bool              m_bIsAdaptive;
  OdUInt64          m_iMaxNodeObjects;

  // callback for getting object by ID
  OdGiExtentsSpaceObjectCallback* m_pGetObjectByIDCallback;

  // for storing the same objects during processing
  OdList<OdUInt64>     m_theSameObjects;

public:
  
  //constructor
  OdGiExtents3dSpaceTree()
  {
    m_pRootNode = NULL;
    m_bIsAdaptive = false;
    m_iMaxNodeObjects = 20;
    m_pGetObjectByIDCallback = NULL;
  }

  //destructor
  ~OdGiExtents3dSpaceTree(){reset();}

  //build the tree (the correctness of the incoming extents should check the caller)
   void buildTree(OdGeExtents3d& extents, int nTypeOfObjects, OdInt depth = 2)
   {   
     if ( depth > MAX_DEPTH )
      depth = MAX_DEPTH;
     else if ( depth < 0 )
      depth = 0;

     if ( m_pRootNode != NULL )
      reset();

     //1. Create root node
     m_pRootNode = new OdGiExtents3dSpaceNode(NULL, extents, 0, nTypeOfObjects);
     m_Nodes.push_back(m_pRootNode);

     //2. Call recursive method for making the tree
     constructChilds(m_pRootNode, 3, depth, nTypeOfObjects); 
  }

  // reset the tree
  void reset()
  {
    m_pRootNode = NULL;

    OdList<OdGiExtents3dSpaceNode*>::iterator it = m_Nodes.begin();
    OdList<OdGiExtents3dSpaceNode*>::iterator itEnd = m_Nodes.end();
    while ( it != itEnd )
    {
      OdGiExtents3dSpaceNode* pNode = *it;
      delete pNode;
      ++it;
    }

    m_Leaves.clear();
    m_Nodes.clear();

    m_theSameObjects.clear();
  }

  // set that the tree should be adaptive or not
  void setAdaptive(bool bAdaptive) {m_bIsAdaptive = bAdaptive;}

  // set the maximal node objects (used only if 'm_bIsAdaptive' = true)
  void setMaxNodeObjects(OdUInt64 nObjects) { m_iMaxNodeObjects = nObjects <= 0 ? m_iMaxNodeObjects : nObjects; }

  // set object by ID callback
  void setGetObjectByIDCallback(OdGiExtentsSpaceObjectCallback* pCallback) { m_pGetObjectByIDCallback = pCallback; }

  //process object
  OdInt64 processObject(OdGiExtentsSpaceObject* pObject, int iObjectType, bool bCheckTheSame = false)
  {
    m_theSameObjects.clear();

    if ( m_pRootNode == NULL )
      return -1;

    nodeProcessObject(m_pRootNode, pObject, iObjectType, bCheckTheSame);
  
    if ( m_theSameObjects.size() == 0 )
      return -1;

    return m_theSameObjects.front();
  }

  // retrieve the leaves nodes
  OdList<OdGiExtents3dSpaceNode*>* retrieveLeaves(){ return &m_Leaves;}

   //retrieve the extents of the root node
  bool getRootExtents(OdGeExtents3d& exts)
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
private:

  // internal recursive method for building the tree
  void constructChilds(OdGiExtents3dSpaceNode* pParentNode, int axislevel, int curDepth, int nTypeOfObjects)
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
    OdGeExtents3d     leftChildExtents;

    OdGePoint3d leftBounPoint = pParentNode->m_extents.maxPoint();
    switch ( axislevel )
    {
    case 3:
      leftBounPoint.x = (pParentNode->m_extents.maxPoint().x + pParentNode->m_extents.minPoint().x)/2.;
      break;
    case 2:
      leftBounPoint.y = (pParentNode->m_extents.maxPoint().y + pParentNode->m_extents.minPoint().y)/2.;
      break;
    case 1:
      leftBounPoint.z = (pParentNode->m_extents.maxPoint().z + pParentNode->m_extents.minPoint().z)/2.;
      break;
    }
  
    leftChildExtents.set(pParentNode->m_extents.minPoint(), leftBounPoint);

    // create left child
    pParentNode->m_pLeftChild = new OdGiExtents3dSpaceNode(pParentNode, leftChildExtents, axislevel > 1 ? pParentNode->m_iDepth : pParentNode->m_iDepth + 1, nTypeOfObjects);
    m_Nodes.push_back(pParentNode->m_pLeftChild);

    // go deeper
    if ( axislevel > 1 )
      constructChilds(pParentNode->m_pLeftChild, axislevel-1, curDepth, nTypeOfObjects);
    else
      constructChilds(pParentNode->m_pLeftChild, 3, curDepth-1, nTypeOfObjects);

    //define the right part extents
    OdGeExtents3d     rightChildExtents;

    OdGePoint3d rightBounPoint = pParentNode->m_extents.minPoint();
    switch ( axislevel )
    {
    case 3:
      rightBounPoint.x = (pParentNode->m_extents.maxPoint().x + pParentNode->m_extents.minPoint().x)/2.;
      break;
    case 2:
      rightBounPoint.y = (pParentNode->m_extents.maxPoint().y + pParentNode->m_extents.minPoint().y)/2.;
      break;
    case 1:
      rightBounPoint.z = (pParentNode->m_extents.maxPoint().z + pParentNode->m_extents.minPoint().z)/2.;
      break;
    }
  
    rightChildExtents.set(rightBounPoint, pParentNode->m_extents.maxPoint());

    // create right child
    pParentNode->m_pRightChild = new OdGiExtents3dSpaceNode(pParentNode, rightChildExtents, axislevel > 1 ? pParentNode->m_iDepth : pParentNode->m_iDepth + 1, nTypeOfObjects);
    m_Nodes.push_back(pParentNode->m_pRightChild);

    // go deeper
    if ( axislevel > 1 )
      constructChilds(pParentNode->m_pRightChild, axislevel-1, curDepth, nTypeOfObjects);
    else
      constructChilds(pParentNode->m_pRightChild, 3, curDepth-1, nTypeOfObjects);

    return;
  }

  // internal recursive method for process objects
  void nodeProcessObject(OdGiExtents3dSpaceNode* pNode, OdGiExtentsSpaceObject* pObject, int iObjectType, bool bCheckTheSame)
  {
    if ( pNode == NULL || pObject == NULL )
      return;

    bool bIntersect = pObject->isInExtents(pNode->m_extents);

    if ( bIntersect )
    {
      if ( pNode->isLeave() )
      {
        OdList<OdUInt64>* pNodeObjects = pNode->getObjectsPtr(iObjectType);
        if ( pNodeObjects )
        {
          if ( bCheckTheSame && m_pGetObjectByIDCallback ) // check that we already have the same object
          {
            OdList<OdUInt64>::iterator itNodes = pNodeObjects->begin();
            OdList<OdUInt64>::iterator itNodesEnd = pNodeObjects->end();
            while ( itNodes != itNodesEnd )
            {
              OdUInt64 objectID = *itNodes;
              OdGiExtentsSpaceObject* pObjectInList = m_pGetObjectByIDCallback->getObjectByID(objectID, iObjectType);
              if ( pObjectInList )
              {
                if ( pObjectInList->isEqual(pObject) )
                {
                  m_theSameObjects.push_back(pObjectInList->getID());
                  return;
                }
              }
              ++itNodes;
            }
            pNodeObjects->push_back( pObject->getID() );
          }
          else // simply add
          {
            pNodeObjects->push_back( pObject->getID() );
          }
        }

        //adaptive case
        if ( m_bIsAdaptive && pNodeObjects->size() > m_iMaxNodeObjects && pNode->m_iDepth <= MAX_DEPTH && m_pGetObjectByIDCallback )
        {
          // remove current node from leaves
          m_Leaves.remove(pNode);

          //try to make additional split
          constructChilds(pNode, 3, 1/*one step deeper*/, pNode->m_iObjectsTypes);

          //put down the objects
          for ( int i = 0; i < pNode->m_iObjectsTypes; i++)
          {
            pNodeObjects = pNode->getObjectsPtr(i);
            if ( pNodeObjects )
            {
              OdList<OdUInt64>::iterator it = pNodeObjects->begin();
              OdList<OdUInt64>::iterator itEnd = pNodeObjects->end();
              while ( it != itEnd )
              {
                OdUInt64 objectID = *it;

                OdGiExtentsSpaceObject* pObjectInList = m_pGetObjectByIDCallback->getObjectByID(objectID, i);
                if ( pObjectInList == NULL ) // it can be if objectID == pObject->ID (just processed)
                {
                  if ( objectID == pObject->getID() )
                    pObjectInList = pObject;
                }
            
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
};


/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents3dSpacePoint : public OdGiExtentsSpaceObject
{
  friend class OdGiExtents3dSpaceEdge;

  // set of the IDs of edges which have this vertex
  std::set<OdUInt64> m_edges;

  // set of the IDs of invisible edges which have this vertex
  std::set<OdUInt64> m_invisibleedges;

  // need for constructing the chain polylines
  bool        m_bVisited;

public:

  //point
  OdGePoint3d  m_pt;

  //constructor
  OdGiExtents3dSpacePoint(const OdGePoint3d& pt, OdUInt64 uniqueID) : OdGiExtentsSpaceObject(uniqueID)
  {
    m_pt.set(pt.x, pt.y, pt.z);
    m_bVisited = false;
  }

  ~OdGiExtents3dSpacePoint()
  {
    m_edges.clear();
    m_invisibleedges.clear();
  }

  void addEdge(OdUInt64 ID)
  {
    m_edges.insert(ID);
  }

  void addInvisible(OdUInt64 ID)
  {
    m_invisibleedges.insert(ID);
  }

  void removeInvisible(OdUInt64 ID)
  {
    m_invisibleedges.erase(ID);
  }

  // check that the edge is in extents (currently not used)
  bool isInExtents(OdGeExtents3d& extents) const
  {
    OdGeTol  tol; 
    if (extents.contains( m_pt, tol ) )
      return true;

    return false;
  }

  bool isInExtents(OdGeExtents2d& extents) const {return false;}

    // check that objects are equal
  bool isEqual(OdGiExtentsSpaceObject* pObject) const
  {
    OdGiExtents3dSpacePoint* pObjectVertex = dynamic_cast<OdGiExtents3dSpacePoint*>(pObject);
    
    if ( pObjectVertex == NULL )
      return false;

    if ( pObjectVertex->m_pt.isEqualTo(m_pt) )
      return true;

    return false;
  }

  OdUInt64 getPower()
  {
    return m_edges.size();
  }

  const std::set<OdUInt64>* getEdges(){return &m_edges;}
  const std::set<OdUInt64>* getInvisilbeEdges(){return & m_invisibleedges;}

  void setVisited(bool bVisit){m_bVisited = bVisit;}
  bool isVisited(){return m_bVisited;}

};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents3dSpaceEdge : public OdGiExtentsSpaceObject
{

public:
  //two vertices
  OdUInt64    m_iVert1;
  OdUInt64    m_iVert2;
  bool        m_bVisited; // need for constructing the pathes
  bool        m_bIsVisible; // means that the edge is not visible
  
  //constructor
  OdGiExtents3dSpaceEdge(OdGiExtents3dSpacePoint& pt1, OdGiExtents3dSpacePoint& pt2, OdInt64 uniqueID) : OdGiExtentsSpaceObject(uniqueID)
  {
    m_iVert1 = pt1.getID();
    m_iVert2 = pt2.getID();

    if ( uniqueID >= 0 )
    {
      pt1.m_edges.insert(uniqueID);
      pt2.m_edges.insert(uniqueID);
    }

    m_bVisited = false;
    m_bIsVisible = true;
  }

  void setVisited(bool bVisit){m_bVisited = bVisit;}
  bool isVisited(){return m_bVisited;}

  OdUInt64 getSecondVertex(OdUInt64 iDfirst)
  {
    if ( iDfirst == m_iVert1 )
      return m_iVert2;

    return m_iVert1;
  }

  bool isInExtents(OdGeExtents3d& extents) const {return false;}
  bool isInExtents(OdGeExtents2d& extents) const {return false;}
  bool isEqual(OdGiExtentsSpaceObject* pObject) const{return false;}
};

/** \details
  .

    Corresponding C++ library: TD_Gi
    <group OdGi_Classes>
*/

class OdGiExtents3dSpaceChainPolyline : public OdGiExtentsSpaceObject
{
protected:

  OdList<OdGiExtents3dSpacePoint*> vertices;

public:

  //constructor
  OdGiExtents3dSpaceChainPolyline(OdInt64 uniqueID) : OdGiExtentsSpaceObject(uniqueID){}

  ~OdGiExtents3dSpaceChainPolyline(){vertices.clear();}

  virtual void addVertex(OdGiExtents3dSpacePoint* pVertex)
  {
    vertices.push_back(pVertex);
  }

  OdUInt64 getNumberOfVertices() {return vertices.size();}

  void getPoints(OdGePoint3d *pPoints)
  {
    if ( pPoints == NULL )
      return;

    OdList<OdGiExtents3dSpacePoint*>::iterator it = vertices.begin();
    OdList<OdGiExtents3dSpacePoint*>::iterator itEnd = vertices.end();
    OdUInt32 ind = 0;
    while ( it != itEnd )
    {
       OdGiExtents3dSpacePoint* pVertex = *it;
       if ( pVertex )
       {
         pPoints[ind].set(pVertex->m_pt.x, pVertex->m_pt.y, pVertex->m_pt.z); 
         ind++;
       }
       ++it;
    }
  }

  void getPoints(OdGePoint3dVector& pts)
  {
    getPoints(pts.asArrayPtr());
  }

  bool isInExtents(OdGeExtents3d& extents) const {return false;}
  bool isInExtents(OdGeExtents2d& extents) const {return false;}
  bool isEqual(OdGiExtentsSpaceObject* pObject) const{return false;}
};


class OdGiExtents3dSpaceUtils
{
 // next start vertex for finding the path
 static OdGiExtents3dSpacePoint* getNextStartVertex(OdArray<OdGiExtents3dSpacePoint*>& points, OdList<OdGiExtents3dSpacePoint*>& leafs)
 {
   OdGiExtents3dSpacePoint* pStartVertex = NULL;
   
   //1. Try to find leafs
   if ( leafs.size() > 0 )
   {
      OdList<OdGiExtents3dSpacePoint*>::iterator itLeafs = leafs.begin();
      OdList<OdGiExtents3dSpacePoint*>::iterator itLeafsEnd = leafs.end();
      while ( itLeafs != itLeafsEnd )
      {
        OdGiExtents3dSpacePoint* pLeaf = *itLeafs;
        if ( pLeaf && !pLeaf->isVisited() )
        {
          pStartVertex = pLeaf;
          break;
        }
        ++itLeafs;
      }
   }

   //2. Try to find first not visited vertex
   if ( pStartVertex == NULL )
   {
     int iVertNumber = points.size();
     for (OdUInt64 i = 0; i < iVertNumber; i++)
     {
       OdGiExtents3dSpacePoint* pVert = points[i];
       if ( pVert && !pVert->isVisited() )
       {
         pStartVertex = pVert;
         break;
       }
     }
   }

   return pStartVertex;
 }

template <class E>
static OdGiExtents3dSpaceEdge* getFirstUnvisitedEdge(OdGiExtents3dSpacePoint* pVertex, OdArray<E*>& edges)
{
  if ( pVertex == NULL )
    return NULL;

  const std::set<OdUInt64>* pEdges = pVertex->getEdges();
  if ( pEdges == NULL )
    return NULL;

  std::set<OdUInt64>::const_iterator itEdge = pEdges->begin();
  std::set<OdUInt64>::const_iterator itEdgeEnd = pEdges->end();
  while ( itEdge != itEdgeEnd )
  {
    OdUInt64 edgeID = *itEdge;
    if ( edgeID < edges.size() )
    {
      OdGiExtents3dSpaceEdge* pEdge = dynamic_cast<OdGiExtents3dSpaceEdge*>(edges[edgeID]);
      if ( pEdge && !pEdge->isVisited() && pEdge->m_bIsVisible )
        return pEdge;
    }
   
    ++itEdge;
  }

  return NULL;
}

 // construct a path from start vertex
template <class T, class E>
static void constructPath(OdGiExtents3dSpacePoint* pStartVertex, OdArray<OdGiExtents3dSpacePoint*>& points,
                   OdArray<E*>& edges, OdList<T*>& polylines)
{
  if ( pStartVertex == NULL )
    return;
  
  OdGiExtents3dSpacePoint* pFirstVertex = pStartVertex;
 
  T *pPath = NULL;
  //loop
  while(pFirstVertex)
  { 
    //get unvisited edge
    OdGiExtents3dSpaceEdge* pFirstEdge = getFirstUnvisitedEdge(pFirstVertex, edges);
    if ( pFirstEdge == NULL )
    {
      pFirstVertex->setVisited(true);
      break;
    }

    // get the second vertex of the edge
    OdGiExtents3dSpacePoint* pNextVertex = points[pFirstEdge->getSecondVertex(pFirstVertex->getID())];
  
    //init the path
    if ( pPath == NULL )
    {
      pPath = new T(0);
      pPath->addVertex(pFirstVertex);
    }
    pPath->addVertex(pNextVertex);
    
    pFirstVertex->setVisited(true);
    pNextVertex->setVisited(true);
    pFirstEdge->setVisited(true);
  
    pFirstVertex = pNextVertex;
  }

  if ( pPath )
    polylines.push_back(pPath);

  return;
}

public:
template <class T, class E>
static OdInt64 calculateChainPolylinesFromEdges(OdArray<OdGiExtents3dSpacePoint*>& points, OdArray<E*>& edges, OdList<T*>& polylines )
{
  //1. Find the vertices with power 1 if exists
   OdList<OdGiExtents3dSpacePoint*> leafVertices;
   int iVertNumber = points.size();
   for (OdUInt64 i = 0; i < iVertNumber; i++)
   {
     OdGiExtents3dSpacePoint* pVert = points[i];
     if ( pVert )
     {
       OdUInt64 vPower = pVert->getPower();
       if ( vPower == 1 )
         leafVertices.push_back(pVert);
       else if ( vPower == 0 )
         pVert->setVisited(true);
     }
   }

  //2.  run loop for construction the pathes
  OdGiExtents3dSpacePoint* pStartVertex = getNextStartVertex(points, leafVertices);
  while ( pStartVertex )
  {
    constructPath(pStartVertex, points, edges, polylines);
    pStartVertex = getNextStartVertex(points, leafVertices);
  }

  return polylines.size();
}
};

#include "TD_PackPop.h"

#endif //#ifndef __ODGIEXTENTS3DSPACETREE_H__
