# rhubarb-geek-nz.RuntimeIdentifier
RuntimeIdentifier tools for PowerShell.

Get, Select and Test RuntimeIdentifiers.

These can either come from the executing assembly's *program*`.deps.json` or the shared runtime's `Microsoft.NETCore.App.deps.json`.

## Example on Windows

```
PS> Get-RuntimeIdentifier
win-x64
win
any
base
PS> Select-RuntimeIdentifier win, unix
win
```

## Example on Linux

```
PS> Get-RuntimeIdentifier
linux-arm64
linux
unix-arm64
unix
any
base
PS> Test-RuntimeIdentifier unix
True
```
