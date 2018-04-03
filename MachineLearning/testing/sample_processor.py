# processor for random testing data
# used for practice

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt 
plt.rc("font", size=14)
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split

# create the model
logReg = LogisticRegression()

f = open('sample-data.txt')
f.readline() # skip the first line
data = np.loadtxt(f) # import textfile as numpy array

# separate data into training and testing
X = data[:, 1:] # everything but first column
Y = data[:,0] # first column
x_train, x_test, y_train, y_test = train_test_split(X, Y, test_size=0.25, random_state=0)
# train the model
logReg.fit(x_train, y_train)

print(logReg.coef_)


