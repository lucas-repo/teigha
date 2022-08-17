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

#include "OdaCommon.h"
#include "NETLOADCommand.h"
#include "../Extensions/ExServices/ExStringIO.h" // via Include
#include "Ed/EdCommandContext.h"
#include "Ed/EdUserIO.h"
//#include "DbCommandContext.h"
#include "RxObjectImpl.h"
#include "DbDatabase.h"
#include "DbHostAppServices.h"


using namespace System::Reflection;

// this temporary implementation just searches assembly for method 
// marked by "CommandMethod" attribute and executes it
// In future this command should store object instance, register 
// appropriate command in command stack and bind it to the method of the object
// (The command should also be removed from the core module to extensions 
// - it is surely application specific)

void NETLOAD_cmd::execute(OdEdCommandContext* pCmdCtx)
{
  using namespace Teigha::Runtime;
  try
  {
    // (support for TeighaViewer)
    String^ s = "";
    OdSmartPtr<ExStringIO> pStringIO = pCmdCtx->arbitraryData( L"NETLOAD/PATH" );
    if (pStringIO.get())
    {
      s = gcnew String((const wchar_t*)pStringIO->getString( L"Assembly path", OdEd::kGstAllowSpaces ));
    }

    if ( s->Length == 0 )
    {
      s = gcnew String((const wchar_t*)pCmdCtx->userIO()->getFilePath( L"Assembly path" ));
    }
    Assembly^ a = Assembly::LoadFile(s);

    array< Object^ >^ attribs = a->GetCustomAttributes( ExtensionApplicationAttribute::typeid, true );
    array< Type^ >^   aTypes  = nullptr;
    if ( attribs->Length == 0 )
    {
      aTypes = a->GetTypes();
    }
    else
    {
      ExtensionApplicationAttribute^ attInit = (ExtensionApplicationAttribute^) attribs[ 0 ];
      aTypes = gcnew array< Type^ > { attInit->Type };
    }

    for each( Type^ t in aTypes )
    {
      if (!t->IsClass)
        continue;

      if ( !Teigha::Runtime::IExtensionApplication::typeid->IsAssignableFrom( t ) )
        continue;

      try
      {
        IExtensionApplication^ pApp = (IExtensionApplication^) Activator::CreateInstance( t );
        pApp->Initialize();
        _appInits.push_back( pApp );
      }
      catch (...){}
      break;
    }

    attribs = a->GetCustomAttributes( CommandClassAttribute::typeid, true );
    if ( attribs->Length == 0 )
    {
      aTypes = a->GetTypes();
    }
    else
    {
      int i, iSize = attribs->Length;
      aTypes = gcnew array< Type^ >( iSize );
      for ( i = 0; i < iSize; i++ )
      {
        CommandClassAttribute^ attInit = ( CommandClassAttribute^ ) attribs[ i ];
        aTypes[ i ] = attInit->Type;
      }
    }
    for each(Type^ t in aTypes)
    {
      if (t->IsClass)
      {
        for each(MethodInfo^ m in t->GetMethods())
        {
          for each (Object^ attr in m->GetCustomAttributes(Teigha::Runtime::CommandMethodAttribute::typeid, true))
          {
            Teigha::Runtime::CommandMethodAttribute^ c = dynamic_cast<Teigha::Runtime::CommandMethodAttribute^>(attr);
            pin_ptr<const wchar_t> globalName = c->GlobalName == nullptr ? L"" : PtrToStringChars(c->GlobalName);
            pin_ptr<const wchar_t> localName = c->LocalizedNameId == nullptr ? L"" : PtrToStringChars(c->LocalizedNameId);
            pin_ptr<const wchar_t> groupName = c->GroupName == nullptr ? L"" : PtrToStringChars(c->GroupName);
            OdSmartPtr<CMD> cmd = OdRxObjectImpl<CMD>::createObject();
            cmd->_Asm = a;
            cmd->_pType = t;
            cmd->_methodName = m->Name;
            cmd->_globalName = globalName;
            cmd->_localName = localName;
            cmd->_groupName = groupName;
            cmd->_flags = (unsigned)c->Flags;;
            odedRegCmds()->addCommand(cmd);
            _commands.push_back(cmd);
          }
        }
      }
    }
  }
  catch (System::Exception^ ex)
  {
    String^ sStr = ex->ToString();
    pin_ptr<const wchar_t> pExStr = PtrToStringChars(sStr);
    pCmdCtx->userIO()->putString(OdString((wchar_t*)pExStr, sStr->Length));
  }
}

void NETLOAD_cmd::unloadCommands()
{
  for (unsigned i = 0; i < _commands.size(); ++i)
  {
    odedRegCmds()->removeCmd(_commands[i]);
  }
  _commands.clear();

  for (unsigned i = 0; i < _appInits.size(); ++i)
  {
    _appInits[ i ]->Terminate();
  }
  _appInits.clear();
}

void NETLOAD_cmd::CMD::execute(OdEdCommandContext* pCommandContext)
{
  //OdDbCommandContext::cast(pCommandContext)->database(); // check for no database exception (support for TeighaViewer)

  // setup host app services
  Teigha::DatabaseServices::HostApplicationServices^ oldHostApp = Teigha::DatabaseServices::HostApplicationServices::Current;

  Teigha::Runtime::RXObject^ pObj = Teigha::Runtime::RXObject::Create(IntPtr(((OdDbDatabase*)pCommandContext->baseDatabase())->appServices()), false);
  Teigha::DatabaseServices::HostApplicationServices^ appSrv = (Teigha::DatabaseServices::HostApplicationServices^)pObj;
  Teigha::DatabaseServices::HostApplicationServices::Current = appSrv;
  // setup working database
  Teigha::DatabaseServices::Database^ db = (Teigha::DatabaseServices::Database^)Teigha::Runtime::RXObject::Create(IntPtr(pCommandContext->baseDatabase()), false);
  Teigha::DatabaseServices::Database^ oldDb = Teigha::DatabaseServices::HostApplicationServices::WorkingDatabase;
  Teigha::DatabaseServices::HostApplicationServices::WorkingDatabase = db;
  // invoke the command
  Object^ ClassObj = Activator::CreateInstance(_pType);
  try
  {
    _pType->InvokeMember(_methodName, BindingFlags::Default | BindingFlags::InvokeMethod, nullptr, ClassObj, nullptr);
  }
  catch (System::Exception^ ex)
  {
    pin_ptr<const wchar_t> m = PtrToStringChars(ex->ToString());
    OutputDebugStringW(m);
  }
  finally
  {
    Teigha::DatabaseServices::HostApplicationServices::WorkingDatabase = oldDb;
    Teigha::DatabaseServices::HostApplicationServices::Current = oldHostApp;

    GC::Collect();
    GC::WaitForPendingFinalizers();
  }
}
