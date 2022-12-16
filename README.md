# BadSyncProgram

A bad program to sync your files between two local folders

## Why?

There are already solutions like rsync, so why does this program exist?

I was trying to sync my files to an NFS share using rsync, but it always reported that it failed making mkstemp. I should probably just fix the share, but instead I wrote this program.

## Usage

To view the help page, run the program with the argument '-h'

## How to install

You can either download and run the PKGBUILD, or download a prebuilt binary

### Prebuilt binaries

Prebuilt binaries can be found under [releases](https://github.com/PatzminiHD/BadSyncProgram/releases)

### PKGBUILD

If you are on an Arch System, you can download and run the [PKGBUILD](https://github.com/PatzminiHD/BadSyncProgram/blob/master/PKGBUILD).

Before you begin, make sure you have the `base-devel` package group installed.

To download and run the PKGBUILD, run 
```
wget https://raw.githubusercontent.com/PatzminiHD/BadSyncProgram/master/PKGBUILD
makepkg -si
```
After a successful install, you can delete all the directories and files that where created using:

```
rm -rf BadSyncProgram pkg src bsprog*.pkg.tar.zst PKGBUILD
```

(You need the -f to delete the .pack files in BadSyncProgram/objects/pack/)

To do all this at once run:

```
wget https://raw.githubusercontent.com/PatzminiHD/BadSyncProgram/master/PKGBUILD &&
makepkg -si &&
rm -rf BadSyncProgram pkg src bsprog*.pkg.tar.zst PKGBUILD
```