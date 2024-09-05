#!/bin/bash

echo 'Generating Bob (normal employee) token'

dotnet user-jwts create --project ../Issues.Api.csproj -n bob@company.com | grep -oP '(?<=Token: ).*' | clip

echo 'Token for Bob is in your Clipboard'