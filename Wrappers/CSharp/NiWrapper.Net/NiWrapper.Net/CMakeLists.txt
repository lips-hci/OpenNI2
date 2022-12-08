cmake_minimum_required (VERSION 3.1)
include(CSharpUtilities)

project(NIWRAPPER_dotnet LANGUAGES CSharp)

set(CMAKE_CSharp_FLAGS "/platform:x64")

set(NIWRAPPER_dotnet_TARGET NiWrapper.Net)

set(NIWRAPPER_dotnet_SRC
    Array.cs
    CameraSettings.cs
    Common.cs
    CoordinateConverter.cs
    Device.cs
    DeviceInfo.cs
    OniVersion.cs
    OpenNI.cs
    OpenNIBase.cs
    OpenNIException.cs
    PlaybackControl.cs
    Recorder.cs
    SensorInfo.cs
    Size.cs
    VideoFrameRef.cs
    VideoMode.cs
    VideoStream.cs
)

add_library(${NIWRAPPER_dotnet_TARGET} SHARED ${NIWRAPPER_dotnet_SRC})

target_link_libraries(${NIWRAPPER_dotnet_TARGET} NiWrapper OpenNI2)

set_target_properties(${NIWRAPPER_dotnet_TARGET} PROPERTIES
    DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
)

LIST(APPEND VS_DOTNET_REFERENCES "Microsoft.CSharp")
LIST(APPEND VS_DOTNET_REFERENCES "System.Core")
LIST(APPEND VS_DOTNET_REFERENCES "System.Data")
LIST(APPEND VS_DOTNET_REFERENCES "System.Xml")
LIST(APPEND VS_DOTNET_REFERENCES "System.Drawing")

set_target_properties(${NIWRAPPER_dotnet_TARGET} PROPERTIES
    VS_DOTNET_REFERENCES "${VS_DOTNET_REFERENCES}"
)