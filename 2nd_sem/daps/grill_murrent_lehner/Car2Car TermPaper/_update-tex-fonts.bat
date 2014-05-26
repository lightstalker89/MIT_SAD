:: Windows shell script to enforce update of TeX font maps (needed under MikTeX 2.9)

@echo off
echo updating TeX font maps (initexmf) ...
initexmf --mkmaps
echo done.