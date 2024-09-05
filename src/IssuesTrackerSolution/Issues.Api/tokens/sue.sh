#!/bin/bash

echo 'Generating Sue (SoftwareCenter, SoftwareCenterAdmin) token'

dotnet user-jwts create --project ../Issues.Api.csproj -n sue@company.com --role SoftwareCenter --role SoftwareCenterAdmin | grep -oP '(?<=Token: ).*' | clip

echo 'Token for Sue is in your Clipboard'