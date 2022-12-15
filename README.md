# BadSyncProgram

A bad program to sync your files between two local folders

## Why?

There are already solutions like rsync, so why does this program exist?

I was trying to sync my files to an NFS share using rsync, but it always reported that it failed making mkstemp. I should probably just fix the share, but instead I wrote this program.

## Usage

To view the help page, run the program with the argument '-h'

## How to install

Currently there are no options for installing, but you can build it using the dotnet sdk

In the future there will be a prebuilt self-contained program uploaded as a release to GitHub