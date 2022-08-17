if(MSVC_VERSION GREATER 1400)
oda_net_sources (${TD_NET_CDEVEX}
    ArcEx/ArcEx.cs
	BlockEx/BlockEx.cs
    CircleEx/CircleEx.cs
	DimEx/DimEx.cs
    EllipseEx/EllipseEx.cs
    FaceEx/FaceEx.cs
    HelixEx/HelixEx.cs
    MlineEx/MlineEx.cs
    MTextEx/MTexExt.cs
    DBPointEx/DBPointEx.cs
    DBTextEx/DBTextEx.cs
    LineEx/LineEx.cs
    Polyline3dEx/Polyline3dEx.cs
    RayEx/RayEx.cs
    ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Core,System.Data,System.Xml,System.Xml.Linq,System.Data.DataSetExtensions )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_library(${TD_NET_CDEVEX})

td_project_group(${TD_NET_CDEVEX} "NET/DevGuideExamples")
endif(MSVC_VERSION GREATER 1400)
