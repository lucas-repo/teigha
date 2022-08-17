if(MSVC_VERSION GREATER 1310)
oda_net_sources (XNet
   XNet.cs
   ../../GeneralAssemblyInfo.cs
   ../../OdaMgdMViewApp/HostAppServ.cs
   )

set ( CMAKE_NET_REFERENCES System,System.Data,System.Xml )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_library(XNet)

td_project_group(XNet "NET/ManagedCommands")
endif(MSVC_VERSION GREATER 1310)