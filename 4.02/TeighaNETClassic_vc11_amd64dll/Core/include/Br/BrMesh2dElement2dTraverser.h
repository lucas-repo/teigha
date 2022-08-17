#ifndef BRMESH2DELEMENT2DTRAVERSER_H_INCLUDED
#define BRMESH2DELEMENT2DTRAVERSER_H_INCLUDED

#include "Br/BrTraverser.h"
#include "Br/BrEnums.h"
#include "Br/BrElement2d.h"
#include "Br/BrMesh2d.h"

/** \details
  This interface class represents mesh element traversers.

  \remarks 
  This class is implemented only for Spatial modeler.

  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrMesh2dElement2dTraverser : public OdBrTraverser
{
public:
  /** \details
    Default constructor.
  */
  OdBrMesh2dElement2dTraverser();

  /** \details
    Destructor.
  */
  ~OdBrMesh2dElement2dTraverser();

  /** \details
    Sets this traverser to a specific mesh element list and start position.
    
    \param element2d [in] Reference to a 2D element object.
    
    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus   setMeshAndElement(const OdBrElement2d& element2d);
  
  /** \details
    Sets this traverser to a specific mesh element list, 
    starting with the first list's element.
    
    \param mesh2d [in] Reference to a mesh object.
    
    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus   setMesh         (const OdBrMesh2d& mesh2d);
  
  /** \details
    Returns the owner of the mesh element list.

    \param mesh2d [in/out] Reference to a mesh object.

    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus   getMesh         (OdBrMesh2d& mesh2d) const;

  /** \details
    Sets the starting position of this traverser to a specific element in the mesh element list.

    \param element2d [in] Reference to an element object.

    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus   setElement    (const OdBrElement2d& element2d);

  /** \details
    Returns the current element in the mesh element list.

    \param element2d [in/out] Reference to an element object.

    \returns Returns odbrOK if successful, or an appropriate error code if not.  
  */
  OdBrErrorStatus   getElement    (OdBrElement2d& element2d) const;
};


#endif

