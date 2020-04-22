åw
kC:\Users\javie\Desktop\Proyecto\ProyectoVersion2\ServidorLoteria\CalculatorService\ServicioCuentaUsuario.cs
	namespace		 	
ServidorLoteria		
 
{

 
[ 
ServiceBehavior 
( 
ConcurrencyMode $
=$ %
ConcurrencyMode& 5
.5 6
Single6 <
,< =
InstanceContextMode= P
=Q R
InstanceContextModeS f
.f g
Singleg m
)m n
]n o
public 

class !
ServicioCuentaUsuario &
:' ("
IServicioCuentaUsuario) ?
{ 
readonly 

Dictionary 
< &
ICalculatorServiceCallback 6
,6 7
string8 >
>> ?
_users@ F
=H I
newJ M

DictionaryN X
<X Y&
ICalculatorServiceCallbackY s
,s t
stringu {
>{ |
(| }
)} ~
;~ 
public 
void 
CerrarSesion  
(  !
string! '
nombreUsuario( 5
)5 6
{ 	
try 
{ 
foreach 
( 
var 
other "
in# %
_users& ,
., -
Keys- 1
)1 2
{ 
if 
( 
other 
. 
Equals $
($ %
nombreUsuario% 2
)2 3
)3 4
_users 
. 
Remove %
(% &
other& +
)+ ,
;, -
} 
Console 
. 
	WriteLine !
(! "
nombreUsuario" /
+0 1
$str2 L
)L M
;M N
} 
catch 
( %
InvalidOperationException ,
), -
{ 
OperationContext  
.  !
Current! (
.( )
GetCallbackChannel) ;
<; <&
ICalculatorServiceCallback< V
>V W
(W X
)X Y
.Y Z
	RespuestaZ c
(c d
$str	d ê
)
ê ë
;
ë í
} 
}   	
public"" 
void""  
GuardarCuentaUsuario"" (
(""( )
	CuentaSet"") 2
cuenta""3 9
)""9 :
{## 	
try$$ 
{%% 
BDLoteriaEntities&& !
db&&" $
=&&% &
new&&' *
BDLoteriaEntities&&+ <
(&&< =
)&&= >
;&&> ?
db'' 
.'' 
	CuentaSet'' 
.'' 
Add''  
(''  !
cuenta''! '
)''' (
;''( )
db(( 
.(( 
SaveChanges(( 
((( 
)((  
;((  !
Console)) 
.)) 
	WriteLine)) !
())! "
cuenta))" (
.))( )
nombreUsuario))) 6
+))7 8
$str))9 L
)))L M
;))M N
db** 
.** 
Dispose** 
(** 
)** 
;** 
}++ 
catch,, 
(,, %
InvalidOperationException,, ,
),,, -
{-- 
OperationContext..  
...  !
Current..! (
...( )
GetCallbackChannel..) ;
<..; <&
ICalculatorServiceCallback..< V
>..V W
(..W X
)..X Y
...Y Z
	Respuesta..Z c
(..c d
$str	..d ä
)
..ä ã
;
..ã å
}// 
}00 	
public22 
void22 
IniciarSesion22 !
(22! "
string22" (
nombreUsuario22) 6
,226 7
string228 >
contrase√±a22? I
)22I J
{33 	
try44 
{55 
BDLoteriaEntities66 !
db66" $
=66% &
new66' *
BDLoteriaEntities66+ <
(66< =
)66= >
;66> ?
db77 
.77 
	CuentaSet77 
.77 
Where77 "
(77" #
d77# $
=>77% '
d77( )
.77) *
nombreUsuario77* 7
==778 :
nombreUsuario77; H
&&77I K
d77L M
.77M N
contrase√±a77N X
==77Y [
contrase√±a77\ f
)77f g
.77g h
First77h m
(77m n
)77n o
;77o p
var88 

connection88 
=88  
OperationContext88! 1
.881 2
Current882 9
.889 :
GetCallbackChannel88: L
<88L M&
ICalculatorServiceCallback88M g
>88g h
(88h i
)88i j
;88j k
_users99 
[99 

connection99 !
]99! "
=99# $
nombreUsuario99% 2
;992 3
var:: 
cuenta:: 
=:: 
(:: 
from:: "
per::# &
in::' )
db::* ,
.::, -
	CuentaSet::- 6
where::7 <
per::= @
.::@ A
nombreUsuario::A N
==::O Q
nombreUsuario::R _
&&::` b
per::c f
.::f g
contrase√±a::g q
==::r t
contrase√±a::u 
select
::Ä Ü
per
::á ä
)
::ä ã
.
::ã å
First
::å ë
(
::ë í
)
::í ì
;
::ì î
OperationContext;;  
.;;  !
Current;;! (
.;;( )
GetCallbackChannel;;) ;
<;;; <&
ICalculatorServiceCallback;;< V
>;;V W
(;;W X
);;X Y
.;;Y Z
DevuelveCuenta;;Z h
(;;h i
cuenta;;i o
);;o p
;;;p q
Console<< 
.<< 
	WriteLine<< !
(<<! "
nombreUsuario<<" /
+<<0 1
$str<<2 H
)<<H I
;<<I J
db== 
.== 
Dispose== 
(== 
)== 
;== 
}>> 
catch?? 
(?? %
InvalidOperationException?? ,
)??, -
{@@ 
OperationContextAA  
.AA  !
CurrentAA! (
.AA( )
GetCallbackChannelAA) ;
<AA; <&
ICalculatorServiceCallbackAA< V
>AAV W
(AAW X
)AAX Y
.AAY Z
	RespuestaAAZ c
(AAc d
$str	AAd ä
)
AAä ã
;
AAã å
}BB 
}DD 	
publicFF 
voidFF "
ModificarCuentaUsuarioFF *
(FF* +
	CuentaSetFF+ 4
cuentaFF5 ;
)FF; <
{GG 	
tryHH 
{II 
BDLoteriaEntitiesJJ !
poJJ" $
=JJ% &
newJJ' *
BDLoteriaEntitiesJJ+ <
(JJ< =
)JJ= >
;JJ> ?
varKK 
cKK 
=KK 
(KK 
fromKK 
perKK !
inKK" $
poKK% '
.KK' (
	CuentaSetKK( 1
whereKK2 7
perKK8 ;
.KK; <
nombreUsuarioKK< I
==KKJ L
cuentaKKM S
.KKS T
nombreUsuarioKKT a
selectKKb h
perKKi l
)KKl m
.KKm n
FirstKKn s
(KKs t
)KKt u
;KKu v
cLL 
.LL 
correoLL 
=LL 
cuentaLL !
.LL! "
correoLL" (
;LL( )
cMM 
.MM 
contrase√±aMM 
=MM 
cuentaMM %
.MM% &
contrase√±aMM& 0
;MM0 1
cNN 
.NN 

fotoPerfilNN 
=NN 
cuentaNN %
.NN% &

fotoPerfilNN& 0
;NN0 1
poOO 
.OO 
SaveChangesOO 
(OO 
)OO  
;OO  !
ConsolePP 
.PP 
	WriteLinePP !
(PP! "
$strPP" 4
)PP4 5
;PP5 6
poQQ 
.QQ 
DisposeQQ 
(QQ 
)QQ 
;QQ 
}RR 
catchSS 
(SS %
InvalidOperationExceptionSS ,
)SS, -
{TT 
OperationContextUU  
.UU  !
CurrentUU! (
.UU( )
GetCallbackChannelUU) ;
<UU; <&
ICalculatorServiceCallbackUU< V
>UUV W
(UUW X
)UUX Y
.UUY Z
	RespuestaUUZ c
(UUc d
$str	UUd ó
)
UUó ò
;
UUò ô
}VV 
}WW 	
publicYY 
voidYY #
RegistrarPuntajeM√°ximoYY *
(YY* +
stringYY+ 1
nombreUsuarioYY2 ?
,YY? @
intYYA D
puntajeMaximoYYE R
)YYR S
{ZZ 	
try[[ 
{\\ 
BDLoteriaEntities]] !
po]]" $
=]]% &
new]]' *
BDLoteriaEntities]]+ <
(]]< =
)]]= >
;]]> ?
var^^ 
c^^ 
=^^ 
(^^ 
from^^ 
per^^ !
in^^" $
po^^% '
.^^' (
	CuentaSet^^( 1
where^^2 7
per^^8 ;
.^^; <
nombreUsuario^^< I
==^^J L
nombreUsuario^^M Z
select^^[ a
per^^b e
)^^e f
.^^f g
First^^g l
(^^l m
)^^m n
;^^n o
if__ 
(__ 
c__ 
.__ 
puntajeMaximo__ #
==__$ &
null__' +
)__+ ,
{`` 
caa 
.aa 
puntajeMaximoaa #
=aa$ %
puntajeMaximoaa& 3
;aa3 4
pobb 
.bb 
SaveChangesbb "
(bb" #
)bb# $
;bb$ %
Consolecc 
.cc 
	WriteLinecc %
(cc% &
$strcc& F
)ccF G
;ccG H
}dd 
elseee 
{ff 
ifgg 
(gg 
cgg 
.gg 
puntajeMaximogg '
<gg( )
puntajeMaximogg* 7
)gg7 8
{hh 
cii 
.ii 
puntajeMaximoii '
=ii( )
puntajeMaximoii* 7
;ii7 8
pojj 
.jj 
SaveChangesjj &
(jj& '
)jj' (
;jj( )
Consolekk 
.kk  
	WriteLinekk  )
(kk) *
$strkk* J
)kkJ K
;kkK L
}ll 
elsemm 
{nn 
Consoleoo 
.oo  
	WriteLineoo  )
(oo) *
$stroo* L
)ooL M
;ooM N
}pp 
}qq 
porr 
.rr 
Disposerr 
(rr 
)rr 
;rr 
}ss 
catchtt 
(tt %
InvalidOperationExceptiontt ,
)tt, -
{uu 
OperationContextvv  
.vv  !
Currentvv! (
.vv( )
GetCallbackChannelvv) ;
<vv; <&
ICalculatorServiceCallbackvv< V
>vvV W
(vvW X
)vvX Y
.vvY Z
	RespuestavvZ c
(vvc d
$str	vvd §
)
vv§ •
;
vv• ¶
}ww 
}xx 	
publiczz 
voidzz 
EnviarMensajeChatzz %
(zz% &
stringzz& ,
nombreUsuariozz- :
,zz: ;
stringzz< B
mensajezzC J
)zzJ K
{{{ 	
string|| 
mensaje1|| 
=|| 
$str|| !
+||" #
nombreUsuario||$ 1
+||2 3
$str||4 7
+||8 9
mensaje||: A
;||A B
Thread}} 
.}} 
Sleep}} 
(}} 
$num}} 
)}} 
;}} 
var~~ 

connection~~ 
=~~ 
OperationContext~~ -
.~~- .
Current~~. 5
.~~5 6
GetCallbackChannel~~6 H
<~~H I&
ICalculatorServiceCallback~~I c
>~~c d
(~~d e
)~~e f
;~~f g
foreach
ÄÄ 
(
ÄÄ 
var
ÄÄ 
other
ÄÄ 
in
ÄÄ !
_users
ÄÄ" (
.
ÄÄ( )
Keys
ÄÄ) -
)
ÄÄ- .
{
ÅÅ 
if
ÇÇ 
(
ÇÇ 
other
ÇÇ 
==
ÇÇ 

connection
ÇÇ '
)
ÇÇ' (
continue
ÉÉ 
;
ÉÉ 
other
ÑÑ 
.
ÑÑ 
	Respuesta
ÑÑ 
(
ÑÑ  
mensaje1
ÑÑ  (
)
ÑÑ( )
;
ÑÑ) *
}
ÖÖ 
}
áá 	
public
ââ 
void
ââ 
SolicitarPuntajes
ââ %
(
ââ% &
)
ââ& '
{
ää 	
try
ãã 
{
åå 
List
çç 
<
çç 
PuntajeUsuario
çç #
>
çç# $
puntajes
çç% -
=
çç. /
new
çç0 3
List
çç4 8
<
çç8 9
PuntajeUsuario
çç9 G
>
ççG H
(
ççH I
)
ççI J
;
ççJ K
using
éé 
(
éé 
var
éé 
ctx
éé 
=
éé  
new
éé! $
BDLoteriaEntities
éé% 6
(
éé6 7
)
éé7 8
)
éé8 9
{
èè 
var
êê 
cuentas
êê 
=
êê  !
from
êê" &
s
êê' (
in
êê) +
ctx
êê, /
.
êê/ 0
	CuentaSet
êê0 9
orderby
ëë" )
s
ëë* +
.
ëë+ ,
puntajeMaximo
ëë, 9

descending
ëë: D
select
íí" (
s
íí) *
;
íí* +
foreach
ìì 
(
ìì 
var
ìì  
valor
ìì! &
in
ìì' )
cuentas
ìì* 1
)
ìì1 2
{
îî 
puntajes
ïï  
.
ïï  !
Add
ïï! $
(
ïï$ %
new
ïï% (
PuntajeUsuario
ïï) 7
(
ïï7 8
valor
ïï8 =
.
ïï= >
nombreUsuario
ïï> K
,
ïïK L
valor
ïïM R
.
ïïR S
puntajeMaximo
ïïS `
)
ïï` a
)
ïïa b
;
ïïb c
Console
ññ 
.
ññ  
	WriteLine
ññ  )
(
ññ) *
valor
ññ* /
.
ññ/ 0
nombreUsuario
ññ0 =
+
ññ> ?
$str
ññ@ C
+
ññD E
valor
ññF K
.
ññK L
puntajeMaximo
ññL Y
)
ññY Z
;
ññZ [
}
óó 
}
òò 
OperationContext
ôô  
.
ôô  !
Current
ôô! (
.
ôô( ) 
GetCallbackChannel
ôô) ;
<
ôô; <(
ICalculatorServiceCallback
ôô< V
>
ôôV W
(
ôôW X
)
ôôX Y
.
ôôY Z
DevuelvePuntajes
ôôZ j
(
ôôj k
puntajes
ôôk s
)
ôôs t
;
ôôt u
}
öö 
catch
õõ 
(
õõ '
InvalidOperationException
õõ ,
)
õõ, -
{
úú 
OperationContext
ùù  
.
ùù  !
Current
ùù! (
.
ùù( ) 
GetCallbackChannel
ùù) ;
<
ùù; <(
ICalculatorServiceCallback
ùù< V
>
ùùV W
(
ùùW X
)
ùùX Y
.
ùùY Z
	Respuesta
ùùZ c
(
ùùc d
$strùùd ±
)ùù± ≤
;ùù≤ ≥
}
ûû 
}
üü 	
}
†† 
}°° ˆ#
lC:\Users\javie\Desktop\Proyecto\ProyectoVersion2\ServidorLoteria\CalculatorService\IServicioCuentaUsuario.cs
	namespace 	
ServidorLoteria
 
{ 
[

 
ServiceContract

 
(

 
CallbackContract

 %
=

& '
typeof

( .
(

. /&
ICalculatorServiceCallback

/ I
)

I J
)

J K
]

K L
public 

	interface "
IServicioCuentaUsuario +
{ 
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
IniciarSesion 
( 
string !
nombreUsuario" /
,/ 0
string1 7
contrase√±a8 B
)B C
;C D
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
CerrarSesion 
( 
string  
nombreUsuario! .
). /
;/ 0
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void  
GuardarCuentaUsuario !
(! "
	CuentaSet" +
cuenta, 2
)2 3
;3 4
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
EnviarMensajeChat 
( 
string %
nombreUsuario& 3
,3 4
string5 ;
mensaje< C
)C D
;D E
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void "
ModificarCuentaUsuario #
(# $
	CuentaSet$ -
cuenta. 4
)4 5
;5 6
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void #
RegistrarPuntajeM√°ximo #
(# $
string$ *
nombreUsuario+ 8
,8 9
int: =
puntajeMaximo> K
)K L
;L M
[ 	
OperationContract	 
( 
IsOneWay #
=$ %
true& *
)* +
]+ ,
void 
SolicitarPuntajes 
( 
)  
;  !
} 
[ 
ServiceContract 
] 
public 

	interface &
ICalculatorServiceCallback /
{ 
[   	
OperationContract  	 
(   
IsOneWay   #
=  $ %
true  & *
)  * +
]  + ,
void!! 
	Respuesta!! 
(!! 
string!! 
mensaje!! %
)!!% &
;!!& '
[## 	
OperationContract##	 
(## 
IsOneWay## #
=##$ %
true##& *
)##* +
]##+ ,
void$$ 
DevuelveCuenta$$ 
($$ 
	CuentaSet$$ %
cuenta$$& ,
)$$, -
;$$- .
[&& 	
OperationContract&&	 
(&& 
IsOneWay&& #
=&&$ %
true&&& *
)&&* +
]&&+ ,
void'' 
DevuelvePuntajes'' 
('' 
List'' "
<''" #
PuntajeUsuario''# 1
>''1 2
puntajes''3 ;
)''; <
;''< =
})) 
[++ 
DataContract++ 
]++ 
public,, 

class,, 
PuntajeUsuario,, 
{-- 
[.. 	

DataMember..	 
].. 
string// 
nombreUsuario// 
;// 
[00 	

DataMember00	 
]00 
private11 
long11 
?11 
puntajeMaximo11 #
;11# $
public22 
PuntajeUsuario22 
(22 
string22 $
nombreUsuario22% 2
,222 3
long224 8
?228 9
puntajeMaximo22: G
)22G H
{33 	
this44 
.44 
nombreUsuario44 
=44  
nombreUsuario44! .
;44. /
this55 
.55 
puntajeMaximo55 
=55  
puntajeMaximo55! .
;55. /
}66 	
}77 
}88 ò
mC:\Users\javie\Desktop\Proyecto\ProyectoVersion2\ServidorLoteria\CalculatorService\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str ,
), -
]- .
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[		 
assembly		 	
:			 
!
AssemblyConfiguration		  
(		  !
$str		! #
)		# $
]		$ %
[

 
assembly

 	
:

	 

AssemblyCompany

 
(

 
$str

 
)

 
]

 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str .
). /
]/ 0
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *