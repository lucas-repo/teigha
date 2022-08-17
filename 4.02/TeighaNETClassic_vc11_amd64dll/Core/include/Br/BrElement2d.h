#ifndef BRELEMENT2D_H_INCLUDED
#define BRELEMENT2D_H_INCLUDED

#include "Br/BrElement.h"
#include "Ge/GeVector3d.h"
/** \details
  This interface class represents linear, two-dimensional first-order elements 
  in a mesh.

  \remarks 
  This class is implemented only for Spatial modeler. 
    
  \sa
  TD_Br
    
  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrElement2d : public OdBrElement
{
public:  
  /** \details
    Default constructor.
  */
  OdBrElement2d();
  
  /** \details
    Destructor.
  */
  ~OdBrElement2d();

  /** \details
    Returns the normal vector of normalized model space. The vector is computed 
    on the plane that is defined by the nodes of element traversed in a clockwise 
    direction.
    
    \param normal [out] Normal vector.

    \remarks
    In case of an error, the value of the normal vector passed as an 
    argument is unchanged.
  */
  OdBrErrorStatus getNormal (OdGeVector3d& normal) const;
};


#endif

