oda_net_sources (WpfSample
  TeighaD3DImage.cs
  oda.bmp
  ../../GeneralAssemblyInfo.cs
   )

oda_net_csharp_app_page (WpfSample App.xaml)
set_source_files_properties (App.xaml PROPERTIES NET_APPDEF 1)
oda_net_csharp_app_page (WpfSample Window1.xaml)
oda_net_csharp_app_page (WpfSample CustomImage1.xaml)
set ( CMAKE_NET_REFERENCES System,System.Core,System.Data,System.Xml,System.Xaml,WindowsBase,PresentationCore,PresentationFramework)
set( CMAKE_SHARED_LINKER_FLAGS "${CMAKE_SHARED_LINKER_FLAGS} /MANIFEST:NO" )
set( CMAKE_MODULE_LINKER_FLAGS "${CMAKE_MODULE_LINKER_FLAGS} /MANIFEST:NO" )
oda_net_csharp_executable(WpfSample)
oda_net_references ( WpfSample ${TD_SWIG_CORE_MGD_LIB} ${TD_SWIG_DB_MGD_LIB} ${TG_SWIG_DB_MGD_LIB})
add_custom_command ( TARGET WpfSample POST_BUILD COMMAND copy "\"$(TargetDir)\\CRT.manifest\"" WpfSample.exe.manifest )

set_target_properties(WpfSample PROPERTIES WIN32_EXECUTABLE 1)
tn_project_group(WpfSample "Examples")
