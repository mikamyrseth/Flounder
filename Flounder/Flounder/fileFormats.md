## .flo 1.0.2
1: flo 1.0.2\
1: (int) n // number of bodies\
n: Shape form, shape param 1, shape param 2\
// repeat\
(float) simulation time\
n: \t(float) posX, (float) posY\
\# lines starting with \# are comment lines that are ignored
#### Example
flo 1.0.2\
3, Micro\
\# Box A\
Rectangle, 0.2, 0.4\
\# Box B\
Rectangle, 2, 0.4\
\# Frame time\
0.0\
\# x, y\
&emsp;0.0, 0.0\
&emsp;0.0, 0.4\
0.1\
&emsp;0.1, 0.0\
&emsp;0.0, 0.4\
0.2\
&emsp;0.3, 0.0\
&emsp;0.1, 0.4
## .flo 1.0.1
1: flo 1.0.1\
1: (int) n // number of bodies\
n: Shape form, shape param 1, shape param 2\
// repeat\
(float) remaining time\
n: \t(float) posX, (float) posY\
\# lines starting with \# are comment lines that are ignored
#### Example
flo 1.0.1\
3, Micro\
\# Box A\
Rectangle, 0.2, 0.4\
\# Box B\
Rectangle, 2, 0.4\
\# Frame time\
0.2\
\# x, y\
&emsp;0.0, 0.0\
&emsp;0.0, 0.4\
0.1\
&emsp;0.1, 0.0\
&emsp;0.0, 0.4\
0.0\
&emsp;0.3, 0.0\
&emsp;0.1, 0.4
## .flo 1.0.0
First line: flo version\
n – number of bodies, precision\
n lines: id, shape\
m * n lines: id, posX, posY\
\# lines starting with \# are comment lines that are ignored
#### Example
flo 1.0.0\
3, Micro\
"box1", { rectangle: { semiSize: { x: 1024, y: 1024 } } }\
"box2", { rectangle: { semiSize: { x: 2048, y: 1024 } } }\
"ball", { circle: { radius: 4096 } }\
"box1", 123049, 201392\
"box2", 538235, 532135\
"ball", -42825, 381246\
"box1", 123049, 201392\
"box2", 538219, 532106\
"ball", -42819, 381250
## .flod 1.0.0
First line: flod version\
Second line: n – integer with number of object\
n lines: id, shape\
m * n lines: id, posX, posY, veloX, veloY, accX, accY\
\# lines starting with \# are comment lines that are ignored
#### Example
flod 1.0.0\
3, Nano\
"box1", { rectangle: { semiSize: { x: 1024, y: 1024 } } }\
"box2", { rectangle: { semiSize: { x: 2048, y: 1024 } } }\
"ball", { circle: { radius: 4096 } }\
"box1", 123049, 201392, 0, 0, 0, 0\
"box2", 538235, 532135, 0, 0, 0, 0\
"ball", -42825, 381246, 0, 0, 0, 0\
"box1", 123049, 201392, 0, 0, 0, 0\
"box2", 538235, 532135, 0, 0, 0, 0\
"ball", -42825, 381246, 0, 0, 0, 0\