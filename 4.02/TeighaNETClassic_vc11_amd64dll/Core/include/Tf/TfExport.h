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


#ifndef _TFCOREEXPORT_DEFINED
#define _TFCOREEXPORT_DEFINED

#include "OdPlatformSettings.h"

#ifdef TFCORE_DLL_EXPORTS
  #define TFCORE_EXPORT           OD_TOOLKIT_EXPORT
  #define TFCORE_EXPORT_STATIC    OD_STATIC_EXPORT
#else
  #define TFCORE_EXPORT           OD_TOOLKIT_IMPORT
  #define TFCORE_EXPORT_STATIC    OD_STATIC_IMPORT
#endif

//////////////////////////////////////////////////////////////////////////

#endif  // _TFCOREEXPORT_DEFINED



