# ApplicationInsights Scripts

This is a collection of scripts to support our build and release infrastructure.


### .NET Releases

The Release Pipeline is a generic process that will:

 - Publish to MyGet
 - Create a request template for Test team
 - Publish to GitHub
 - Publish to NuGet

Our release pipeline is driven by variables parsed from the ReleaseMetaData.

### ReleaseMetaData

ReleaseMetaData is an XML file of variables that will drive the release pipeline.

 - `GenerateReleaseMetadata.ps1` is a generic script that will create all metadata for dotnet solutions. This will parse every `nupkg` (`nuspec` and `dll`) and the `changelog.md` to create a summary. This file should be added to your repo's bulid. The output file "releaseMetaData.xml" should be published as an artifact.

 - `metadata_to_cti.ps1` will parse the metadata and create a draft email request for SDK testing.

 - `metadata_to_releaseNotes.ps1` will parse the metadata and post release notes to GitHub.