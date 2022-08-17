#ifndef BRMESH_H_INCLUDED
#define BRMESH_H_INCLUDED

#include "Br/BrMeshEntity.h"

/** \details
  This class defines the interface base class for meshes.

  \remarks 
  This class is implemented only for Spatial modeler.
  
  \sa
  TD_Br
  
  <group OdBr_Classes>
  */
class ODBR_TOOLKIT_EXPORT OdBrMesh : public OdBrMeshEntity
{
public:
  /** \details
    Virtual destructor.
  */
  virtual ~OdBrMesh();

protected:
  /** \details
    Default constructor.
  */
  OdBrMesh();

  OdBrMesh& operator = (const OdBrMesh&);
};


#endif

