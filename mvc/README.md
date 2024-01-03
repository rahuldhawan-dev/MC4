# MapCall MVC

Main MapCall site, implemented using ASP.NET MVC 5.

## Working with this project

This project utilizes build automation scripts written using [Cake](https://cakebuild.net/).  For a full list of available cake targets, run `build --ShowDescription` from the command line.

There are some idiosyncracies in how our scripts will process flags and their arguments.  Essentially there are two different syntaxes; `build -flag Argument`, and `build --flag="Argument"`.  If you are not using the equals sign to set the argument for a flag, you must use a single hyphen to precede the argument.  If you are using equals sign, you must use two hyphens and the argument value should be wrapped in quotes.  The only reason to use two hyphens, equals sign, and quotes is if your argument contains non-word characters such as spaces or punctuation (e.x. the periods separating segments of a fully qualified domain name).

If you need to run a target without running any of its dependencies (e.x. you want to `Migrate` without having to go through a `Build` first), add the `--Exclusive` flag.

## Cake Targets
Moved [here](../doc/cake.md#cake-targets).
