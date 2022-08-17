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


/*  OdAllocExport.h

*/
#ifndef _OD_ALLOCEXPORT_DEFINED
#define _OD_ALLOCEXPORT_DEFINED


#include "OdPlatformSettings.h"

#ifdef ALLOC_DLL_EXPORTS
  #define ALLOCDLL_EXPORT        OD_TOOLKIT_EXPORT
  #define ALLOCDLL_EXPORT_STATIC OD_STATIC_EXPORT
#else
#ifdef EMCC
  #define ALLOCDLL_EXPORT           __attribute__((visibility("default")))
  #define ALLOCDLL_EXPORT_STATIC    __attribute__((visibility("default")))
#else
  #define ALLOCDLL_EXPORT           OD_TOOLKIT_IMPORT
  #define ALLOCDLL_EXPORT_STATIC    OD_STATIC_IMPORT
#endif
#endif

//////////////////////////////////////////////////////////////////////////

#endif  /* _OD_ALLOCEXPORT_DEFINED */
