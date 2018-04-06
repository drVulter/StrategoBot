#!/bin/bash
counter=1
while [ 1 == 1 ]
do
    mono Program.exe
    ((counter++))
    echo $counter
done
