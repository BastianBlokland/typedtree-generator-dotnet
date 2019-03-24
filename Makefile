.PHONY: build
default: build

# --------------------------------------------------------------------------------------------------
# MakeFile used as a convient way for executing development utlitities.
# --------------------------------------------------------------------------------------------------

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
