if (TD_NET_ALL_EXAMPLES)
add_subdirectory(OdReadExMgd)
add_subdirectory(OdViewExMgd)
add_subdirectory(OdWriteExMgd)
add_subdirectory(OdaMgdMViewApp)
add_subdirectory(VBViewEx)
add_subdirectory(WpfView)
add_subdirectory(DevGuideExamples/DevGuideExamplesProject)
add_subdirectory(DevGuideExamples/C\#)
add_subdirectory(DevGuideExamples/VB.NET)
endif(TD_NET_ALL_EXAMPLES)

if(TD_NET_ALL_EXAMPLES OR TD_NET_SDK_EXAMPLES)
add_subdirectory(ManagedCommands/NETLOADCommand)
add_subdirectory(ManagedCommands/ManagedTestCommand)
add_subdirectory(ManagedCommands/XNet)
add_subdirectory(ManagedCommands/Create3dSolids)
endif(TD_NET_ALL_EXAMPLES OR TD_NET_SDK_EXAMPLES)
