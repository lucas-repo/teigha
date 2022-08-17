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


#ifndef _INC_DDBRBREPCOMPLEXTRAVERSER_3F83F47F036B_INCLUDED
#define _INC_DDBRBREPCOMPLEXTRAVERSER_3F83F47F036B_INCLUDED

#include "Br/BrBrep.h"
#include "Br/BrComplex.h"
#include "Br/BrTraverser.h"

#include "TD_PackPush.h"

/** \details
    This class is the interface class for BREP complex traversers.
    
    \sa
    TD_Br

    <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrBrepComplexTraverser : public OdBrTraverser
{
public:
	OdBrBrepComplexTraverser();

  /** \details
    Sets this Traverser object to a specific BREP complex list.
    \param brep [in]  Owner of the complex list.
    
    \remarks
    This Traverser object is set to the first element in the list.
    
    Returns odbrOK if successful, or an appropriate error code if not.
  */
  OdBrErrorStatus setBrep(const OdBrBrep& brep);

  /** \details
    Sets the starting position of this Traverser object.
    \param complex [in]  Defines the starting position.
  */
  OdBrErrorStatus setComplex(const OdBrComplex& complex);

  /** \details
    Sets this Traverser object to a specific BREP complex list
    and starting position.
    \param complex [in]  Defines the complex list and starting position.
  */
  OdBrErrorStatus setBrepAndComplex(const OdBrComplex& complex);

  /** \details
    Returns the complex object at the current Traverser position.
  */
  OdBrComplex getComplex() const;


  /** \details
    Returns the owner of the complex list associated with this Traverser object.
  */
  OdBrBrep getBrep() const;
};

#include "TD_PackPop.h"

#endif /* _INC_DDBRBREPCOMPLEXTRAVERSER_3F83F47F036B_INCLUDED */

