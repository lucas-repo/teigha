#ifndef _OdTfDwgFiler_h_Included_
#define _OdTfDwgFiler_h_Included_

#include "DbFiler.h"
#include "../Source/database/DbUndoFiler.h"
#include "RxDynamicModule.h"
#include <set>
#include <OdPlatformStreamer.h>
#include <list>
#include <vector>
#include <fstream>
#include <stack>
#include "../Source/Tf/TfModule.h"
/*
#define _ÑheckForArrayElemet OdTf::TypesEnum t = m_pFilerXmlData->stepToNextSchemaItem();\
const char *positionInArray = NULL;\
while (m_pImpl->m_pDiffIterator->FirstChildElement())\
m_pImpl->m_pDiffIterator = m_pImpl->m_pDiffIterator->FirstChildElement();\
if (strcmp(m_pImpl->m_pSchemaIterator->Attribute("name"), m_pImpl->m_pDiffIterator->Value()) == 0)\
{\
  positionInArray = m_pImpl->m_pDiffIterator->Attribute("positionInArray");\
  if ((positionInArray && m_pFilerXmlData->m_valueCache[m_pImpl->m_arrayName] == atoi(positionInArray))\
    || !positionInArray)\
  {\
    ApplyChanges::writeData(t, m_pImpl, m_pFilerXmlData);\
    m_pImpl->stepToNextDiffItem();\
  }
  */
class OdTfDwgFilerImpl;
class TiXmlDocument;
class OdTfDiffFilerImpl;
class ChangesCalculation;

struct BinaryDataStruct
{
  char *pBinaryData;
  OdUInt32 size;
  OdUInt32 posBinaryData;
  
};

class OdTfDwgFiler : public OdDbUndoFiler
{
  friend class ChangesCalculation;
  OdTfDwgFilerImpl* m_pImpl;
  ChangesCalculation* m_pChangesCalc;
  bool m_usesMultiUserEdit;
  bool m_bApplyDiff;
  bool m_bPassXmlData;
  inline void checkChanges(OdTf::TypesEnum tBase, void *value, OdString string = OdString());
  void wrObjectId(const OdDbObjectId &id);
  template<class T>
  void wrValue(const T &value, OdUInt32 countBytes = 0);
  template<class T>
  void wrGeValue(T value, OdUInt8 countPoints , double , double , double z = 0);

public:
  OdTfDiffFilerImpl *m_pFilerXmlData;
  void setChangesCalc(ChangesCalculation *pointer) { m_pChangesCalc = pointer; }
  OdTfDwgFilerImpl* getImpl() {return m_pImpl;};
  bool& bApplyDiff() { return m_bApplyDiff; };
  void setUsedMUE() {m_usesMultiUserEdit = true;};

  ODRX_HEAP_OPERATORS();
  ODRX_DECLARE_MEMBERS(OdTfDwgFiler);
  OdTfDwgFiler();
  ~OdTfDwgFiler();
  virtual FilerType filerType() const ODRX_OVERRIDE { return kUndoFiler; }
  virtual OdDbDatabase* database() const ODRX_OVERRIDE;
  virtual void seek(OdInt32 offset, OdDb::FilerSeekType seekType) ODRX_OVERRIDE {}
  virtual OdUInt32 tell() const ODRX_OVERRIDE { return 0; }
  virtual bool rdBool() ODRX_OVERRIDE;
  virtual OdString rdString();
  virtual void rdBytes(void* buffer, OdUInt32 numBytes);
  virtual OdInt8 rdInt8();
  virtual OdUInt8 rdUInt8();
  virtual OdInt16 rdInt16();
  virtual OdInt32 rdInt32();
  virtual OdInt64 rdInt64();
  virtual void* rdAddress();
  virtual double rdDouble();
  virtual OdDbHandle rdDbHandle();
  virtual OdDbObjectId rdSoftOwnershipId();
  virtual OdDbObjectId rdHardOwnershipId();
  virtual OdDbObjectId rdHardPointerId();
  virtual OdDbObjectId rdSoftPointerId();
  virtual OdGePoint2d rdPoint2d();
  virtual OdGePoint3d rdPoint3d();
  virtual OdGeVector2d rdVector2d();
  virtual OdGeVector3d rdVector3d();
  virtual OdGeScale3d rdScale3d();
  virtual void wrBool(bool value);
  virtual void wrString(const OdString &value);
  virtual void wrBytes(const void* buffer, OdUInt32 numBytes);
  virtual void wrInt8(OdInt8 value);
  virtual void wrUInt8(OdUInt8 value);
  virtual void wrInt16(OdInt16 value);
  virtual void wrInt32(OdInt32 value);
  virtual void wrInt64(OdInt64 value);
  virtual void wrAddress(const void* value);
  virtual void wrDouble(double value);
  virtual void wrDbHandle(const OdDbHandle& value);
  virtual void wrSoftOwnershipId(const OdDbObjectId& value);
  virtual void wrHardOwnershipId(const OdDbObjectId& value);
  virtual void wrSoftPointerId(const OdDbObjectId& value);
  virtual void wrHardPointerId(const OdDbObjectId& value);
  virtual void wrPoint2d(const OdGePoint2d& value);
  virtual void wrPoint3d(
    const OdGePoint3d& value);

  /** \details
  Writes the specified 2D vector value to this Filer object.
  \param value [in]  Value.
  */
  virtual void wrVector2d(
    const OdGeVector2d& value);

  /** \details
  Writes the specified 3D vector value to this Filer object.
  \param value [in]  Value.
  */
  virtual void wrVector3d(
    const OdGeVector3d& value);

  /** \details
  Writes the specified 3D scale value to this Filer object.
  \param value [in]  Value.
  */
  virtual void wrScale3d(
    const OdGeScale3d& value);

  virtual bool usesReferences() const ODRX_OVERRIDE { return true; }

  virtual void addReference(OdDbObjectId id, OdDb::ReferenceType rt) ODRX_OVERRIDE;

  static TiXmlDocument* saveDwgToXml(OdDbDatabase* pDb, const char* pcszXsdFilename, OdDbObjectId idObj);

  // undo specific overrides
  virtual void wrDatabase(OdDbDatabase* pDb) ODRX_OVERRIDE;
  virtual OdDbDatabase* rdDatabase() ODRX_OVERRIDE;
  virtual void wrClass(const OdRxClass* c) ODRX_OVERRIDE;
  virtual OdRxClass* rdClass() ODRX_OVERRIDE;
};

class ChangesCalculation
{
  friend class OdTfDwgFiler;
public:  
  ChangesCalculation();
  /** \details
  Calculate and save difference (to xml) between rollback data in database and stream of changing
  \param pDb [in]  pointer to database.
  \param bufPtr [in]  pointer to stream of changes( get it from OdDbHistoryManager::getChanges(OdUInt32 revisions, bool rollback) )
  \param filePath [in] path to save xml file with difference.
  \note
  */
  void calculateDifference(OdDbDatabase *pDb, OdStreamBufPtr bufPtr, const char *filePath);

private:
  OdTfDiffFilerImpl *m_pFilerServerData;
  OdTfDwgFilerImpl *m_filer;
  OdSmartPtr<OdTfDwgFiler> m_pFiler;
  OdString m_tableName;
  OdStreamBuf *m_buf; 
  /** \details
  Initialisation fields before to write differences to xml
  \param filePath [in]  path to save xml file with difference
  \note
  */
  void initParts(const char *filePath);
  
  /** \details
  Use it if types of data what are compared not match
  \param t1 [in]  type getting from scheme of rollbacl data
  \param t2 [in]  type getting from scheme of binary stream
  \note Before compare data from base and from binary stream, compare types 
  */
  void matchingPositionScheme(OdTf::TypesEnum &t1, OdTf::TypesEnum &t2, OdTfDwgFiler *actual);
  /** \details
  Write "log message" from binary stream to xml
  */
  void writeLogMessageToXml();
  /** \details
  Write "idStream, stringStream" from binary stream to xml
  */
  void writeId_StringStreamsToXml();
  /** \details
  Writing partialUndo data to xml from binary stream
  \param numRev [in]  number of current revision 
  \param nextDataStream [in]  size to step to next revision in stream
  \param sizeDataStream [in]  size data of revision in stream
  \param curPos [in]  position of start part of data in revision
  \param nextRecPos [in]  size to step to next part data in revision
  \param massDataLengthRevision [in]  array of lenghts revisions
  \param opCode [in]  opCode for partialUndo
  \param partialUndo [in]  size to step to next part data in revision
  \note
  */
  void parsingPartialUndo(OdInt32 &numRev, OdInt32 &nextDataStream, OdInt32 &sizeDataStream, OdInt32 &curPos, OdInt32 &nextRecPos,
    OdInt32 *massDataLengthRevision, OdInt16 &opCode, bool &partialUndo );
  /** \details
  Calculate length each revision in stream
  \param countRevision [in]  amount revisions in stream
  \param massDataLengthRevision [in] array to save length each revision
  \note
  */
  void getLenghtEachRevision(OdInt32 &countRevision, OdInt32 *massDataLengthRevision);
  /** \details
  Get data from stream and compare with data from rollback database. 
  \param value [in]  pointer to data from rollback database
  \param t [in] type of data
  \param string [in] if data can't transferred how pointer (example string, handle)
  \param buf [in] for send data of hexBinary type
  \param numBytes [in] count bytes for buf
  \note If data different, write data from stream to xml
  */
  void isDifferentWrite(void* value, OdTf::TypesEnum t, OdString &string, const void * buf = 0x0, OdUInt32 numBytes = 0);

    template <class T>
    OdString rdString(T *buf)
    {
      OdInt16 strLen;
      buf->getBytes(&strLen, 2);
      OdBinaryData buff;
      buff.resize(strLen * 2);
      buf->getBytes(buff.asArrayPtr(), buff.length());
      const OdUInt8* pBuff = buff.asArrayPtr();
      return OdPlatformStreamer::getUnicodeStrFromBuffer(pBuff, strLen);
    }
};


class ApplyChanges
{
  
public:
  ApplyChanges() {};
  ~ApplyChanges() {};
  static OdDbHandleMap m_handleMap;
  static bool m_bIsServer;
  struct TRecordNewObject
  {
    OdString s_name;
    OdUInt32 s_pos;
    size_t s_numInList;
    TRecordNewObject() : s_pos(-1) { ; };
    TRecordNewObject(OdString name, OdUInt32 pos, size_t num)
      : s_name(name), s_pos(pos), s_numInList(num) {;};
  };
  /** \details
  Sets changing from xml file to databse.
  \param pDb [in]  pointer to database.
  \param bApplyDiff [in]  set in TfDwgFiler flag to apply Difference(not calculate)
  \param bPreserveHandles [in] If true then the new objects in the changes stream retain their handles, otherwise they may be translated
  \param filePath [in] path to load xml file with difference.
  \param clientMap [in, optional] If present, it represents the changes in handles caused by merging server changes on a client. May be used to find erased added objects while reapplying client changes
  \param serverMap [in, optional] If present, it represents the changes in handles caused by merging client changes on a server. The target handles in this map will be used for the new objects added while reapplying client changes.
  Returns the map of the newly added objects handles [handle read from the file] -> [actual handle in the database]
  \note  Generation from xml file binary stream and send it to OdDbHistoryManager::applyChanges(OdStreamBuf* s, bool bPreserveHandles,
  OdDbHandleMap* clientMap, OdDbHandleMap* serverMap)
  */
  OdDbHandleMap applyDifference(OdDbDatabase *pDb, bool bApplyDiff, bool bPreserveHandles, const char *filePath, OdDbHandleMap* clientMap = 0, OdDbHandleMap* serverMap = 0);
  /** \details
  Write data from xml to char array
  \param t [in] type of write data
  \param m_filer [in]  filer with iterator by xml
  \param schemeIter [in] filer with iterator by scheme for xml
  \note  
  */
  static void writeData(OdTf::TypesEnum t, OdTfDwgFilerImpl *m_filer, OdTfDiffFilerImpl *schemeIter, OdString *tableNameFromXml = 0);
  template<class T>
  /** \details
  Check tags in scheme with atribute: isCondition, isArrayLength, isBreak
  \param value [in] data
  \param schemeIter [in] filer where to stored value cache
  \note
  */
  static inline void checkCondition(T value, OdTfDwgFilerImpl *schemeIter);
	
private:
  BinaryDataStruct m_binData;
  OdTfDwgFilerImpl *m_filer;
  OdSmartPtr<OdTfDwgFiler> m_pFiler;
  std::list<std::pair<OdUInt32, char*> > m_listBinObjects;//ñïèñîê îáúåêòîâ äëÿ îäíîé ðåâèçèè
  std::map<OdDbHandle, TRecordNewObject> m_mapCreatedObjects;
  /** \details
  Parsing partial undo object from xml to char array
  \param handle [in] handle of object
  \param tableName [in] table name of current object
  \param schema [in] scheme for iterator
  \note
  */
  void writePartialUndoData(OdDbHandle handle, OdString tableName, TiXmlDocument &schema);
  /** \details
  Parsing added object by partial undo
  \param schemeIter [in] iterator for scheme of current object
  \param t [in] type of data
  \note
  */
  void appendedObjectData(OdTfDiffFilerImpl *schemeIter, OdTf::TypesEnum &t);
  /** \details
  Initialisation fields to get differences from xml and generate binary data
  \param pDb [in]  pointer to database.
  \param schema [in] init scheme for iterator
  \param xml [in]  will hold loaded xml
  \param pRoot [in]  root tag of xml and scheme
  \param bApplyDiff [in]  set in TfDwgFiler flag to apply Difference(not calculate)
  \param filePath [in]  path to save xml file with difference
  \note
  */
  void init(OdDbDatabase * pDb, TiXmlDocument &schema, TiXmlDocument &xml, TiXmlElement *pRoot, bool bApplyDiff, const char *filePath);
  /** \details
  Get idStream, stringStream, LogMessage data from xml and write to char array
  \note
  */
  void writeInformationData();
  /** \details
  Change the data of object what addedin thos revision and are not applied yet
  \param pDb [in]  pointer to database.
  \param schema [in] scheme for iterator
  \param tableName [in] name table of object
  \param handle [in]  handle of object
  \note
  */
  void changeDataAddedObject(OdDbDatabase * pDb, TiXmlDocument &schema, OdString &tableName, OdDbHandle &handle);
  /** \details
  Creat binary stream for revision from list char arrays of objects
  \param size [in/out]  size of all data together
  \note
  */
  char* getFinalData(OdUInt32 &size, OdInt32 mergeBlockingAndMarks);


  TiXmlElement* stepBeginOfTable(TiXmlElement *e)
  {
    while (strcmp(e->Parent()->Value(), "TeighaDrawing") != 0)
      e = (TiXmlElement*)e->Parent();
    return (TiXmlElement*)e;
  }

  TiXmlElement* stepToLastDiffItemInPrevTable(TiXmlElement *e)
  {
    if (strcmp(e->Value(), "handleNotSystemData") == 0)
    {
      while (strcmp(e->Parent()->Value(), "TeighaDrawing") != 0)
        e = (TiXmlElement*)e->Parent();
    }
    TiXmlElement *n = (TiXmlElement*)e->PreviousSibling()->LastChild();
    do
    {
      while (n->LastChild())
        n = (TiXmlElement*)n->LastChild();
      while (n->NextSiblingElement())
        n = n->NextSiblingElement();
    } while (n->NextSiblingElement() || n->FirstChildElement());

    return n;
  };
  
};



inline OdUInt32 encodeToBase64(const unsigned char* bytesToEncode, OdUInt32 length,
  OdAnsiString& sOut, bool inQuotes = false)
{
  static const char* base64Dict = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

  //char* res = new char[len * 8 / 6 + 2];
  char* res = sOut.getBuffer(length * 8 / 6 + 4 + (inQuotes ? 2 : 0));
  if (inQuotes)
    *res++ = '\"';

  OdUInt32 len = length,
    finalLen = 0,
    i = 0,
    j = 0;
  unsigned char charArraySrc[3];
  unsigned char charArrayFin[4];

  while (len--) {
    charArraySrc[i++] = *(bytesToEncode++);
    if (i == 3) {
      charArrayFin[0] = (charArraySrc[0] & 0xfc) >> 2;
      charArrayFin[1] = ((charArraySrc[0] & 0x03) << 4) + ((charArraySrc[1] & 0xf0) >> 4);
      charArrayFin[2] = ((charArraySrc[1] & 0x0f) << 2) + ((charArraySrc[2] & 0xc0) >> 6);
      charArrayFin[3] = charArraySrc[2] & 0x3f;

      for (i = 0; i < 4; i++)
        res[finalLen++] = base64Dict[charArrayFin[i]];
      i = 0;
    }
  }

  if (i != 0)
  {
    for (j = i; j < 3; j++)
      charArraySrc[j] = '\0';

    charArrayFin[0] = (charArraySrc[0] & 0xfc) >> 2;
    charArrayFin[1] = ((charArraySrc[0] & 0x03) << 4) + ((charArraySrc[1] & 0xf0) >> 4);
    charArrayFin[2] = ((charArraySrc[1] & 0x0f) << 2) + ((charArraySrc[2] & 0xc0) >> 6);
    charArrayFin[3] = charArraySrc[2] & 0x3f;

    for (j = 0; j < i + 1; j++)
      res[finalLen++] = base64Dict[charArrayFin[j]];

    while ((i++ < 3))
      res[finalLen++] = '=';
  }

  ODA_ASSERT_ONCE(finalLen <= (length * 8 / 6 + 3));
  if (inQuotes)
  {
    res[finalLen] = '\"';
    res[finalLen + 1] = 0; // res[finalLen + 1] = 0;
    finalLen += 2;
  }
  else
    res[finalLen] = 0; // res[finalLen + 1] = 0;

                       //sOut.releaseBuffer();
  return finalLen;
}

inline OdUInt32 decodeBase64(const OdAnsiString& stringToDecode, OdBinaryData& out)
{
  static unsigned char* decode64Dict = NULL;
  if (!decode64Dict)
  {
    static const char* base64Dict = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    static OdBinaryData dataDict;
    dataDict.resize(256);
    decode64Dict = dataDict.asArrayPtr();
    for (OdUInt32 i = 0; i < 64; i++)
      decode64Dict[base64Dict[i]] = (unsigned char)i;
  }

  OdUInt32 length = stringToDecode.getLength();
  if (length % 4 != 0)
    return -1;
  const char* pToDecode = stringToDecode.c_str();

  OdUInt32 outLength = length * 3 / 4;
  outLength -= (pToDecode[length - 2] == '=') ? 2 : ((pToDecode[length - 1] == '=') ? 1 : 0);

  out.resize(outLength);
  unsigned char* data = (unsigned char*)out.asArrayPtr();

  for (OdUInt32 i = 0, j = 0; i < length;)
  {
    OdUInt32 sextet_a = (pToDecode[i] == '=') ? (0 & i++) : decode64Dict[pToDecode[i++]];
    OdUInt32 sextet_b = (pToDecode[i] == '=') ? (0 & i++) : decode64Dict[pToDecode[i++]];
    OdUInt32 sextet_c = (pToDecode[i] == '=') ? (0 & i++) : decode64Dict[pToDecode[i++]];
    OdUInt32 sextet_d = (pToDecode[i] == '=') ? (0 & i++) : decode64Dict[pToDecode[i++]];

    OdUInt32 triple = (sextet_a << 3 * 6) + (sextet_b << 2 * 6) + (sextet_c << 1 * 6) + (sextet_d << 0 * 6);

    if (j < outLength)
      data[j++] = (triple >> 2 * 8) & 0xFF;
    if (j < outLength)
      data[j++] = (triple >> 1 * 8) & 0xFF;
    if (j < outLength)
      data[j++] = (triple >> 0 * 8) & 0xFF;
  }
  return outLength;
}

inline OdString decodeBase64ToOdString(const char* data)
{
  OdAnsiString ansiStr(data);
  OdBinaryData out;
  decodeBase64(ansiStr, out);
  const OdUInt8* pBuff = out.asArrayPtr();
  return OdPlatformStreamer::getUnicodeStrFromBuffer(pBuff, out.size() / 2);
}

//////////////////////////////////////////////////////////////////////////




#endif
