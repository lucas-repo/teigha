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

#ifndef TYPEVER_H
#define TYPEVER_H /*!DOM*/

/** See also: AfTypeVerEnum 
*/
typedef int AfTypeVer;

enum AfTypeVerEnum
{
  kAfVerMask           = 0x00FFFFFF,
  kAfVerInvalid        = kAfVerMask,
  kAfVerAny            = 0x00000000,
  kAfVerUnknown        = kAfVerAny,

  kAfVer105            = 105,
  kAfVer106            = 106,
  kAfVer107            = 107,
  kAfVer200            = 200,
  kAfVer201            = 201,
  kAfVer400            = 400,
  kAfVer500            = 500,
  kAfVer600            = 600,
  kAfVer700            = 700,
  kAfVer1000           = 1000,
  kAfVer1100           = 1100,
  kAfVer20800          = 20800,
  kAfVer21200          = 21200,
  kAfVer21500          = 21500,
  kAfVer21600          = 21600,
  kAfVer21700          = 21700,
  kAfVer21800          = 21800,
#if defined(OD_SWIGCSHARP)
  kAfTypeMask          = (uint)0xFF000000,
#elif defined(OD_SWIGJAVA)
  kAfTypeMask          = (long)0xFF000000,
#else
  kAfTypeMask          = (OdInt32)0xFF000000,
#endif
  kAfTypeInvalid       = kAfTypeMask,
  kAfTypeAny           = 0x00000000,
  kAfTypeUnknown       = kAfTypeAny,

  kAfTypeASCII         = 0x01000000,
  kAfTypeBinary        = 0x02000000,
#if defined(OD_SWIGCSHARP)
  kAfTypeIndexed       = (uint)0x80000000, // used only with kAfTypeASCII
#elif defined(OD_SWIGJAVA)
  kAfTypeIndexed       = (long)0x80000000, // used only with kAfTypeASCII
#else
  kAfTypeIndexed       = (OdInt32)0x80000000, // used only with kAfTypeASCII
#endif

  
  kAfTypeVerInvalid    = kAfTypeInvalid | kAfVerInvalid,
  kAfTypeVerAny        = kAfTypeAny     | kAfVerAny,
  kAfTypeVerUnknown    = kAfTypeVerAny,

  //////// these constants are for convenience //////////

  kAf_ASCII_Any        = kAfTypeASCII|kAfVerAny,
  kAf_ASCII_106        = kAfTypeASCII|kAfVer106,
  kAf_ASCII_400        = kAfTypeASCII|kAfVer400,
  kAf_ASCII_500        = kAfTypeASCII|kAfVer500,
  kAf_ASCII_600        = kAfTypeASCII|kAfVer600,
  kAf_ASCII_700        = kAfTypeASCII|kAfVer700,
  kAf_ASCII_20800      = kAfTypeASCII|kAfVer20800,
  kAf_ASCII_21200      = kAfTypeASCII|kAfVer21200,
  kAf_ASCII_21500      = kAfTypeASCII|kAfVer21500,

  kAf_Binary_Any       = kAfTypeBinary|kAfVerAny,
  kAf_Binary_106       = kAfTypeBinary|kAfVer106,
  kAf_Binary_400       = kAfTypeBinary|kAfVer400,
  kAf_Binary_500       = kAfTypeBinary|kAfVer500,
  kAf_Binary_600       = kAfTypeBinary|kAfVer600,
  kAf_Binary_700       = kAfTypeBinary|kAfVer700,
  kAf_Binary_20800     = kAfTypeBinary|kAfVer20800,
  kAf_Binary_21200     = kAfTypeBinary|kAfVer21200,
  kAf_Binary_21500     = kAfTypeBinary|kAfVer21500
};

/** \details
  
    <group TD_Namespaces>
*/
namespace OdDb
{
  enum BoolOperType
  {
    kBoolUnite     = 0,
    kBoolIntersect = 1,
    kBoolSubtract  = 2
  };
}

#endif //TYPEVER_H
