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

// RxRcsFileServices.h - interface of module that handles .rcs files

#ifndef __OD_RCSFILESERVICES_MODULE__
#define __OD_RCSFILESERVICES_MODULE__

#include "RxModule.h"
#include "RootExport.h"

#include "TD_PackPush.h"
#include "SharedPtr.h"
#include "Ge/GePoint3d.h"
#include "Ge/GePoint3dArray.h"
#include "CmEntityColorArray.h"
#include "Ge/GeVector3d.h"
#include "Ge/GeMatrix3d.h"
#include "Ge/GeExtents3d.h"

/** \details
  <group OdRcs_Classes>
  This class is an interface class for the points data iterator that enables getting points from a voxel.

  Corresponding C++ library: RcsFileServices
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRcsPointDataIterator
{
public:
  virtual ~OdRcsPointDataIterator(){};

  /** \details    
    Moves the current position to the beginning of the points data within the current voxel.
  */
  virtual void start() = 0;  
  
  /** \details
    Returns true if and only if the traversal by this Iterator object is complete.
  */
  virtual bool done() const = 0;

  /** \details
    Fills the coordinates and colors arrays. The iterator object steps forward.
    
    \param requiredQty [in]  Required number of points to obtain.
    \param points [out]  Points coordinates array to fill.
    \param colorArray [out]  Points colors array to fill.

    \returns
    Returns the actual number of the points obtained.
  */
  virtual OdUInt64 getPoints(OdGePoint3dArray& points, OdCmEntityColorArray& colorArray, 
    OdUInt64 requiredQty) = 0;
};

typedef OdSharedPtr<OdRcsPointDataIterator> OdRcsPointDataIteratorPtr;

/** \details
  <group OdRcs_Classes>
  This class is an interface class for the voxel cube containing points.

  Corresponding C++ library: RcsFileServices
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRcsVoxel
{
public:
  virtual ~OdRcsVoxel(){};

  /** \details
    Returns the pointer to the points data iterator.
  */
  virtual OdRcsPointDataIteratorPtr newPointDataIterator() const = 0;

  /** \details
    Returns the extents of the voxel.
  */
  virtual OdGeExtents3d getBox1() const = 0;
};

typedef OdSharedPtr<OdRcsVoxel> OdRcsVoxelPtr;

/** \details
  <group OdRcs_Classes>
  This class is an interface class for the voxel data iterator that enables getting voxels from an .rcs file.

  Corresponding C++ library: RcsFileServices
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRcsVoxelDataIterator
{
public:
  virtual ~OdRcsVoxelDataIterator(){};

  /** \details
    Returns the pointer to the current voxel. The iterator object steps forward.
  */
  virtual OdRcsVoxelPtr getVoxel() = 0;

  /** \details
    Moves the current position to the beginning of the voxels data.
  */
  virtual void start() = 0;  
  
  /** details
    Returns true if and only if the traversal by this Iterator object is complete.
  */
  virtual bool done() const = 0;  
};

typedef OdSharedPtr<OdRcsVoxelDataIterator> OdRcsVoxelDataIteratorPtr;

/** \details
  <group OdRcs_Classes>
  This class is the interface class for the point clouds data reader that enables getting info from .rcs file.

  Corresponding C++ library: RcsFileServices
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRcsFileReader
{
public:  
  virtual ~OdRcsFileReader(){};

  /** \details
    Returns the pointer to the voxel data iterator.
  */
  virtual OdRcsVoxelDataIteratorPtr newVoxelDataIterator() const = 0;

  /** \details
    Returns the translation vector.
  */
  virtual OdGeVector3d getTranslation() const = 0;
  
  /** \details
    Returns the rotation vector. Each of this vector's coordinates is a rotation angle around the corresponding axis.
  */
  virtual OdGeVector3d getRotation() const = 0;
  
  /** \details
    Returns the scale vector.
  */
  virtual OdGeVector3d getScale() const = 0;

  /** \details
    Returns the complete transformation matrix.
  */
  virtual OdGeMatrix3d getTransformMatrix() const = 0;

  /** \details
    Returns the hasRGB flag.
  */
  virtual bool hasRGB() const = 0;
  
  /** \details
    Returns the hasNormals flag.
  */
  virtual bool hasNormals() const = 0;
  
  /** \details
    Returns the hasIntensity flag.
  */
  virtual bool hasIntensity() const = 0;
  
  /** \details
    Returns the scan Id string.
  */
  virtual OdString getScanIdString() const = 0;
};

typedef OdSharedPtr<OdRcsFileReader> OdRcsFileReaderPtr;

/** \details
  This class is an interface class for the module that provides loading point cloud data from .rcs files for Teigha

  Corresponding C++ library: RcsFileServices

  <group OdRx_Classes>
*/
class ODRX_ABSTRACT FIRSTDLL_EXPORT OdRxRcsFileServices : public OdRxModule
{
public:

  /** \details
    Reads the header of the .rcs file using specified file path.
    \returns
    Returns the pointer to the reader object.

    \param filePath [in]  Path to the file to read.
  */
  virtual OdRcsFileReaderPtr getRcsFileReader(const OdString& filePath) const = 0;
};

typedef OdSmartPtr<OdRxRcsFileServices> OdRxRcsFileServicesPtr;

#include "TD_PackPop.h"

#endif // __OD_RCSFILESERVICES_MODULE__
