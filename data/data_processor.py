# file to do basic data processing for StrategoBot trials

import sys
import numpy as np

file_name = sys.argv[1]


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

