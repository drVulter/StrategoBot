# processor for random testing data
# used for practice

import numpy as np
from sklearn.linear_model import LogisticRegression

f = open('sample-data.txt')
f.readline() # skip the first line
data = np.loadtxt(f) # import textfile as numpy array

# separate data
X = data[:, 1:] # everything but first column
Y = data[:,0] # first column
