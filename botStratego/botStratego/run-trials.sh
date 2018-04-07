#!/bin/bash
counter=1
for run in {1..1000}
do
    mono GameTrial.exe >> strategy.txt
    echo "Success!"
done
