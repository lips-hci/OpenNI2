cmake_minimum_required (VERSION 3.1)

project(OpenNI2_CSharp_wrapper LANGUAGES CSharp)

include(${CMAKE_SOURCE_DIR}/cmake/SetupBuildEnvironment.cmake)

#CSharp wrapper
add_subdirectory(NiWrapper.Net)

#CSharp samples
# Basic - console mode
add_subdirectory(NiHello.Net)
add_subdirectory(SimpleRead.Net)

# Basic - window mode
add_subdirectory(SimpleViewer.Net)

# Advanced - WPF application with C#
add_subdirectory(Ni2WPFViewer)

# Advanced - From NiWrapper.Net/Samples
add_subdirectory(NiViewer.Net)
