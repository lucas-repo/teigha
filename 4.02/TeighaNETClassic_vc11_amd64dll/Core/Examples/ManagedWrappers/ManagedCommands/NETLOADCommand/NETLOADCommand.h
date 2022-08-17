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
#pragma once
#include "RxObject.h"
#include "RxModule.h"
#include <vector>
#include <vcclr.h>
#include <gcroot.h>
#include "Ed/EdCommandStack.h"
#include "StaticRxObject.h"


using namespace System;

class NETLOAD_cmd : public OdEdCommand
{
public:
  class CMD : public OdEdCommand
  {
  public:
    gcroot<System::Reflection::Assembly^> _Asm;
    gcroot<System::Type^> _pType;
    gcroot<System::String^> _methodName;
    OdString _globalName;
    OdString _localName;
    OdString _groupName;
    OdInt32 _flags;
    virtual const OdString groupName() const
    { 
      return _groupName; 
    }
    virtual const OdString globalName() const
    {
      return _globalName;
    }
    virtual const OdString localName() const 
    {
      if (_localName.isEmpty() )
        return _globalName;
      return _localName;
    }
    virtual OdInt32 flags() const
    {
      return _flags;
    }
    virtual void execute(OdEdCommandContext* pCommandContext);
  };
  std::vector<OdSmartPtr<CMD> > _commands;
  std::vector< gcroot< Teigha::Runtime::IExtensionApplication^ > > _appInits;
  void unloadCommands();

  const OdString groupName() const 
  { 
    return OD_T("ManagedCommands"); 
  }
  const OdString globalName() const 
  { 
    return OD_T("NETLOAD"); 
  }
  void execute(OdEdCommandContext* pCmdCtx);
};
