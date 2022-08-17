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

#include "../Stdafx.h"
#include "../Windows/Window.h"
#include "../Windows/Menu.h"
#include "AppEvents.h"

namespace Teigha
{
  namespace ApplicationServices
  {
    ref class Document;
    ref class DocumentCollection;
    ref class MainListener;
   // ref class DocumentWindowCollection;

    public ref class Application sealed
    {
        static Application()
        {
          InitClass();
        }

      internal:
        static void InitClass();

        static CMDIFrameWnd* GetMainFrame();

      public:
        static Object^ GetSystemVariable( String^ name );

        static void SetSystemVariable( String^ name, Object^ value );

        static bool IsFileLocked( String^ pathname );

        static void Quit();

        static void ShowAlertDialog( String^ message );

        static DialogResult ShowModalDialog( Form^ formToShow );
        static DialogResult ShowModalDialog( IWin32Window^ owner, Form^ formToShow );
        static DialogResult ShowModalDialog( IWin32Window^ owner, Form^ formToShow, bool persistSizeAndPosition );

        static void ShowModelessDialog( Form^ formToShow );
        static void ShowModelessDialog( IWin32Window^ owner, Form^ formToShow );
        static void ShowModelessDialog( IWin32Window^ owner, Form^ formToShow, bool persistSizeAndPosition );

        static void AddDefaultContextMenuExtension( Windows::ContextMenuExtension^ menuExtension );
        static void RemoveDefaultContextMenuExtension( Windows::ContextMenuExtension^ menuExtension );

        static void AddObjectContextMenuExtension( Runtime::RXClass^ runtimeClass, Windows::ContextMenuExtension^ menuExtension );
        static void RemoveObjectContextMenuExtension( Runtime::RXClass^ runtimeClass, Windows::ContextMenuExtension^ menuExtension );

      public:
        static property DocumentCollection^ DocumentManager
        {
          DocumentCollection^ get();
        }

        static property Windows::Window^ MainWindow
        {
          Windows::Window^ get();
        }

        /*static property ApplicationServices::DocumentWindowCollection^ DocumentWindowCollection
        {
          ApplicationServices::DocumentWindowCollection^ get();
        }*/

      public:
        static IMLEMENT_SIMPLE_EVENT( EventHandler, BeginQuit,   EventArgs );
        static IMLEMENT_SIMPLE_EVENT( EventHandler, EnterModal,  EventArgs );
        static IMLEMENT_SIMPLE_EVENT( EventHandler, Idle,        EventArgs );
        static IMLEMENT_SIMPLE_EVENT( EventHandler, LeaveModal,  EventArgs );
        static IMLEMENT_SIMPLE_EVENT( EventHandler, QuitAborted, EventArgs );
        static IMLEMENT_SIMPLE_EVENT( EventHandler, QuitWillStart, EventArgs );
        static IMLEMENT_SIMPLE_EVENT( PreTranslateMessageEventHandler, PreTranslateMessage, PreTranslateMessageEventArgs );
        static IMLEMENT_SIMPLE_EVENT( SystemVariableChangedEventHandler, SystemVariableChanged, SystemVariableChangedEventArgs );
        static IMLEMENT_SIMPLE_EVENT( SystemVariableChangingEventHandler, SystemVariableChanging, SystemVariableChangingEventArgs );

        //static event TabbedDialogEventHandler^ DisplayingCustomizeDialog;
        //static event TabbedDialogEventHandler^ DisplayingDraftingSettingsDialog;
        //static event TabbedDialogEventHandler^ DisplayingOptionDialog;

      private:
        static EventHandler^ m_pBeginQuit;
        static EventHandler^ m_pEnterModal;
        static EventHandler^ m_pIdle;
        static EventHandler^ m_pLeaveModal;
        static EventHandler^ m_pQuitAborted;
        static EventHandler^ m_pQuitWillStart;
        static PreTranslateMessageEventHandler^ m_pPreTranslateMessage;
        static SystemVariableChangedEventHandler^ m_pSystemVariableChanged;
        static SystemVariableChangingEventHandler^ m_pSystemVariableChanging;

      internal:
        static MainListener^ m_pMainListener;
        static ArrayList^ m_defaultContextMenuExts;
        static Collections::Generic::Dictionary< Runtime::RXClass^, ArrayList^ >^ m_classMenuExts;
    };
  }
}
