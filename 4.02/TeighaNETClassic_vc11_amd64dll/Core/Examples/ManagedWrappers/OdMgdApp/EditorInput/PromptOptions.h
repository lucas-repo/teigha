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
#include "PromptEvents.h"

#define IMP_PROP_FROMBASE( Type, Name )             \
property Type Name {                                \
  Type get() new { return __super::Name; }          \
  void set( Type val ) new { __super::Name = val; } \
}

namespace Teigha
{
  namespace EditorInput
  {
    ref class PromptResult;
    ref class PromptDoubleResult;
    ref class PromptEntityResult;
    ref class PromptNestedEntityResult;
    ref class PromptFileNameResult;
    ref class PromptIntegerResult;
    ref class PromptPointResult;
    ref class PromptSelectionResult;

    ref class Keyword;
    ref class KeywordCollection;
    ref class SelectionFilter;

    public ref class PromptOptions abstract
    {
      internal:
        PromptOptions( const wchar_t* promptString, const wchar_t* pKeywords );

        PromptOptions( const wchar_t* promptString, const wchar_t* pKeywords, Object^ defVal );

        static String^ TrimTrailingCharacters( String^ message );

      public protected:
        PromptOptions( String^ message );

        PromptOptions( String^ messageAndKeywords, String^ globalKeywords );

        virtual PromptResult^ DoIt();
        
        String^ FormatPrompt();

        virtual String^ GetDefaultValueString();

      public:
        void SetMessageAndKeywords( String^ messageAndKeywords, String^ globalKeywords );

        DECLARE_PROP( bool, AppendKeywordsToMessage );

        DECLARE_PROP_GET( bool, IsReadOnly );

        DECLARE_PROP_GET( KeywordCollection^, Keywords );

        DECLARE_PROP( String^, Message );

      private protected:
        DECLARE_PROP( bool, UseDefaultValue );

        DECLARE_PROP( Object^, DefaultValueWorker );

        DECLARE_PROP_GET( bool, HasDefaultKeyword );

      private protected:
        void throwIfReadOnly();

      private:
        bool m_appendKeywords;

        Object^ m_defaultValue;

        KeywordCollection^ m_keywords;

        String^ m_message;

        bool m_readOnly;
    };

    public ref class PromptEditorOptions abstract : public PromptOptions
    {
      internal:
        PromptEditorOptions(const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords);

        PromptEditorOptions(const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, Object^ defVal);

      public protected:
        PromptEditorOptions( String^ message );

        PromptEditorOptions( String^ messageAndKeywords, String^ globalKeywords );

      public:
        DECLARE_PROP( bool, AllowArbitraryInput );
        DECLARE_PROP( bool, AllowNegative );
        DECLARE_PROP( bool, AllowNone );
        DECLARE_PROP( bool, AllowZero );
        DECLARE_PROP( bool, LimitsChecked );
        DECLARE_PROP( bool, Only2d );
        DECLARE_PROP( bool, UseDashedLine );
        DECLARE_PROP( int, InitGetFlags );

      private protected:
        //void ApplyDefaults( int% retcode, String^% stringResult );

        //Object^ ApplyDefaults( int% retcode, String^% stringResult, Object^ value );

        //void InitGet();

        //int ProcessForNoneOrEmptySel( int retcode );

      private:
        int m_iFlags;
    };

    public ref class PromptNumericalOptions : public PromptEditorOptions
    {
      private protected:
        PromptNumericalOptions(String^ message);

        PromptNumericalOptions(String^ messageAndKeywords, String^ globalKeywords);

      private protected:
        PromptNumericalOptions(const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords);
        PromptNumericalOptions(const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, Object^ defVal);

      public:
        IMP_PROP_FROMBASE( bool, AllowArbitraryInput );
        IMP_PROP_FROMBASE( bool, AllowNegative );
        IMP_PROP_FROMBASE( bool, AllowNone );
        IMP_PROP_FROMBASE( bool, AllowZero );
        IMP_PROP_FROMBASE( bool, UseDefaultValue );
    };

    public ref class PromptDoubleOptions sealed : public PromptNumericalOptions
    {
      internal:
        PromptDoubleOptions( const double* dfault, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptDoubleOptions(String^ message);

        PromptDoubleOptions(String^ messageAndKeywords, String^ globalKeywords);

        DECLARE_PROP( double, DefaultValue );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

        virtual String^ GetDefaultValueString() sealed override;
    };

    public ref class PromptDistanceOptions sealed : public PromptNumericalOptions
    {
      internal:
        PromptDistanceOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptDistanceOptions( String^ message );

        PromptDistanceOptions( String^ messageAndKeywords, String^ globalKeywords );

        DECLARE_PROP( Geometry::Point3d, BasePoint );

        DECLARE_PROP( double, DefaultValue );

        DECLARE_PROP( bool, UseBasePoint );

        IMP_PROP_FROMBASE( bool, Only2d );

        IMP_PROP_FROMBASE( bool, UseDashedLine );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

        virtual String^ GetDefaultValueString() sealed override;

      private:
        Geometry::Point3d m_basePoint;
        bool m_useBasePoint;
    };

    public ref class PromptCornerOptions : public PromptEditorOptions
    {
      internal:
        PromptCornerOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptCornerOptions( String^ message, Geometry::Point3d basePoint );

        PromptCornerOptions( String^ messageAndKeywords, String^ globalKeywords, Geometry::Point3d basePoint );

        IMP_PROP_FROMBASE( bool, AllowArbitraryInput );

        IMP_PROP_FROMBASE( bool, AllowNone );

        DECLARE_PROP( Geometry::Point3d, BasePoint );

        IMP_PROP_FROMBASE( bool, LimitsChecked );

        IMP_PROP_FROMBASE( bool, UseDashedLine );

      public protected:
        virtual PromptResult^ DoIt() override;

      private:
        Geometry::Point3d m_basePoint;
    };

    public ref class PromptPointOptions : public PromptCornerOptions
    {
      internal:
        PromptPointOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptPointOptions( String^ message );

        PromptPointOptions( String^ messageAndKeywords, String^ globalKeywords );

        DECLARE_PROP( bool, UseBasePoint );

      public protected:
        virtual PromptResult^ DoIt() override;

      private:
        bool m_useBasePoint;
    };

    public ref class PromptSelectionOptions sealed
    {
      internal:
        static String^ NormalizeMessage( String^ message );

        PromptSelectionResult^ Select( SelectionFilter^ filter );

      public:
        PromptSelectionOptions();

        void SetKeywords( String^ keywords, String^ globalKeywords );

        IMPLEMENT_HARD_PROP_GS( bool, AllowDuplicates, m_allowDuplicates );

        IMPLEMENT_HARD_PROP_GS( bool, AllowSubSelections, m_allowSubSelections );

        IMPLEMENT_HARD_PROP_GS( bool, ForceSubSelections, m_forceSubSelections );

        IMPLEMENT_HARD_PROP_GET( KeywordCollection^, Keywords, m_keywords );

        IMPLEMENT_HARD_PROP_GS( String^, MessageForAdding, m_msgAdding );

        IMPLEMENT_HARD_PROP_GS( String^, MessageForRemoval, m_msgRemoval );

        IMPLEMENT_HARD_PROP_GS( bool, PrepareOptionalDetails, m_prepareOptionalDetails );

        IMPLEMENT_HARD_PROP_GS( bool, RejectObjectsFromNonCurrentSpace, m_rejectObjectInNonCurrentSpace );

        IMPLEMENT_HARD_PROP_GS( bool, RejectObjectsOnLockedLayers, m_rejectObjectsOnLockedLayers );

        IMPLEMENT_HARD_PROP_GS( bool, RejectPaperspaceViewport, m_rejectPaperspaceViewport );

        IMPLEMENT_HARD_PROP_GS( bool, SelectEverythingInAperture, m_everythingInAperture );

        IMPLEMENT_HARD_PROP_GS( bool, SingleOnly, m_singleOnly );

        IMPLEMENT_HARD_PROP_GS( bool, SinglePickInSpace, m_singlePickInSpace );

        event SelectionTextInputEventHandler^ KeywordInput
        {
          void add( SelectionTextInputEventHandler^ value )
          {
            m_keywordInputEvent += value;
          }
          void remove( SelectionTextInputEventHandler^ value )
          {
            m_keywordInputEvent += value;
          }
        }

        event SelectionTextInputEventHandler^ UnknownInput
        {
          void add( SelectionTextInputEventHandler^ value )
          {
            m_unknownInputEvent += value;
          }
          void remove( SelectionTextInputEventHandler^ value )
          {
            m_unknownInputEvent += value;
          }
        }

      private:
        bool m_allowDuplicates;
        bool m_allowSubSelections;
        bool m_everythingInAperture;
        bool m_forceSubSelections;
        KeywordCollection^ m_keywords;
        String^ m_msgAdding;
        String^ m_msgRemoval;
        bool m_prepareOptionalDetails;
        bool m_rejectObjectInNonCurrentSpace;
        bool m_rejectObjectsOnLockedLayers;
        bool m_rejectPaperspaceViewport;
        bool m_singleOnly;
        bool m_singlePickInSpace;

        SelectionTextInputEventHandler^ m_keywordInputEvent;
        SelectionTextInputEventHandler^ m_unknownInputEvent;
    };

    public ref class PromptEntityOptions sealed : public PromptEditorOptions
    {
      internal:
        PromptEntityOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, bool allowObjectOnLockedLayer );

      public:
        PromptEntityOptions( String^ message );

        PromptEntityOptions( String^ messageAndKeywords, String^ globalKeywords );

        void AddAllowedClass( Type^ type, bool exactMatch );

        void RemoveAllowedClass( Type^ type );

        void SetRejectMessage( String^ message );

        IMP_PROP_FROMBASE( bool, AllowNone );

        DECLARE_PROP( bool, AllowObjectOnLockedLayer );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

      private:
        bool IsAllowedObject( const OdDbObjectId& idEnt );

      private:
        ArrayList^ m_allowedClasses;
        bool m_allowObjectOnLockedLayer;
        String^ m_rejectMessage;
    };

    public ref class PromptAngleOptions : public PromptEditorOptions
    {
      internal:
        PromptAngleOptions( const OdGePoint3d* pointIn, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords, bool useAngleBase );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

        virtual String^ GetDefaultValueString() sealed override;

      public:
        PromptAngleOptions( String^ message );

        PromptAngleOptions( String^ messageAndKeywords, String^ globalKeywords );

        IMP_PROP_FROMBASE( bool, AllowArbitraryInput );

        IMP_PROP_FROMBASE( bool, AllowNone );

        IMP_PROP_FROMBASE( bool, AllowZero );

        IMP_PROP_FROMBASE( bool, UseDashedLine );

        IMP_PROP_FROMBASE( bool, UseDefaultValue );

        DECLARE_PROP( double, DefaultValue );

        DECLARE_PROP( Geometry::Point3d, BasePoint );

        DECLARE_PROP( bool, UseAngleBase );

        DECLARE_PROP( bool, UseBasePoint );

      private:
        Geometry::Point3d m_basePoint;
        bool m_useAngleBase;
        bool m_useBasePoint;
    };

    public ref class PromptIntegerOptions : public PromptNumericalOptions
    {
      internal:
        PromptIntegerOptions( const int* dfault, const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptIntegerOptions( String^ message );

        PromptIntegerOptions( String^ messageAndKeywords, String^ globalKeywords );

        PromptIntegerOptions( String^ messageAndKeywords, String^ globalKeywords, int lowerLimit, int upperLimit );

        DECLARE_PROP( int, DefaultValue );
        
        IMPLEMENT_HARD_PROP_GS( int, LowerLimit, m_lowerLimit );

        IMPLEMENT_HARD_PROP_GS( int, UpperLimit, m_upperLimit );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

      private:
        int m_lowerLimit;
        int m_upperLimit;
    };

    public ref class PromptKeywordOptions : public PromptEditorOptions
    {
      internal:
        PromptKeywordOptions( const wchar_t* promptString, int initGetFlags, const wchar_t* pKeywords );

      public:
        PromptKeywordOptions( String^ message );

        PromptKeywordOptions( String^ messageAndKeywords, String^ globalKeywords );

        IMP_PROP_FROMBASE( bool, AllowArbitraryInput );

        IMP_PROP_FROMBASE( bool, AllowNone );

      public protected:
        virtual PromptResult^ DoIt() sealed override;
    };

    public ref class PromptStringOptions : public PromptEditorOptions
    {
      internal:
        PromptStringOptions( const wchar_t* promptString, int initGetFlags );

      public:
        PromptStringOptions( String^ message );

        DECLARE_PROP( bool, AllowSpaces );

        DECLARE_PROP( String^, DefaultValue );

        IMP_PROP_FROMBASE( bool, UseDefaultValue );

      public protected:
        virtual PromptResult^ DoIt() sealed override;

      private:
        bool m_allowSpaces;
    };

    public ref class PromptFileOptions abstract
    {
      private protected:
        PromptFileOptions( String^ message );

        DECLARE_PROP( int, Flags );

        DECLARE_PROP( bool, ShowReadOnlyCore );

      internal:
        PromptFileNameResult^ DoIt();

      public:
        DECLARE_PROP( bool, AllowUrls );

        DECLARE_PROP( String^, DialogCaption );

        DECLARE_PROP( String^, DialogName );

        DECLARE_PROP( String^, Filter );

        DECLARE_PROP( int, FilterIndex );

        DECLARE_PROP( String^, InitialDirectory );

        DECLARE_PROP( String^, InitialFileName );

        DECLARE_PROP( String^, Message );

        DECLARE_PROP( bool, PreferCommandLine );

      private:
        String^ GetCommandLinePrompt();

        String^ GetDescriptions();

        String^ GetDialogCaption();

        String^ GetDialogName();

        String^ GetExtensions();

      private:
        String^ m_caption;
        String^ m_dialogName;
        String^ m_filter;
        int m_filterIndex;
        int m_flags;
        String^ m_initialDirectory;
        String^ m_initialFileName;
        String^ m_message;
        bool m_showReadOnly;
    };

    public ref class PromptOpenFileOptions : public PromptFileOptions
    {
      public:
        PromptOpenFileOptions( String^ message );

        DECLARE_PROP( bool, SearchPath );

        DECLARE_PROP( bool, TransferRemoteFiles );
    };

    public ref class PromptSaveFileOptions : public PromptFileOptions
    {
      public:
        PromptSaveFileOptions( String^ message );

        DECLARE_PROP( bool, DeriveInitialFilenameFromDrawingName );

        DECLARE_PROP( bool, DisplaySaveOptionsMenuItem );

        DECLARE_PROP( bool, ForceOverwriteWarningForScriptsAndLisp );
    };
  }
}
