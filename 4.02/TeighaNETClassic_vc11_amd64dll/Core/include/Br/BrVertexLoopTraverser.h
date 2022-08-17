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



#ifndef _INC_DDBRVERTEXLOOPTRAVERSER_3F83F8DC0000_INCLUDED
#define _INC_DDBRVERTEXLOOPTRAVERSER_3F83F8DC0000_INCLUDED

#include "Br/BrTraverser.h"

class OdBrLoopVertexTraverser;
class OdBrVertex;
class OdBrLoop;

#include "TD_PackPush.h"


/** \details
  This class is the interface class for vertex loop traversers.

  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrVertexLoopTraverser : public OdBrTraverser
{
public:
  OdBrVertexLoopTraverser();

  /** \details
    Sets this Traverser object to a specific vertex edge list.
    \param vertex [in]  Owner of the edge list.
    
    \remarks
    This Traverser object is set to the first element in the list.

    Returns odbrOK if successful, or an appropriate error code if not.
  */
  OdBrErrorStatus setVertex(const OdBrVertex& vertex);

  /** \details
    Sets the starting position of this Traverser object.
    \param loop [in]  Defines the starting position.
  */
  OdBrErrorStatus setLoop(const OdBrLoop& loop);
  
  /** \details
    Sets this Traverser object to a specific vertex loop list
    and starting position.
    \param loopVertex [in]  Defines the vertex loop list and starting position.
  */
  OdBrErrorStatus setVertexAndLoop(const OdBrLoopVertexTraverser& loopVertex);

  /** \details
    Returns the loop object at the current Traverser position.
  */
  OdBrLoop getLoop() const;
  
  /** \details
    Returns the owner of the loop list associated with this Traverser object.
  */
  OdBrVertex getVertex() const;
};

#include "TD_PackPop.h"

#endif /* _INC_DDBRVERTEXLOOPTRAVERSER_3F83F8DC0000_INCLUDED */

