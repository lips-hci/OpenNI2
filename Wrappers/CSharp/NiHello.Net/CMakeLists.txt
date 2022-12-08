cmake_minimum_required (VERSION 3.1)

include(CSharpUtilities)

project(NiHello_dotnet LANGUAGES CSharp)

set(CMAKE_CSharp_FLAGS "/platform:x64")

set(EXE_NAME NiHello.Net)

add_executable(${EXE_NAME} Program.cs)

target_link_libraries(${EXE_NAME} PRIVATE NiWrapper.Net NiWrapper)

set_property(TARGET ${EXE_NAME} PROPERTY FOLDER "CSharp_samples")

## Installation
install(TARGETS ${EXE_NAME} NiWrapper.Net NiWrapper DESTINATION .)
