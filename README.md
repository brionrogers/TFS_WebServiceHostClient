# TFS_WebServiceHostClient
Client that allows users to pull information from a TFS server and displays information about the work item store.

#Introduction
This code was developed during my Spring 2015 Capstone project at the University of West Florida. Our project was to provide platform independent communication between a computer and a Microsoft Team Foundation Server. The client needed a way to track effort hours for TFS on a Mac that didn't require the user to interface with the Team Foundation Server web portal. The C# libraries available for TFS appear to be Windows dependent and thus were unsuitable for use. Using web services seemed like a suitable alternative. The project was intended to give computers using Mac OS X access to Microsoft TFS without having to use the Java SDK.

#Security
This software has had absolutely no security measures implemented in it and does require users to submit their TFS credentials as well as the base uri of the TFS server. 

#Overview
This code is both a web service host as well as a client. The client portion to be run on Mac OS X needs to be completed but as of 05/20/2015 I no longer have access to Xamarin Studio's (and by extension MonoDevelop) since the recent release no longer works on Mountain Lion. I intend to finish it once I get access to a (newer) computer that I can install Yosemite on (may buy a Mac Mini to finish development on)

The user provides their login credentials as well as the base URI of the TFS server to the web service host. The host processes the request and the parameters specified and attempts to read the TFS server indicated by the base uri and return information regarding the work item store of that TFS server.
