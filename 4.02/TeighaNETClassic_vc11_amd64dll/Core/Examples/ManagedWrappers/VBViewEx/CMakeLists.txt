if(MSVC_VERSION GREATER 1310)

oda_net_sources (VBViewEx
	 ../GeneralAssemblyInfo.vb
   )
oda_net_vb_form(VBViewEx Form1.vb)

   
set ( CMAKE_NET_REFERENCES System,System.Data,System.Deployment,System.Xml,System.Drawing,System.Windows.Forms )

set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_vb_executable(VBViewEx VBViewEx.My.MyApplication)
oda_net_documentation_file(VBViewEx VBViewEx.xml)
oda_net_disable_warnings(VBViewEx 42016,41999,42017,42018,42019,42032,42036,42020,42021,42022)
oda_net_imports(VBViewEx     Microsoft.VisualBasic,System,System.Collections,System.Collections.Generic,System.Data,System.Drawing,System.Diagnostics,System.Windows.Forms,Teigha,Teigha.DatabaseServices,Teigha.Export_Import,Teigha.Geometry,Teigha.GraphicsInterface,Teigha.GraphicsSystem)

if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET VBViewEx POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" VBViewEx.exe.manifest )
endif( MSVC80 OR MSVC90 )

set_target_properties(VBViewEx PROPERTIES WIN32_EXECUTABLE 1)
td_project_group(VBViewEx "NET")
endif(MSVC_VERSION GREATER 1310)