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




#ifndef OD_DBTABLECONTENT_H
#define OD_DBTABLECONTENT_H

#include "DbObject.h"
#include "DbFormattedTableData.h"

#include "TD_PackPush.h"

	/** \details
    Work-in-progress. This class represents TableContent objects in an OdDbDatabase instance.

    \sa
    TD_Db

	<group OdDb_Classes>
	*/
class TOOLKIT_EXPORT OdDbTableContent : public OdDbFormattedTableData
{
public:
  ODDB_DECLARE_MEMBERS(OdDbTableContent);

  OdDbTableContent();

  virtual OdDbObjectId tableStyleId() const;

  virtual void setTableStyleId(
    const OdDbObjectId& idTableStyle);

  virtual const OdString cellStyle(
    OdInt32 nRow, 
    OdInt32 nCol) const;

  virtual void setCellStyle(
    OdInt32 nRow, 
    OdInt32 nCol, 
    const OdString& pszCellStyle);

  virtual double rowHeight(
    OdInt32 nRow) const;

  virtual void setRowHeight(
    OdInt32 nRow, 
    double fHeight);

  virtual double columnWidth(
    OdInt32 nCol) const;

  virtual void setColumnWidth(
    OdInt32 nCol, 
    double fWidth);

  virtual OdResult dwgInFields(
    OdDbDwgFiler* pFiler);
  virtual void dwgOutFields(
    OdDbDwgFiler* pFiler) const;
  virtual OdResult dxfInFields(
    OdDbDxfFiler* pFiler);
  virtual void dxfOutFields(
    OdDbDxfFiler* pFiler) const;
};

typedef OdSmartPtr<OdDbTableContent> OdDbTableContentPtr;

#include "TD_PackPop.h"

#endif // OD_DBTABLECONTENT_H
