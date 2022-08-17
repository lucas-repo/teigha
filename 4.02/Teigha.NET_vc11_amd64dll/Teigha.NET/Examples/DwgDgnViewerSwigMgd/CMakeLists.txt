oda_net_sources (DwgDgnViewerSwigMgd
    Orbit.cs
    CustomCommand.cs
    Program.cs
	../../GeneralAssemblyInfo.cs
   )
oda_net_csharp_form (DwgDgnViewerSwigMgd Form1.cs)
oda_net_csharp_form (DwgDgnViewerSwigMgd Form2.cs)
oda_net_csharp_form (DwgDgnViewerSwigMgd UserControl1.cs)
set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Windows.Forms )
set( CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /MANIFEST:NO" )
set( CMAKE_MODULE_LINKER_FLAGS "${CMAKE_MODULE_LINKER_FLAGS} /MANIFEST:NO" )
oda_net_csharp_executable(DwgDgnViewerSwigMgd)
set( CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /MANIFEST:NO" )
set( CMAKE_MODULE_LINKER_FLAGS "${CMAKE_MODULE_LINKER_FLAGS} /MANIFEST:NO" )
oda_net_references(DwgDgnViewerSwigMgd ${TD_SWIG_CORE_MGD_LIB} ${TG_SWIG_DB_MGD_LIB} ${TD_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET DwgDgnViewerSwigMgd POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" DwgDgnViewerSwigMgd.exe.manifest )


set_target_properties(DwgDgnViewerSwigMgd PROPERTIES WIN32_EXECUTABLE 1)
tn_project_group(DwgDgnViewerSwigMgd "Examples")
