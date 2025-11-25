param(
    [string]$Target = "help",
    [string]$SphinxOpts = "",
    [string]$O = ""
)

$SPHINXBUILD = "uv run sphinx-build"
$SOURCEDIR = "source"
$BUILDDIR = "build"

$arguments = @(
	"-M", $Target,
	"`"$SOURCEDIR`"", "`"$BUILDDIR`"",
	"$SphinxOpts",
	"$O"
)

Invoke-Expression "$SPHINXBUILD $arguments"
