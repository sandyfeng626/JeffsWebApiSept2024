#!/bin/bash

echo 'Generating Brad (SoftwareCenter) token'

dotnet user-jwts create --project ../Issues.Api.csproj -n brad@company.com --role SoftwareCenter  | grep -oP '(?<=Token: ).*' | clip

echo 'Token for Brad is in your Clipboard'