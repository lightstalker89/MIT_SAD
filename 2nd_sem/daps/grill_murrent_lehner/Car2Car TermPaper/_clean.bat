:: Windows shell script to clean up temporary Latex/BibTeX files

@echo off
echo Cleaning up LaTeX files ...
del *.aux
del *.bbl
del *.blg
del *.dvi 
del *.log
del *.toc
del *.out
del *.pdfsync
del *.blg
del *.ps
del *.tps
del *.run.xml
del *.synctex
del *-blx.bib
echo done.