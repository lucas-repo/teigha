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

#include "Keyword.h"

namespace Teigha
{
  namespace EditorInput
  {
    Keyword::Keyword( KeywordCollection^ parent )
    {
      m_readOnly = false;
      m_col = parent;
    }

#define IMPLEMENT_KW_PROP( Type, Name, Member )                                 \
Type Keyword::Name::get()                                                       \
{ return Member; }                                                              \
void Keyword::Name::set( Type value )                                           \
{ if ( IsReadOnly ) throw gcnew InvalidOperationException(); Member = value; }

    IMPLEMENT_KW_PROP( String^, DisplayName, m_displayName )
    IMPLEMENT_KW_PROP( bool,    Enabled,     m_enabled )
    IMPLEMENT_KW_PROP( String^, GlobalName,  m_globalName )
    IMPLEMENT_KW_PROP( bool,    IsReadOnly,  m_readOnly )
    IMPLEMENT_KW_PROP( String^, LocalName,   m_localName )
    IMPLEMENT_KW_PROP( bool,    Visible,     m_visible )

    KeywordCollection::KeywordCollection()
    {
      m_imp = gcnew Collections::ArrayList();
    }

    KeywordCollection::KeywordCollection( const wchar_t* interopString )
    {
      m_imp = gcnew Collections::ArrayList();
      if ( interopString == nullptr )
        return;

      array< wchar_t >^ aSep = {'_'};
      array< String^ >^ aLocGlob = ( gcnew String( interopString ) )->Split( aSep );

      Text::RegularExpressions::Regex^ regex = gcnew Text::RegularExpressions::Regex( "\\s+" );
      array< String^ >^ aLocs = regex->Split( aLocGlob[0]->Trim() );

      array< String^ >^ aGlob = aLocs;
      if ( aLocGlob->Length > 1 )
        aGlob = regex->Split( aLocGlob[ 1 ]->Trim() );

      int i, iSize = aLocs->Length;
      int iGSize = aGlob->Length;
      for ( i = 0; i < iSize; i++ )
      {
        String^ strLoc = aLocs[ i ];
        String^ strGlob = iGSize < i ? aGlob[ i ] : strLoc;
        Add( strLoc, strGlob );
      }

      for each ( Keyword^ kw in this )
        kw->IsReadOnly = true;

      m_imp = ArrayList::ReadOnly( m_imp );
    }

    Keyword^ KeywordCollection::GetDefaultKeyword()
    {
      return m_default;
    }

    void KeywordCollection::Add( String^ globalName )
    {
      Add(globalName, globalName, globalName, true, true);
    }

    void KeywordCollection::Add( String^ globalName, String^ localName )
    {
      Add(globalName, localName, localName, true, true);
    }

    void KeywordCollection::Add( String^ globalName, String^ localName, String^ displayName )
    {
      Add(globalName, localName, displayName, true, true);
    }

    void KeywordCollection::Add( String^ globalName, String^ localName, String^ displayName, bool visible, bool enabled )
    {
      Keyword^ keyword = gcnew Keyword( this );

      keyword->GlobalName  = globalName;
      keyword->LocalName   = localName;
      keyword->DisplayName = displayName;
      keyword->Visible     = visible;
      keyword->Enabled     = enabled;
      m_imp->Add(keyword);
    }

    void KeywordCollection::Clear()
    {
      m_default = nullptr;
      m_imp->Clear();
    }

    void KeywordCollection::CopyTo( array<Keyword^>^ arr, int index )
    {
      CopyTo( (Array^) arr, index );
    }

    void KeywordCollection::CopyTo( Array^ arr, int index )
    {
      int i, iSize = Count;
      for ( i = 0; i < iSize; i++, index++ )
        arr->SetValue( Item[ i ], index );
    }

    Keyword^ KeywordCollection::Item::get( int index )
    {
      return (Keyword^) m_imp[index];
    }

    String^ KeywordCollection::GetDisplayString( bool showNoDefault )
    {
      if ( Count != 0 )
      {
        bool flag = false;
        Text::StringBuilder^ builder = gcnew Text::StringBuilder();
        builder->Append("[");

        ArrayList^ aKwds = gcnew ArrayList( Count );
        for each ( Keyword^ keyword in this )
        {
          if ( keyword->Visible && keyword->Enabled && String::Compare( keyword->GlobalName, "dummy" ) != 0 )
          {
            flag = true;
            aKwds->Add( keyword->DisplayName );
          }
        }
        for ( int i = 0; i < aKwds->Count; ++i )
        {
          builder->Append( (String^) aKwds[ i ] );
          if ( i != aKwds->Count-1 )
            builder->Append("/");
        }
        builder->Append("]");

        if ( flag && !showNoDefault && m_default != nullptr )
        {
          builder->Append(" <");
          builder->Append( m_default->DisplayName );
          builder->Append(">");
        }
        return builder->ToString();
      }
      return nullptr;
    }

    Collections::IEnumerator^ KeywordCollection::GetEnumerator()
    {
      return m_imp->GetEnumerator();
    }

    String^ KeywordCollection::GetInteropString()
    {
      if ( Count == 0 )
        return String::Empty;

      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      for each (Keyword^ kw in this )
      {
        if ( kw->Enabled )
        {
          builder->Append( kw->LocalName );
          builder->Append(" ");
        }
      }

      builder->Append("_ ");

      for each (Keyword^ kw in this )
      {
        if ( kw->Enabled )
        {
          builder->Append( kw->GlobalName );
          builder->Append(" ");
        }
      }
      return builder->ToString();
    }

    String^ KeywordCollection::Default::get()
    {
      if (m_default != nullptr)
        return m_default->GlobalName;

      return nullptr;
    }

    void KeywordCollection::Default::set( String^ value )
    {
      for each ( Keyword^ kw in this )
      {
        if ( kw->Enabled && 0 == String::Compare( kw->GlobalName, value ) )
        {
          m_default = kw;
          return;
        }
      }
      throw gcnew ArgumentException();
    }
  }
}
