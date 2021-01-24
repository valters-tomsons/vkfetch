#!/bin/bash

if [ "$EUID" -ne 0 ]
  then echo -e "\e[33m Root required for installation!"
  exit
fi

cd ..
dotnet publish -c Release -o release

FILE=release/vkfetch
if ! test -f "$FILE"; then
    echo -e "> Release binary not found"
    echo -e "\e[33m Make sure project builds correctly!"
    exit
fi

cd release || exit

TARGET=/usr/bin/vkfetch
cp vkfetch "$TARGET" || { echo -e "\e[31mFailed to copy binary"; exit 1; }

echo "Installed vkfetch to $TARGET"