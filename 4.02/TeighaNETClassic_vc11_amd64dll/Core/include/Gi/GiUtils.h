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


#ifndef __OD_GI_UTILS__
#define __OD_GI_UTILS__

template <class T3dPtIter>
void odgiSquareValues(OdInt32 nPoints, T3dPtIter pPoints, OdGeVector3d& n1, OdGeVector3d& n2)
{
  OdGeVector3d res;
  nPoints -= 2;
  n1 = n2 = OdGeVector3d::kIdentity;
  for(T3dPtIter pPt1 = pPoints + 1, pPt2 = pPoints + 2; nPoints-- > 0; ++pPt1, ++pPt2)
  {
    res = (*pPt2 - *pPoints).crossProduct(*pPt1 - *pPoints);
    if(res.dotProduct(n1) >= 0.)
      n1 += res;
    else
      n2 += res;
  }
}

template <class T3dPtIter>
OdGeVector3d odgiFaceNormal(OdInt32 nPoints, T3dPtIter pPoints)
{
  OdGeVector3d n1, n2;
  odgiSquareValues(nPoints, pPoints, n1, n2);
  n1 += n2;
  OdGe::ErrorCondition f;
  n1.normalize(OdGeContext::gZeroTol, f);
  if(f!=OdGe::kOk)
    return n2.normalize(OdGeContext::gZeroTol, f);
  return n1;
}


#endif // __OD_GI_UTILS__
