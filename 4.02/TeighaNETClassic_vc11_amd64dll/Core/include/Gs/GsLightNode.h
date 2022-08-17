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

#ifndef __OD_GS_LIGHT_NODE__
#define __OD_GS_LIGHT_NODE__

#include "TD_PackPush.h"

#include "Gs/GsEntityNode.h"
#include "Gi/GiLightTraitsData.h"

// Lights are entities that have some special meaning

/** <group OdGs_Classes> 
*/
class GS_TOOLKIT_EXPORT OdGsLightNode : public OdGsEntityNode
{ 
  OdGeMatrix3d m_model2World; // In case light isn't in model space
  OdGiLightTraitsData* m_pLightTraits;
#ifndef AM_CORE10435
  OdGsLightNode *m_pNextLight; // Idea with LightList exchanging failed since light node can have multiple VpData entries
  OdRxObjectPtr m_pLightSubData; // #21117 : pointer added for future needs. Probably temporary.
  typedef OdVector<OdIntPtr, OdMemoryAllocator<OdIntPtr> > LightInsertPath;
  struct OdGsLightNode_internal : public OdRxObject
  {
    OdGsNode *m_pLightParent;
    typedef OdVector<OdIntPtr, OdMemoryAllocator<OdIntPtr> > LightInsertPath;
    LightInsertPath m_lightInsertParents;
    OdGsLightNode_internal() : m_pLightParent(NULL) {}
  };
#else // AM_CORE10435
  OdGsNode *m_pLightParent;
  typedef OdVector<OdIntPtr, OdMemoryAllocator<OdIntPtr> > LightInsertPath;
  LightInsertPath m_lightInsertParents;
#endif // AM_CORE10435
protected:
  void update();

public:
  ODRX_DECLARE_MEMBERS(OdGsLightNode);

  ~OdGsLightNode();
  OdGsLightNode(OdGsBaseModel* pModel, const OdGiDrawable* pUnderlyingDrawable, bool bSetGsNode = true);
public:

  void setModelTransform(const OdGeMatrix3d& xform) { m_model2World = xform; }
  OdGeMatrix3d modelTransform() const { return m_model2World; }

  void setLightOwner(OdGsUpdateContext& ctx, OdGsContainerNode* pParent);
  bool isOwnedBy(OdGsUpdateContext& ctx, OdGsContainerNode* pParent) const;
#ifdef AM_CORE10435
  bool isOwnedBy(const OdGsNode *pOwner) const { return m_pLightParent == pOwner; }
  OdGsNode *getLightOwner() const { return m_pLightParent; }
#else // AM_CORE10435
  bool isOwnedBy(const OdGsNode *pOwner) const;
  OdGsNode *getLightOwner() const;
#endif // AM_CORE10435

  OdGiDrawable::DrawableType drawableType()
  {
    OdGiDrawablePtr pUnderlyingDrawable = underlyingDrawable();
    if (!pUnderlyingDrawable.isNull())
      return underlyingDrawable()->drawableType();
    return (OdGiDrawable::DrawableType)-1; // Requested drawable was erased
  }

  virtual bool isLight() const { return true; }

  OdGiPointLightTraitsData* pointLightTraitsData() { return (OdGiPointLightTraitsData*)m_pLightTraits; }
  OdGiSpotLightTraitsData* spotLightTraitsData() { return (OdGiSpotLightTraitsData*)m_pLightTraits; }
  OdGiDistantLightTraitsData* distantLightTraitsData() { return (OdGiDistantLightTraitsData*)m_pLightTraits; }
  OdGiWebLightTraitsData* webLightTraitsData() { return (OdGiWebLightTraitsData*)m_pLightTraits; }
  OdGiLightTraitsData* lightTraitsData() { return m_pLightTraits; }

  // OdGsNode virtual overrides
  
  virtual void invalidate(OdGsContainerNode* pParent, OdGsViewImpl* pView, OdUInt32 mask);
  virtual void update(OdGsUpdateContext& ctx, OdGsContainerNode* pParent,
      OdSiSpatialIndex*);

  virtual bool saveClientNodeState(OdGsFiler *pFiler, OdGsBaseVectorizer *pVectorizer) const;
  virtual bool loadClientNodeState(OdGsFiler *pFiler, OdGsBaseVectorizer *pVectorizer);

  /** \details
    Returns true if and only if specified drawable is a light entity.
    \param drawableType [in]  Drawable type.
  */
  static bool drawableIsLight(OdGiDrawable::DrawableType drawableType);
};

typedef OdSmartPtr<OdGsLightNode> OdGsLightNodePtr;

inline bool OdGsLightNode::drawableIsLight(OdGiDrawable::DrawableType drawableType)
{
  return (drawableType >= OdGiDrawable::kDistantLight && drawableType <= OdGiDrawable::kSpotLight) || (drawableType == OdGiDrawable::kWebLight);
}

#include "TD_PackPop.h"

#endif // __OD_GS_LIGHT_NODE__
