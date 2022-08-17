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

#include "Window.h"

namespace Teigha
{
  namespace Windows
  {
    Window::Window( CWnd* pWnd, bool autoDelete )
      : DisposableWrapper( IntPtr( pWnd ), autoDelete )
    {
    }

    Window::Window( HWND hWnd, bool autoDelete )
      : DisposableWrapper( IntPtr( hWnd ), autoDelete )
    {
    }

    CWnd* Window::GetWnd()
    {
      return (CWnd*) UnmanagedObject.ToPointer();
    }

    void Window::Close()
    {
      ::PostMessage( GetWnd()->GetSafeHwnd(), WM_CLOSE, 0, 0 );
    }

    void Window::DeleteUnmanagedObject()
    {
      Close();
    }

    IntPtr Window::Handle::get()
    {
      return IntPtr( GetWnd()->GetSafeHwnd() );
    }

    Vector Window::GetDeviceIndependentScale( IntPtr hWnd )
    {
      Vector vector = Vector( 1, 1 );
      HWND hWndConv = (HWND) hWnd.ToPointer();
      if ( hWndConv != 0 )
      {
        HDC windowDC = ::GetWindowDC( hWndConv );
        if ( windowDC != 0 )
        {
          vector.X = ::GetDeviceCaps( windowDC, LOGPIXELSX ) / 96.0;
          vector.Y = ::GetDeviceCaps( windowDC, LOGPIXELSY ) / 96.0;
          ::ReleaseDC( hWndConv, windowDC );
        }
      }
      return vector;
    }

    Vector Window::GetDeviceIndependentScale()
    {
      CWnd* pWnd = GetWnd();
      if ( pWnd != nullptr )
        return Window::GetDeviceIndependentScale( IntPtr( pWnd->GetSafeHwnd() ) );

      return Vector( 1, 1 ) ;
    }

    Drawing::Point Window::DeviceIndependentLocation::get()
    {
      Drawing::Point point = Drawing::Point( 0, 0 ) ;
      Vector deviceIndependentScale = GetDeviceIndependentScale();

      double x = Location.X;
      point.X = (int)(x / deviceIndependentScale.X);

      double y = Location.Y;
      point.Y = (int)(y / deviceIndependentScale.Y);

      return point;
    }

    void Window::DeviceIndependentLocation::set( Drawing::Point value )
    {
      Vector deviceIndependentScale = GetDeviceIndependentScale();
      Drawing::Point point = Drawing::Point( (int) ( deviceIndependentScale.X * value.X ), (int) ( deviceIndependentScale.Y * value.Y ) );
      Location = point;
    }

    Drawing::Size Window::DeviceIndependentSize::get()
    {
      Drawing::Size size = Drawing::Size( 0, 0 );
      Vector deviceIndependentScale = GetDeviceIndependentScale();

      double width = Size.Width;
      size.Width = (int)(width / deviceIndependentScale.X);

      double height = Size.Height;
      size.Height = (int)(height / deviceIndependentScale.Y);

      return size;
    }

    void Window::DeviceIndependentSize::set( Drawing::Size value )
    {
      Vector deviceIndependentScale = GetDeviceIndependentScale();
      Drawing::Size size = Drawing::Size( (int) ( deviceIndependentScale.X * value.Width ), (int) ( deviceIndependentScale.Y * value.Height ) );
      Size = size;
    }

    Drawing::Icon^ Window::Icon::get()
    {
      HICON hIcon = GetWnd()->GetIcon( true );
      if ( hIcon == 0 )
        hIcon = GetWnd()->GetIcon( false );
      
      if ( hIcon == 0 )
        return nullptr;

      return Drawing::Icon::FromHandle( IntPtr( hIcon ) );
    }

    void Window::Icon::set( Drawing::Icon^ value )
    {
      if ( value != nullptr )
      {
        auto_handle< Bitmap > image = gcnew Bitmap( value->Width, value->Height, Imaging::PixelFormat::Format32bppArgb );
        if ( image.get() != nullptr )
        {
          auto_handle< Graphics > graphics = Graphics::FromImage( image.get() );
          if ( graphics.get() != nullptr )
          {
            graphics->DrawIcon( value, 0, 0 );
            HICON hIcon = (HICON) image->GetHicon().ToPointer();
            if ((hIcon != nullptr))
            {
              GetWnd()->SetIcon( hIcon, 1 );
              GetWnd()->SetIcon( hIcon, 0 );
            }
          }
        }
      }
    }

    Drawing::Point Window::Location::get()
    {
      CRect rect;
      GetWnd()->GetWindowRect( &rect );
      return Drawing::Point( rect.left, rect.bottom );
    }

    void Window::Location::set( Drawing::Point point )
    {
      GetWnd()->SetWindowPos( nullptr, point.X, point.Y, 0, 0, SWP_NOSIZE | SWP_FRAMECHANGED );
    }

    Drawing::Size Window::Size::get()
    {
      CRect rect;
      GetWnd()->GetWindowRect( &rect );
      return Drawing::Size( rect.Width(), rect.Height() );
    }

    void Window::Size::set( Drawing::Size size )
    {
      GetWnd()->SetWindowPos( nullptr, 0, 0, size.Width, size.Height, SWP_NOMOVE | SWP_NOZORDER | SWP_FRAMECHANGED );
    }

    String^ Window::Text::get()
    {
      CStringW strText;
      GetWnd()->GetWindowTextW( strText );
      return gcnew String( (const wchar_t*) strText );
    }

    void Window::Text::set( String^ strText )
    {
      pin_ptr< const wchar_t > pText = PtrToStringChars( strText );
      GetWnd()->SetWindowTextW( pText );
    }

    bool Window::Visible::get()
    {
      return 0 != GetWnd()->IsWindowVisible();
    }

    void Window::Visible::set( bool bVal )
    {
      GetWnd()->ShowWindow( bVal ? SW_SHOW : SW_HIDE );
    }

    FormWindowState Window::WindowState::get()
    {
      CWnd* pWnd = GetWnd();
      if ( pWnd->IsIconic() )
        return FormWindowState::Minimized;

      if ( pWnd->IsZoomed() )
        return FormWindowState::Maximized;

      return FormWindowState::Normal;
    }

    void Window::WindowState::set( FormWindowState eVal )
    {
      int iShowMode = SW_SHOWNORMAL;
      switch ( eVal )
      {
        case FormWindowState::Minimized:
          iShowMode = SW_MINIMIZE;
          break;
        case FormWindowState::Maximized:
          iShowMode = SW_MAXIMIZE;
          break;
      }
      GetWnd()->ShowWindow( iShowMode );
    }

    CWnd* WindowOnHandle::GetWnd()
    {
      return CWnd::FromHandle( (HWND) UnmanagedObject.ToPointer() );
    }
  }
}
