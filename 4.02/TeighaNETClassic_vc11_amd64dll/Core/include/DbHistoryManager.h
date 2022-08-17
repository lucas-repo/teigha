#ifndef _ODDBHISTORYMANAGER_H_INCLUDED_
#define _ODDBHISTORYMANAGER_H_INCLUDED_

#include "OdString.h"
#include "DbHandle.h"
#include "OdStreamBuf.h"
#define STL_USING_MAP
#include "OdaSTL.h"

typedef OdArray<OdDbHandle> OdDbHandleArray;
typedef std::map<OdDbHandle, OdDbHandle> OdDbHandleMap;
class OdDbDatabaseImpl;

struct OdDbHistoryLogRecord
{
  OdString message;
  OdTimeStamp timestamp;
  OdString author;
};

class TOOLKIT_EXPORT OdDbHistoryManager
{
public:
  /** \details
  Current revision number.
  */
  OdUInt32 revision();

  /** \details
  Additional details associated with the given revision, such as log message, timestamp, etc.
  Returns false if the given revision does not exist.
  */
  bool getLogMessage(OdUInt32 revision, OdDbHistoryLogRecord& log);

  /** \details
  Associate additional details with the given revision, such as log message, timestamp, etc.
  Returns false if the given revision does not exist.
  */
  bool setLogMessage(OdUInt32 revision, const OdDbHistoryLogRecord& log);

  /** \details
  Objects changed in the given revision.
  Array is not cleared, handles are appended to the end.
  */
  void getChangedObjects(OdUInt32 revision, OdDbHandleArray& ids);

  /** \details
  Return the changes made in the last revisions as a portable binary stream.
  
  \param revisions [in] Number of revisions to get
  \param rollback [in] If true, then these revisions are reverted in database (e.g. to be reapplied after other changes)
  */
  OdStreamBufPtr getChanges(OdUInt32 revisions, bool rollback = false);
  
  /** \details
  Apply the changes received from OdDbHistoryManager.changes (maybe in the copied database)
  \param changes [in] Binary stream containing the changes
  \param bPreserveHandles [in] If true then the new objects in the changes stream retain their handles, otherwise they may be translated
  \param clientMap [in, optional] If present, it represents the changes in handles caused by merging server changes on a client. May be used to find erased added objects while reapplying client changes
  \param serverMap [in, optional] If present, it represents the changes in handles caused by merging client changes on a server. The target handles in this map will be used for the new objects added while reapplying client changes.

  Returns the map of the newly added objects handles [handle read from the stream] -> [actual handle in the database]
  */
  OdDbHandleMap applyChanges(OdStreamBuf* changes, bool bPreserveHandles, OdDbHandleMap* clientMap = 0, OdDbHandleMap* serverMap = 0);

  /** \details
  Last version synchronized with the server.
  */
  OdUInt32 lastSyncRevision();
  /** \details
  Set the last version synchronized with the server. (Presumably, after applying changes and resolving conflicts)
  */
  void setLastSyncRevision(OdUInt32);
private:
  friend class OdDbDatabaseImpl;
  OdDbHistoryManager(OdDbDatabaseImpl*);
  OdDbDatabaseImpl* m_pImpl;
};

#endif
