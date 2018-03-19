# generate some random sample data for testing
# completely random!!!!!

from random import *

f = open('sample-data.txt', 'w')
f.write('Win marshallDiff generalDiff colonelDiff majorDiff captainDiff lieutenantDiff sergeantDiff minerDiff scoutDiff spyDiff bombDiff flagDiff\n')
for i in range(100000):
    win = randint(0,1)
    d1 = randint(-1,1)
    d2 = randint(-1,1)
    d3 = randint(-2,2)
    d4 = randint(-3,3)
    d5 = randint(-4,4)
    d6 = randint(-4,4)
    d7 = randint(-4,4)
    d8 = randint(-5,5)
    d9 = randint(-8,8)
    d10 = randint(-1,1)
    d11 = randint(-6,6)
    flag = random()
    if (flag > 0.9):
        # only 10% of data points are right at winning move
        if (win == 1):
            d12 = win
        else:
            d12 = -1
    else:
        # not a winning move
        d12 = 0
    f.write('%d %d %d %d %d %d %d %d %d %d %d %d %d\n' %
            (win, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12))

# all done so close the file
f.close()


