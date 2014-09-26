# touch

A Windows console version of the Linux touch command written in C#.

**Syntax**

	touch [options] file(s)

The touch command updates the access and modification times of each file to the current system time.

If you specify files that does not already exist, touch creates empty files with those names (unless the *-c* option is specified; see below).

**Options**

	-a                    Change only the access time.
	-c, --no-create       Do not create any files.
	-d, -t, --date=STRING Parse the string and use it instead of the current time.
	-m                    Change only the modification time.
	-r, --reference=FILE  Use the file's time instead of current time.
	-b seconds            Add number of seconds to the referenced time.
	--help                Display this help and exit.
	--version             Output version information and exit.

**Example**

	touch file.txt

**Download**

<https://github.com/nagilum/touch/releases/tag/v1.0>
