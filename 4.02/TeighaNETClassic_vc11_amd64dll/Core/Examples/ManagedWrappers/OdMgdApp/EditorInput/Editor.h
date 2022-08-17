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

namespace Teigha
{
  namespace ApplicationServices
  {
    ref class Document;
  }

  using namespace ApplicationServices;

  namespace EditorInput
  {
    ref class PromptOptions;
    ref class PromptDoubleOptions;
    ref class PromptPointOptions;
    ref class PromptOpenFileOptions;
    ref class PromptSaveFileOptions;
    ref class PromptIntegerOptions;
    ref class PromptKeywordOptions;
    ref class PromptStringOptions;
    ref class PromptAngleOptions;
    ref class PromptCornerOptions;

    ref class PromptResult;
    ref class PromptDoubleResult;
    ref class PromptPointResult;
    ref class PromptFileNameResult;
    ref class PromptIntegerResult;
    ref class SelectionFilter;
    ref class SelectionSet;

    public ref class Editor sealed  : public MarshalByRefObject
    {
      internal:
        Editor( ApplicationServices::Document^ doc );

      public:
        PromptResult^ DoPrompt( PromptOptions^ opt );

        PromptDoubleResult^ GetAngle( PromptAngleOptions^ options );

        PromptDoubleResult^ GetAngle( String^ message );

        PromptPointResult^ GetCorner( PromptCornerOptions^ options );

        PromptPointResult^ GetCorner( String^ message, Geometry::Point3d basePoint );

        PromptDoubleResult^ GetDistance( PromptDistanceOptions^ options );

        PromptDoubleResult^ GetDistance( String^ message );

        PromptDoubleResult^ GetDouble( PromptDoubleOptions^ options );

        PromptDoubleResult^ GetDouble( String^ message );

        PromptEntityResult^ GetEntity( PromptEntityOptions^ options );

        PromptEntityResult^ GetEntity( String^ message );

        PromptFileNameResult^ GetFileNameForOpen( PromptOpenFileOptions^ options );

        PromptFileNameResult^ GetFileNameForOpen( String^ message );

        PromptFileNameResult^ GetFileNameForSave( PromptSaveFileOptions^ options );

        PromptFileNameResult^ GetFileNameForSave( String^ message );

        PromptIntegerResult^ GetInteger( PromptIntegerOptions^ options );

        PromptIntegerResult^ GetInteger( String^ message );

        PromptResult^ GetKeywords( PromptKeywordOptions^ options );

        PromptResult^ GetKeywords( String^ message, ...array<String^>^ globalKeywords );

        PromptResult^ GetString( PromptStringOptions^ options );

        PromptResult^ GetString( String^ message );

        //PromptNestedEntityResult^ GetNestedEntity( PromptNestedEntityOptions^ options );

        //PromptNestedEntityResult^ GetNestedEntity( String^ message );

        PromptPointResult^ GetPoint( PromptPointOptions^ options );

        PromptPointResult^ GetPoint( String^ message );

        PromptSelectionResult^ GetSelection();

        PromptSelectionResult^ GetSelection( SelectionFilter^ filter );

        PromptSelectionResult^ GetSelection( PromptSelectionOptions^ options );

        PromptSelectionResult^ GetSelection( PromptSelectionOptions^ options, SelectionFilter^ filter );

        PromptSelectionResult^ SelectAll();

        PromptSelectionResult^ SelectAll( SelectionFilter^ filter );

        PromptSelectionResult^ SelectCrossingPolygon( Geometry::Point3dCollection^ polygon );

        PromptSelectionResult^ SelectCrossingPolygon( Geometry::Point3dCollection^ polygon, SelectionFilter^ filter );

        PromptSelectionResult^ SelectCrossingWindow( Geometry::Point3d pt1, Geometry::Point3d pt2 );

        PromptSelectionResult^ SelectCrossingWindow( Geometry::Point3d pt1, Geometry::Point3d pt2, SelectionFilter^ filter );

        PromptSelectionResult^ SelectFence( Geometry::Point3dCollection^ fence );

        PromptSelectionResult^ SelectFence( Geometry::Point3dCollection^ fence, SelectionFilter^ filter );

        PromptSelectionResult^ SelectWindow( Geometry::Point3d pt1, Geometry::Point3d pt2 );

        PromptSelectionResult^ SelectWindow( Geometry::Point3d pt1, Geometry::Point3d pt2, SelectionFilter^ filter );

        PromptSelectionResult^ SelectWindowPolygon( Geometry::Point3dCollection^ polygon );

        PromptSelectionResult^ SelectWindowPolygon( Geometry::Point3dCollection^ polygon, SelectionFilter^ filter );

        PromptSelectionResult^ SelectImplied();

        PromptSelectionResult^ SelectLast();

        PromptSelectionResult^ SelectPrevious();

        void SetImpliedSelection( SelectionSet^ selectionSet );

        void SetImpliedSelection( array< DatabaseServices::ObjectId >^ selectedObjects );

        Geometry::Point3d Snap( String^ snapMode, Geometry::Point3d input );

        void SwitchToModelSpace();

        void SwitchToPaperSpace();

        //EditorUserInteraction^ StartUserInteraction( Control^ modalForm );

        //int TurnForcedPickOff();

        //int TurnForcedPickOn();

        Point PointToScreen( Geometry::Point3d pt, int viewportNumber );

        Geometry::Point3d PointToWorld( Point pt );

        Geometry::Point3d PointToWorld( Point pt, int viewportNumber );

        void UpdateScreen();

        void UpdateTiledViewportsFromDatabase();

        void UpdateTiledViewportsInDatabase();

        DatabaseServices::ViewTableRecord^ GetCurrentView();

        int GetViewportNumber( Point point );

        DatabaseServices::ObjectId ViewportIdFromNumber( int viewportNumber );

        void SetCurrentView( DatabaseServices::ViewTableRecord^ value );

        int GetCommandVersion();

        int InitCommandVersion( int nVersion );

        void Regen();

        void WriteMessage( String^ message );

        void WriteMessage( String^ message, ...array< Object^ >^ parameter );

        IMLEMENT_SIMPLE_EVENT( DraggingEventHandler, Dragging, DraggingEventArgs );
        IMLEMENT_SIMPLE_EVENT( DraggingEndedEventHandler, DraggingEnded, DraggingEndedEventArgs );
        IMLEMENT_SIMPLE_EVENT( EventHandler, EnteringQuiescentState, EventArgs );
        IMLEMENT_SIMPLE_EVENT( EventHandler, LeavingQuiescentState, EventArgs );
        //IMLEMENT_SIMPLE_EVENT( PointFilterEventHandler, PointFilter, PointFilterEventArgs );
        //IMLEMENT_SIMPLE_EVENT( PointMonitorEventHandler, PointMonitor, PointMonitorEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptDoubleResultEventHandler, PromptedForAngle, PromptDoubleResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptPointResultEventHandler, PromptedForCorner, PromptPointResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptDoubleResultEventHandler, PromptedForDistance, PromptDoubleResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptDoubleResultEventHandler, PromptedForDouble, PromptDoubleResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptEntityResultEventHandler, PromptedForEntity, PromptEntityResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptIntegerResultEventHandler, PromptedForInteger, PromptIntegerResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptStringResultEventHandler, PromptedForKeyword, PromptStringResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptNestedEntityResultEventHandler, PromptedForNestedEntity, PromptNestedEntityResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptPointResultEventHandler, PromptedForPoint, PromptPointResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptSelectionResultEventHandler, PromptedForSelection, PromptSelectionResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptStringResultEventHandler, PromptedForString, PromptStringResultEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptForEntityEndingEventHandler, PromptForEntityEnding, PromptForEntityEndingEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptForSelectionEndingEventHandler, PromptForSelectionEnding, PromptForSelectionEndingEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptAngleOptionsEventHandler, PromptingForAngle, PromptAngleOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptPointOptionsEventHandler, PromptingForCorner, PromptPointOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptDistanceOptionsEventHandler, PromptingForDistance, PromptDistanceOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptDoubleOptionsEventHandler, PromptingForDouble, PromptDoubleOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptEntityOptionsEventHandler, PromptingForEntity, PromptEntityOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptIntegerOptionsEventHandler, PromptingForInteger, PromptIntegerOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptKeywordOptionsEventHandler, PromptingForKeyword, PromptKeywordOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptNestedEntityOptionsEventHandler, PromptingForNestedEntity, PromptNestedEntityOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptPointOptionsEventHandler, PromptingForPoint, PromptPointOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptSelectionOptionsEventHandler, PromptingForSelection, PromptSelectionOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( PromptStringOptionsEventHandler, PromptingForString, PromptStringOptionsEventArgs );
        IMLEMENT_SIMPLE_EVENT( RolloverEventHandler, Rollover, RolloverEventArgs );
        IMLEMENT_SIMPLE_EVENT( SelectionAddedEventHandler, SelectionAdded, SelectionAddedEventArgs );
        IMLEMENT_SIMPLE_EVENT( SelectionRemovedEventHandler, SelectionRemoved, SelectionRemovedEventArgs );

      public:
        DECLARE_PROP_GET( DatabaseServices::ObjectId, ActiveViewportId );

        DECLARE_PROP_GET( Geometry::Matrix3d, CurrentUserCoordinateSystem );

        DECLARE_PROP_GET( DatabaseServices::ObjectId, CurrentViewportObjectId );

        DECLARE_PROP_GET( ApplicationServices::Document^, Document );
        
        DECLARE_PROP_GET( bool, IsDragging );
        
        DECLARE_PROP_GET( bool, IsQuiescent );
        
        DECLARE_PROP_GET( bool, IsQuiescentForTransparentCommand );
        
        DECLARE_PROP_GET( bool, MouseHasMoved );
        
        DECLARE_PROP_GET( bool, UseCommandLineInterface );

      private:
        DraggingEventHandler^ m_pDragging;
        DraggingEndedEventHandler^ m_pDraggingEnded;
        EventHandler^ m_pEnteringQuiescentState;
        EventHandler^ m_pLeavingQuiescentState;
        //PointFilterEventHandler^ m_pPointFilter;
        //PointMonitorEventHandler^ m_pPointMonitor;
        PromptDoubleResultEventHandler^ m_pPromptedForAngle;
        PromptPointResultEventHandler^ m_pPromptedForCorner;
        PromptDoubleResultEventHandler^ m_pPromptedForDistance;
        PromptDoubleResultEventHandler^ m_pPromptedForDouble;
        PromptEntityResultEventHandler^ m_pPromptedForEntity;
        PromptIntegerResultEventHandler^ m_pPromptedForInteger;
        PromptStringResultEventHandler^ m_pPromptedForKeyword;
        PromptNestedEntityResultEventHandler^ m_pPromptedForNestedEntity;
        PromptPointResultEventHandler^ m_pPromptedForPoint;
        PromptSelectionResultEventHandler^ m_pPromptedForSelection;
        PromptStringResultEventHandler^ m_pPromptedForString;
        PromptForEntityEndingEventHandler^ m_pPromptForEntityEnding;
        PromptForSelectionEndingEventHandler^ m_pPromptForSelectionEnding;
        PromptAngleOptionsEventHandler^ m_pPromptingForAngle;
        PromptPointOptionsEventHandler^ m_pPromptingForCorner;
        PromptDistanceOptionsEventHandler^ m_pPromptingForDistance;
        PromptDoubleOptionsEventHandler^ m_pPromptingForDouble;
        PromptEntityOptionsEventHandler^ m_pPromptingForEntity;
        PromptIntegerOptionsEventHandler^ m_pPromptingForInteger;
        PromptKeywordOptionsEventHandler^ m_pPromptingForKeyword;
        PromptNestedEntityOptionsEventHandler^ m_pPromptingForNestedEntity;
        PromptPointOptionsEventHandler^ m_pPromptingForPoint;
        PromptSelectionOptionsEventHandler^ m_pPromptingForSelection;
        PromptStringOptionsEventHandler^ m_pPromptingForString;
        RolloverEventHandler^ m_pRollover;
        SelectionAddedEventHandler^ m_pSelectionAdded;
        SelectionRemovedEventHandler^ m_pSelectionRemoved;

      internal:
        [ThreadStatic]
        static Editor^ g_CurrentEditor;
        ApplicationServices::Document^ m_doc;
      //  PromptStatus RunCommand( ...array< Object^ >^ parameter );
      //  void removeInputContextEvent( long long flag );
    };
  }
}
