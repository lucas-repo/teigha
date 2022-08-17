#
#  NETLOADCommand library
#
set (tb_netloadcommand_source
  NETLOADCommand.cpp
  NetloadModule.cpp
  AssemblyInfo.cpp
  ../../GeneralAssemblyInfo.cpp
)
SOURCE_GROUP ("Source Files" FILES ${tb_netloadcommand_source})

set (tb_netloadcommand_include
  NETLOADCommand.h
  NetloadModule.h
  resource.h
)
SOURCE_GROUP ("Header Files" FILES ${tb_netloadcommand_include})



  td_sources(NETLOADCommand
			 ${tb_netloadcommand_source}
			 ${tb_netloadcommand_include})
			 
td_sources(NETLOADCommand NetloadModule.rc )

include_directories( ${PROJECT_BINARY_DIR}/Include)

add_definitions( "/wd4793" )

td_tx(NETLOADCommand ${TD_DB_LIB} ${TD_DBROOT_LIB} ${TD_ROOT_LIB} ${TD_ALLOC_LIB})

SET (CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})

set_target_properties (NETLOADCommand PROPERTIES COMPILE_FLAGS_REMOVE "/RTC1 /RTCsu /Zc:wchar_t- /Gm" )
set_target_properties(NETLOADCommand PROPERTIES COMPILE_FLAGS "/W3 /EHa")
set(CMAKE_MANIFEST 1) 
set(CMAKE_CLR_FLAG 1)

oda_set_cmake_net_framework(CMAKE_NET_FRAMEWORK)

if( MSVC80 OR MSVC90 )
set(CMAKE_NET_REFERENCES System.dll,System.Data.dll,System.XML.dll,${TD_MGD_LIB})
else( MSVC80 OR MSVC90 )
set(CMAKE_NET_REFERENCES System.dll,System.Data.dll,System.XML.dll)
endif( MSVC80 OR MSVC90 )

set ( CMAKE_PROJECT_REFERENCES ${TD_MGD_LIB})
td_project_group(NETLOADCommand "NET/ManagedCommands")
