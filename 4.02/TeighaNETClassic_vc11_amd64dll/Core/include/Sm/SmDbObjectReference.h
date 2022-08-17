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




#ifndef _SmDbObjectReference_h_Included_
#define _SmDbObjectReference_h_Included_

#include "Sm/SmFileReference.h"

/** \details
  
    <group TD_Namespaces>
*/
namespace dst
{
  /** \details

       
  */
  struct SMDLL_EXPORT OdSmDbObjectReference : OdSmFileReference
  {
    ODRX_DECLARE_MEMBERS( OdSmDbObjectReference );
    virtual void setDbHandle(const OdString& ) = 0;
    virtual OdString getDbHandle() const = 0;
  };
  typedef OdSmartPtr<OdSmDbObjectReference> OdSmDbObjectReferencePtr;
}

#endif //_SmDbObjectReference_h_Included_
