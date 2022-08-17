if(MSVC_VERSION GREATER 1310)

oda_net_sources (OdaMgdMViewApp
    ../GeneralAssemblyInfo.cs
    AuxDir/AuxCls.cs
    AuxDir/DrawableImp.cs
    AuxDir/SelectionReactor.cs
    GripPoints/ExGripData.cs
    GripPoints/ExGripDrag.cs
    GripPoints/GripManager.cs
    GripPoints/Overruling/LineOverrule.cs
    HostAppServ.cs
    Program.cs
   )
oda_net_csharp_form (OdaMgdMViewApp ChildForm.cs)
oda_net_csharp_form (OdaMgdMViewApp  MainForm.cs)
set ( CMAKE_NET_REFERENCES System,System.Data,System.Drawing,System.Windows.Forms )
set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
oda_net_csharp_executable(OdaMgdMViewApp)
if( MSVC80 OR MSVC90 )
add_custom_command ( TARGET OdaMgdMViewApp POST_BUILD COMMAND copy "\"$(TargetDir)\\mgd.manifest\"" OdaMgdMViewApp.exe.manifest )
endif( MSVC80 OR MSVC90 )
set_target_properties(OdaMgdMViewApp PROPERTIES WIN32_EXECUTABLE 1)
td_project_group(OdaMgdMViewApp "NET")

endif(MSVC_VERSION GREATER 1310)
