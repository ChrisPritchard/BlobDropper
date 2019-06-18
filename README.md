# BlobDropper

Downloads all publically accessible files from a blob container on Azure. Quickly knocked up as a personal backup solution.

Usage: 

  `BlobDropper.exe [blob connection string] [blob container name] [local directory]`

OR (if running off source):

  `dotnet run [blob connection string] [blob container name] [local directory]`

The connection string is found under access keys in the Azure management dashboard.

Application built with dotnet core, should work on all platforms core works on.
