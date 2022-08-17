#ifndef BRMESHENTITY_H_INCLUDED
#define BRMESHENTITY_H_INCLUDED

#include "Br/BrEnums.h"
#include "Br/BrEntity.h"
#include "Br/BrExport.h"

/** \details
  This class defines the interface base class for mesh objects and subobjects.

  \remarks 
  This class is implemented only for Spatial modeler.

  \sa
  TD_Br
  
  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrMeshEntity
{
  friend class OdBrEntityInternals;
public:

  /** \details
    Virtual destructor.
  */
  virtual ~OdBrMeshEntity();

  /** \details
    Checks if content of the two mesh objects is equal.
    
    \param other [in] Another mesh object.
    
    \returns Returns true if two mesh objects are equal, 
    or returns false in the other case.  
  */
  bool      isEqualTo       (const OdBrMeshEntity* other) const;

  /** \details
    Returns true if and only if this mesh object is uninitialized.
  */
  bool      isNull          () const;

  /** \details
    Returns the topology object associated with this mesh object.

    \param entity [out] Dimensionally lowest topology object.

    \returns Returns odbrOK if successful, or an appropriate error code if not.
  */
  OdBrErrorStatus   getEntityAssociated(OdBrEntity*& entity) const;

  // Validation
  /** \details
    Sets the validation level. 

    \param level [in] Validation level.

    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus setValidationLevel(const BrValidationLevel& level = *(BrValidationLevel*)NULL);
  
  /** \details
    Returns the validation level used by the mesh object.

    \param level [out] Validation level.

    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus getValidationLevel(BrValidationLevel& level) const;
  
  /** \details
    Checks whether the object associated with this mesh has changed since the mesh 
    object was last set.

    \returns Returns true if the associated object has changed or if it is impossible to 
    determine whether it has changed. Returns false if the associated object has 
    not changed or if it is a non-database resident.
  */
  bool    brepChanged    () const;

protected:
  /** \details
    Default constructor.
  */
  OdBrMeshEntity();

  /** \details
    Copy constructor.
  */
  OdBrMeshEntity(const OdBrMeshEntity&);

  /** \details
    Assignment operator.
  */
  OdBrMeshEntity&     operator =      (const OdBrMeshEntity&);

  OdRxObjectPtr m_pImp;
  bool m_bIsValidate; //The enum has only 2 values.
};


#endif

