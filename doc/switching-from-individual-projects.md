# Switching over from individual projects

MapCall used to be a bunch of individual projects prior to becoming a monorepo.
These instructions are left over from that time, and preserved just in case.

## Initializing Monorepo
1. Follow the
   [Working with this project](../README.md#working-with-this-project) steps
2. Pull to get the latest `commit-hooks` project and initialize it with
   `c:\solutions\commit-hooks>rake init` (make sure the output contains
    `mapcall-monorepo`)
3. For any existing feature branches which need to be brought forward, use the
   steps below in [Bringing existing tickets forward](#bringing-existing-tickets-forward)

## Bringing existing tickets forward
Substitute `1234` with your ticket number and `mmsinc` with whatever project
(mapcall_mvc, mapcall-scheduler, contractors, etc.) contains your changes.  You
will only need to do the `git remote add <project> ..\<project>` step once per
project.

```
# make sure you're up to date
c:\solutions\mmsinc>git pull
# start from your feature/bug branch
c:\solutions\mmsinc>git checkout MC-1234
# first we merge in the branch that adjusts the paths
c:\solutions\mmsinc>git merge origin/MC-2939
# at this point there may be some conflicts.  you will need to resolve them, and
# and commit (no need to push)
c:\solutions\mmsinc>cd ..\mapcall-monorepo
c:\solutions\mapcall-monorepo>bugbranch mc 1234
# create a remote to the old project so we can pull that branch over
c:\solutions\mapcall-monorepo>git remote add mmsinc ..\mmsinc
c:\solutions\mapcall-monorepo>git fetch mmsinc
# merge in the branch
c:\solutions\mapcall-monorepo>git merge mmsinc/MC-1234 --allow-unrelated-histories
# note: if you end up with a merge conflict that involves entire files, it has
# to do with line endings not matching.  run `git merge --abort` and then redo the
# above command with `-Xignore-space-change` added to the end
c:\solutions\mapcall-monorepo>git push
```
