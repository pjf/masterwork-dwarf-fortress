#!/bin/bash

function die {
  echo $*
  exit 1
}

function update_savegame {
  if [ -d "$1/raw" ] ; then
    rm -rf "$1/raw" || die "Error deleting old '$1/raw'."
  fi
  
  cp -a "raw" "$1/raw" || die "Error copying 'raw' to '$1/raw'."
}

function update_savegames {
  if [ ! -d "raw" ] ; then
    die "No raw directory to copy from!"
  fi
  
  for bad_file in "raw/objects/inorganic_stone_layer - Copy.txt" ; do
    if [ -e "$bad_file" ] ; then
      echo "Deleting '$bad_file'"
      rm -f "$bad_file" || die "Error deleting file '$bad_file'."
    fi
  done
  
  if [ ! -d "data/save" ] ; then
    die "No 'data/save' directory to copy to!"
  fi
  
  for file in data/save/* ; do
    if [ -d "$file" -a "`basename ${file}`" != "current" ]; then
      echo -n "Updating '$file'... "
      update_savegame "$file"
      echo "done."
    fi
  done
  
  echo "---------------------"
  echo "- Update Completed. -"
  echo "---------------------"
}

echo "-------------------------------------------------------"
echo "- Welcome to the Dwarf Fortress Savegame RAW Updater. -"
echo "-------------------------------------------------------"
select choice in "Update savegames" "Quit"; do
  case "$choice" in
    "Update savegames")
      update_savegames
      exit 0
    ;;
    "Quit")
      echo "Aye, no point fix'n what ain't broken."
      exit 1
    ;;
    *)
      if [ "$warned" == "y" ] ; then
        echo -e "Rid thee from my sight, before I make \033[1;33m+Elf Leather Headscarf+\033[m of ye."
        exit 1
      else
        echo "I've half a mind to report ye as an elven spy!"
        warned="y"
      fi
    ;;
  esac
done