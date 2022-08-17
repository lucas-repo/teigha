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

#include "Document.h"
#include "TransactionManager.h"
#include "../EditorInput/Editor.h"
#include "../Windows/Window.h"
#include <map>

namespace Teigha
{
  namespace ApplicationServices
  {
    class OdMgdCmdReactor : public OdEdCommandStackReactor
    {
      public:
        static OdSmartPtr< OdMgdCmdReactor > createObject( Document^ doc )
        {
          TD_START_WRAP_EXCEPTIONS;
          OdSmartPtr< OdMgdCmdReactor > pRes = OdRxObjectImpl< OdMgdCmdReactor >::createObject();
          pRes->m_Doc = gcnew WeakReference( doc );
          return pRes;
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual void commandAdded(OdEdCommand* )
        {
        }
        
        virtual void commandWillBeRemoved(OdEdCommand* )
        {
        }
        
        virtual void commandWillStart(OdEdCommand* pCommand, OdEdCommandContext* )
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( m_Doc->IsAlive )
            ((Document^) m_Doc->Target)->CommandWillStart( m_Doc->Target, gcnew CommandEventArgs( gcnew String(( const wchar_t* ) pCommand->globalName())));
          TD_END_WRAP_EXCEPTIONS;
        }
        
        virtual void commandEnded(OdEdCommand* pCommand, OdEdCommandContext* )
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( m_Doc->IsAlive )
            ((Document^) m_Doc->Target)->CommandEnded( m_Doc->Target, gcnew CommandEventArgs( gcnew String(( const wchar_t* ) pCommand->globalName())));
          TD_END_WRAP_EXCEPTIONS;
        }
        
        virtual void commandCancelled(OdEdCommand* pCommand, OdEdCommandContext* )
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( m_Doc->IsAlive )
            ((Document^) m_Doc->Target)->CommandCancelled( m_Doc->Target, gcnew CommandEventArgs( gcnew String(( const wchar_t* ) pCommand->globalName())));
          TD_END_WRAP_EXCEPTIONS;
        }
        
        virtual void commandFailed(OdEdCommand* pCommand, OdEdCommandContext* )
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( m_Doc->IsAlive )
            ((Document^) m_Doc->Target)->CommandFailed( m_Doc->Target, gcnew CommandEventArgs( gcnew String(( const wchar_t* ) pCommand->globalName())));
          TD_END_WRAP_EXCEPTIONS;
        }
        
        virtual OdEdCommandPtr unknownCommand(const OdString& commandName, OdEdCommandContext* )
        {
          TD_START_WRAP_EXCEPTIONS;
          if ( m_Doc->IsAlive )
            ((Document^) m_Doc->Target)->UnknownCommand( m_Doc->Target, gcnew UnknownCommandEventArgs( gcnew String(( const wchar_t* ) commandName )));

          return OdEdCommandPtr();
          TD_END_WRAP_EXCEPTIONS;
        }

      private:
        gcroot< WeakReference^ > m_Doc;
    };

    Document::Document( CDocument* pDoc )
      : Runtime::DisposableWrapper( IntPtr( odGetAppDocument( pDoc ).get() ), true )
    {
      TD_START_WRAP_EXCEPTIONS;
      GetImpObj()->addRef();
      ::odedRegCmds()->addReactor( OdMgdCmdReactor::createObject( this ).get() );
      TD_END_WRAP_EXCEPTIONS;
    }

    Document^ Document::Create( CDocument* pDoc )
    {
      typedef std::map< CDocument*, gcroot< WeakReference^ > > DocColl;
      static DocColl g_aDocs;

      DocColl::iterator it = g_aDocs.find( pDoc );
      if ( it == g_aDocs.end() || !it->second->IsAlive )
      {
        Document^ doc = gcnew Document( pDoc );
        g_aDocs[ pDoc ] = gcnew WeakReference( doc );
        return doc;
      }
      return (Document^)it->second->Target;
    }

    EditorInput::Editor^ Document::Editor::get()
    {
      return gcnew EditorInput::Editor( this );
    }

    void Document::DeleteUnmanagedObject()
    {
      TD_START_WRAP_EXCEPTIONS;
      GetImpObj()->release();
      TD_END_WRAP_EXCEPTIONS;
    }

    DatabaseServices::Database^ Document::Database::get()
    {
      return (DatabaseServices::Database^) Runtime::RXObject::Create( IntPtr( GetImpObj()->database().get() ), true );
    }

    String^ Document::CommandInProgress::get()
    {
      return String::Empty; // TODO:
    }

    bool Document::IsActive::get()
    {
      TD_START_WRAP_EXCEPTIONS;
      CMDIFrameWnd* pMainWnd = (CMDIFrameWnd*)AfxGetMainWnd();
      CFrameWnd* pActive = pMainWnd->GetActiveFrame();
      return pActive->GetActiveDocument() == GetImpObj()->cDoc();
      TD_END_WRAP_EXCEPTIONS;
    }

    bool Document::IsReadOnly::get()
    {
      return false;
    }

    String^ Document::Name::get()
    {
      return gcnew String( (const wchar_t*) GetImpObj()->cDoc()->GetTitle() );
    }

    ApplicationServices::TransactionManager^ Document::TransactionManager::get()
    {
      return gcnew ApplicationServices::TransactionManager( Database->TransactionManager->UnmanagedObject, true );
    }

    Hashtable^ Document::UserData::get()
    {
      if ( nullptr == m_userData )
        m_userData = gcnew Hashtable();

      return m_userData;
    }

    Windows::Window^ Document::Window::get()
    {
      CDocument* pDoc = GetImpObj()->cDoc();
      POSITION pos = pDoc->GetFirstViewPosition();
      CFrameWnd* pFrame = pDoc->GetNextView( pos )->GetParentFrame();
      if ( nullptr == pFrame )
        return nullptr;

      return gcnew Windows::NonFinalizableWindow( pFrame );
    }

    void Document::CloseAndDiscard()
    {
      CloseInternal( true, nullptr );
    }
    
    void Document::CloseAndSave( String^ fileName )
    {
      CloseInternal( false, fileName );
    }
    
    void Document::CloseInternal( bool discard, String^ fileName )
    {
      if ( !discard )
      {
        pin_ptr< const wchar_t > pName = PtrToStringChars( fileName );
        GetImpObj()->cDoc()->DoSave( pName );
      }
      GetImpObj()->cDoc()->OnCloseDocument();
    }

    value class CmdParams
    {
      public:
        CmdParams( String^ command, bool activate, bool wrapUpInactiveDoc, bool echoCommand )
        {
          Command           = command;
          Activate          = activate;
          WrapUpInactiveDoc = wrapUpInactiveDoc;
          EchoCommand       = echoCommand;
        }
        property String^ Command;
        property bool Activate;
        property bool WrapUpInactiveDoc;
        property bool EchoCommand;
    };

    class OdMgdDocCmdReactor : public OdApplicationReactor
    {
      public:
        static OdSmartPtr< OdMgdDocCmdReactor > createObject( Document^ doc )
        {
          TD_START_WRAP_EXCEPTIONS;
          OdSmartPtr< OdMgdDocCmdReactor > pRes = OdRxObjectImpl< OdMgdDocCmdReactor >::createObject();
          pRes->m_doc = doc;
          return pRes;
          TD_END_WRAP_EXCEPTIONS;
        }

        virtual void documentToBeDestroyed( CDocument* pDoc )
        {
          if ( pDoc == m_doc->GetImpObj()->cDoc() )
            m_doc = nullptr;
        }

        virtual void DocumentActivated( CDocument* pActivatedDoc )
        {
          if ( pActivatedDoc != m_doc->GetImpObj()->cDoc() )
            return;

          for each ( CmdParams^ par in m_doc->m_CmdQueue )
            m_doc->SendStringToExecute( par->Command, par->Activate, par->WrapUpInactiveDoc, par->EchoCommand );

          m_doc->m_CmdQueue = nullptr;
          m_doc = nullptr;
        }

      private:
        gcroot< Document^ > m_doc;
    };

    void Document::SendStringToExecute( String^ command, bool activate, bool wrapUpInactiveDoc, bool echoCommand )
    {
      TD_START_WRAP_EXCEPTIONS;
      CMDIFrameWnd* pMainFrame = (CMDIFrameWnd*) AfxGetMainWnd();
      CDocument* pActiveDoc = pMainFrame->GetActiveDocument();
      CDocument* pDoc = GetImpObj()->cDoc();

      if ( pDoc != pActiveDoc && wrapUpInactiveDoc )
      {
        if ( m_CmdQueue == nullptr )
        {
          m_CmdQueue = gcnew ArrayList();
          odAddAppReactor( OdMgdDocCmdReactor::createObject( this ).get() );
        }

        m_CmdQueue->Add( gcnew CmdParams( command, activate, wrapUpInactiveDoc, echoCommand ) );
        return;
      }

      if ( pDoc != pActiveDoc && !activate )
        return;

      POSITION pos = pDoc->GetFirstViewPosition();
      CView* pView = pDoc->GetNextView( pos );
      CMDIChildWnd* pFrame = (CMDIChildWnd*) pView->GetParentFrame();
      pFrame->ActivateFrame();

      pin_ptr< const wchar_t > pCmd = PtrToStringChars( command );
      GetImpObj()->ExecuteCommand( pCmd, echoCommand );
      TD_END_WRAP_EXCEPTIONS;
    }

    Database^ Document::TryGetDatabase()
    {
      OdDbDatabasePtr pDb = GetImpObj()->database();
      if ( pDb.isNull() )
        return nullptr;
      return (DatabaseServices::Database^) Runtime::RXObject::Create( IntPtr( pDb.get() ), true );
    }
  }
}
