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

namespace Teigha
{
  namespace Windows
  {
    using namespace Teigha::Runtime;
    using namespace System;
    using namespace System::Windows;
    using namespace System::Windows::Forms;
    using namespace System::Drawing;

    public ref class Window : public DisposableWrapper, IWin32Window
    {
      internal:
        virtual CWnd* GetWnd();

      private protected:
        Window( CWnd* pWnd, bool autoDelete );
        Window( HWND  hWnd, bool autoDelete );

      public:
        void Close();

        static Vector GetDeviceIndependentScale( IntPtr hWnd );

        virtual property IntPtr Handle
        {
          IntPtr get();
        }

        DECLARE_PROP( Drawing::Point, DeviceIndependentLocation );

        DECLARE_PROP( Drawing::Size, DeviceIndependentSize );

        DECLARE_PROP( Drawing::Icon^, Icon );

        DECLARE_PROP( Drawing::Point, Location );

        DECLARE_PROP( Drawing::Size, Size );

        DECLARE_PROP( String^, Text );

        DECLARE_PROP( bool, Visible );

        DECLARE_PROP( FormWindowState, WindowState );

      protected:
        virtual void DeleteUnmanagedObject() override;

      private:
        Vector GetDeviceIndependentScale();
    };

    private ref class NonFinalizableWindow : public Window
    {
      public:
        NonFinalizableWindow( CWnd* pWnd ) : Window( pWnd, true )
        {
          GC::SuppressFinalize(this);
        }
      protected:
        NonFinalizableWindow( HWND hWnd ) : Window( hWnd, true )
        {
          GC::SuppressFinalize(this);
        }
    };

    private ref class WindowOnHandle : public NonFinalizableWindow
    {
      public:
        WindowOnHandle( HWND hWnd ) : NonFinalizableWindow( hWnd ) {}

      internal:
        virtual CWnd* GetWnd() sealed override;
    };
  }
}
