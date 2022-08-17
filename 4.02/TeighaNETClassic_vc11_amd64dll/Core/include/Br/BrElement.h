#ifndef BRELEMENT_H_INCLUDED
#define BRELEMENT_H_INCLUDED

#include "Br/BrMeshEntity.h"
/** \details
  This class defines the interface base-class for mesh elements. Each element is
  represented by an ordered, minimal closed subset of connected nodes in a mesh.
 
  \remarks 
  This class is implemented only for Spatial modeler. 
  
  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrElement : public OdBrMeshEntity
{
public:
  /** \details
    Virtual destructor.
  */
  virtual ~OdBrElement();

protected:
  /** \details
    Default constructor.
  */
  OdBrElement();
  
  /** \details
    Copy constructor.
  */
  OdBrElement(const OdBrElement& src);
  
  /** \details
    Assignment operator.
  */
  OdBrElement&    operator =      (const OdBrElement& src);
};


#endif

