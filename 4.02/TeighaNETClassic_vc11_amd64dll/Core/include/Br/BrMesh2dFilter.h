#ifndef BRMESH2DFILTER_H_INCLUDED
#define BRMESH2DFILTER_H_INCLUDED

#include "Br/BrEntity.h"
#include "Br/BrMesh2dControl.h"
#include "Br/BrExport.h"

/** \details
  This interface class represents 2D mesh filters.

  \remarks 
  This class is implemented only for Spatial modeler.

  \sa
  TD_Br

  <group OdBr_Classes>
*/
class ODBR_TOOLKIT_EXPORT OdBrMesh2dFilter
{
  const OdBrEntity* m_BrEntity;
  OdBrMesh2dControl m_meshControl;
public:

  /** \details
    Default constructor.
  */
  OdBrMesh2dFilter();

  /** \details
    Destructor.
  */
  ~OdBrMesh2dFilter();

  /** \details
    This function associates a 2D mesh control with a topology object.

    \param brEntity [in] Topology object.
    \param meshControl [in] 2D mesh control object.

    \returns Returns odbrOK if successful, or an appropriate error code if not.
  */
  OdBrErrorStatus set(const OdBrEntity& brEntity, const OdBrMesh2dControl &meshControl);

  /** \details
    Returns the topology object associated with the 2D mesh control.
  */
  const OdBrEntity* getBrEntity() const;
  
  /** \details
    Returns the 2D mesh control, used as a filter in 2D mesh generation.
  */
  const OdBrMesh2dControl& getMeshControl() const;

private:
  OdBrMesh2dFilter(const OdBrMesh2dFilter& src);
  OdBrMesh2dFilter& operator = (const OdBrMesh2dFilter& src);
};

inline
OdBrMesh2dFilter::OdBrMesh2dFilter() : m_BrEntity(0) {}

inline
OdBrMesh2dFilter::~OdBrMesh2dFilter() {}

inline
OdBrErrorStatus OdBrMesh2dFilter::set(const OdBrEntity& brEntity, const OdBrMesh2dControl &meshControl)
{
  m_BrEntity = &brEntity;
  m_meshControl = meshControl;
  return odbrOK;
}

inline
const OdBrEntity* OdBrMesh2dFilter::getBrEntity() const
{
  return m_BrEntity;
}

inline
const OdBrMesh2dControl& OdBrMesh2dFilter::getMeshControl() const
{
  return m_meshControl;
}


#endif
