if(MSVC_VERSION GREATER 1310)
oda_net_sources (Create3dSolids
   Create3dSolids.cs
   ../../GeneralAssemblyInfo.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_library(Create3dSolids)

td_project_group(Create3dSolids "NET/ManagedCommands")
endif(MSVC_VERSION GREATER 1310)
