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