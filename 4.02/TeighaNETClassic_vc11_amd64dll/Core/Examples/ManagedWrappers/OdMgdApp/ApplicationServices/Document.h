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

#include "../stdafx.h"
#include "AppEvents.h"

namespace Teigha
{
  namespace EditorInput
  {
    ref class Editor;
  }

  namespace Windows
  {
    ref class Window;
  }
  
  using namespace DatabaseServices;

  namespace ApplicationServices
  {
    ref class TransactionManager;

    public ref class Document sealed : public Runtime::DisposableWrapper
    {
      internal:
        DECLARE_IMP( OdApDocument );

        Document( CDocument* pDoc );

        static Document^ Create( CDocument* pDoc );

      public:
        
        void CloseAndDiscard();
        
        void CloseAndSave(String^ fileName);
        
        void SendStringToExecute(String^ command, bool activate, bool wrapUpInactiveDoc, bool echoCommand);
        
        Database^ TryGetDatabase();

      public:
        DECLARE_PROP_GET( EditorInput::Editor^, Editor );

        DECLARE_PROP_GET( DatabaseServices::Database^, Database );
      
        DECLARE_PROP_GET( String^, CommandInProgress );

        //property Manager GraphicsManager;

        DECLARE_PROP_GET( bool, IsActive );

        DECLARE_PROP_GET( bool, IsReadOnly );

        DECLARE_PROP_GET( String^, Name );

        //property StatusBar StatusBar;

        DECLARE_PROP_GET( ApplicationServices::TransactionManager^, TransactionManager );

        DECLARE_PROP_GET( Hashtable^, UserData );

        DECLARE_PROP_GET( Windows::Window^, Window );

      public:
        IMLEMENT_SIMPLE_EVENT( DocumentBeginCloseEventHandler, BeginDocumentClose, DocumentBeginCloseEventArgs );

        IMLEMENT_SIMPLE_EVENT( EventHandler, CloseAborted, EventArgs );

        IMLEMENT_SIMPLE_EVENT( EventHandler, CloseWillStart, EventArgs );

        IMLEMENT_SIMPLE_EVENT( CommandEventHandler, CommandCancelled, CommandEventArgs );

        IMLEMENT_SIMPLE_EVENT( CommandEventHandler, CommandEnded, CommandEventArgs );

        IMLEMENT_SIMPLE_EVENT( CommandEventHandler, CommandFailed, CommandEventArgs );

        IMLEMENT_SIMPLE_EVENT( CommandEventHandler, CommandWillStart, CommandEventArgs );

        IMLEMENT_SIMPLE_EVENT( EventHandler, ImpliedSelectionChanged, EventArgs );

        IMLEMENT_SIMPLE_EVENT( EventHandler, LispCancelled, EventArgs );

        IMLEMENT_SIMPLE_EVENT( EventHandler, LispEnded, EventArgs );

        IMLEMENT_SIMPLE_EVENT( LispWillStartEventHandler, LispWillStart, LispWillStartEventArgs );

        IMLEMENT_SIMPLE_EVENT( UnknownCommandEventHandler, UnknownCommand, UnknownCommandEventArgs );

      private:
        void CloseInternal( bool discard, String^ fileName );

      protected:
        virtual void DeleteUnmanagedObject() sealed override;

      private:
        Hashtable^ m_userData;

        DocumentBeginCloseEventHandler^ m_pBeginDocumentClose;
        EventHandler^ m_pCloseAborted;
        EventHandler^ m_pCloseWillStart;
        CommandEventHandler^ m_pCommandCancelled;
        CommandEventHandler^ m_pCommandEnded;
        CommandEventHandler^ m_pCommandFailed;
        CommandEventHandler^ m_pCommandWillStart;
        EventHandler^ m_pImpliedSelectionChanged;
        EventHandler^ m_pLispCancelled;
        EventHandler^ m_pLispEnded;
        LispWillStartEventHandler^ m_pLispWillStart;
        UnknownCommandEventHandler^ m_pUnknownCommand;

      internal:
        ArrayList^ m_CmdQueue;
    };
  }
}
