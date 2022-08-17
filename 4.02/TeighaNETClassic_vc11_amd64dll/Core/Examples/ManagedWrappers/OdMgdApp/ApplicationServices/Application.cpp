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

#include "Application.h"
#include "Document.h"
#include "DocumentCollection.h"
//#include "DocumentWindowCollection.h"
#include "DbHostAppServices.h"
#include <afxext.h>

namespace Teigha
{
  namespace ApplicationServices
  {
    CMDIFrameWnd* Application::GetMainFrame()
    {
      return (CMDIFrameWnd*) AfxGetMainWnd();
    }

    DocumentCollection^ Application::DocumentManager::get()
    {
      return gcnew DocumentCollection();
    }

    Windows::Window^ Application::MainWindow::get()
    {
      return gcnew Windows::NonFinalizableWindow( GetMainFrame() );
    }

    /*ApplicationServices::DocumentWindowCollection^ Application::DocumentWindowCollection::get()
    {
      return ApplicationServices::DocumentWindowCollection::Create();
    }*/

    template< class T >
    Object^ getManaged( const T& val )
    {
      return val;
    }
    template<>
    Object^ getManaged( const OdDb::LineWeight& val )
    {
      return (DatabaseServices::LineWeight) val;
    }
    template<>
    Object^ getManaged( const OdString& val )
    {
      return gcnew String( (const wchar_t*) val );
    }
    template<>
    Object^ getManaged( const OdDb::ProxyImage& val )
    {
      return ( int ) val;
    }

    template< class T >
    T getNative( Object^ val )
    {
      return ( T ) val;
    }

    template<>
    OdDb::LineWeight getNative( Object^ val )
    {
      return (OdDb::LineWeight) (int) val;
    }

    template<>
    OdString getNative( Object^ val )
    {
      String^ str = (String^) val;
      pin_ptr<const wchar_t> pStr = PtrToStringChars( str );
      return OdString( pStr );
    }

    template<>
    OdDb::ProxyImage getNative( Object^ val )
    {
      return (OdDb::ProxyImage) (int) val;
    }

    Object^ Application::GetSystemVariable( String^ name )
    {
      DatabaseServices::HostApplicationServices^ appSrv = DatabaseServices::HostApplicationServices::Current;
      OdDbHostAppServices* pAppSrv = (OdDbHostAppServices*) appSrv->UnmanagedObject.ToPointer();
      name = name->ToUpper();

#define REGVAR_DEF( type, Name, unused3, unused4, unused5 )\
      if ( name == #Name ) return getManaged( pAppSrv->get##Name() );
#include "SysVarDefs.h"
#undef REGVAR_DEF

      return nullptr;
    }

    void Application::SetSystemVariable( String^ name, Object^ value )
    {
      DatabaseServices::HostApplicationServices^ appSrv = DatabaseServices::HostApplicationServices::Current;
      OdDbHostAppServices* pAppSrv = (OdDbHostAppServices*) appSrv->UnmanagedObject.ToPointer();
      name = name->ToUpper();

#define REGVAR_DEF( type, Name, unused3, unused4, unused5 )\
      if ( name == #Name ) {pAppSrv->set##Name( getNative< type >( value ) ); return;}
#include "SysVarDefs.h"
#undef REGVAR_DEF

      throw gcnew ArgumentException();
    }

    bool Application::IsFileLocked( String^ /*pathname*/ )
    {
      return false; // TODO:
    }

    void Application::Quit()
    {
      ::SendMessage( GetMainFrame()->GetSafeHwnd(), WM_CLOSE, 0, 0 );
    }

    ref class Win32WindowImpl : public IWin32Window
    {
      public:
        Win32WindowImpl( HWND hWnd )
        {
          m_pHandle = IntPtr( hWnd );
        }

        virtual property IntPtr Handle
        {
          IntPtr get()
          {
            return m_pHandle;
          }
        }

      private:
        IntPtr m_pHandle;
    };

    void Application::ShowAlertDialog( String^ message )
    {
      MessageBox::Show( gcnew Win32WindowImpl( AfxGetMainWnd()->GetSafeHwnd() ),
                        message, message, MessageBoxButtons::OK, MessageBoxIcon::Error );
    }

    DialogResult Application::ShowModalDialog( Form^ formToShow )
    {
      return ShowModalDialog( nullptr, formToShow, true );
    }

    DialogResult Application::ShowModalDialog( IWin32Window^ owner, Form^ formToShow )
    {
      return ShowModalDialog( owner, formToShow, true );
    }

    DialogResult Application::ShowModalDialog( IWin32Window^ owner, Form^ formToShow, bool /*persistSizeAndPosition*/ )
    {
      IWin32Window^ pOwner = owner;
      if ( pOwner == nullptr )
        pOwner = gcnew Win32WindowImpl( GetMainFrame()->GetSafeHwnd() );

      // TODO: Add persistSizeAndPosition processing.
      return formToShow->ShowDialog( pOwner );
    }

    void Application::ShowModelessDialog( Form^ formToShow )
    {
      ShowModelessDialog( nullptr, formToShow, true );
    }

    void Application::ShowModelessDialog( IWin32Window^ owner, Form^ formToShow )
    {
      ShowModelessDialog( owner, formToShow, true );
    }

    void Application::ShowModelessDialog( IWin32Window^ owner, Form^ formToShow, bool /*persistSizeAndPosition*/ )
    {
      IWin32Window^ pOwner = owner;
      if ( pOwner == nullptr )
        pOwner = gcnew Win32WindowImpl( GetMainFrame()->GetSafeHwnd() );

      // TODO: Add persistSizeAndPosition processing.
      return formToShow->Show( pOwner );
    }

    ref class CurrentViewerListener : public System::Windows::Forms::NativeWindow
    {
      public:
        void Attach( HWND hWnd )
        {
          if ( Handle != IntPtr::Zero )
            ReleaseHandle();

          AssignHandle( IntPtr( hWnd ) );
        }

      protected:
        void DestroyMenu( HMENU menu )
        {
          CMenu m;
          m.Attach( menu );
          int i, iSize = m.GetMenuItemCount();
          for ( i = 0; i < iSize; i++ )
          {
            MENUITEMINFO mi = {0};
            mi.cbSize = sizeof( MENUITEMINFO );
            m.GetMenuItemInfo( i, &mi, true );
            if ( 0 != ( mi.fMask & MIIM_SUBMENU ) )
              DestroyMenu( mi.hSubMenu );
          }
        }

        HMENU GetMenu()
        {
          CMenu menu;
          menu.CreatePopupMenu();

          Teigha::ApplicationServices::Document^ doc = Teigha::ApplicationServices::Application::DocumentManager->MdiActiveDocument;
          OdDbSelectionSetPtr pSSet = doc->GetImpObj()->selectionSet();

          m_aCurrent = nullptr;
          int iEnts = pSSet.isNull() ? 0 : pSSet->numEntities();
          if ( 0 == iEnts )
          {
            m_aCurrent = Teigha::ApplicationServices::Application::m_defaultContextMenuExts;
          }
          else if ( 1 == iEnts )
          {
            OdDbObjectPtr pObj = pSSet->objectIdArray()[ 0 ].openObject();
            for each ( Runtime::RXClass^ key in Teigha::ApplicationServices::Application::m_classMenuExts->Keys )
            {
              OdRxClass* pClass = (OdRxClass*) key->UnmanagedObject.ToPointer();
              if ( pObj->isKindOf( pClass ) )
              {
                m_aCurrent = Teigha::ApplicationServices::Application::m_classMenuExts[ key ];
                break;
              }
            }
          }

          if ( m_aCurrent != nullptr ) for each ( Teigha::Windows::ContextMenuExtension^ item in m_aCurrent )
          {
            pin_ptr< const wchar_t > pTitle = PtrToStringChars( item->Title );
            HMENU hMenu = CreateMenu( item->MenuItems );
            menu.AppendMenu( MF_POPUP, (UINT_PTR) hMenu, pTitle );

            item->FirePopup( EventArgs::Empty );
          }
          return menu.Detach();
        }

        HMENU CreateMenu( Teigha::Windows::MenuItemCollection^ menuColl )
        {
          CMenu menu;
          menu.CreatePopupMenu();
          for each ( Teigha::Windows::MenuItem^ item in menuColl )
          {
            pin_ptr< const wchar_t > pText = PtrToStringChars( item->Text );
            if ( !item->Visible )
              continue;

            if ( item->MenuItems->Count != 0 )
            {
              HMENU hMenu = CreateMenu( item->MenuItems );
              menu.AppendMenu( MF_POPUP, (UINT_PTR) hMenu, pText );
            }
            else
            {
              int iFlags = MF_STRING;
              if ( item->Checked )
                iFlags |= MFS_CHECKED;
              if ( item->Enabled )
                iFlags |= MFS_ENABLED;
              menu.AppendMenu( iFlags, item->ID, pText );
            }
          }
          return menu.Detach();
        }

        virtual void WndProc( Message %m ) override
        {
          switch ( m.Msg )
          {
            case WM_CONTEXTMENU:
            {
              CPoint point( m.LParam.ToInt32() );
              HWND hWnd = (HWND) m.WParam.ToPointer();
              OnContextMenu( point, hWnd );
              break;
            }
            case WM_MOUSEMOVE:
            {
              // TODO:
              break;
            }
          }
          NativeWindow::WndProc( m );
        }

        void OnContextMenu( CPoint point, HWND /*hWnd*/ )
        {
          HMENU hMenu = GetMenu();
          ::TrackPopupMenu( hMenu, TPM_LEFTBUTTON, point.x, point.y, 0, AfxGetMainWnd()->GetSafeHwnd(), 0 );
          DestroyMenu( hMenu );
        }

      internal:
        ArrayList^ m_aCurrent;
    };

    ref class MainListener : public System::Windows::Forms::NativeWindow
    {
      public:
        MainListener( HWND hWnd )
        {
          AssignHandle( IntPtr( hWnd ) );
          m_editorListener = gcnew CurrentViewerListener();
          ReassignEditor();
        }

        void ReassignEditor()
        {
          CMDIFrameWnd* pWnd = (CMDIFrameWnd*) AfxGetMainWnd();
          CMDIChildWnd* pChild = pWnd->MDIGetActive();
          if ( pChild != nullptr )
          {
            CView* pView = pChild->GetActiveView();
            if ( !pView->IsKindOf( RUNTIME_CLASS( CFormView ) ) )
              m_editorListener->Attach( pView->GetSafeHwnd() );
          }
        }

      protected:
        virtual void WndProc( Message %m ) override
        {
          switch ( m.Msg )
          {
            case WM_COMMAND:
            {
              int iId = m.WParam.ToInt32();
              OnCommand( iId );
              break;
            }
            case WM_INITMENUPOPUP:
            {
              HMENU hMenu = (HMENU) m.WParam.ToPointer();
              int iIndex = LOWORD( m.LParam.ToInt32() );
              bool bSys  = 0 != HIWORD( m.LParam.ToInt32() );
              OnUpdatePopup( hMenu, iIndex, bSys );
              break;
            }
          }
          NativeWindow::WndProc( m );
        }

        void OnCommand( int iId )
        {
          if ( m_editorListener->m_aCurrent == nullptr )
            return;

          for each ( Teigha::Windows::ContextMenuExtension^ ext in m_editorListener->m_aCurrent )
            for each ( Teigha::Windows::MenuItem^ item in ext->MenuItems )
              item->OnCommand( iId );
        }

        void OnUpdatePopup( HMENU /*hMenu*/, int /*iIndex*/, bool )
        {
        }

      internal:
        CurrentViewerListener^ m_editorListener;
    };

    class OdUnmgdApplicationReactor : public OdApplicationReactor
    {
      public:
        virtual void OnBeginQuit()
        {
          Application::BeginQuit( nullptr, gcnew EventArgs() );
        }

        virtual void OnEnterModal()
        {
          Application::EnterModal( nullptr, gcnew EventArgs() );
        }

        virtual void OnIdle( int )
        {
          Application::Idle( nullptr, gcnew EventArgs() );
        }

        virtual void OnLeaveModal()
        {
          Application::LeaveModal( nullptr, gcnew EventArgs() );
        }

        virtual void OnPreTranslateMessage( MSG* pMsg )
        {
          Interop::MSG msg;
          msg.hwnd    = IntPtr( pMsg->hwnd );
          msg.wParam  = IntPtr( (int)pMsg->wParam );
          msg.lParam  = IntPtr( (int)pMsg->lParam );
          msg.message = pMsg->message;
          msg.pt_x    = pMsg->pt.x;
          msg.pt_y    = pMsg->pt.y;
          msg.time    = pMsg->time;

          Application::PreTranslateMessage( nullptr, gcnew PreTranslateMessageEventArgs( msg ) );
        }

        virtual void OnQuitAborted()
        {
          Application::QuitAborted( nullptr, gcnew EventArgs() );
        }

        virtual void OnQuitWillStart()
        {
          Application::QuitWillStart( nullptr, gcnew EventArgs() );
        }

        virtual void documentActivated( CDocument* )
        {
          Teigha::ApplicationServices::Application::m_pMainListener->ReassignEditor();
        }
    };

    void Application::AddDefaultContextMenuExtension( Windows::ContextMenuExtension^ menuExtension )
    {
      if ( menuExtension == nullptr )
        throw gcnew ArgumentNullException( "menuExtension" );

      if ( !m_defaultContextMenuExts->Contains( menuExtension ) )
      {
        m_defaultContextMenuExts->Add( menuExtension );
        //menuExtension->Register();
      }
    }

    void Application::RemoveDefaultContextMenuExtension( Windows::ContextMenuExtension^ menuExtension )
    {
      if ( menuExtension == nullptr )
        throw gcnew ArgumentNullException("menuExtension") ;

      if ( m_defaultContextMenuExts->Contains( menuExtension ) )
      {
        m_defaultContextMenuExts->Remove( menuExtension );
        //menuExtension->Unregister();
      }
    }

    void Application::RemoveObjectContextMenuExtension( Runtime::RXClass^ runtimeClass, Windows::ContextMenuExtension^ menuExtension )
    {
      if ( menuExtension == nullptr )
        throw gcnew ArgumentNullException( "menuExtension" );

      if ( runtimeClass == nullptr )
        throw gcnew ArgumentNullException("runtimeClass");

      if ( m_classMenuExts->ContainsKey( runtimeClass ) )
        m_classMenuExts[ runtimeClass ]->Remove( menuExtension );
      //menuExtension.Unregister(runtimeClass);
    }

    void Application::AddObjectContextMenuExtension( Runtime::RXClass^ runtimeClass, Windows::ContextMenuExtension^ menuExtension )
    {
      if ( menuExtension == nullptr )
        throw gcnew ArgumentNullException( "menuExtension" );

      if ( runtimeClass == nullptr )
        throw gcnew ArgumentNullException("runtimeClass");

      if ( !m_classMenuExts->ContainsKey( runtimeClass ) )
        m_classMenuExts->Add( runtimeClass, gcnew ArrayList() );

      if ( !m_classMenuExts[ runtimeClass ]->Contains( menuExtension ) )
      {
        m_classMenuExts[ runtimeClass ]->Add( menuExtension );
        //menuExtension.Register(runtimeClass);
      }
    }

    void Application::InitClass()
    {
      odAddAppReactor( OdRxObjectImpl< OdUnmgdApplicationReactor >::createObject().get() );

      m_defaultContextMenuExts = gcnew ArrayList();
      m_classMenuExts = gcnew Collections::Generic::Dictionary< Runtime::RXClass^, ArrayList^ >();
      m_pMainListener = gcnew MainListener( AfxGetMainWnd()->GetSafeHwnd() );
    }
  }
}
