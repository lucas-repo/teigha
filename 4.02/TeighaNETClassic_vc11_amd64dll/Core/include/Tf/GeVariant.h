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

#ifndef __GE_VARIANT_H__
#define __GE_VARIANT_H__

#include "Tf/TfExport.h"

#include "Ge/GeCurveBoundary.h"
#include "Ge/GeEntity2d.h"
#include "Ge/GeEntity3d.h"
#include "Ge/GeExtents2d.h"
#include "Ge/GeExtents3d.h"
#include "Ge/GeInterval.h"
#include "Ge/GeKnotVector.h"
#include "Ge/GeMatrix2d.h"
#include "Ge/GeMatrix3d.h"
#include "Ge/GePoint2d.h"
#include "Ge/GePoint3d.h"
#include "Ge/GeQuaternion.h"
#include "Ge/GeScale2d.h"
#include "Ge/GeScale3d.h"
#include "Ge/GeVector2d.h"
#include "Ge/GeVector3d.h"

/** \details
  Corresponding C++ library: TD_Ge
  <group OdGe_Classes>
*/
class TFCORE_EXPORT OdGeVariant {
public:
  typedef enum {
    kEmpty,
    kGeCurveBoundary,
    kGeEntity2d,
    kGeEntity3d,
    kGeExtents2d,
    kGeExtents3d,
    kGeInterval,
    kGeKnotVector,
    kGeMatrix2d,
    kGeMatrix3d,
    kGePoint2d,
    kGePoint3d,
    kGeQuaternion,
    kGeScale2d,
    kGeScale3d,
    kGeVector2d,
    kGeVector3d
  } Type;

private:
  int m_type;
  void* m_data;

  virtual void setVarType(int newType, int& type, void*& data);
public:
  struct TypeFactory {
    virtual void construct(void*& data) const = 0;
    virtual void destroy(void*& data) const = 0;
  };
  static const TypeFactory* typeFactory(int type);

  void makeEmpty();

  virtual ~OdGeVariant();

  /** \details
    Returns the type of this GeVariant object.
  */
  int  varType() const { return m_type; }
  /** \details
    Returns the type of this GeVariant object.
  */
  Type type()    const { return Type(m_type); }

  OdGeVariant();
  OdGeVariant(const OdGeVariant&);
  OdGeVariant(const OdGeCurveBoundary& value);
  OdGeVariant(const OdGeEntity2d& value);
  OdGeVariant(const OdGeEntity3d& value);
  OdGeVariant(const OdGeExtents2d& value);
  OdGeVariant(const OdGeExtents3d& value);
  OdGeVariant(const OdGeInterval& value);
  OdGeVariant(const OdGeKnotVector& value);
  OdGeVariant(const OdGeMatrix2d& value);
  OdGeVariant(const OdGeMatrix3d& value);
  OdGeVariant(const OdGePoint2d& value);
  OdGeVariant(const OdGePoint3d& value);
  OdGeVariant(const OdGeQuaternion& value);
  OdGeVariant(const OdGeScale2d& value);
  OdGeVariant(const OdGeScale3d& value);
  OdGeVariant(const OdGeVector2d& value);
  OdGeVariant(const OdGeVector3d& value);
  
  OdGeVariant& operator =(const OdGeVariant&);
  operator OdGeCurveBoundary() const;
  operator OdGeExtents2d() const;
  operator OdGeExtents3d() const;
  operator OdGeInterval() const;
  operator OdGeKnotVector() const;
  operator OdGeMatrix2d() const;
  operator OdGeMatrix3d() const;
  operator OdGePoint2d() const;
  operator OdGePoint3d() const;
  operator OdGeQuaternion() const;
  operator OdGeScale3d() const;
  operator OdGeVector2d() const;
  operator OdGeVector3d() const;
  
  void setGeCurveBoundary(const OdGeCurveBoundary& value);
  void setGeEntity2d(const OdGeEntity2d& value);
  void setGeEntity3d(const OdGeEntity3d& value);
  void setGeExtents2d(const OdGeExtents2d& value);
  void setGeExtents3d(const OdGeExtents3d& value);
  void setGeInterval(const OdGeInterval& value);
  void setGeKnotVector(const OdGeKnotVector& value);
  void setGeMatrix2d(const OdGeMatrix2d& value);
  void setGeMatrix3d(const OdGeMatrix3d& value);
  void setGePoint2d(const OdGePoint2d& value);
  void setGePoint3d(const OdGePoint3d& value);
  void setGeQuaternion(const OdGeQuaternion& value);
  void setGeScale2d(const OdGeScale2d& value);
  void setGeScale3d(const OdGeScale3d& value);
  void setGeVector2d(const OdGeVector2d& value);
  void setGeVector3d(const OdGeVector3d& value);
  
  const OdGeCurveBoundary& getGeCurveBoundary() const;
  const OdGeEntity2d& getGeEntity2d() const;
  const OdGeEntity3d& getGeEntity3d() const;
  const OdGeExtents2d& getGeExtents2d() const;
  const OdGeExtents3d& getGeExtents3d() const;
  const OdGeInterval& getGeInterval() const;
  const OdGeKnotVector& getGeKnotVector() const;
  const OdGeMatrix2d& getGeMatrix2d() const;
  const OdGeMatrix3d& getGeMatrix3d() const;
  const OdGePoint2d& getGePoint2d() const;
  const OdGePoint3d& getGePoint3d() const;
  const OdGeQuaternion& getGeQuaternion() const;
  const OdGeScale2d& getGeScale2d() const;
  const OdGeScale3d& getGeScale3d() const;
  const OdGeVector2d& getGeVector2d() const;
  const OdGeVector3d& getGeVector3d() const;
};
  
#endif // __GE_VARIANT_H__
