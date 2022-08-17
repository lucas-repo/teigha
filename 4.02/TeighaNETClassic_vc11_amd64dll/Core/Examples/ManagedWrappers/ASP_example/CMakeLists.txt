if(MSVC_VERSION GREATER 1310)

oda_net_csharp_asp ( ASP_example Default.aspx )

set ( CMAKE_NET_REFERENCES System,System.Configuration,System.Core,System.Data,System.Data.DataSetExtensions,System.Drawing,System.EnterpriseServices,System.Web,System.Web.Extensions,System.Web.Mobile,System.Web,System.Web.Services,System.Xml,System.Xml.Linq )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})

oda_net_csharp_library(ASP_example)
add_custom_command ( TARGET ASP_example POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" ASP_example.dll.manifest )
set_target_properties(ASP_example PROPERTIES ASP_NET_PROJECT 1)
td_project_group(ASP_example "NET")

endif(MSVC_VERSION GREATER 1310)
