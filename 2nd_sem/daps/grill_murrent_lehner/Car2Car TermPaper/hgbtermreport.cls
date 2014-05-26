%% hgbtermreport.cls
%% Autor: Wilhelm Burger (wilhelm.burger@fh-hagenberg.at)
%% Diese Datei ist Teil des hagenberg-Package zur Erstellung von 
%% Bachelor- und Diplomarbeiten.

\NeedsTeXFormat{LaTeX2e}
\newcommand{\hgbtermreportDate}{2012/05/10} %Versionsdatum
\ProvidesClass{hgbtermreport}[\hgbtermreportDate\space LaTeX-Vorlage fuer Semester- und Projektberichte]

\RequirePackage{ifthen}
\newcommand{\hgb@MainLanguage}{german}	% also referenced in hgb.sty

%% begin class options ---------------------------------------------
\DeclareOption{english}{\renewcommand{\hgb@MainLanguage}{english}}
\DeclareOption{german}{\renewcommand{\hgb@MainLanguage}{german}}
\DeclareOption*{\ClassWarning{hgbtermreport}{Unknown document option "\CurrentOption"}}
\ProcessOptions
%% end class options ---------------------------------------------
 
\LoadClass[a4paper,11pt,oneside]{report}
 
\RequirePackage{hgb}	% load associated style file hgb.sty


%------------ Automatischer Eintrag der PDF Metadaten ------------

%\renewcommand{\maketitle}{	%replace standard LaTeX-command
%\hypersetup{
%		pdftitle={\@title},
%		pdfauthor={\@author},
%		pdfsubject={\@studiengang, \@studienort},
%		pdfcreator={LaTeX using class hgbtermreport},
%		pdfproducer={},
%		pdfkeywords={}
%}%
%\@maketitlepages
%}


%% ---------------------------------------------------------------
%% ---------------------------------------------------------------
%% ---------------------------------------------------------------
%%%  VVV move stuff below into hgb.sty ???
%% ---------------------------------------------------------------
%% ---------------------------------------------------------------
%% ---------------------------------------------------------------



%% --------------- Headings --------------------------------------

\RequirePackage{fancyhdr}
\pagestyle{fancy}

\@ifundefined{chapter}%
   {}%
	 {\renewcommand{\chaptermark}[1]{%
			\ifthenelse{\value{chapter} < 1}%
				{\markboth{#1}{}}%
				{\markboth{\thechapter.\ #1}{}}
		}}

\addtolength{\headheight}{\baselineskip}
\renewcommand{\headrulewidth}{0pt} %% thin line under header
%\renewcommand{\headrulewidth}{0.4pt} %% no line under header
\renewcommand{\footrulewidth}{0pt}
\fancyhf{}                            %% Clear all fields.
%\fancyhead[LE,RO]{\sffamily\thepage} %{\bf \rightmark}
%\fancyhead[LO,RE]{\sffamily\nouppercase{\leftmark}}  %{\slshape \leftmark}
\fancyhead[R]{\sffamily\thepage} %{\bf \rightmark}
\fancyhead[L]{\sffamily\nouppercase{\leftmark}}  %{\slshape \leftmark}

\fancypagestyle{plain}{%
	\fancyhf{} % clear all header and footer fields
	\fancyfoot[C]{\sffamily\thepage} % except the center
	\renewcommand{\headrulewidth}{0pt}
	\renewcommand{\footrulewidth}{0pt}
}

%% ------------ biblatex  -----------------------------------------

\usepackage{csquotes}	% recommended for biblatex
\usepackage[style=numeric-comp]{biblatex}[2011/05/12]
% do not use these:
%\usepackage[style=alphabetic]{biblatex}[2011/05/12]
%\usepackage[style=authoryear]{biblatex}[2011/05/12]
%\usepackage[style=ieee]{biblatex}[2011/05/12]
%\usepackage[style=nature]{biblatex}[2011/05/12]

\ExecuteBibliographyOptions{sortcites=true,defernumbers=true,isbn=false,doi=false}

%% 3 categories of references:
\newcommand{\@bibtitleLiteratur}{Literatur}
\newcommand{\@bibtitleAvmedia}{Filme und audiovisuelle Medien}
\newcommand{\@bibtitleOnline}{Online-Quellen}	

\defbibfilter{literature}{
	not ( type=movie or type=video or type=music or type=audio or 
				type=online or type=electronic or type=www )}

\defbibfilter{avmedia}{%
	( type=movie or type=video or type=music or type=audio )}

\defbibfilter{online}{%
	( type=online or type=electronic or type=www )}


\defbibheading{noheader}[]{}	% 'none' should be defined but isn't

% German format in biblatex is flawed: it adds wrong spaces and prohibits
% line breaking within date strings. Corrected by Wilbur as follows.
% Taken from german.lbx: replaced \thinspace by breaking zero space \hskip0pt 
\DefineBibliographyExtras{ngerman}{% 
  \def\mkbibdatelong#1#2#3{%
    \iffieldundef{#3}
      {\hskip0pt}
      {\mkbibordinal{\thefield{#3}}%
       \iffieldundef{#2}{}{\space}}%	%was: \nobreakspace
    \iffieldundef{#2}
      {\hskip0pt}
      {\mkbibmonth{\thefield{#2}}%
       \iffieldundef{#1}{\hskip0pt}{\space}}%
    \stripzeros{\thefield{#1}}}%
%
  \def\mkbibdateshort#1#2#3{%
    \iffieldundef{#3}
      {\hskip0pt}
      {\mkdatezeros{\thefield{#3}}\adddot
       \iffieldundef{#2}{}{\hskip0pt}}%
    \iffieldundef{#2}
      {\hskip0pt}
      {\mkdatezeros{\thefield{#2}}%
       \iffieldundef{#1}
	 {\hskip0pt}
	 {\iffieldundef{#3}{/}{\adddot\hskip0pt}}}%
    \mkdatezeros{\thefield{#1}}}%
}

%This is only a wrapper to \addbibresource to allow the use
%of the classic bibtex-workflow in the future.
\newcommand{\AddBibFile}[1]{
	\addbibresource{#1}%
}

%See http://www.jr-x.de/publikationen/latex/tipps/zeilenumbruch.html
\newenvironment{bibhyphenation}% 
	{\hyphenpenalty=20%		(50)
	 \tolerance=9999%				(200)
	 \exhyphenpenalty=0%		(50)
	 \linepenalty=0%				(10)
	 \setlength{\emergencystretch}{3em}}%
	{}

% Currently the only public macro:
\newcommand{\MakeBibliography}[2][]{
	% opt. arg (#1): optional argument "nosplit" to get a 1-piece bibliography
	% req. arg (#2): main title of bibliography
	\clearpage
	\ifthenelse{\equal{#1}{nosplit}}%
		{% create a single bibliography with all entries:
			\printbibheading[heading=bibintoc,title={#2}]
			\begin{bibhyphenation}	
			\printbibliography[heading=noheader]
			\end{bibhyphenation}
		}
		{% split the bibliography using the predefined categories:
			\printbibheading[heading=bibintoc,title={#2}]
			\begin{bibhyphenation}
			\printbibliography[filter=literature,heading=subbibintoc,title={\@bibtitleLiteratur}]
			\printbibliography[filter=avmedia,heading=subbibintoc,title={\@bibtitleAvmedia}]
			\printbibliography[filter=online,heading=subbibintoc,title={\@bibtitleOnline}]
			\end{bibhyphenation}
		}%
}

%%-------------------------------------------------------------

\endinput

