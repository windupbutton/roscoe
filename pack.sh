#!/bin/sh

(cd src/WindupButton.Roscoe ; dotnet pack --configuration Release)
(cd src/WindupButton.Roscoe.Postgres ; dotnet pack --configuration Release)
(cd src/WindupButton.Roscoe.SqlServer ; dotnet pack --configuration Release)
