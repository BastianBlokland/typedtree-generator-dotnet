.PHONY: build
default: build

# --------------------------------------------------------------------------------------------------
# MakeFile used as a convient way for executing development utlitities.
# --------------------------------------------------------------------------------------------------

run:
	dotnet run --project src/TypedTree.Generator.Cli/TypedTree.Generator.Cli.csproj -- $(ARGS)

clean:
	./ci/clean.sh

build: clean
	./ci/build.sh

pack: clean
	./ci/pack.sh

publish: pack
	./ci/publish.sh

test:
	./ci/test.sh
