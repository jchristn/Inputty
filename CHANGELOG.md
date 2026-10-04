# Change Log

## Current Version

v1.0.14

- **Breaking for strong-name consumers:** the `GetSomeInput` assembly is no longer strong-name signed (previous PublicKeyToken `f28223f764d60818`)
- Targets: `netstandard2.0`, `netstandard2.1`, `net462`, `net48`, `net8.0`, `net10.0`
- Symbol package (`.snupkg`) published alongside the NuGet package
- Test dependency updates: Touchstone 0.1.12 -> 0.2.0, Microsoft.NET.Test.Sdk 18.9.0 -> 18.10.1, NUnit 4.6.1 -> 5.0.0, NUnit3TestAdapter 6.2.0 -> 6.3.0
- Tests run under Touchstone (`Test.Automated`), xUnit (`Test.Xunit`), and NUnit (`Test.Nunit`) on net8.0 and net10.0

## Previous Versions

v1.0.13

- `netstandard2.0` targeting

v1.0.12

- Nullable `DateTime` support

v1.0.11

- `Guid` support

v1.0.10

- More timestamp formats

v1.0.9

- `DateTime` with default value

v1.0.0

- Initial release
