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
  namespace ApplicationServices
  {
    ref class Document;

    public ref class DocumentCollection sealed : public MarshalByRefObject, public Collections::ICollection
    {
      public:
        DocumentCollection();

        void CloseAll();
        
        void CopyTo( array<Document^>^ arr, int index );

        Document^ GetDocument( DatabaseServices::Database^ db );

        virtual Collections::IEnumerator^ GetEnumerator();

        Document^ Open( String^ fileName );

        Document^ Open( String^ fileName, bool forReadOnly );

        Document^ Open( String^ fileName, bool forReadOnly, String^ password );

      public:
        DECLARE_PROP( Document^, MdiActiveDocument );
        
        virtual DECLARE_PROP_GET( int, Count );

      public:
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentActivated, DocumentCollectionEventArgs );
        //IMLEMENT_SIMPLE_EVENT( DocumentActivationChangedEventHandler, DocumentActivationChanged, DocumentActivationChangedEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentBecameCurrent,     DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentCreated,           DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentCreateStarted,     DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentCreationCanceled,  DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentDestroyedEventHandler, DocumentDestroyed,          DocumentDestroyedEventArgs );
        //IMLEMENT_SIMPLE_EVENT( DocumentLockModeChangedEventHandler, DocumentLockModeChanged );
        //IMLEMENT_SIMPLE_EVENT( DocumentLockModeChangeVetoedEventHandler, DocumentLockModeChangeVetoed );
        //IMLEMENT_SIMPLE_EVENT( DocumentLockModeWillChangeEventHandler, DocumentLockModeWillChange );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentToBeActivated,   DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentToBeDeactivated, DocumentCollectionEventArgs );
        IMLEMENT_SIMPLE_EVENT( DocumentCollectionEventHandler, DocumentToBeDestroyed,   DocumentCollectionEventArgs );

      private:
        virtual property bool IsSynchronized
        {
          bool get() sealed = Collections::ICollection::IsSynchronized::get { return false; }
        }
        virtual property Object^ SyncRoot
        {
          Object^ get() sealed = Collections::ICollection::SyncRoot::get { return nullptr; }
        }
        
        virtual void CopyTo( Array^ arr, int index ) sealed = Collections::ICollection::CopyTo;

      private:

        DocumentActivationChangedEventHandler^    m_pDocumentActivationChanged;
        //DocumentLockModeChangedEventHandler^      m_pDocumentLockModeChanged;
        //DocumentLockModeChangeVetoedEventHandler^ m_pDocumentLockModeChangeVetoed;
        //DocumentLockModeWillChangeEventHandler^   m_pDocumentLockModeWillChange;
        DocumentCollectionEventHandler^ m_pDocumentActivated;
        DocumentCollectionEventHandler^ m_pDocumentBecameCurrent;
        DocumentCollectionEventHandler^ m_pDocumentCreated;
        DocumentCollectionEventHandler^ m_pDocumentCreateStarted;
        DocumentCollectionEventHandler^ m_pDocumentCreationCanceled;
        DocumentCollectionEventHandler^ m_pDocumentToBeActivated;
        DocumentCollectionEventHandler^ m_pDocumentToBeDeactivated;
        DocumentCollectionEventHandler^ m_pDocumentToBeDestroyed;
        DocumentDestroyedEventHandler^  m_pDocumentDestroyed;
    };
  }
}
