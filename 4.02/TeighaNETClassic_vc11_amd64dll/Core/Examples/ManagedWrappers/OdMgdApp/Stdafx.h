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
#pragma warning(disable:4793)

#include <vector>
#include <vcclr.h>
#include <msclr/auto_handle.h>
#include <afxwin.h>
#include "OdaCommon.h"
#include "DbDatabase.h"
#include "ExDbCommandContext.h"
#include "Ed/EdCommandStack.h"
#include "../../win/OdaMfcApp/OdaMfcExport.h"

using namespace msclr;
using namespace System;
using namespace System::Windows;
using namespace System::Windows::Forms;
using namespace System::Reflection;
using namespace System::Collections;

#include "../../../ManagedWrappers/MacroHelper.h"

#define IMLEMENT_SIMPLE_EVENT( Type, Name, ArgType )  \
  event Type^ Name                                    \
  {                                                   \
    void add( Type^ value )                           \
    {                                                 \
      m_p##Name += value;                             \
    }                                                 \
    void remove( Type^ value )                        \
    {                                                 \
      m_p##Name -= value;                             \
    }                                                 \
    void raise( Object^ sender, ArgType^ args )       \
    {                                                 \
      if ( nullptr != m_p##Name )                     \
        m_p##Name( sender, args );                    \
    }                                                 \
  }

#define DECLARE_IMP(ClassName) ClassName* GetImpObj(){return (ClassName*)UnmanagedObject.ToPointer();}
