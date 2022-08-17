if(MSVC_VERSION GREATER 1400)
oda_net_sources (${TD_NET_VBDEVEX}
    ArcEx/ArcEx.vb
    #BlockEx/BlockEx.vb
    CircleEx/CircleEx.vb
    DimEx/DimEx.vb
    EllipseEx/EllipseEx.vb
    FaceEx/FaceEx.vb
    HelixEx/HelixEx.vb
    MlineEx/MlineEx.vb
    MTextEx/MTextEx.vb
    DBPointEx/DBPointEx.vb
    DBTextEx/DBTextEx.vb
    LineEx/LineEx.vb
    Polyline3dEx/Polyline3dEx.vb
    RayEx/RayEx.vb
    ../../GeneralAssemblyInfo.vb
   )

set ( CMAKE_NET_REFERENCES System,System.Core,System.Data,System.Xml,System.Xml.Linq,System.Data.DataSetExtensions )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})

oda_net_vb_library(${TD_NET_VBDEVEX})
oda_net_disable_warnings(${TD_NET_VBDEVEX} 42016,41999,42017,42018,42019,42032,42036,42020,42021,42022)
oda_net_imports(${TD_NET_VBDEVEX} Microsoft.VisualBasic,System,System.Collections,System.Collections.Generic,System.Data,System.Diagnostics,System.Linq,System.Xml.Linq,Teigha,Teigha.DatabaseServices,Teigha.Geometry,Teigha.Runtime,Teigha.Colors)
oda_net_documentation_file(${TD_NET_VBDEVEX} VBNETDevGuideExamplesProject.xml)
td_project_group(${TD_NET_VBDEVEX} "NET/DevGuideExamples")
endif(MSVC_VERSION GREATER 1400)
