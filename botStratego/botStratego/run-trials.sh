#!/bin/bash
counter=1
for run in {1..1000}
do
    mono GameTrial.exe >> setUpBoard-trials.txt
    echo "Success!"
done
