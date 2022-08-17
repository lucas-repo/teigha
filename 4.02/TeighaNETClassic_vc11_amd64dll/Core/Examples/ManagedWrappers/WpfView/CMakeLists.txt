if(MSVC_VERSION GREATER 1400)
oda_net_sources (WpfView
    ../GeneralAssemblyInfo.cs
   )

oda_net_csharp_form(WpfView DrawControl.cs)
oda_net_csharp_app_page(WpfView MainWindow.xaml)
oda_net_csharp_app_def(WpfView App.xaml)
if( MSVC80 OR MSVC90 )
set ( CMAKE_NET_REFERENCES System,System.Data,System.Core,System.Windows.Forms,System.Xml.Linq,System.Data.DataSetExtensions,System.Xml,System.Drawing,WindowsBase,PresentationCore,PresentationFramework,UIAutomationProvider,WindowsFormsIntegration )
else( MSVC80 OR MSVC90 )
set ( CMAKE_NET_REFERENCES System,System.Data,System.Core,System.Windows.Forms,System.Xaml,System.Xml.Linq,System.Data.DataSetExtensions,System.Xml,System.Drawing,WindowsBase,PresentationCore,PresentationFramework,Microsoft.CSharp,UIAutomationProvider,WindowsFormsIntegration )
endif( MSVC80 OR MSVC90 )

set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_executable(WpfView)
set_target_properties(WpfView PROPERTIES WIN32_EXECUTABLE 1)

if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET WpfView POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" WpfView.exe.manifest )
endif( MSVC80 OR MSVC90 )

td_project_group(WpfView "NET")
endif(MSVC_VERSION GREATER 1400)