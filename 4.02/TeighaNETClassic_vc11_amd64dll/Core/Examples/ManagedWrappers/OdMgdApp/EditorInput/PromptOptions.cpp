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

#include "Editor.h"
#include "PromptOptions.h"
#include "Keyword.h"
#include "PromptResults.h"
#include "PromptEvents.h"

#include "../ApplicationServices/Application.h"
#include "../ApplicationServices/DocumentCollection.h"
#include "../ApplicationServices/Document.h"

#include "DbLayerTableRecord.h"
#include "DbEntity.h"

// TODO: Implement PromptingForXXX events.

namespace Teigha
{
  namespace EditorInput
  {
    private ref class PromptParser
    {
      internal:

        property KeywordCollection^ Keywords { KeywordCollection^ get() { return m_keywords; } }

        property String^ Prompt { String^ get() { return m_prompt; } }

        void Parse( String^ promptAndKeywords, String^ globalKeywords )
        {
          int length = promptAndKeywords->LastIndexOf("[");
          int iLastBr = promptAndKeywords->LastIndexOf("]");
          if ( length < 0 || iLastBr <= (length + 1) )
            throw gcnew ArgumentException("No bracketed keyword list");

          if (length > 0)
              m_prompt = promptAndKeywords->Substring(0, length)->TrimEnd();
          else
            m_prompt = String::Empty;

          array< String^ >^ strArray = promptAndKeywords->Substring( length + 1, iLastBr - length - 1 )->Split(gcnew array< wchar_t > { '/' } );

          int iKwSize = strArray->Length;
          if ( iKwSize <= 0 )
            throw gcnew ArgumentException( "Bracketed keyword list is empty" );

          array< String^ >^ strLocalKws = gcnew array<String^>( iKwSize );
          for ( int i = 0; i < iKwSize; i++ )
            strLocalKws[ i ] = PromptParser::ParseLocalKeyword( strArray[ i ] );

          array<String^>^ strGlobKws = PromptParser::SplitByWhitespace( globalKeywords );

          if ( iKwSize != strGlobKws->Length )
            throw gcnew ArgumentException( "Mismatched number of global and local keywords" );

          m_keywords = gcnew KeywordCollection();

          for ( int i =0; i < iKwSize; i++ )
            m_keywords->Add( strGlobKws[ i ], strLocalKws[ i ], strArray[ i ], true, true);
        }

      private protected:

        inline static String^ ParseLocalKeyword( String^ displayKeyword )
        {
          int iStartBr = displayKeyword->LastIndexOf( '(' );
          int iEndBr   = displayKeyword->LastIndexOf( ')' );

          if ( iStartBr > 0 && iEndBr > iStartBr + 1 )
            return displayKeyword->Substring( iStartBr + 1, iEndBr - iStartBr - 1 );

          array< String^ >^ strArray = PromptParser::SplitByWhitespace( displayKeyword );
          for each ( String^ current in strArray )
          {
            for each ( char ch in current )
            {
              if ( wchar_t::IsUpper( ch ) )
                return current;
            }
          }

          for each ( String^ current in strArray )
          {
            for each ( char ch in current )
            {
              if ( wchar_t::IsDigit( ch ) )
                return current;
            }
          }

          for each ( String^ current in strArray )
          {
            for each ( char ch in current )
            {
              if ( wchar_t::IsLetterOrDigit( ch ) )
                return current;
            }
          }

          throw gcnew ArgumentException( "Couldn't parse local keyword" );
        }

        inline static array<String^>^ SplitByWhitespace( String^ text )
        {
          Text::RegularExpressions::Regex^ regex = gcnew Text::RegularExpressions::Regex( "\\s+" );
          return regex->Split( text );
        }

      private protected:
        KeywordCollection^ m_keywords;
        String^ m_prompt;
    };

    PromptOptions::PromptOptions( const wchar_t* promptString, const wchar_t* pKeywords )
    {
      m_message = gcnew String( promptString );
      m_readOnly = true;
      m_appendKeywords = true;
      m_defaultValue = nullptr;
      m_keywords = gcnew KeywordCollection( pKeywords );
    }

    PromptOptions::PromptOptions( const wchar_t* promptString, const wchar_t* pKeywords, Object^ defVal )
    {
      m_message = gcnew String( promptString );
      m_readOnly = true;
      m_appendKeywords = true;
      m_defaultValue = defVal;
      m_keywords = gcnew KeywordCollection( pKeywords );
    }

    PromptOptions::PromptOptions( String^ message )
    {
      m_message   = message;
      m_keywords  = nullptr;
      m_readOnly  = false;
      m_appendKeywords = true;
      m_defaultValue   = nullptr;
      if ( message == nullptr )
        throw gcnew ArgumentException();
    }

    PromptOptions::PromptOptions( String^ messageAndKeywords, String^ globalKeywords )
    {
      m_message  = nullptr;
      m_keywords = nullptr;
      m_readOnly = false;
      m_appendKeywords = true;
      m_defaultValue   = nullptr;
      SetMessageAndKeywords(messageAndKeywords, globalKeywords);
    }

    String^ PromptOptions::TrimTrailingCharacters( String^ message )
    {
      array< wchar_t >^ trimChars = {' ', ':'};
      return message->TrimEnd( trimChars );
    }

    PromptResult^ PromptOptions::DoIt()
    {
      return nullptr;
    }
    
    String^ PromptOptions::FormatPrompt()
    {
      String^ strTrimedMsg = m_message->TrimEnd( gcnew array< wchar_t > {' ', ':'} );
      String^ displayString = Keywords->GetDisplayString( m_defaultValue != nullptr );

      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      if ( m_appendKeywords && !String::IsNullOrEmpty( displayString ) )
      {
        builder->AppendFormat( "{0} {1}", strTrimedMsg, displayString );
      }
      else
      {
        builder->Append( strTrimedMsg ) ;
      }

      if ( m_defaultValue != nullptr )
        builder->AppendFormat( " <{0}>", this->GetDefaultValueString());

      builder->Append(": ");
      return builder->ToString();
    }

    String^ PromptOptions::GetDefaultValueString()
    {
      return DefaultValueWorker->ToString();
    }

    void PromptOptions::SetMessageAndKeywords( String^ messageAndKeywords, String^ globalKeywords )
    {
      throwIfReadOnly();

      PromptParser^ parser = gcnew PromptParser();
      parser->Parse( messageAndKeywords, globalKeywords );
      m_message = parser->Prompt;
      m_keywords = parser->Keywords;
    }

    void PromptOptions::throwIfReadOnly()
    {
      if ( m_readOnly )
        throw gcnew InvalidOperationException() ;
    }

    bool PromptOptions::AppendKeywordsToMessage::get()
    {
      return m_appendKeywords;
    }

    void PromptOptions::AppendKeywordsToMessage::set( bool value )
    {
      throwIfReadOnly();
      m_appendKeywords = value;
    }

    bool PromptOptions::IsReadOnly::get()
    {
      return m_readOnly;
    }

    String^ PromptOptions::Message::get()
    {
      return m_message;
    }

    void PromptOptions::Message::set( String^ value )
    {
      throwIfReadOnly();

      m_message = value;
    }

    KeywordCollection^ PromptOptions::Keywords::get()
    {
      if ( m_keywords == nullptr )
        m_keywords = gcnew KeywordCollection();

      return this->m_keywords;
    }

    bool PromptOptions::UseDefaultValue::get()
    {
      return m_defaultValue != nullptr;
    }

    void PromptOptions::UseDefaultValue::set( bool value )
    {
      if ( m_keywords == nullptr )
        m_keywords = gcnew KeywordCollection();

      if ( !value )
        m_defaultValue = nullptr;
    }

    Object^ PromptOptions::DefaultValueWorker::get()
    {
      if ( m_defaultValue == nullptr )
        throw gcnew NullReferenceException( "DefaultValue is not set." );

      return m_defaultValue;
    }

    void PromptOptions::DefaultValueWorker::set( Object^ value )
    {
      throwIfReadOnly();
      m_defaultValue = value;
    }

    bool PromptOptions::HasDefaultKeyword::get()
    {
      return Keywords->Default != nullptr;
    }

    PromptEditorOptions::PromptEditorOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptOptions( promptString, pKeywords )
    {
      m_iFlags = initGetFlags;
    }

    PromptEditorOptions::PromptEditorOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, Object^ defVal )
      : PromptOptions( promptString, pKeywords, defVal )
    {
      m_iFlags = initGetFlags;
    }

    PromptEditorOptions::PromptEditorOptions( String^ message )
      : PromptOptions( message )
    {
      m_iFlags = 1; // TODO:
    }

    PromptEditorOptions::PromptEditorOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptOptions( messageAndKeywords, globalKeywords )
    {
      m_iFlags = 1; // TODO:
    }

#define IMPLEMENT_FLAG_PROP( Name, Val )                            \
bool PromptEditorOptions::Name::get()                               \
{ return Val == ( m_iFlags & Val ); }                               \
void PromptEditorOptions::Name::set( bool value )                   \
{ throwIfReadOnly();                                                \
  m_iFlags &= ~Val; if ( value ) m_iFlags |= Val;                   \
}
#define IMPLEMENT_FLAG_PROP_INV( Name, Val )                        \
bool PromptEditorOptions::Name::get()                               \
{ return Val != ( m_iFlags & Val ); }                               \
void PromptEditorOptions::Name::set( bool value )                   \
{ throwIfReadOnly();                                                \
  m_iFlags &= ~Val; if ( !value ) m_iFlags |= Val;                  \
}

    IMPLEMENT_FLAG_PROP( AllowArbitraryInput, 0 );
    IMPLEMENT_FLAG_PROP_INV( AllowNegative, OdEd::kInpNonNeg );
    IMPLEMENT_FLAG_PROP( AllowNone, OdEd::kInpThrowEmpty );
    IMPLEMENT_FLAG_PROP( AllowZero, OdEd::kInpNonZero );
    IMPLEMENT_FLAG_PROP( LimitsChecked, 0 );
    IMPLEMENT_FLAG_PROP( Only2d, 0 );
    IMPLEMENT_FLAG_PROP( UseDashedLine, 0 );

    int PromptEditorOptions::InitGetFlags::get()
    {
      return m_iFlags;
    }

    void PromptEditorOptions::InitGetFlags::set( int value )
    {
      throwIfReadOnly();
      m_iFlags = value;
    }

    PromptNumericalOptions::PromptNumericalOptions( String^ message )
      : PromptEditorOptions( message )
    {
    }

    PromptNumericalOptions::PromptNumericalOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptEditorOptions( messageAndKeywords, globalKeywords )
    {
    }

    PromptNumericalOptions::PromptNumericalOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords )
    {
    }

    PromptNumericalOptions::PromptNumericalOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, Object^ defVal )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords, defVal )
    {
    }

    PromptDoubleOptions::PromptDoubleOptions( const double* defVal, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptNumericalOptions( promptString, initGetFlags, pKeywords, defVal == nullptr ? 0.0 : *defVal )
    {
    }

    PromptDoubleOptions::PromptDoubleOptions( String^ message )
      : PromptNumericalOptions( message )
    {
    }

    PromptDoubleOptions::PromptDoubleOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptNumericalOptions( messageAndKeywords, globalKeywords )
    {
    }

    double PromptDoubleOptions::DefaultValue::get()
    {
      return (double) DefaultValueWorker;
    }

    void PromptDoubleOptions::DefaultValue::set( double value )
    {
      DefaultValueWorker = value;
    }

    PromptResult^ PromptDoubleOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdEdUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      double dResVal = UseDefaultValue ? DefaultValue : 0;
      try
      {
        OdEd::CommonInputOptions opts = (OdEd::CommonInputOptions) InitGetFlags;

        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );

        Editor::g_CurrentEditor->PromptingForDouble( Editor::g_CurrentEditor, gcnew PromptDoubleOptionsEventArgs( this ) );
        dResVal = pIO->getReal( pPrompt, opts, dResVal, pKWords );
        strResult = dResVal.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& ) // TODO:
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }
      PromptDoubleResult^ res = gcnew PromptDoubleResult( eStatus, strResult, dResVal );
      Editor::g_CurrentEditor->PromptedForDouble( Editor::g_CurrentEditor, gcnew PromptDoubleResultEventArgs( res ) );
      return res;
    }

    String^ PromptDoubleOptions::GetDefaultValueString()
    {
      return DefaultValueWorker->ToString();
    }

    PromptDistanceOptions::PromptDistanceOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptNumericalOptions( promptString, initGetFlags, pKeywords )
    {
      m_useBasePoint = pointIn != nullptr;
      if ( m_useBasePoint )
        m_basePoint = Geometry::Point3d( pointIn->x, pointIn->y, pointIn->z );
    }

    PromptDistanceOptions::PromptDistanceOptions( String^ message )
      : PromptNumericalOptions( message )
    {
    }

    PromptDistanceOptions::PromptDistanceOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptNumericalOptions( messageAndKeywords, globalKeywords )
    {
    }

    Geometry::Point3d PromptDistanceOptions::BasePoint::get()
    {
      return m_basePoint;
    }

    void PromptDistanceOptions::BasePoint::set( Geometry::Point3d value )
    {
      throwIfReadOnly();
      m_basePoint = value;
    }

    double PromptDistanceOptions::DefaultValue::get()
    {
      return *(double^)DefaultValueWorker;
    }

    void PromptDistanceOptions::DefaultValue::set( double value )
    {
      throwIfReadOnly();
      DefaultValueWorker = value;
    }

    bool PromptDistanceOptions::UseBasePoint::get()
    {
      return m_useBasePoint;
    }

    void PromptDistanceOptions::UseBasePoint::set( bool value )
    {
      throwIfReadOnly();
      m_useBasePoint = value;
    }

    PromptResult^ PromptDistanceOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      double dResVal = UseDefaultValue ? DefaultValue : 0;
      try
      {
        OdEd::CommonInputOptions opts = (OdEd::CommonInputOptions) InitGetFlags;

        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );

        Editor::g_CurrentEditor->PromptingForDistance( Editor::g_CurrentEditor, gcnew PromptDistanceOptionsEventArgs( this ) );
        dResVal = pIO->getDist( pPrompt, opts, dResVal, pKWords );
        strResult = dResVal.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& ) // TODO:
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptDoubleResult^ res = gcnew PromptDoubleResult( eStatus, strResult, dResVal );
      Editor::g_CurrentEditor->PromptedForDistance( Editor::g_CurrentEditor, gcnew PromptDoubleResultEventArgs( res ) );
      return res;
    }

    String^ PromptDistanceOptions::GetDefaultValueString()
    {
      // TODO:
      return DefaultValue.ToString();
    }

    PromptCornerOptions::PromptCornerOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords )
    {
      if ( pointIn != nullptr )
        m_basePoint = Geometry::Point3d( pointIn->x, pointIn->y, pointIn->z );
    }

    PromptCornerOptions::PromptCornerOptions( String^ message, Geometry::Point3d basePoint )
      : PromptEditorOptions( message )
    {
      m_basePoint = basePoint;
    }

    PromptCornerOptions::PromptCornerOptions( String^ messageAndKeywords, String^ globalKeywords, Geometry::Point3d basePoint )
      : PromptEditorOptions( messageAndKeywords, globalKeywords )
    {
      m_basePoint = basePoint;
    }

    Geometry::Point3d PromptCornerOptions::BasePoint::get()
    {
      return m_basePoint;
    }

    void PromptCornerOptions::BasePoint::set( Geometry::Point3d pt )
    {
      throwIfReadOnly();
      m_basePoint = pt;
    }

    PromptResult^ PromptCornerOptions::DoIt()
    {
      // TODO:
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      Geometry::Point3d ptRes = Geometry::Point3d::Origin;
      try
      {
        OdEd::CommonInputOptions opts = (OdEd::CommonInputOptions) InitGetFlags;

        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );

        Editor::g_CurrentEditor->PromptingForCorner( Editor::g_CurrentEditor, nullptr /*TODO*/ );
        OdGePoint3d pt = pIO->getPoint( pPrompt, opts, (const OdGePoint3d*) &ptRes, pKWords );
        ptRes = Geometry::Point3d( pt.x, pt.y, pt.z );
        strResult = ptRes.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& ) // TODO:
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }
      PromptPointResult^ res = gcnew PromptPointResult( eStatus, strResult, ptRes );
      Editor::g_CurrentEditor->PromptedForCorner( Editor::g_CurrentEditor, gcnew PromptPointResultEventArgs( res ) );
      return res;
    }

    PromptPointOptions::PromptPointOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptCornerOptions( pointIn, promptString, initGetFlags, pKeywords )
    {
      m_useBasePoint = pointIn != nullptr;
    }

    PromptPointOptions::PromptPointOptions( String^ message )
      : PromptCornerOptions( message, Geometry::Point3d::Origin )
    {
      m_useBasePoint = false;
    }

    PromptPointOptions::PromptPointOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptCornerOptions( messageAndKeywords, globalKeywords, Geometry::Point3d::Origin )
    {
      m_useBasePoint = false;
    }

    bool PromptPointOptions::UseBasePoint::get()
    {
      return m_useBasePoint;
    }

    void PromptPointOptions::UseBasePoint::set( bool value )
    {
      throwIfReadOnly();
      m_useBasePoint = value;
    }

    PromptResult^ PromptPointOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      Geometry::Point3d ptRes = UseBasePoint ? BasePoint : Geometry::Point3d::Origin;
      try
      {
        OdEd::CommonInputOptions opts = (OdEd::CommonInputOptions) InitGetFlags;

        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );

        Editor::g_CurrentEditor->PromptingForPoint( Editor::g_CurrentEditor, gcnew PromptPointOptionsEventArgs( this ) );
        OdGePoint3d pt = pIO->getPoint( pPrompt, opts, (const OdGePoint3d*) &ptRes, pKWords );
        ptRes = Geometry::Point3d( pt.x, pt.y, pt.z );
        strResult = ptRes.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& ) // TODO:
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptPointResult^ res = gcnew PromptPointResult( eStatus, strResult, ptRes );
      Editor::g_CurrentEditor->PromptedForPoint( Editor::g_CurrentEditor, gcnew PromptPointResultEventArgs( res ) );
      return res;
    }

    PromptSelectionOptions::PromptSelectionOptions()
    {
      m_keywords = gcnew KeywordCollection();
    }

    String^ PromptSelectionOptions::NormalizeMessage( String^ message )
    {
      if ( message == nullptr )
        return nullptr;

      return PromptOptions::TrimTrailingCharacters( message ) + ": ";
    }

    PromptSelectionResult^ PromptSelectionOptions::Select( SelectionFilter^ filter )
    {
      filter;

      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      pin_ptr< const wchar_t > pPrompt = nullptr;
      if ( !String::IsNullOrEmpty( MessageForAdding ) )
        pPrompt = PtrToStringChars( MessageForAdding );

      if ( !String::IsNullOrEmpty( MessageForRemoval ) )
        pPrompt = PtrToStringChars( MessageForRemoval );

      OdDbSelectionSetPtr pSSet = OdDbSelectionSet::createObject();
      PromptStatus eStatus = PromptStatus::OK;
      try
      {
        int iFlags = OdEd::kSelDefault;
        if ( AllowSubSelections )
          iFlags |= OdEd::kSelAllowSubents;
        if ( SingleOnly )
          iFlags |= OdEd::kSelSingleEntity;
        if ( !RejectPaperspaceViewport )
          iFlags |= OdEd::kSelAllowPSVP;
        if ( !RejectObjectsFromNonCurrentSpace )
          iFlags |= OdEd::kSelAllowInactSpaces;
        if ( SinglePickInSpace )
          iFlags |= OdEd::kSelSinglePass;

        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );

        Editor::g_CurrentEditor->PromptingForSelection( Editor::g_CurrentEditor, gcnew PromptSelectionOptionsEventArgs( this ) );
        pSSet = pIO->select( pPrompt, iFlags, 0, pKWords );
      }
      catch( const OdEdKeyword& e )
      {
        if ( m_keywordInputEvent != nullptr )
          m_keywordInputEvent( nullptr, gcnew SelectionTextInputEventArgs( ( const wchar_t* ) e.keyword() ) );

        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& e )
      {
        if ( m_unknownInputEvent != nullptr )
          m_unknownInputEvent( nullptr, gcnew SelectionTextInputEventArgs( ( const wchar_t* ) e.string() ) );

        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }
      PromptSelectionResult^ res = gcnew PromptSelectionResult( eStatus, pSSet );
      Editor::g_CurrentEditor->PromptedForSelection( Editor::g_CurrentEditor, gcnew PromptSelectionResultEventArgs( res ) );
      return res;
    }

    void PromptSelectionOptions::SetKeywords( String^ keywords, String^ globalKeywords )
    {
      PromptParser^ parser = gcnew PromptParser();
      parser->Parse( keywords, globalKeywords );

      if ( parser->Prompt->Length > 0 )
        throw gcnew ArgumentException( "Cannot set a prompt in PromptSelectionOptions" );

      m_keywords = parser->Keywords;
    }

    private ref class AllowedClass
    {
      public:
        AllowedClass( Runtime::RXClass^ classToMatch, bool exactMatch )
        {
          ClassToMatch = classToMatch;
          ExactMatch = exactMatch;
        }
        
        Runtime::RXClass^ ClassToMatch;

        bool ExactMatch;
    };

    PromptEntityOptions::PromptEntityOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, bool allowObjectOnLockedLayer )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords )
    {
      m_allowObjectOnLockedLayer = allowObjectOnLockedLayer;
    }

    PromptEntityOptions::PromptEntityOptions( String^ message )
      : PromptEditorOptions(message)
    {
      m_allowObjectOnLockedLayer = false;
    }

    PromptEntityOptions::PromptEntityOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptEditorOptions( messageAndKeywords, globalKeywords )
    {
      m_allowObjectOnLockedLayer = false;
    }

    void PromptEntityOptions::AddAllowedClass( Type^ type, bool exactMatch )
    {
      throwIfReadOnly();

      if ( m_rejectMessage == nullptr )
        throw gcnew InvalidOperationException("Use SetRejectMessage first.");

      Runtime::RXClass^ classToMatch = Runtime::RXObject::GetClass( type );
      if ( classToMatch == nullptr )
        return;

      if ( m_allowedClasses == nullptr )
        m_allowedClasses = gcnew ArrayList();

      for each ( AllowedClass^ allClass in m_allowedClasses )
      {
        if ( ( (Runtime::RXObject^)allClass->ClassToMatch)->UnmanagedObject == classToMatch->UnmanagedObject )
          allClass->ExactMatch = exactMatch;
      }
      m_allowedClasses->Add( gcnew AllowedClass( classToMatch, exactMatch ) );
    }

    void PromptEntityOptions::RemoveAllowedClass( Type^ type )
    {
      throwIfReadOnly();

      if ( m_allowedClasses == nullptr )
        return;

      Runtime::RXObject^ rxClass = Runtime::RXObject::GetClass( type );
      if ( rxClass == nullptr )
        return;

      for ( int i = 0; i < m_allowedClasses->Count; i++ )
      {
        if ( ( (Runtime::RXObject^) m_allowedClasses[ i ])->UnmanagedObject == rxClass->UnmanagedObject )
          m_allowedClasses->RemoveAt( i );
      }
    }

    void PromptEntityOptions::SetRejectMessage( String^ message )
    {
      throwIfReadOnly();

      m_rejectMessage = message;
    }

    bool PromptEntityOptions::AllowObjectOnLockedLayer::get()
    {
      return m_allowObjectOnLockedLayer;
    }

    void PromptEntityOptions::AllowObjectOnLockedLayer::set( bool value )
    {
      throwIfReadOnly();

       m_allowObjectOnLockedLayer = value;
    }

    PromptResult^ PromptEntityOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      Geometry::Point3d ptRes = Geometry::Point3d::Origin;
      DatabaseServices::ObjectId id;
      try
      {
        OdEd::CommonInputOptions opts = (OdEd::CommonInputOptions) InitGetFlags;
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );
        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );

        Editor::g_CurrentEditor->PromptingForEntity( Editor::g_CurrentEditor, gcnew PromptEntityOptionsEventArgs( this ) );
        OdDbSelectionSetPtr pSSet = pIO->select( pPrompt, opts | OdEd::kSelSingleEntity, 0, pKWords );
        OdGePoint3d pt = pIO->getLASTPOINT();
        ptRes = Geometry::Point3d( pt.x, pt.y, pt.z );

        OdDbObjectIdArray aIds = pSSet->objectIdArray();
        int i, iSize = aIds.size();
        for ( i = 0; i < iSize; i++ )
        {
          if ( IsAllowedObject( aIds[ i ] ) )
            id = DatabaseServices::ObjectId( IntPtr( ( OdDbStub* ) aIds[ i ] ) );
        }
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& ) // TODO:
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptEntityResult^ res = gcnew PromptEntityResult( eStatus, strResult, id, ptRes );
      Editor::g_CurrentEditor->PromptedForEntity( Editor::g_CurrentEditor, gcnew PromptEntityResultEventArgs( res ) );
      return res;
    }

    bool PromptEntityOptions::IsAllowedObject( const OdDbObjectId& idEnt )
    {
      OdDbEntityPtr pEnt = idEnt.openObject();
      OdDbLayerTableRecordPtr pLayer;
      if ( pEnt.isNull() )
        goto errorText;

      pLayer = pEnt->layerId().openObject();
      if ( !pLayer.isNull() && pLayer->isLocked() && !AllowObjectOnLockedLayer )
        goto errorText;

      if ( m_allowedClasses != nullptr )
      {
        for each ( AllowedClass^ allClass in m_allowedClasses )
        {
          OdRxClass* pClass = (OdRxClass*)allClass->ClassToMatch->UnmanagedObject.ToPointer();
          if ( allClass->ExactMatch )
          {
            if ( pEnt->isA() == pClass )
              return true;
          }
          else
          {
            if ( pEnt->isKindOf( pClass ) )
              return true;
          }
        }
        goto errorText;
      }
      return true;

    errorText:
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      pin_ptr< const wchar_t > pMess = PtrToStringChars( m_rejectMessage );
      pIO->putString( pMess );
      return false;
    }

    PromptAngleOptions::PromptAngleOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, bool useAngleBase )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords )
    {
      m_useBasePoint = pointIn != nullptr;
      m_useAngleBase = useAngleBase;
      if ( m_useBasePoint )
        m_basePoint = Geometry::Point3d( pointIn->x, pointIn->y, pointIn->z );
    }

    PromptAngleOptions::PromptAngleOptions( String^ message )
      : PromptEditorOptions(message)
    {
      m_useBasePoint = false;
      m_useAngleBase = true;
    }

    PromptAngleOptions::PromptAngleOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptEditorOptions( messageAndKeywords, globalKeywords )
    {
      m_useBasePoint = false;
      m_useAngleBase = true;
    }

    PromptResult^ PromptAngleOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      double dRes = 0;
      try
      {
        int opts = InitGetFlags;
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );
        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );

        Editor::g_CurrentEditor->PromptingForAngle( Editor::g_CurrentEditor, gcnew PromptAngleOptionsEventArgs( this ) );
        dRes = pIO->getAngle( pPrompt, opts, HasDefaultKeyword ? DefaultValue : 0.0, pKWords );
        strResult = dRes.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& )
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptDoubleResult^ res = gcnew PromptDoubleResult( eStatus, strResult, dRes );
      Editor::g_CurrentEditor->PromptedForAngle( Editor::g_CurrentEditor, gcnew PromptDoubleResultEventArgs( res ) );
      return res;
    }

    String^ PromptAngleOptions::GetDefaultValueString()
    {
      // TODO:
      return DefaultValue.ToString();
    }

    double PromptAngleOptions::DefaultValue::get()
    {
      return *( double^ ) DefaultValueWorker;
    }

    void PromptAngleOptions::DefaultValue::set( double value )
    {
      throwIfReadOnly();
      DefaultValueWorker = value;
    }

    Geometry::Point3d PromptAngleOptions::BasePoint::get()
    {
      return m_basePoint;
    }

    void PromptAngleOptions::BasePoint::set( Geometry::Point3d value )
    {
      throwIfReadOnly();
      m_basePoint = value;
    }

    bool PromptAngleOptions::UseAngleBase::get()
    {
      return m_useAngleBase;
    }

    void PromptAngleOptions::UseAngleBase::set( bool value )
    {
      throwIfReadOnly();
      m_useAngleBase = value;
    }

    bool PromptAngleOptions::UseBasePoint::get()
    {
      return m_useBasePoint;
    }

    void PromptAngleOptions::UseBasePoint::set( bool value )
    {
      throwIfReadOnly();
      m_useBasePoint = value;
    }

    PromptIntegerOptions::PromptIntegerOptions( const int* dfault, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptNumericalOptions( promptString, initGetFlags, pKeywords, dfault!= nullptr ? (Object^)*dfault : nullptr )
    {
      m_lowerLimit = int::MinValue;
      m_upperLimit = int::MaxValue;
    }

    PromptIntegerOptions::PromptIntegerOptions( String^ message )
      : PromptNumericalOptions( message )
    {
      m_lowerLimit = int::MinValue;
      m_upperLimit = int::MaxValue;
    }

    PromptIntegerOptions::PromptIntegerOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptNumericalOptions( messageAndKeywords, globalKeywords )
    {
      m_lowerLimit = int::MinValue;
      m_upperLimit = int::MaxValue;
    }

    PromptIntegerOptions::PromptIntegerOptions( String^ messageAndKeywords, String^ globalKeywords, int lowerLimit, int upperLimit )
      : PromptNumericalOptions( messageAndKeywords, globalKeywords )
    {
      m_lowerLimit = lowerLimit;
      m_upperLimit = upperLimit;
    }

    int PromptIntegerOptions::DefaultValue::get()
    {
      return *(int^) DefaultValueWorker;
    }

    void PromptIntegerOptions::DefaultValue::set( int value )
    {
      DefaultValueWorker = value;
    }

    PromptResult^ PromptIntegerOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      int iRes = 0;
      try
      {
        int opts = InitGetFlags;
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );
        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );

        Editor::g_CurrentEditor->PromptingForInteger( Editor::g_CurrentEditor, gcnew PromptIntegerOptionsEventArgs( this ) );
        iRes = pIO->getInt( pPrompt, opts, HasDefaultKeyword ? DefaultValue : 0, pKWords );
        strResult = iRes.ToString();
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& )
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptIntegerResult^ res = gcnew PromptIntegerResult( eStatus, strResult, iRes );
      Editor::g_CurrentEditor->PromptedForInteger( Editor::g_CurrentEditor, gcnew PromptIntegerResultEventArgs( res ) );
      return res;
    }

    PromptKeywordOptions::PromptKeywordOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords )
      : PromptEditorOptions( promptString, initGetFlags, pKeywords )
    {
    }

    PromptKeywordOptions::PromptKeywordOptions( String^ message )
      : PromptEditorOptions( message )
    {
    }

    PromptKeywordOptions::PromptKeywordOptions( String^ messageAndKeywords, String^ globalKeywords )
      : PromptEditorOptions( messageAndKeywords, globalKeywords )
    {
    }

    PromptResult^ PromptKeywordOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      try
      {
        int opts = InitGetFlags;
        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );
        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );

        int iDefKw = -1, i = 0;
        for each ( Keyword^ kw in Keywords )
        {
          if ( Object::ReferenceEquals( Keywords->Default, kw ) )
          {
            iDefKw = i;
            break;
          }
          i++;
        }

        Editor::g_CurrentEditor->PromptingForKeyword( Editor::g_CurrentEditor, gcnew PromptKeywordOptionsEventArgs( this ) );
        strResult = gcnew String( (const wchar_t*) pIO->getKeyword( pPrompt, pKWords, iDefKw, opts ) );
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& )
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptResult^ res = gcnew PromptResult( eStatus, strResult );
      Editor::g_CurrentEditor->PromptedForKeyword( Editor::g_CurrentEditor, gcnew PromptStringResultEventArgs( res ) );
      return res;
    }

    PromptStringOptions::PromptStringOptions( const wchar_t* promptString, int initGetFlags )
      : PromptEditorOptions( promptString, initGetFlags, nullptr )
    {
    }

    PromptStringOptions::PromptStringOptions( String^ message )
      : PromptEditorOptions( message )
    {
    }

    PromptResult^ PromptStringOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      try
      {
        int opts = InitGetFlags;
        if ( AllowSpaces )
          opts |= OdEd::kGstAllowSpaces;

        pin_ptr< const wchar_t > pKWords = PtrToStringChars( Keywords->GetInteropString() );
        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( FormatPrompt() );
        pin_ptr< const wchar_t > pDefVal = PtrToStringChars( HasDefaultKeyword ? DefaultValue : String::Empty );

        Editor::g_CurrentEditor->PromptingForString( Editor::g_CurrentEditor, gcnew PromptStringOptionsEventArgs( this ) );
        strResult = gcnew String( (const wchar_t*) pIO->getString( pPrompt, opts, pDefVal, pKWords ) );
      }
      catch( const OdEdKeyword& e )
      {
        strResult = gcnew String( ( const wchar_t* ) e.keyword() );
        eStatus = PromptStatus::Keyword;
      }
      catch( const OdEdOtherInput& )
      {
        eStatus = PromptStatus::Other;
      }
      catch( const OdEdEmptyInput& )
      {
        eStatus = PromptStatus::None;
      }
      catch( const OdEdCancel& )
      {
        eStatus = PromptStatus::Cancel;
      }
      catch(...)
      {
        eStatus = PromptStatus::Error;
      }

      PromptResult^ res = gcnew PromptResult( eStatus, strResult );
      Editor::g_CurrentEditor->PromptedForKeyword( Editor::g_CurrentEditor, gcnew PromptStringResultEventArgs( res ) );
      return res;
    }

    bool PromptStringOptions::AllowSpaces::get()
    {
      return m_allowSpaces;
    }

    void PromptStringOptions::AllowSpaces::set( bool value )
    {
      throwIfReadOnly();

      m_allowSpaces = value;
    }

    String^ PromptStringOptions::DefaultValue::get()
    {
      return (String^) DefaultValueWorker;
    }

    void PromptStringOptions::DefaultValue::set( String^ value )
    {
      DefaultValueWorker = value;
    }

    PromptFileOptions::PromptFileOptions( String^ message )
    {
      m_message = message;
      m_flags = 0;
    }

    PromptFileNameResult^ PromptFileOptions::DoIt()
    {
      OdDbCommandContextPtr pCmdCtx = Editor::g_CurrentEditor->m_doc->GetImpObj()->cmdCtx();
      OdDbUserIO* pIO = pCmdCtx->dbUserIO();

      PromptStatus eStatus = PromptStatus::OK;
      String^ strResult = String::Empty;
      try
      {
        String^ strExt = GetExtensions()->Split( ';' )[ 0 ];

        pin_ptr< const wchar_t > pPrompt = PtrToStringChars( GetCommandLinePrompt() );
        pin_ptr< const wchar_t > pCaption = PtrToStringChars( GetDialogCaption() );
        pin_ptr< const wchar_t > pExt = PtrToStringChars( strExt );
        pin_ptr< const wchar_t > pInitialFileName = PtrToStringChars( InitialFileName );
        pin_ptr< const wchar_t > pFilter = PtrToStringChars( Filter );

        strResult = gcnew String( ( const wchar_t* ) pIO->getFilePath( pPrompt, Flags, pCaption, pExt, pInitialFileName, pFilter ) );
      }
      catch(...)
      {
        eStatus = PromptStatus::Cancel;
      }
      return gcnew PromptFileNameResult( eStatus, strResult, false );
    }

    int PromptFileOptions::Flags::get()
    {
      return m_flags;
    }

    void PromptFileOptions::Flags::set( int value )
    {
      m_flags = value;
    }

    bool PromptFileOptions::ShowReadOnlyCore::get()
    {
      return m_showReadOnly;
    }

    void PromptFileOptions::ShowReadOnlyCore::set( bool value )
    {
      m_showReadOnly = value;
    }

    bool PromptFileOptions::AllowUrls::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptFileOptions::AllowUrls::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    String^ PromptFileOptions::DialogCaption::get()
    {
      return GetDialogCaption();
    }

    void PromptFileOptions::DialogCaption::set( String^ value )
    {
      m_caption = value;
    }

    String^ PromptFileOptions::DialogName::get()
    {
      return GetDialogName();
    }

    void PromptFileOptions::DialogName::set( String^ value )
    {
      m_dialogName = value;
    }

    String^ PromptFileOptions::Filter::get()
    {
      return m_filter;
    }

    void PromptFileOptions::Filter::set( String^ value )
    {
      m_filter = value;
    }

    int PromptFileOptions::FilterIndex::get()
    {
      return m_filterIndex;
    }

    void PromptFileOptions::FilterIndex::set( int value )
    {
      m_filterIndex = value;
    }

    String^ PromptFileOptions::InitialDirectory::get()
    {
      return m_initialDirectory;
    }

    void PromptFileOptions::InitialDirectory::set( String^ value )
    {
      m_initialDirectory = nullptr;
      if ( value != nullptr )
      {
        wchar_t trimChar = IO::Path::DirectorySeparatorChar;
        m_initialDirectory = value->TrimEnd( trimChar ) + trimChar;
      }
    }

    String^ PromptFileOptions::InitialFileName::get()
    {
      return m_initialFileName;
    }

    void PromptFileOptions::InitialFileName::set( String^ value )
    {
      m_initialFileName = value;
    }

    String^ PromptFileOptions::Message::get()
    {
      return m_message;
    }

    void PromptFileOptions::Message::set( String^ value )
    {
      m_message = value;
    }

    bool PromptFileOptions::PreferCommandLine::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptFileOptions::PreferCommandLine::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    String^ PromptFileOptions::GetCommandLinePrompt()
    {
      return m_message;
    }

    String^ PromptFileOptions::GetDescriptions()
    {
      if ( m_filter == nullptr )
        return nullptr;

      array< String^ >^ strParts = m_filter->Split( '|' );
      if ( strParts->Length%2 != 0 )
        throw gcnew ArgumentException( "Bad filter string" );

      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      for ( int index = 0; index < strParts->Length; index+=2 )
      {
        if ( strParts[ index ]->Length > 0 )
        {
          if ( builder->Length != 0 )
            builder->Append('|');

          builder->Append( strParts[index] );

          array< String^ >^ strExts = strParts[ index + 1 ]->Split( ';' );
          for ( int i = 0; i < strExts->Length - 1; i++ )
            builder->Append( '|' );
        }
      }
      return builder->ToString();
    }

    String^ PromptFileOptions::GetDialogCaption()
    {
      if ( m_caption != nullptr )
        return m_caption;

      return m_message;
    }

    String^ PromptFileOptions::GetDialogName()
    {
      if ( m_dialogName != nullptr )
        return m_dialogName;

      String^ strRes = m_caption != nullptr ? m_caption : m_message;
      if ( strRes != nullptr )
        strRes->Replace(" ", "_");

      return strRes;
    }

    String^ PromptFileOptions::GetExtensions()
    {
      if ( m_filter == nullptr )
        return nullptr;

      array< String^ >^ strParts = m_filter->Split( '|' );

      int iSize = strParts->Length;
      if ( iSize % 2 != 0 )
        throw gcnew ArgumentException( "Bad filter string" );

      Text::StringBuilder^ builder = gcnew Text::StringBuilder();
      for ( int i = 1; i < iSize; i+= 2 )
      {
        array< String^ >^ strExts = strParts[ i ]->Split( ';' );
        for each ( String^ strExt in strExts )
        {
          if ( builder->Length != 0 )
            builder->Append( ';' );

          builder->Append( strExt->TrimStart(' ')->Substring( 2 ) );
        }
      }
      return builder->ToString();
    }

    PromptOpenFileOptions::PromptOpenFileOptions( String^ message )
      : PromptFileOptions( message )
    {
      Flags = OdEd::kGfpForOpen;
    }

    bool PromptOpenFileOptions::SearchPath::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptOpenFileOptions::SearchPath::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    bool PromptOpenFileOptions::TransferRemoteFiles::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptOpenFileOptions::TransferRemoteFiles::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    PromptSaveFileOptions::PromptSaveFileOptions( String^ message )
      : PromptFileOptions( message )
    {
      Flags = OdEd::kGfpForSave;
    }

    bool PromptSaveFileOptions::DeriveInitialFilenameFromDrawingName::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptSaveFileOptions::DeriveInitialFilenameFromDrawingName::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    bool PromptSaveFileOptions::DisplaySaveOptionsMenuItem::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptSaveFileOptions::DisplaySaveOptionsMenuItem::set( bool )
    {
      throw gcnew NotSupportedException();
    }

    bool PromptSaveFileOptions::ForceOverwriteWarningForScriptsAndLisp::get()
    {
      throw gcnew NotSupportedException();
    }

    void PromptSaveFileOptions::ForceOverwriteWarningForScriptsAndLisp::set( bool )
    {
      throw gcnew NotSupportedException();
    }
  }
}
