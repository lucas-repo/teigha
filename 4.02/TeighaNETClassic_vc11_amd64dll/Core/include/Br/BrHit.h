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

#ifndef _INC_TDBRBREPHIT_
#define _INC_TDBRBREPHIT_

#include "Br/BrEntity.h"

#include "TD_PackPush.h"

/** \details
  <group OdBr_Classes>

  This class is the interface class for contexted hits generated by line containment queries.

  Corresponding C++ library: TD_Br
*/
class ODBR_TOOLKIT_EXPORT OdBrHit
{
public:

  OdBrHit();
  OdBrHit( const OdBrHit& src );
  virtual ~OdBrHit();
                 
  bool isEqualTo( const OdBrHit* pOtherHit ) const;
  bool isNull() const;

  // Topological Containment
  OdBrErrorStatus   getEntityHit( OdBrEntity*& entityHit ) const;

  OdBrErrorStatus   getEntityEntered( OdBrEntity*& entityEntered ) const;

  // Topology
  OdBrErrorStatus   getEntityAssociated( OdBrEntity*& entity ) const;

  // Geometry
  OdBrErrorStatus   getPoint( OdGePoint3d& point ) const;

  // Validation
  OdBrErrorStatus	  setValidationLevel(const BrValidationLevel& validationLevel);
  OdBrErrorStatus  	getValidationLevel(BrValidationLevel& validationLevel) const;
  bool brepChanged() const;
protected:
  void *m_pHitImp;
  bool m_bIsValidate; //The enum has only 2 values.
  friend class OdBrEntityInternals;
};


#include "TD_PackPop.h"

#endif /* _INC_TDBRBREPHIT_*/

