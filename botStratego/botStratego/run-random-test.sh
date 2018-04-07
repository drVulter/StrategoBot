#!/bin/bash
counter=1
for run in {1..1000}
do
    mono RandomSetUp.exe >> random.txt
    echo "Success!"
done
