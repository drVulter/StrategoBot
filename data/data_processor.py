# file to do basic data processing for StrategoBot trials

import sys
import numpy as np
import matplotlib.pyplot as plt

file_name = sys.argv[1]

if (file_name == "setUpBoard-trials.txt"):
    title = "Trials With Semi-Random Setup"
    plt.figure("semirandom")
else:
    title = "Trials With Random Setup"
    plt.figure("random")
#with open(file_name, 'r') as f:
#    times = f.read().splitlines()


#sum = 0 # sum of all times
#for time in times:
#    sum = sum + int(time)

#avg_time = sum / len(times)

f = open(file_name)
data = np.loadtxt(f) # import textfile as numpy array

print("The mean time is {}".format(np.mean(data)))
print("The standard deviation is {}".format(np.std(data)))
print("The variance is {}".format(np.var(data)))
print("The median is {}".format(np.median(data)))

hist, bins = np.histogram(data, bins=10)
width = 0.7 * (bins[1] - bins[0])
center = (bins[:-1] + bins[1:]) / 2
plt.bar(center, hist, align='center', width=width)
plt.title(title)
plt.ylabel('Number of Trials')
plt.xlabel('Number of Moves Before Repetition')
plt.show()

