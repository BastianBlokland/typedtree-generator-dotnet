.PHONY: build
default: build

# --------------------------------------------------------------------------------------------------
# MakeFile used as a convient way for executing development utlitities.
# --------------------------------------------------------------------------------------------------

clean:
	./ci/clean.sh

build:
	./ci/build.sh

test:
	./ci/test.sh
