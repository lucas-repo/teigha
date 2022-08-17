#ifndef BRNODE_H_INCLUDED
#define BRNODE_H_INCLUDED

#include "Br/BrMeshEntity.h"
#include "Br/BrEnums.h"
/** \details
  This interface class represents nodes in a mesh.

  \remarks 
  This class is implemented only for Spatial modeler.

  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrNode : public OdBrMeshEntity
{
public:
  /** \details
    Default constructor.
  */
  OdBrNode();

  /** \details
    Destructor.
  */
  ~OdBrNode();

  /** \details
    Returns the base point of this node.
    
    \param point [out] Base point.
    
    \returns Returns odbrOK if successful, or an appropriate error code if not.   
  */
  OdBrErrorStatus getPoint(OdGePoint3d& point) const;
};

#endif

