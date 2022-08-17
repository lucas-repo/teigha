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

#ifndef __ODGSMODEL_H_INCLUDED_
#define __ODGSMODEL_H_INCLUDED_

#include "Gs/Gs.h"

class OdGiPathNode;

#include "TD_PackPush.h"

/** \details

    Corresponding C++ library: TD_Gs

    <group OdGs_Classes> 
*/
class FIRSTDLL_EXPORT ODRX_ABSTRACT OdGsCache : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdGsCache);

  /** \details
    Returns pointer to the Model object associated with this Cache object.
  */
  virtual OdGsModel* model() const = 0;

  /** \details
    Reset pointer to the underlying GiDrawable object associated with this Cache object.
  */
  virtual void setDrawableNull() = 0;

  /** \details
    Returns the extents of the underlying GiDrawable object(s) associated with this Cache object.
    
    \param extents [out]  Receives the extents.
    
    \returns
    Returns true if and only if the GiDrawable object(s) have extents.
  */
  virtual bool extents(OdGeExtents3d& extents) const = 0;
};

typedef OdGiDrawablePtr (*OdGiOpenDrawableFn)(OdDbStub* id);

class OdGsModelReactor;

/** \details
    The class represents collections of drawable objects in the Teigha framework.

    Corresponding C++ library: TD_Gs

    <group OdGs_Classes> 
*/
class FIRSTDLL_EXPORT ODRX_ABSTRACT OdGsModel : public OdRxObject
{
public:
  ODRX_DECLARE_MEMBERS(OdGsModel);

  /** \details
    Specifies the function called by open() for this Model object.
    \param openDrawableFn [in]  Function to be called.
  */
  virtual void setOpenDrawableFn(OdGiOpenDrawableFn openDrawableFn) = 0;

  /** \details
    Notification function called whenever a drawable object is added to this Model object.

    \param pAdded [in]  Pointer to the added Drawable object.
    \param pParent [in]  Pointer to the parent of the added Drawable object.
  */
  virtual void onAdded(OdGiDrawable* pAdded, OdGiDrawable* pParent) = 0;

  /** \param parentID [in]  Object ID of the parent of the added Drawable object.
  */
  virtual void onAdded(OdGiDrawable* pAdded, OdDbStub* parentID) = 0;
  
  /** \details
    Notification function called whenever a drawable object is modified in this Model object.

    \param pModified [in]  Pointer to the modified Drawable object.
    \param pParent [in]  Pointer to the parent of the modified Drawable object.
  */
  virtual void onModified(OdGiDrawable* pModified, OdGiDrawable* pParent) = 0;

  /** \param parentID [in]  Object ID of the parent of the modified Drawable object.
  */
  virtual void onModified(OdGiDrawable* pModified, OdDbStub* parentID) = 0;
  
  /** \details
    Notification function called whenever a drawable object is erased from this Model object.

    \param pErased [in]  Pointer to the erased Drawable object.
    \param pParent [in]  Pointer to the parent of the erased Drawable object.
  */
  virtual void onErased(OdGiDrawable* pErased, OdGiDrawable* pParent) = 0;

  /** \param parentID [in]  Object ID of the parent of the erased Drawable object.
  */
  virtual void onErased(OdGiDrawable* pErased, OdDbStub* parentID) = 0;

  // Invalidation Hint
  
  enum InvalidationHint
  {
    kInvalidateIsolines       = 0,
    kInvalidateViewportCache  = 1,
    kInvalidateAll            = 2,
    kInvalidateMaterials      = 3,
    kInvalidateLinetypes      = 4
  };

  /** \details
    Invalidates the specified cached data contained in this Model object.
    \param pView [in]  Pointer to the VectorizeView object for which data are to be invalidated.
    \param hint [in]  Invalidation hint.
    
    \remarks
    hint must be one of the following:
    
    <table>
    Name                        Value
    kInvalidateIsolines         0
    kInvalidateViewportCache    1
    kInvalidateAll              2
    </table>
    
  */
  virtual void invalidate(InvalidationHint hint) = 0;

  virtual void invalidate(OdGsView* pView) = 0;

  virtual void setTransform(const OdGeMatrix3d&) = 0;

  virtual OdGeMatrix3d transform() const = 0;

  virtual void highlight(const OdGiPathNode& path, bool bDoIt = true, const OdGsView* pView = 0) = 0;

  enum RenderType
  { 
    kMain = 0,           // Use main Z-buffer
    kSprite,             // Use alternate Z-buffer (for sprites)
    kDirect,             // Render on device directly
    kHighlight,          // Render on device directly (skipping frame buffer and Z-buffer)
    kHighlightSelection, // Render on device directly using highlighting style (skipping frame buffer and Z-buffer)
    kDirectTopmost,      // Render on top of all other render types without Z-buffer
    kContrast,           // Render with contrast style
    kUser1,
    kUser2,
    kUser3,
    kCount               // Count of RenderTypes
  };

  virtual void setRenderType(RenderType renderType) = 0;
  virtual RenderType renderType() const = 0;

  //virtual bool addSceneGraphRoot(OdGiDrawable* pRoot) = 0;
  //virtual bool eraseSceneGraphRoot(OdGiDrawable* pRoot) = 0;

  //virtual void onPaletteModified() = 0;
  //virtual bool getTransformAt(const OdGsPath*, OdGeMatrix3d &) = 0;
  //virtual void setExtents(const OdGePoint3d&, const OdGePoint3d&) = 0;
  //virtual void setViewClippingOverride(bool bOverride) = 0;
  //virtual void setMaterialsOverride(bool bOverride) = 0;
  virtual void setRenderModeOverride(OdGsView::RenderMode mode = OdGsView::kNone) = 0;

  enum AdditionMode
  {
    kAddDrawable = 0, // Add new drawable
    kUneraseDrawable  // Added drawables are unerased
  };

  virtual void setAdditionMode(AdditionMode mode) = 0;
  virtual AdditionMode additionMode() const = 0;

  virtual void setBackground(OdDbStub *backgroundId) = 0;
  virtual OdDbStub *background() const = 0;

  virtual void setVisualStyle(OdDbStub *visualStyleId) = 0;
  virtual OdDbStub *visualStyle() const = 0;
  virtual void setVisualStyle(const OdGiVisualStyle &visualStyle) = 0;
  virtual bool visualStyle(OdGiVisualStyle &visualStyle) const = 0;

  virtual void addModelReactor(OdGsModelReactor *pReactor) = 0;
  virtual void removeModelReactor(OdGsModelReactor *pReactor) = 0;

  virtual void setEnableSectioning(bool bEnable) = 0;
  virtual bool isSectioningEnabled() const = 0;
  virtual bool setSectioning(const OdGePoint3dArray &points, const OdGeVector3d &upVector) = 0;
  virtual bool setSectioning(const OdGePoint3dArray &points, const OdGeVector3d &upVector,
                             double dTop, double dBottom) = 0;
  virtual void setSectioningVisualStyle(OdDbStub *visualStyleId) = 0;
};

/** \details
    GsModel reactor.

    Corresponding C++ library: TD_Gs
    <group OdGs_Classes> 
*/
class OdGsModelReactor
{
  public:
    OdGsModelReactor() { }
    virtual ~OdGsModelReactor() { }

    //virtual bool onSceneGraphRootAdded(OdGsModel *pModel, OdGiDrawable *pAdded) { return true; }
    //virtual bool onSceneGraphRootErased(OdGsModel *pModel, OdGiDrawable *pErased) { return true; }

    virtual bool onAdded(OdGsModel *pModel, OdGiDrawable *pAdded, OdGiDrawable *pParent) { return true; }
    virtual bool onAdded(OdGsModel *pModel, OdGiDrawable *pAdded, OdDbStub *parentID) { return true; }

    virtual bool onErased(OdGsModel *pModel, OdGiDrawable *pErased, OdGiDrawable *pParent) { return true; }
    virtual bool onErased(OdGsModel *pModel, OdGiDrawable *pErased, OdDbStub *parentID) { return true; }

    virtual bool onModified(OdGsModel *pModel, OdGiDrawable *pModified, OdGiDrawable *pParent) { return true; }
    virtual bool onModified(OdGsModel *pModel, OdGiDrawable *pModified, OdDbStub *parentID) { return true; }
};

#include "TD_PackPop.h"

#endif // __ODGSMODEL_H_INCLUDED_
