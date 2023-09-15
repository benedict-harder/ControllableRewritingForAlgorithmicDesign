# .NET-solution coupled to Rhino/Grasshopper for algorithmic modular architecture

# How to install:
- Download Rhino7 
- Clone this repository project
- Install most recent, stable Rhino3dm NuGet-Package (not RhinoCommon nor the outdated RhinoFileIO.Desktop libraries) for the two solution projects where Rhino.Geometry is used
- Reference your local GH/ GH_io library in the GrammarMetaModel-library. If the package manager installs RhinoCommon, uninstall RhinoCommon again.
- Clone Rhino compute and run a local test server (under port 8081). Run Rhino 7 at least once to validate the licence before doing Resthopper-requests via .NET.

