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




#ifndef _ODAXWRAP_H_INCLUDED_
#define _ODAXWRAP_H_INCLUDED_

#if defined(_MSC_VER) && (_MSC_VER > 1310) && defined(_TOOLKIT_IN_DLL_)
#include "TD_PackPush.h"

#define _INC_MALLOC
#include <comdef.h>

#include "oaidl.h"
#include "OdaX2.h"
class OdDbObjectId;
class OdGePoint3d;
class OdGeVector3d;
class OdGePoint2d;
class OdGeVector2d;
class OdDbHostAppServices;

interface __declspec(uuid("C9E0781D-BA3D-4224-9FA4-58ECEA2BC559")) 
IAcadBaseObject : public IUnknown
{
  // IAcadBaseObject methods
  virtual HRESULT STDMETHODCALLTYPE SetObjectId(OdDbObjectId& objId, 
    OdDbObjectId ownerId = OdDbObjectId::kNull, TCHAR* keyName = 0) = 0;
  virtual HRESULT STDMETHODCALLTYPE GetObjectId(OdDbObjectId* objId) = 0;
  virtual HRESULT STDMETHODCALLTYPE Clone(OdDbObjectId ownerId, LPUNKNOWN* pUnkClone) = 0;
  virtual HRESULT STDMETHODCALLTYPE GetClassID(CLSID& clsid) = 0;
  virtual HRESULT STDMETHODCALLTYPE NullObjectId() = 0;
  virtual HRESULT STDMETHODCALLTYPE OnModified() = 0;
};

typedef IAcadBaseObject* LPACADBASEOBJECT;


// Definition of interface: IRetrieveApplication
interface __declspec(uuid("0E25DE83-2257-4b6d-B73B-33F1D21FFD8D"))
IRetrieveHostAppServices : IUnknown
{
  virtual HRESULT STDMETHODCALLTYPE GetHostAppServices(OdDbHostAppServices** ppHostAppServices) = 0;
};

_COM_SMARTPTR_TYPEDEF(IRetrieveHostAppServices, __uuidof(IRetrieveHostAppServices));

// Definition of interface: IRetrieveApplication
interface __declspec(uuid("765B4640-664A-11cf-93F3-0800099EB3B7")) 
IRetrieveApplication : public IUnknown
{
  // IRetrieveApplication methods
  virtual HRESULT STDMETHODCALLTYPE SetApplicationObject(LPDISPATCH pAppDisp) = 0;
  virtual HRESULT STDMETHODCALLTYPE GetApplicationObject(LPDISPATCH* pAppDisp) = 0;
};

typedef IRetrieveApplication* LPRETRIEVEAPPLICATION;

// Definition of interface: IAcadBaseDatabase
interface __declspec(uuid("CD3EB5B8-F3FC-48c2-84EE-954EFC4D4208")) 
IAcadBaseDatabase : public IUnknown
{
  // IAcadBaseObject methods
  virtual HRESULT STDMETHODCALLTYPE SetDatabase(OdDbDatabase*& pDb) = 0;
  virtual HRESULT STDMETHODCALLTYPE GetDatabase(OdDbDatabase** pDb) = 0;
  virtual HRESULT STDMETHODCALLTYPE GetClassID(CLSID& clsid) = 0;
};

typedef IAcadBaseDatabase* LPACADBASEDATABASE;


_COM_SMARTPTR_TYPEDEF(IAcadBaseObject, __uuidof(IAcadBaseObject));
_COM_SMARTPTR_TYPEDEF(IRetrieveApplication, __uuidof(IRetrieveApplication));
_COM_SMARTPTR_TYPEDEF(IAcadBaseDatabase, __uuidof(IAcadBaseDatabase));


// {4D07FC10-F931-11ce-B001-00AA006884E5}
DEFINE_GUID(IID_ICategorizeProperties, 0x4d07fc10, 0xf931, 0x11ce, 0xb0, 0x1, 0x0, 0xaa, 0x0, 0x68, 0x84, 0xe5);

// category ID: negative values are 'standard' categories,  positive are control-specific
#define PROPCAT_Nil -1
#define PROPCAT_Misc -2
#define PROPCAT_Font -3
#define PROPCAT_Position -4
#define PROPCAT_Appearance -5
#define PROPCAT_Behavior -6
#define PROPCAT_Data -7
#define PROPCAT_List -8
#define PROPCAT_Text -9
#define PROPCAT_Scale -10
#define PROPCAT_DDE -11
#define PROPCAT_General -12
#define PROPCAT_Mass -13
#define PROPCAT_Pattern -14
#define PROPCAT_DataPoints -15
#define PROPCAT_Mesh -16
#define PROPCAT_ImageAdjust -17
#define PROPCAT_ControlPoints -18
#define PROPCAT_PrimaryUnits -19
#define PROPCAT_AltUnits -20
#define PROPCAT_Fit -21
#define PROPCAT_LinesArrows -22
#define PROPCAT_Tolerances -23
#define PROPCAT_Geometry -24
#define PROPCAT_Table -25
#define PROPCAT_3dVisualization -26
#define PROPCAT_UnderlayAdjust -27
#define PROPCAT_SectionObject -28


typedef int PROPCAT;

#ifndef __OBJEXT_H
interface __declspec(uuid("4D07FC10-F931-11ce-B001-00AA006884E5"))
ICategorizeProperties : public IUnknown
{
  // Return a property category for the specified property.
  virtual HRESULT STDMETHODCALLTYPE MapPropertyToCategory(
    /*[in]*/ DISPID dispid, /*[out]*/ PROPCAT* ppropcat) = 0;

  // Return the name associated with the specified category ID, as a BSTR.
  virtual HRESULT STDMETHODCALLTYPE GetCategoryName(
    /*[in]*/ PROPCAT propcat, /*[in]*/ LCID lcid, /*[out]*/BSTR* pbstrName) = 0;
};

typedef ICategorizeProperties FAR* LPCATEGORIZEPROPERTIES;
#endif

//
// Utility functions
//
TOOLKIT_EXPORT IUnknown* OdOxGetIUnknownOfObject(OdDbObjectId objId, LPDISPATCH pApp);
//TOOLKIT_EXPORT IUnknown* OdOxGetIUnknownOfObject(OdDbObject* pObj, LPDISPATCH pApp);
TOOLKIT_EXPORT IUnknown* OdOxGetIUnknownOfDatabase(OdDbDatabase* pDb, LPDISPATCH pApp);


#define FACILITY_ODA                  32

#define OdHresultFromOdResult(res)    MAKE_HRESULT(res!=eOk ? 3 : 0, FACILITY_ODA, res)

#include "TD_PackPop.h"
#endif //_WIN32

#endif // _ODAXWRAP_H_INCLUDED_

