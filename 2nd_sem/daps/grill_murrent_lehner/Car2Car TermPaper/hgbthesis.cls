%% hgbthesis.cls
%% Autor: Wilhelm Burger (wilhelm.burger@fh-hagenberg.at)
%% Diese Datei ist Teil des hagenberg-Package zur Erstellung von 
%% Bachelor- und Diplomarbeiten.

\NeedsTeXFormat{LaTeX2e}
\newcommand{\hgbthesisDate}{2012/12/08} %Versionsdatum
\ProvidesClass{hgbthesis}[\hgbthesisDate\space LaTeX-Vorlage fuer Diplomarbeiten, Masterarbeiten, Bachelorarbeiten etc.]

\RequirePackage{ifthen}
   
\newcommand{\hgb@MainLanguage}{german}	% also referenced in hgb.sty
\newcommand{\@matitelseiten}{}
\newcommand{\@datitelseiten}{}
\newcommand{\@batitelseiten}{}
\newcommand{\@prtitelseiten}{}
\newcommand{\@maketitlepages}{\@matitelseiten}  %default: Praktikum


%% begin class options ---------------------------------------------
\DeclareOption{master}{\renewcommand{\@maketitlepages}{\@matitelseiten}}
\DeclareOption{diplom}{\renewcommand{\@maketitlepages}{\@datitelseiten}}
\DeclareOption{bachelor}{\renewcommand{\@maketitlepages}{\@batitelseiten}}
\DeclareOption{praktikum}{\renewcommand{\@maketitlepages}{\@prtitelseiten}}
\DeclareOption{english}{\renewcommand{\hgb@MainLanguage}{english}}
\DeclareOption{german}{\renewcommand{\hgb@MainLanguage}{german}}
\DeclareOption*{\ClassWarning{hgbthesis}{Unknown document option "\CurrentOption"}}
\ProcessOptions
%% end class options ---------------------------------------------
 
\LoadClass[a4paper,11pt,oneside]{book}
 
\RequirePackage{hgb}	% load associated style file hgb.sty


%-------------- Setup f�r Seiten im Front-Teil --------------------

\newcommand{\@studiengang}{\@MissingArg{Studiengang??}}
\newcommand{\studiengang}[1]{%
	\renewcommand{\@studiengang}{#1}
}

\newcommand{\@studienort}{\@MissingArg{Studienort??}}
\newcommand{\studienort}[1]{%
	\renewcommand{\@studienort}{#1}
}

\newcommand{\@betreuerBez}{????}
\newcommand{\@betreuerName}{\@MissingArg{Betreuer??}}

\newcommand{\betreuer}[1]{%
  \renewcommand{\@betreuerBez}{Betreuer}
	\renewcommand{\@betreuerName}{#1}
}
\newcommand{\betreuerin}[1]{%
  \renewcommand{\@betreuerBez}{Betreuerin}
	\renewcommand{\@betreuerName}{#1}
}

\newcommand{\@nummer}{\@MissingArg{Arbeit Nummer??}}
\newcommand{\nummer}[1]{%
	\renewcommand{\@nummer}{#1}
}

\newcommand{\@gegenstand}{\@MissingArg{Gegenstand??}}
\newcommand{\gegenstand}[1]{%
	\renewcommand{\@gegenstand}{#1}
}

\newcommand{\@fhbetreuer}{\@MissingArg{Fhbetreuer??}}
\newcommand{\fhbetreuer}[1]{%
	\renewcommand{\@fhbetreuer}{#1}
}

\newcommand{\@fhbetreuertel}{\@MissingArg{FhbetreuerTel??}}
\newcommand{\fhbetreuertel}[1]{%
	\renewcommand{\@fhbetreuertel}{#1}
}

\newcommand{\@fhbetreuermail}{\@MissingArg{FhbetreuerMail??}}
\newcommand{\fhbetreuermail}[1]{%
	\renewcommand{\@fhbetreuermail}{#1}
}

\newcommand{\@semester}{\@MissingArg{Semester??}}
\newcommand{\semester}[1]{%
	\renewcommand{\@semester}{#1}
}

%% -------------------------------------------------

\newcounter{@abgabetag}
\newcounter{@abgabemonat}
\newcounter{@abgabejahr}

\setcounter{@abgabetag}{\the\day}
\setcounter{@abgabemonat}{\the\month}
\setcounter{@abgabejahr}{\the\year}

\newcommand{\abgabedatum}[3]{%	%\abgabedatum{yyyy}{mm}{dd}
	\setcounter{@abgabejahr}{#1}
	\setcounter{@abgabemonat}{#2}
	\setcounter{@abgabetag}{#3}
}

\newcommand{\abgabemonat}[1]{%
	\ClassWarning{hgbthesis}{\noexpand\abgabemonat is deprecated - 
	use \noexpand\abgabedatum instead!}
}

\newcommand{\abgabejahr}[1]{%
	\ClassWarning{hgbthesis}{\noexpand\abgabejahr is deprecated - 
	use \noexpand\abgabedatum instead!}
}

%% ----------------------------------------------------------

\newcommand{\@firma}{\@MissingArg{Firma??}}
\newcommand{\firma}[1]{%
\renewcommand{\@firma}{#1}
}

\newcommand{\@firmenUrl}{\@MissingArg{www.???}}
\newcommand{\firmenUrl}[1]{%
\renewcommand{\@firmenUrl}{#1}
}

\newcommand{\@firmenTel}{\@MissingArg{Tel??}}
\newcommand{\firmenTel}[1]{%
\renewcommand{\@firmenTel}{#1}
}

\newcommand{\@betreuertel}{\@MissingArg{BetreuerTel??}}
\newcommand{\betreuertel}[1]{%
	\renewcommand{\@betreuertel}{#1}
}

\newcommand{\@betreuermail}{\@MissingArg{BetreuerMail??}}
\newcommand{\betreuermail}[1]{%
	\renewcommand{\@betreuermail}{#1}
}

\newcommand{\@license}{\@MissingArg{Lizenztext??}}
\newcommand{\license}[1]{%
\renewcommand{\@license}{#1}
}


%------------------ Titelseiten -------------------

%% deprecated - use \maketitle instead!
\newcommand{\titelseite}{ 
  \ClassWarning{hgbthesis}{\noexpand\titelseite is deprecated - use \noexpand\maketitle instead}
  \maketitle
} 

\newcommand{\@batitel}{%
\begin{titlepage}%
\rule{0mm}{1mm}
\vspace*{20mm}
%\noindent Left side \hfill right side\\
\begin{center}%
  \setlength{\unitlength}{1cm}
  \begin{picture}(12,4)
  %\thinlines\put(0,0){\framebox(12,4){}}
  \put(0,1.8){\parbox{12cm}{\centering\LARGE\bf%
    \setlength{\baselineskip}{20pt}%
    \@title}}
  \end{picture}
    \vskip 10mm%
    {\large\sc\@author}
    \vskip 40mm%
    {\Large S\,E\,M\,I\,N\,A\,R\,A\,R\,B\,E\,I\,T}%
    \vskip 8mm%
    %{\large\sf Nr.~\@nummer}
    \vskip 8mm%
    %{\large eingereicht am} \vskip 1mm {Fachhochschul-Masterstudiengang}%
    \vskip 4mm%
    {\large\sc\@studiengang}%
    \vskip 4mm%
    {\large in \@studienort}%
    \vskip 16mm%
    %{\large im \monthname[\value{@abgabemonat}] \arabic{@abgabejahr}}%
\end{center}\par
  \vfil\null
\end{titlepage}%
}

\newcommand{\@matitel}{%
\begin{titlepage}%
\begin{german}
\rule{0mm}{1mm}
\vspace*{20mm}
%\noindent Left side \hfill right side\\
\begin{center}%
  \setlength{\unitlength}{1cm}
  \begin{picture}(12,4)
  %\thinlines\put(0,0){\framebox(12,4){}}
  \put(0,1.8){\parbox{12cm}{\centering\LARGE\bf%
    \setlength{\baselineskip}{20pt}%
    \@title}}
  \end{picture}
    \vskip 10mm%
    {\large\sc\@author}
    \vskip 40mm%
    {\Large M\,A\,S\,T\,E\,R\,A\,R\,B\,E\,I\,T}%
    \vskip 20mm%
    {\large eingereicht am} \vskip 1mm {Fachhochschul-Bachelorstudiengang}%
    \vskip 4mm%
    {\large\sc\@studiengang}%
    \vskip 4mm%
    {\large in \@studienort}%
    \vskip 16mm%
    {\large im \monthname[\value{@abgabemonat}] \arabic{@abgabejahr}}%
%   {\large im \@abgabemonat\ \@abgabejahr}%
\end{center}\par
  \vfil\null
\end{german}
\end{titlepage}%
}

\newcommand{\@datitel}{%
\begin{titlepage}%
\rule{0mm}{1mm}
\vspace*{20mm}
%\noindent Left side \hfill right side\\
\begin{center}%
  \setlength{\unitlength}{1cm}
  \begin{picture}(12,4)
  %\thinlines\put(0,0){\framebox(12,4){}}
  \put(0,1.8){\parbox{12cm}{\centering\LARGE\bf%
    \setlength{\baselineskip}{20pt}%
    \@title}}
  \end{picture}
    \vskip 10mm%
    {\large\sc\@author}
    \vskip 40mm%
    {\Large D\,I\,P\,L\,O\,M\,A\,R\,B\,E\,I\,T}%
    \vskip 20mm%
    {\large eingereicht am} \vskip 1mm {Fachhochschul-Bachelorstudiengang}%
    \vskip 4mm%
    {\large\sc\@studiengang}%
    \vskip 4mm%
    {\large in \@studienort}%
    \vskip 16mm%
    {\large im \monthname[\value{@abgabemonat}] \arabic{@abgabejahr}}%
\end{center}\par
  \vfil\null
\end{titlepage}%
}

\newcommand{\@prtitel}{%
\begin{titlepage}%
\rule{0mm}{1mm}
\vspace*{20mm}
%\noindent Left side \hfill right side\\
\begin{center}%
  \setlength{\unitlength}{1cm}
  \begin{picture}(12,4)
  %\thinlines\put(0,0){\framebox(12,4){}}
  \put(0,1.8){\parbox{12cm}{\centering\LARGE\bf%
    \setlength{\baselineskip}{20pt}%
    Endbericht zum Berufspraktikum}}
  \end{picture}
    \vskip 10mm%
    {\large\sc\@author}
    \vskip 40mm%
    {\Large B\,A\,C\,H\,E\,L\,O\,R\,A\,R\,B\,E\,I\,T}%
    \vskip 8mm%
    {\large\sf Nr.~\@nummer}
    \vskip 8mm%
    {\large eingereicht am} \vskip 1mm {Fachhochschul-Bachelorstudiengang}%
    \vskip 4mm%
    {\large\sc\@studiengang}%
    \vskip 4mm%
    {\large in \@studienort}%
    \vskip 16mm%
    {\large im \monthname[\value{@abgabemonat}] \arabic{@abgabejahr}}%
\end{center}\par
  \vfil\null
\end{titlepage}%
}

%------------------ Copyright-Seite -------------------

\newcommand{\strictlicense}{%
\ifthenelse{\equal{\hgb@MainLanguage}{german}}%
	{\license{Alle Rechte vorbehalten}}%
	{\license{All Rights Reserved}}
}

\newcommand{\@ccLicenseAt}{%
Diese Arbeit wird unter den Bedingungen der \textsl{Creative Commons Lizenz} 
\emph{Namens\-nennung--NichtKommerziell--Keine\-Bearbeitung �sterreich} 
(CC BY-NC-ND) ver�ffentlicht --
siehe \url{http://creativecommons.org/licenses/by-nc-nd/3.0/at/}.}

\newcommand{\@ccLicenseEn}{%
This work is published under the conditions of the 
\textsl{Creative Commons License Attribution--Non\-Commercial--No\-Derivatives} 
(CC BY-NC-ND)---see
\url{http://creativecommons.org/licenses/by-nc-nd/3.0/}.}

\newcommand{\cclicense}{%
\ifthenelse{\equal{\hgb@MainLanguage}{german}}%
{\license{\@ccLicenseAt}} % german CC license text
{\license{\@ccLicenseEn}} % english CC license text (default)
}
	
%\strictlicense
\cclicense	% Creative Commons License (default)

\newcommand{\@makelicense}{ 
  \sbox\@tempboxa{\@license}%
  \ifdim \wd\@tempboxa >\hsize
    \begin{sloppypar}\noindent\@license\end{sloppypar}
  \else
    \global \@minipagefalse
    \hb@xt@\hsize{\hfil\box\@tempboxa\hfil}%
  \fi
} 

\newcommand{\@copyrightseite}{%
\newpage
\setcounter{page}{2}
\thispagestyle{plain}
\rule{0mm}{1mm}
\vspace*{75mm}
\begin{center}
\copyright\ Copyright\ \arabic{@abgabejahr}\ \@author
\end{center}
\vspace{8mm}
\@makelicense
%\begin{center}
%\includegraphics[width=2.0cm]{images/by-nc-nd-eu}
%\end{center}
}

%------------------ Firmen-Seite -------------------
\newcommand{\@firmenseite}{%
\newpage
\setcounter{page}{3}
\thispagestyle{plain}
\rule{0mm}{1mm}
\begin{center}
\vspace{25mm} 
{Praktikumsstelle:}
\\
\vspace{3mm}
{\Large\sf\@firma\\ \mbox{}}
\\
{\sf \@firmenTel}
\\
{\sf \@firmenUrl}
\\
\vspace{45mm} 
{Kontaktperson:}
\\
\vspace{3mm} 
{\Large\sf\@betreuerName\\ \mbox{}}
{\sf \@betreuertel}
\\
{\sf \@betreuermail}
\\
\end{center}
}

%------------------ Betreuer-Seite -------------------

\newcommand{\@babetreuerseite}{%
\newpage
\setcounter{page}{2}
\thispagestyle{plain}
\rule{0mm}{1mm}
\begin{center}

\vspace{40mm} 
{Diese Arbeit entstand im Rahmen des }\\
\vspace{6mm} 
{\Large\sf \@gegenstand\\ \mbox{}}\\
\medskip
im\\
\bigskip
{\@semester}\\

\vspace{45mm} 
{Betreuer:}\\
%{\@betreuerBez:}\\
\vspace{3mm} 
{\Large\sf \@fhbetreuer\\ \mbox{}}
{\sf \@fhbetreuertel}
\\
{\sf \@fhbetreuermail}
\\
\end{center}
}

%% -------------- Erkl�rung / Declaration ---------

%\newcommand{\@erklaerungsseiteDe}{%
%\begin{german}
%\chapter{Erkl�rung}
%\noindent
%Standardfassung der FH-O� ab 04.04.2012:
%Ich erkl�re eidesstattlich, dass ich die vorliegende Arbeit selbstst�ndig und ohne fremde Hilfe verfasst, 
%andere als die angegebenen Quellen nicht benutzt und die den benutzten Quellen entnommenen Stellen als 
%solche gekennzeichnet habe. Die Arbeit wurde bisher in gleicher oder �hnlicher Form keiner anderen 
%Pr�fungsbeh�rde vorgelegt.
%\par
%\vspace{10mm}
%\noindent
%\@studienort, am \formatdate{\value{@abgabetag}}{\value{@abgabemonat}}{\value{@abgabejahr}}
%\par
%\vspace{12mm}
%\noindent
%\@author
%\end{german}
%}


%\newcommand{\leadingzero}[1]{\ifnum #1<10 0\the#1\else\the#1\fi}

%\newcommand{\@erklaerungsseiteEn}{%
%\begin{english}
%\chapter{Declaration}
%\noindent
%I hereby declare and confirm that this thesis is entirely the result of my own original work. 
%Where other sources of information have been used, they have been indicated as such and properly 
%acknowledged. I further declare that this or similar work has not been submitted for credit elsewhere.
%\par
%\vspace{10mm}
%\noindent
%\@studienort, \the\year-\leadingzero{\month}-\leadingzero{\day} %http://de.wikibooks.org/wiki/LaTeX-W%F6rterbuch:_today 
%\@studienort, {\usdate\formatdate{\value{@abgabetag}}{\value{@abgabemonat}}{\value{@abgabejahr}}}
%\par
%\vspace{12mm}
%\noindent
%\@author
%\end{english}
%}

%\newcommand{\@erklaerungsseite}{%
%\ifthenelse{\equal{\hgb@MainLanguage}{german}}%
%	{\@erklaerungsseiteDe}%
%	{\@erklaerungsseiteEn}
%}

%\newcommand{\@erklaerungsseite}{%
%\begin{german}
%\chapter[Erkl�rung]{~}
%\section*{Erkl�rung}
%
%\noindent
%Ich erkl�re eidesstattlich, dass ich die vorliegende Arbeit selbstst�ndig und ohne fremde Hilfe verfasst, andere als die angegebenen Quellen nicht benutzt und die den benutzten Quellen entnommenen Stellen als solche gekennzeichnet habe. Die Arbeit wurde bisher in gleicher oder �hnlicher Form keiner anderen Pr�fungsbeh�rde vorgelegt.
%
%\vspace{10mm}
%\noindent
%\@studienort, am \today
%
%\vspace{12mm}
%\noindent
%\@author
%\end{german}
%}


%------------ Struktur der Frontseiten -------------------
\usepackage{floatflt} 
\usepackage{wrapfig}
\usepackage{datetime}
\renewcommand{\@matitelseiten}{
\@matitel
\@copyrightseite
\@erklaerungsseite
}
     
\renewcommand{\@datitelseiten}{
\@datitel
\@copyrightseite
\@erklaerungsseite
}

\renewcommand{\@batitelseiten}{
\@batitel
%\@babetreuerseite
%\@firmenseite
%\@erklaerungsseite
}

%\renewcommand{\@prtitelseiten}{
%\@prtitel
%\@firmenseite
%\@erklaerungsseite
%}

%------------ Automatischer Eintrag der PDF Metadaten ------------

\renewcommand{\maketitle}{	%replace standard LaTeX-command
\hypersetup{
		pdftitle={\@title},
		pdfauthor={\@author},
		pdfsubject={\@studiengang, \@studienort},
		pdfcreator={LaTeX using class hgbthesis},
		pdfproducer={},
		pdfkeywords={}
}%
\@maketitlepages
}

%% --------------- Headings --------------------------------------

\RequirePackage{fancyhdr}
\usepackage{fancyhdr}
\pagestyle{fancy}
\@ifundefined{chapter}%
   {}%
	 {\renewcommand{\chaptermark}[1]{%
			\ifthenelse{\value{chapter} < 1}%
				{\markboth{#1}{}}%
				{\markboth{\thechapter.\ #1}{}}
		}}

\addtolength{\headheight}{\baselineskip}
%\renewcommand{\headrulewidth}{0pt} %% thin line under header
\renewcommand{\headrulewidth}{0.4pt} %% no line under header
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
\usepackage[style=numeric-comp,sorting=none,minnames=3,maxnames=5,backend=bibtex8]{biblatex}[2011/11/13] 

%Note: biblatex with bibtex backend is frozen at version 1.7.
% do not use these:
%\usepackage[style=alphabetic]{biblatex}[2011/05/12]
%\usepackage[style=authoryear]{biblatex}[2011/05/12]
%\usepackage[style=ieee]{biblatex}[2011/05/12]
%\usepackage[style=nature]{biblatex}[2011/05/12]

\ExecuteBibliographyOptions{sortcites=true,defernumbers=true,isbn=false,doi=false}

%% titles of reference section + 3 categories of references:
\newcommand{\@bibtitle}{Quellenverzeichnis}
\newcommand{\@bibtitleLiteratur}{Literatur}
\newcommand{\@bibtitleAvmedia}{Filme und audiovisuelle Medien}
\newcommand{\@bibtitleOnline}{Online-Quellen}	

\ifthenelse{\equal{\hgb@MainLanguage}{english}}{%
	\renewcommand{\@bibtitle}{References}
	\renewcommand{\@bibtitleLiteratur}{Literature}
	\renewcommand{\@bibtitleAvmedia}{Films and audio-visual media}
	\renewcommand{\@bibtitleOnline}{Online sources}	
}{}

\defbibfilter{literature}{
	not ( type=movie or type=video or type=music or type=audio or 
				type=online or type=electronic or type=www or type=software )}

\defbibfilter{avmedia}{%
	( type=movie or type=video or type=music or type=audio or type=software )}

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

% Similar problem with badly braking date strings in english.
% Taken from english.lbx: added breaking zero space \hskip0pt to /
\DefineBibliographyExtras{english}{% 
  \def\mkbibdatelong#1#2#3{%
    \iffieldundef{#2}
      {}
      {\mkbibmonth{\thefield{#2}}%
       \iffieldundef{#3}
         {\iffieldundef{#1}{}{\space}}
	 {\space}}%	%was: \nobreakspace
    \iffieldundef{#3}
      {}
      {\stripzeros{\thefield{#3}}%
       \iffieldundef{#1}{}{,\space}}%
    \stripzeros{\thefield{#1}}}%
%
  \def\mkbibdateshort#1#2#3{%
    \iffieldundef{#2}
      {}
      {\mkdatezeros{\thefield{#2}}%
       \iffieldundef{#3}
         {\iffieldundef{#1}{}{/{\hskip0pt}}}
	 {/{\hskip0pt}}}%
    \iffieldundef{#3}
      {}
      {\mkdatezeros{\thefield{#3}}%
       \iffieldundef{#1}{}{/{\hskip0pt}}}%
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
% \MakeBibliography ... creates a reference section split subsections (default)
% \MakeBibliography[nosplit] ... creates a one-piece reference section
\newcommand{\MakeBibliography}[1][nosplit]{
	% opt. arg (#1): optional argument "nosplit" to get a 1-piece bibliography
	\clearpage
	\ifthenelse{\equal{#1}{nosplit}}%
		{% create a single bibliography with all entries:
			%\printbibheading[heading=bibintoc,title={#2}]
			\printbibheading[heading=bibintoc,title={\@bibtitle}]
			\begin{bibhyphenation}	
			\printbibliography[heading=noheader]
			\end{bibhyphenation}
		}
		{% split the bibliography using the predefined categories:
			%\printbibheading[heading=bibintoc,title={#2}]
			\printbibheading[heading=bibintoc,title={\@bibtitle}]
			\begin{bibhyphenation}
			\printbibliography[filter=literature,heading=subbibintoc,title={\@bibtitleLiteratur}]
			\printbibliography[filter=avmedia,heading=subbibintoc,title={\@bibtitleAvmedia}]
			\printbibliography[filter=online,heading=subbibintoc,title={\@bibtitleOnline}]
			\end{bibhyphenation}
		}%
}
%%-------------------------------------------------------------

\endinput

