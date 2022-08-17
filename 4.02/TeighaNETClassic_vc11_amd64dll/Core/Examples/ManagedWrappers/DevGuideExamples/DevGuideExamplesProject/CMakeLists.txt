if(MSVC_VERSION GREATER 1400)
oda_net_sources (DevGuideExamplesProject
    ../../GeneralAssemblyInfo.cs
   )

oda_net_csharp_app_page(DevGuideExamplesProject Window1.xaml)
oda_net_csharp_app_def(DevGuideExamplesProject App.xaml)
if(MSVC_VERSION GREATER 1500)
set ( CMAKE_NET_REFERENCES System,System.Core,System.Windows.Forms,System.Xml.Linq,System.Data.DataSetExtensions,System.Data,System.Xml,WindowsBase,System.Xaml,PresentationCore,PresentationFramework )
else(MSVC_VERSION GREATER 1500)
set ( CMAKE_NET_REFERENCES System,System.Core,System.Windows.Forms,System.Xml.Linq,System.Data.DataSetExtensions,System.Data,System.Xml,WindowsBase,PresentationCore,PresentationFramework )
endif(MSVC_VERSION GREATER 1500)

set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB},${TD_NET_CDEVEX},${TD_NET_VBDEVEX})
oda_net_csharp_executable(DevGuideExamplesProject)
if(MSVC90)
add_custom_command ( TARGET DevGuideExamplesProject POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" DevGuideExamplesProject.exe.manifest )
endif(MSVC90)
td_project_group(DevGuideExamplesProject "NET/DevGuideExamples")
endif(MSVC_VERSION GREATER 1400)
