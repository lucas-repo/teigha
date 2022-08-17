#ifndef BRMESH2D_H_INCLUDED
#define BRMESH2D_H_INCLUDED

#include "Br/BrMesh.h"
#include "Br/BrMesh2dFilter.h"

/** \details
  This class defines the interface class for 2D meshes. 

  \remarks 
  This class is implemented only for Spatial modeler.

  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrMesh2d : public OdBrMesh
{
public:
  /** \details
    Default constructor.
  */
  OdBrMesh2d();

  /** \details
    Destructor.
  */
  ~OdBrMesh2d();

  // Assignment operator
  OdBrMesh2d& operator = (const OdBrMesh2d& src);
};

#endif
